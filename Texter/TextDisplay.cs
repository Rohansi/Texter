using System;
using System.IO;
using SFML.Window;
using SFML.Graphics;

namespace Texter
{
	public class TextDisplay : TextRenderer
	{
		#region Shaders
		const string DisplayVertexShader = @"
void main() {
	gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}";

		const string DisplayFragmentShader = @"
#version 130
#extension GL_EXT_gpu_shader4 : enable

const ivec2 charSize	= ivec2(#W#, #H#);
const ivec2 fontSizeC	= ivec2(16, 16);
const ivec2 fontSize	= ivec2(charSize.x * fontSizeC.x, charSize.y * fontSizeC.y);

uniform sampler2D	data;
uniform vec2		dataSize;
uniform sampler2D	font;
uniform sampler2D	palette;

vec4 texelGet(sampler2D tex, ivec2 size, ivec2 coord) {
	return texture2D(tex, vec2(float(coord.x) / float(size.x),
						       float(coord.y) / float(size.y)));
}

void main() {
	ivec2 chPos = ivec2(int(gl_TexCoord[0].x * (dataSize.x * charSize.x)) / charSize.x,
						int(gl_TexCoord[0].y * (dataSize.y * charSize.y)) / charSize.y);

	// r - character
	// g - foreground color
	// b - background color
	vec4 chData = texelGet(data, ivec2(dataSize) - 1, chPos);

	int ch = int(chData.r * 255);

	ivec2 fnPos = ivec2((ch % fontSizeC.x) * charSize.x, (ch / fontSizeC.y) * charSize.y);

	ivec2 offset = ivec2(int(gl_TexCoord[0].x * (dataSize.x * charSize.x)) % charSize.x, 
						 int(gl_TexCoord[0].y * (dataSize.y * charSize.y)) % charSize.y);

	vec4 fnCol = texelGet(font, fontSize - 1, fnPos + offset);

	if (fnCol.rgb == 1) {
		gl_FragColor = texelGet(palette, ivec2(255, 1), ivec2(int(chData.g * 255), 0));
	} else if (fnCol.rgb == 0) {
		gl_FragColor = texelGet(palette, ivec2(255, 1), ivec2(int(chData.b * 255), 0));
	}
}";
		#endregion

		public static string DataFolder = "Data/";

		static Image palette = new Image(Path.Combine(DataFolder, "Palette.png"));
		static Texture paletteTexture = new Texture(palette);

		static Texture fontTexture = new Texture(Path.Combine(DataFolder, "Font.png"));

		Image data;
		Texture dataTexture;

		RenderTexture display;
		Shader renderer;

		Color color = new Color(0, 15, 0);

		public TextDisplay(int width, int height, uint charWidth = 6, uint charHeight = 8)
		{
			Width = width;
			Height = height;

			data = new Image((uint)width, (uint)height, Color.Black);
			dataTexture = new Texture(data);

			display = new RenderTexture((uint)width * charWidth, (uint)height * charHeight);

			string shader = DisplayFragmentShader
				.Replace("#W#", charWidth.ToString())
				.Replace("#H#", charHeight.ToString());

			renderer = Shader.FromString(DisplayVertexShader, shader);
			renderer.SetParameter("data", dataTexture);
			renderer.SetParameter("dataSize", width, height);
			renderer.SetParameter("font", fontTexture);
			renderer.SetParameter("palette", paletteTexture);
		}

		public void Draw(RenderTarget rt, Vector2f position)
		{
			paletteTexture.Update(palette);
			dataTexture.Update(data);

			Sprite s = new Sprite(display.Texture);
			s.Position = position;
			rt.Draw(s, new RenderStates(renderer));
		}

		public override void Clear(Character c)
		{
			color.R = (byte)c.Char;
			color.G = c.Fore;
			color.B = c.Back;

			for (uint y = 0; y < Height; y++)
			{
				for (uint x = 0; x < Width; x++)
				{
					data.SetPixel(x, y, color);
				}
			}
		}

		public override void Set(int x, int y, Character c)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return;
			color.R = (byte)c.Char;
			color.G = c.Fore;
			color.B = c.Back;
			data.SetPixel((uint)x, (uint)y, color);
		}

		public override Character Get(int x, int y)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return Character.Create(0, 0, 0);
			Color p = data.GetPixel((uint)x, (uint)y);
			return Character.Create(p.R, p.G, p.B);
		}

		public override void DrawString(int x, int y, string str, byte fore = 15, byte back = 0)
		{
			foreach (char c in str)
			{
				if (c == '\n')
				{
					x = 0;
					y++;
					continue;
				}

				Set(x, y, Character.Create((byte)c, fore, back));
				x++;
			}
		}

		public override void DrawImage(int x, int y, TextImage img)
		{
			int endX = Math.Min(x + img.Width, Width);
			int endY = Math.Min(y + img.Width, Height);
			int xStart = x;

			for (; y < endY; y++)
			{
				for (x = xStart; x < endX; x++)
				{
					Set(x, y, img.Get(x, y));
				}
			}
		}

		public override void DrawImagePartial(int x, int y, TextImage img, int startX, int startY, int width, int height)
		{
			if (startX < 0 || startY < 0 || width < 0 || height < 0)
				throw new ArgumentOutOfRangeException();

			if (x >= width || y >= height || x <= -width || y <= -height)
				return;

			int endX = Math.Min(x + width - 1, img.Width - 1);
			int endY = Math.Min(y + height - 1, img.Height - 1);
			int imgX = startX;
			int imgY = startY;
			int xStart = x;

			for (; y <= endY; y++)
			{
				for (x = xStart; x <= endX; x++)
				{
					Set(x, y, img.Get(imgX++, imgY));
				}

				imgX = startX;
				imgY++;
			}
		}

		public static void PaletteSet(int x, Color c)
		{
			if (x < 0 || x > 255)
				throw new ArgumentOutOfRangeException();

			palette.SetPixel((uint)x, 0, c);
		}

		public static Color PaletteGet(int x)
		{
			if (x < 0 || x > 255)
				throw new ArgumentOutOfRangeException();

			return palette.GetPixel((uint)x, 0);
		}
	}
}
