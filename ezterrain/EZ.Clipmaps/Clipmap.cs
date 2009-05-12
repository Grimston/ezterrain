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
			texOffset.SetValue(0.0f);
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

		public void Render(RenderInfo info)
		{
			gradient.Bind();
			textureArray.Bind();
			program.Bind();

			eye.SetValue(info.Viewer.Position);

			int distanceScale = (int)(Math.Sqrt(Math.Abs(info.Viewer.Position.Z)) / 16);

			GL.EnableClientState(EnableCap.VertexArray);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
			GL.VertexPointer(3, VertexPointerType.Float,
							 0,
							 (IntPtr)VertexP.PositionOffset);

			texScale.SetValue(1.0f / (sideVertexCount - 1));
			texOffset.SetValue(0.5f);

			meshLevel.SetValue(0.0f + distanceScale);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, fullGridIndexBuffer);
			GL.DrawElements(BeginMode.Triangles,
							fullGridIndices.Length,
							DrawElementsType.UnsignedInt,
							IntPtr.Zero);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, hollowGridIndexBuffer);
			for (float i = 1.0f; i <= MaxLevel; i++)
			{
				float level = i + distanceScale;
				texScale.SetValue((1.0f / (sideVertexCount - 1)) / (1 << (int)level));
				meshLevel.SetValue(level);
				GL.DrawElements(BeginMode.Triangles,
								hollowGridIndices.Length,
								DrawElementsType.UnsignedInt,
								IntPtr.Zero);
			}

			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

			GL.DisableClientState(EnableCap.VertexArray);

			program.Unbind();
			gradient.Unbind();
			textureArray.Unbind();
		}

		private void UpdateTextures(Vector3 eye)
		{
			for (int i = 0; i <= MaxLevel; i++)
			{
				float offsetScale = (float)(1 << (MaxLevel - i));
				float eyeScale = (float)(1 << i);

				Rectangle rect = new Rectangle(i == MaxLevel ? 0 : (int)Math.Round(128 * offsetScale + 256 * eye.X / (sideVertexCount - 1) / eyeScale - 128),
												i == MaxLevel ? 0 : (int)Math.Round(128 * offsetScale + 256 * eye.Y / (sideVertexCount - 1) / eyeScale - 128),
												257,
												257);

				BitmapData imageData = images[i].LockBits(rect, ImageLockMode.ReadOnly, images[i].PixelFormat);
				Rectangle textureBounds = new Rectangle(0, 0, 257, 257);
				BitmapData textureData = textureArray.Images[i].Bitmap.LockBits(textureBounds, ImageLockMode.WriteOnly, images[i].PixelFormat);

				for (int j = 0; j < 257; j++)
				{
					memcpy((IntPtr)(textureData.Scan0.ToInt32() + j * textureData.Stride),
							(IntPtr)(imageData.Scan0.ToInt32() + j * imageData.Stride),
							textureData.Stride);
				}


				textureArray.Images[i].Bitmap.UnlockBits(textureData);
				images[i].UnlockBits(imageData);

				textureArray.Images[i].DirtyRegions.Add(textureBounds);
			}
		}

		[DllImport("msvcrt.dll", SetLastError = false)]
		static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);
	}
}
