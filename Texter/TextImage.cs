
namespace Texter
{
    public class TextImage : TextRenderer
    {
        Character[,] _data;

        public TextImage(uint width, uint height)
        {
            Width = width;
            Height = height;

            _data = new Character[width, height];
            Clear(Character.Blank);
        }

        public override void Set(int x, int y, Character character)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return;

            if (character.HasTransparentComponent)
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

                _data[x, y] = new Character(glyph, fore, back);
            }
            else
            {
                _data[x, y] = character;
            }
        }

        public override Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return Character.Blank;

            return _data[x, y];
        }
    }
}
