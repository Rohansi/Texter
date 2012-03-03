using System;

namespace Texter
{
	public class Character
	{
		public byte Char, Fore, Back;

		public static Character Create(byte ch, byte fore, byte back)
		{
			Character c = new Character();
			c.Char = ch;
			c.Fore = fore;
			c.Back = back;
			return c;
		}
	}

	public abstract class TextRenderer
	{
		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public abstract void Clear(Character c);
		public abstract void Set(int x, int y, Character c);
		public abstract void DrawImage(int x, int y, TextImage img);
		public abstract void DrawImagePartial(int x, int y, TextImage img, int startX, int startY, int width, int height);
		public abstract void DrawString(int x, int y, string str, byte fore, byte back);
	}
}
