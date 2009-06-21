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
		private Point textureCenterOffset;
		uint[] hollowGridIndices;
		uint[] fullGridIndices;
		private TextureArray textureArray;
		private Vector3[] lastEyes;
		private Bitmap[] images;
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
			textureCenterOffset = new Point((int)sideVertexCount / 2,
											(int)sideVertexCount / 2);
		}

		private void ConstructTexUniPairs()
		{
			images = new Bitmap[MaxLevel + 1];
			lastEyes = new Vector3[MaxLevel + 1];
			Bitmap[] arrayImages = new Bitmap[MaxLevel + 1];

			for (int i = 0; i <= MaxLevel; i++)
			{
				arrayImages[i] = new Bitmap(257, 257, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
				images[i] = new Bitmap(ResourceManager.GetImagePath(string.Format("l{0}.bmp", i)));
				lastEyes[i] = new Vector3(float.NaN, float.NaN, float.NaN);
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

			int distanceStep = (int)(Math.Abs(info.Viewer.Position.Z) / sideVertexCount);

			int level = distanceStep;
			UpdateTexUniforms(info.Viewer.Position, level);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, fullGridIndexBuffer);
			Draw(distanceStep, fullGridIndices);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, hollowGridIndexBuffer);
			for (int i = 1; i <= MaxLevel; i++)
			{
				level = i + distanceStep;
				UpdateTexUniforms(info.Viewer.Position, level);

				Draw(level, hollowGridIndices);
			}

			EndRender();

			lastEye = info.Viewer.Position;
		}

		private void UpdateTexUniforms(Vector3 eye, int level)
		{
			float texScaleFactor = (1.0f / (sideVertexCount - 1)) / (1 << level);

			Vector2 clipOffset = eye.Xy;
			clipOffset.X = BitmapExtensions.Repeat(clipOffset.X + sideVertexCount/2, (int)sideVertexCount);
			clipOffset.Y = BitmapExtensions.Repeat(clipOffset.Y + sideVertexCount / 2, (int)sideVertexCount);

			Vector2 offset = clipOffset;

			texScale.SetValue(texScaleFactor);
			texOffset.SetValue(offset);
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
				UpdateImage(i, eye);
			}
		}

		private void UpdateImage(int level, Vector3 eye)
		{
			Vector3 lastEye = lastEyes[level];
			bool updateAll = float.IsNaN(lastEye.X);

			float positionScale = 1.0f / (1 << level);
			Point center = GetEyePoint(eye, positionScale);
			Rectangle clipRect = center.GetCenteredRect((int)sideVertexCount);

			if (updateAll)
			{
				UpdateWhole(level, eye, clipRect);
			}
			else
			{
				Point oldCenter = GetEyePoint(lastEye, positionScale);

				switch (GetUpdateType(center, oldCenter))
				{
					case ImageUpdateType.Whole:
						UpdateWhole(level, eye, clipRect);
						break;
					case ImageUpdateType.Partial:
						UpdatePartial(level, eye, clipRect, oldCenter);
						break;
				}
			}
		}

		private ImageUpdateType GetUpdateType(Point center, Point oldCenter)
		{
			Size centerDiff = new Size(Math.Abs(center.X - oldCenter.X),
										Math.Abs(center.Y - oldCenter.Y));

			if (centerDiff.Width >= sideVertexCount
			 || centerDiff.Height >= sideVertexCount)
			{
				return ImageUpdateType.Whole;
			}
			else if (centerDiff.Width > 0 && centerDiff.Height > 0)
			{
				return ImageUpdateType.Partial;
			}
			else
			{
				return ImageUpdateType.None;
			}
		}

		private Point GetEyePoint(Vector3 eye, float positionScale)
		{
			Point point = new Point((int)Math.Round(eye.X * positionScale),
									 (int)Math.Round(eye.Y * positionScale));
			point.Offset(textureCenterOffset);

			return point;
		}

		private void UpdatePartial(int level, Vector3 eye, Rectangle clipRect, Point oldCenter)
		{
			Rectangle oldClipRect = oldCenter.GetCenteredRect((int)sideVertexCount);

			foreach (Rectangle rect in BitmapExtensions.Diff(oldClipRect.Location, clipRect.Location, clipRect.Size))
			{
				UpdateImage(images[level], textureArray.Images[level], rect);
			}

			lastEyes[level] = eye;
		}

		private void UpdateWhole(int level, Vector3 eye, Rectangle clipRect)
		{
			UpdateImage(images[level], textureArray.Images[level], clipRect);
			lastEyes[level] = eye;
		}

		private void UpdateImage(Bitmap source, TextureArrayElement target, Rectangle rect)
		{
			Point destinationOffset = new Point(BitmapExtensions.Repeat(rect.X, target.Bitmap.Width),
												BitmapExtensions.Repeat(rect.Y, target.Bitmap.Height));
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
