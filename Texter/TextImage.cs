﻿using System;

namespace Texter
{
	public class TextImage : TextRenderer
	{
		Character[,] data;

		public TextImage(uint width, uint height)
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

		public override void Set(int x, int y, Character character)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return;

			data[x, y] = character;
		}

		public override Character Get(int x, int y)
		{
			if (x < 0 || x > Width || y < 0 || y > Height)
				throw new ArgumentOutOfRangeException();

			return data[x, y];
		}
	}
}
