using System;

namespace Texter
{
	public class TextImage : TextRenderer
	{
		Character[,] data;

		public TextImage(int width, int height)
		{
			Width = width;
			Height = height;

			data = new Character[width, height];
			for (uint y = 0; y < Height; y++)
			{
				for (uint x = 0; x < Width; x++)
				{
					data[x, y] = Character.Create(0, 15, 0);
				}
			}
		}

		public override void Clear(Character c)
		{
			for (uint y = 0; y < Height; y++)
			{
				for (uint x = 0; x < Width; x++)
				{
					data[x, y] = c;
				}
			}
		}

		public override void Set(int x, int y, Character c)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return;

			data[x, y] = c;
		}

		public override Character Get(int x, int y)
		{
			if (x < 0 || x > Width || y < 0 || y > Height)
				throw new ArgumentOutOfRangeException();

			return data[x, y];
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
			int endY = Math.Min(y + img.Height, Height);
			for (; y < endY; y++)
			{
				for (; x < endX; x++)
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
	}
}
