using System;

namespace Texter
{
	public class Character
	{
		public byte Char, ForegroundColor, BackgroundColor;

		public static Character Create(char character, byte foregroundColor, byte backgroundColor)
		{
			Character c = new Character();
			c.Char = (byte)character;
			c.ForegroundColor = foregroundColor;
			c.BackgroundColor = backgroundColor;
			return c;
		}

		public static Character Create(byte character, byte foregroundColor, byte backgroundColor)
		{
			Character c = new Character();
			c.Char = character;
			c.ForegroundColor = foregroundColor;
			c.BackgroundColor = backgroundColor;
			return c;
		}
	}

	public abstract class TextRenderer
	{
		public uint Width { get; protected set; }
		public uint Height { get; protected set; }

		public abstract void Set(int x, int y, Character character);
		public abstract Character Get(int x, int y);

		public void Clear(Character character)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					Set(x, y, character);
				}
			}
		}

		public void DrawImage(int x, int y, TextImage image)
		{
			int endX = Math.Min(x + (int)image.Width, (int)Width);
			int endY = Math.Min(y + (int)image.Width, (int)Height);
			int xStart = x;

			for (; y < endY; y++)
			{
				for (x = xStart; x < endX; x++)
				{
					Set(x, y, image.Get(x, y));
				}
			}
		}

		public void DrawImagePartial(int x, int y, TextImage image, int startX, int startY, uint width, uint height)
		{
			if (startX < 0 || startY < 0 || width < 0 || height < 0)
				throw new ArgumentOutOfRangeException();

			if (x >= width || y >= height || x <= -width || y <= -height)
				return;

			int endX = Math.Min(x + (int)width - 1, (int)image.Width - 1);
			int endY = Math.Min(y + (int)height - 1, (int)image.Height - 1);
			int imgX = startX;
			int imgY = startY;
			int xStart = x;

			for (; y <= endY; y++)
			{
				for (x = xStart; x <= endX; x++)
				{
					Set(x, y, image.Get(imgX++, imgY));
				}

				imgX = startX;
				imgY++;
			}
		}

		public void DrawText(int x, int y, string text, byte foregroundColor = 15, byte backgroundColor = 0)
		{
			foreach (char c in text)
			{
				if (c == '\n')
				{
					x = 0;
					y++;
					continue;
				}

				Set(x, y, Character.Create(c, foregroundColor, backgroundColor));
				x++;
			}
		}
	}
}
