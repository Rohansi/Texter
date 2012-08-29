using System;

namespace Texter
{
	public class Character
	{
		public byte Char, Fore, Back;

		public static Character Create(byte character, byte foregroundColor, byte backgroundColor)
		{
			Character c = new Character();
			c.Char = character;
			c.Fore = foregroundColor;
			c.Back = backgroundColor;
			return c;
		}
	}

	public abstract class TextRenderer
	{
		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public abstract void Clear(Character character);
		public abstract void Set(int x, int y, Character character);
		public abstract Character Get(int x, int y);

		public void DrawImage(int x, int y, TextImage image)
		{
			int endX = Math.Min(x + image.Width, Width);
			int endY = Math.Min(y + image.Width, Height);
			int xStart = x;

			for (; y < endY; y++)
			{
				for (x = xStart; x < endX; x++)
				{
					Set(x, y, image.Get(x, y));
				}
			}
		}

		public void DrawImagePartial(int x, int y, TextImage image, int startX, int startY, int width, int height)
		{
			if (startX < 0 || startY < 0 || width < 0 || height < 0)
				throw new ArgumentOutOfRangeException();

			if (x >= width || y >= height || x <= -width || y <= -height)
				return;

			int endX = Math.Min(x + width - 1, image.Width - 1);
			int endY = Math.Min(y + height - 1, image.Height - 1);
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

				Set(x, y, Character.Create((byte)c, foregroundColor, backgroundColor));
				x++;
			}
		}
	}
}
