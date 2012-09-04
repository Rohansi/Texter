using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Texter
{
	public class Character
	{
		public static readonly Character Blank = Create(0, 0, 0);

		public int Glyph { get; private set; }
		public int Foreground { get; private set; }
		public int Background { get; private set; }

		public bool HasTransparentComponent
		{
			get
			{
				return Glyph == -1 || Foreground == -1 || Background == -1;
			}
		}

		public static Character Create(int glyph = -1, int foreground = -1, int background = -1)
		{
			if (glyph < byte.MinValue || glyph > byte.MaxValue)
				glyph = -1;

			if (foreground < byte.MinValue || foreground > byte.MaxValue)
				foreground = -1;

			if (background < byte.MinValue || background > byte.MaxValue)
				background = -1;

			return new Character
			{
				Glyph = glyph,
				Foreground = foreground,
				Background = background
			};
		}

		public static Character Create(char glyph, int foreground = -1, int background = -1)
		{
			return Create((byte)glyph, foreground, background);
		}
	}
}
