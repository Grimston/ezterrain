using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZ.Core;
using OpenTK.Math;
using OpenTK.Graphics;
using EZ.Objects;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace Ez.Clipmaps
{
	public class Clipmap : IRenderable
	{
		public const int MaxLevel = 4;

		private uint sideVertexCount;
		uint[] hollowGridIndices;
		uint[] fullGridIndices;
		private TextureArray textureArray;
		private List<Bitmap> images;
		private Texture2D gradient;
		private Program program;
		private Uniform texScale;
		private Uniform texOffset;
		private Uniform meshLevel;
		private Uniform eye;

		int vertexBuffer;
		int fullGridIndexBuffer;
		int hollowGridIndexBuffer;

		public Clipmap(uint sideVertexCount)
		{
			this.sideVertexCount = sideVertexCount;
			gradient = new Texture2D(TextureUnit.Texture0, ResourceManager.GetImagePath("gradient.bmp"));
			program = new Program();
			ConstructTexUniPairs();
			texScale = new Uniform(program, "texScale");
			texOffset = new Uniform(program, "texOffset");
			meshLevel = new Uniform(program, "level");
			eye = new Uniform(program, "eye");
			lastEye = new Vector3(float.NaN, float.NaN, float.NaN);
		}

		private void ConstructTexUniPairs()
		{
			images = new List<Bitmap>(MaxLevel + 1);
			Bitmap[] arrayImages = new Bitmap[MaxLevel + 1];

			for (int i = 0; i <= MaxLevel; i++)
			{
				arrayImages[i] = new Bitmap(257, 257, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				images.Add(new Bitmap(ResourceManager.GetImagePath(string.Format("l{0}.bmp", i))));
			}

			textureArray = new TextureArray(TextureUnit.Texture1, new Size(257, 257), arrayImages);
			textureArray.WrapS = TextureWrapMode.Repeat;
			textureArray.WrapT = TextureWrapMode.Repeat;
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
			textureArray.Initialize();
			gradient.Initialize();

			program.Initialize(Shader.FromFile(ShaderType.VertexShader, ResourceManager.GetProgramPath("clipmap.vert")),
							   Shader.FromFile(ShaderType.FragmentShader, ResourceManager.GetProgramPath("clipmap.frag")));

			new Uniform(program, "gradient").SetValue(0);
			new Uniform(program, "noiseArray").SetValue(1);
			new Uniform(program, "heightScale").SetValue((float)Math.Log(sideVertexCount, 1.2));
			Vector3 light = new Vector3(0, 0, 1);
			light.Normalize();
			new Uniform(program, "lightDirection").SetValue(light);
			texScale.SetValue(1.0f / (sideVertexCount - 1));
			texOffset.SetValue(0.5f, 0.5f);
			meshLevel.SetValue(0.0f);


			VertexP[] vertices = Grid.GetCenteredVertexArray(sideVertexCount);

			//indices = Grid.GetIndexArray(sideVertexCount, 0);
			this.hollowGridIndices = Grid.GetHollowGridIndexArray(sideVertexCount);
			this.fullGridIndices = Grid.GetFullGridIndexArray(sideVertexCount);

			GL.GenBuffers(1, out vertexBuffer);
			GL.GenBuffers(1, out fullGridIndexBuffer);
			GL.GenBuffers(1, out hollowGridIndexBuffer);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.BufferData(BufferTarget.ArrayBuffer,
						  (IntPtr)(vertices.Length * VertexP.SizeInBytes),
						  vertices,
						  BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, fullGridIndexBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer,
						  new IntPtr(fullGridIndices.Length * 4/*size of an index*/),
						  fullGridIndices,
						  BufferUsageHint.StaticDraw);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, hollowGridIndexBuffer);
			GL.BufferData(BufferTarget.ElementArrayBuffer,
						  new IntPtr(hollowGridIndices.Length * 4/*size of an index*/),
						  hollowGridIndices,
						  BufferUsageHint.StaticDraw);

			//construct meshes
			Initialized = true;
		}

		public bool Update(RenderInfo info)
		{
			//TODO: update texture coordinates
			UpdateTextures(info.Viewer.Position);
			gradient.Update();
			textureArray.Update();

			//always render
			return true;
		}

		private Vector3 lastEye;

		public void Render(RenderInfo info)
		{
			BeginRender(info);

			int distanceScale = (int)(Math.Sqrt(Math.Abs(info.Viewer.Position.Z)) / 16);

			Vector2 offset = new Vector2(0.0f, 0.0f) + info.Viewer.Position.Xy / (sideVertexCount - 1);

			texOffset.SetValue(offset.X, offset.Y);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, fullGridIndexBuffer);
			Draw(distanceScale, fullGridIndices);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, hollowGridIndexBuffer);
			for (float i = 1.0f; i <= MaxLevel; i++)
			{
				float level = i + distanceScale;
				Draw(level, hollowGridIndices);
			}

			EndRender();

			lastEye = info.Viewer.Position;
		}

		private void BeginRender(RenderInfo info)
		{
			gradient.Bind();
			textureArray.Bind();
			program.Bind();

			eye.SetValue(info.Viewer.Position);

			GL.EnableClientState(EnableCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.VertexPointer(3, VertexPointerType.Float,
							 0,
							 (IntPtr)VertexP.PositionOffset);

		}

		private void EndRender()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.DisableClientState(EnableCap.VertexArray);

			program.Unbind();
			gradient.Unbind();
			textureArray.Unbind();
		}

		private void Draw(float level, uint[] indices)
		{
			texScale.SetValue((1.0f / (sideVertexCount - 1)) / (1 << (int)level));
			meshLevel.SetValue(level);
			GL.DrawElements(BeginMode.Triangles,
							indices.Length,
							DrawElementsType.UnsignedInt,
							IntPtr.Zero);
		}

		private void UpdateTextures(Vector3 eye)
		{
			for (int i = 0; i <= MaxLevel; i++)
			{
				UpdateTexture(i, lastEye, eye);
			}
		}

		private void UpdateTexture(int level, Vector3 lastEye, Vector3 eye)
		{
			Bitmap source = images[level];
			TextureArrayElement target = textureArray.Images[level];

			Vector3 diff = eye - lastEye;
			if (diff != Vector3.Zero)
			{
				UpdateRegion(source, GetSourceRegion(eye, level), target);
			}
		}

		private static Rectangle GetSourceRegion(Vector3 eye, int level)
		{
			Vector2d offset = GetOffset(eye, level);

			Rectangle rect = new Rectangle((int)offset.X, (int)offset.Y, 257, 257);
			return rect;
		}

		private static Vector2d GetOffset(Vector3 eye, int level)
		{
			double offsetScale = (double)(1 << (MaxLevel - level - 1));
			double eyeScale = 1 / (double)(1 << level);

			Vector2d offset = level == MaxLevel ? Vector2d.Zero
											: new Vector2d(Math.Round(256 * (offsetScale - 0.5) + eye.X * eyeScale),
														   Math.Round(256 * (offsetScale - 0.5) + eye.Y * eyeScale));
			return offset;
		}


		private void UpdateRegion(Bitmap source, Rectangle region, TextureArrayElement target)
		{
			int xOffset = region.X % target.Bitmap.Width;
			int yOffset = region.Y % target.Bitmap.Height;

			Rectangle targetRegion = new Rectangle(xOffset, yOffset, region.Width, region.Height);

			if (!target.Bounds.Contains(targetRegion))
			{
				UpdateIntersectingRegion(source, region, target, targetRegion);
			}
			else
			{
				UpdateRegion(source, region, target, targetRegion);
			}
		}

		private void UpdateIntersectingRegion(Bitmap source, Rectangle sourceRegion, TextureArrayElement target, Rectangle targetRegion)
		{
			Rectangle targetBounds = target.Bounds;

			Rectangle topLeft = new Rectangle(targetRegion.X, targetRegion.Y, targetBounds.Width - targetRegion.X, targetBounds.Height - targetRegion.Y);
			Rectangle topLeftSource = new Rectangle(0, 0, topLeft.Width, topLeft.Height);
			topLeftSource.Offset(sourceRegion.Location);
			UpdateRegion(source, topLeftSource, target, topLeft);

			Rectangle bottomRight = new Rectangle(0, 0, targetRegion.Width - topLeft.Width, targetRegion.Height - topLeft.Height);
			Rectangle bottomRightSource = new Rectangle(topLeft.Width, topLeft.Height, bottomRight.Width, bottomRight.Height);
			bottomRightSource.Offset(sourceRegion.Location);
			UpdateRegion(source, bottomRightSource, target, bottomRight);

			Rectangle topRight = new Rectangle(0, bottomRight.Height, bottomRight.Width, topLeft.Height);
			Rectangle topRightSource = new Rectangle(topLeft.Width, 0, topRight.Width, topRight.Height);
			topRightSource.Offset(sourceRegion.Location);
			UpdateRegion(source, topRightSource, target, topRight);

			Rectangle bottomLeft = new Rectangle(bottomRight.Width, 0, topLeft.Width, bottomRight.Height);
			Rectangle bottomLeftSource = new Rectangle(0, topLeft.Height, bottomLeft.Width, bottomLeft.Height);
			bottomLeftSource.Offset(sourceRegion.Location);
			UpdateRegion(source, bottomLeftSource, target, bottomLeft);
		}

		private void UpdateRegion(Bitmap source, Rectangle region, TextureArrayElement target, Rectangle targetRegion)
		{
			if (region.Width > 0 && region.Height > 0
				&& targetRegion.Width > 0 && targetRegion.Height > 0)
			{
				UpdateRegion(source, region, target.Bitmap, targetRegion);
				target.DirtyRegions.Add(targetRegion);
			}
		}

		private void UpdateRegion(Bitmap source, Rectangle region, Bitmap target, Rectangle targetRegion)
		{
			BitmapData sourceData = source.LockBits(region, ImageLockMode.ReadOnly, target.PixelFormat);
			BitmapData targetData = target.LockBits(targetRegion, ImageLockMode.WriteOnly, target.PixelFormat);

			for (int row = 0; row < targetData.Height; row++)
			{
				memcpy((IntPtr)(targetData.Scan0.ToInt32() + row * targetData.Stride),
						(IntPtr)(sourceData.Scan0.ToInt32() + row * sourceData.Stride),
						targetData.Stride);
			}

			target.UnlockBits(targetData);
			source.UnlockBits(sourceData);
		}

		[DllImport("msvcrt.dll", SetLastError = false)]
		static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

		private void UpdateImage(Bitmap source, Point center, Bitmap target, ref Point lastPoint)
		{
			int width = 257;

			Rectangle clipRect = center.GetCenteredRect(width);

			if (lastPoint.X < 0
				|| Math.Abs(clipRect.X - lastPoint.X) >= width
				|| Math.Abs(clipRect.Y - lastPoint.Y) >= width)
			{
				UpdateImage(source, clipRect, target);
			}
			else
			{
				foreach (Rectangle rect in BitmapExtensions.Diff(lastPoint, clipRect.Location, clipRect.Size))
				{
					UpdateImage(source, rect, target);
				}
			}

			lastPoint = clipRect.Location;
		}

		private void UpdateImage(Bitmap source, Rectangle rect, Bitmap target)
		{
			Point destinationOffset = new Point(BitmapExtensions.Repeat(rect.X, target.Width),
												BitmapExtensions.Repeat(rect.Y, target.Height));
			source.CopyPartsTo(target, new CopyInfo(rect.Location, destinationOffset, rect.Size));
		}

		private static double GetScale(int value, int range)
		{
			return (double)value / range;
		}

		private static int GetScaled(double scale, int range)
		{
			return (int)Math.Round(scale * range);
		}
	}
}
