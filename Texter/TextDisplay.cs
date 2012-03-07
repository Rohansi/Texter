using System;
using System.IO;
using SFML.Window;
using SFML.Graphics;

namespace Texter
{
	public class TextDisplay : TextRenderer
	{
		public static string DataFolder = "Data/Texter/";

		public const int CharacterWidth	= 6;
		public const int CharacterHeight = 8;

		static Image palette = new Image(Path.Combine(DataFolder, "palette.png"));
		static Texture paletteTexture = new Texture(palette);

		static Texture fontTexture = new Texture(Path.Combine(DataFolder, "font.png"));

		Image data;
		Texture dataTexture;

		RenderTexture display;
		Shader renderer;

		Color color = new Color(0, 15, 0);

		public TextDisplay(int width, int height)
		{
			Width = width;
			Height = height;

			data = new Image((uint)width, (uint)height, Color.Black);
			dataTexture = new Texture(data);

			display = new RenderTexture((uint)width * CharacterWidth, (uint)height * CharacterHeight);

			renderer = new Shader(Path.Combine(DataFolder, "textrenderer.sfx"));
			renderer.SetTexture("data", dataTexture);
			renderer.SetParameter("dataSize", width, height);
			renderer.SetTexture("font", fontTexture);
			renderer.SetTexture("palette", paletteTexture);
		}

		public void Draw(RenderTarget rt, Vector2f position)
		{
			paletteTexture.Update(palette);
			dataTexture.Update(data);

			Sprite s = new Sprite(display.Texture);
			s.Position = position;
			rt.Draw(s, renderer);
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

		public override void DrawString(int x, int y, string str, byte fore = 15, byte back = 0)
		{
			foreach (char c in str)
			{
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
