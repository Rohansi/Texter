using System;

namespace Texter
{
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

		public void DrawImage(int x, int y, TextRenderer image)
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

		public void DrawImagePartial(int x, int y, TextRenderer image, uint startX, uint startY, uint width, uint height)
		{
			if (x >= width || y >= height || x <= -width || y <= -height)
				return;

			int endX = Math.Min(x + (int)width - 1, (int)image.Width - 1);
			int endY = Math.Min(y + (int)height - 1, (int)image.Height - 1);
			int imgX = (int)startX;
			int imgY = (int)startY;
			int xStart = x;

			for (; y <= endY; y++)
			{
				for (x = xStart; x <= endX; x++)
				{
					Set(x, y, image.Get(imgX++, imgY));
				}

				imgX = (int)startX;
				imgY++;
			}
		}

		public void DrawText(int x, int y, string text, Character color)
		{
			foreach (char c in text)
			{
				if (c == '\n')
				{
					x = 0;
					y++;
					continue;
				}

				Set(x, y, Character.Create(c, color.Foreground, color.Background));
				x++;
			}
		}

		public void DrawRectangle(int x, int y, uint w, uint h, Character fill, Character border = null)
		{
			for (int yy = y; yy < y + h; yy++)
			{
				for (int xx = x; xx < x + w; xx++)
				{
					if (border != null && (xx == x || yy == y || xx == (x + w - 1) || yy == (y + h - 1)))
					{
						Set(xx, yy, border);
						continue;
					}

					Set(xx, yy, fill);
				}
			}
		}
	}
}
