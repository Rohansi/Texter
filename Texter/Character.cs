using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Texter
{
	public class Character
	{
		public int Glyph { get; private set; }
		public int ForegroundColor { get; private set; }
		public int BackgroundColor { get; private set; }

		public bool HasTransparentComponent
		{
			get
			{
				return Glyph == -1 || ForegroundColor == -1 || BackgroundColor == -1;
			}
		}

		public static Character Create(int glyph, int foregroundColor, int backgroundColor)
		{
			if (glyph < byte.MinValue || glyph > byte.MaxValue)
				glyph = -1;

			if (foregroundColor < byte.MinValue || foregroundColor > byte.MaxValue)
				foregroundColor = -1;

			if (backgroundColor < byte.MinValue || backgroundColor > byte.MaxValue)
				backgroundColor = -1;

			return new Character
			{
				Glyph = glyph,
				ForegroundColor = foregroundColor,
				BackgroundColor = backgroundColor
			};
		}

		public static Character Create(char glyph, int foregroundColor, int backgroundColor)
		{
			return Create((byte)glyph, foregroundColor, backgroundColor);
		}

		public Character Clone()
		{
			return Create(Glyph, ForegroundColor, BackgroundColor);
		}
	}
}
