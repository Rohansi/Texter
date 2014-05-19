
namespace Texter
{
    public class TextImage : ITextRenderer
    {
        public uint Width { get; private set; }
        public uint Height { get; private set; }

        private Character[,] _data;

        /// <summary>
        /// Constructs a text image.
        /// </summary>
        /// <param name="width">Width in characters</param>
        /// <param name="height">Height in characters</param>
        public TextImage(uint width, uint height)
        {
            Width = width;
            Height = height;

            _data = new Character[width, height];
            this.Clear(Character.Blank);
        }

        public void Set(int x, int y, Character character, bool useBlending = true)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return;

            if (useBlending && character.HasTransparentComponent)
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

        public Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return Character.Blank;

            return _data[x, y];
        }
    }
}
