using System;

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
            Clear(Character.Blank);
        }

        public override void Set(int x, int y, Character character, bool blend = true)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return;

            if (blend && character.HasTransparentComponent)
            {
                int glyph = character.Glyph;
                int fore = character.Foreground;
                int back = character.Background;

                Character ch = Get(x, y);

                if (glyph == -1)
                    glyph = ch.Glyph;

                if (fore == -1)
                    fore = ch.Foreground;

                if (back == -1)
                    back = ch.Background;

                data[x, y] = Character.Create(glyph, fore, back);
            }
            else
            {
                data[x, y] = character;
            }
        }

        public override Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return Character.Blank;

            return data[x, y];
        }
    }
}
