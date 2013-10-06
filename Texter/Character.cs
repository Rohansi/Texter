
namespace Texter
{
    public class Character
    {
        public static readonly Character Blank = new Character(0, 0, 0);
        public static readonly Character Transparent = new Character();

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

        public Character(int glyph = -1, int foreground = -1, int background = -1)
        {
            if (glyph < byte.MinValue || glyph > byte.MaxValue)
                glyph = -1;

            if (foreground < byte.MinValue || foreground > byte.MaxValue)
                foreground = -1;

            if (background < byte.MinValue || background > byte.MaxValue)
                background = -1;

            Glyph = glyph;
            Foreground = foreground;
            Background = background;
        }

        public Character(char glyph, int foreground = -1, int background = -1)
            : this((int)glyph, foreground, background)
        {
        }
    }
}
