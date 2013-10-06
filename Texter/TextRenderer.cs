
namespace Texter
{
    public abstract class TextRenderer
    {
        public uint Width { get; protected set; }
        public uint Height { get; protected set; }

        public abstract void Set(int x, int y, Character character, bool useBlending = true);
        public abstract Character Get(int x, int y);

        public TextRegion Region(int x, int y, uint w, uint h)
        {
            return new TextRegion(this, x, y, w, h);
        }

        public TextEffect Effect(TextEffect.Func effect)
        {
            return new TextEffect(this, effect);
        }

        public void Clear(Character character)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Set(x, y, character, false);
                }
            }
        }

        public void DrawImage(int x, int y, TextRenderer image)
        {
            if (x < -image.Width || x > Width || y < -image.Height || y > Height)
                return;

            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    Set(x + xx, y + yy, image.Get(xx, yy));
                }
            }
        }

        public void DrawImagePartial(int x, int y, TextRenderer image, uint startX, uint startY, uint width, uint height)
        {
            if (x < -width || x > Width || y < -height || y > Height)
                return;

            for (int yy = 0; yy < height; yy++)
            {
                for (int xx = 0; xx < width; xx++)
                {
                    Set(x + xx, y + yy, image.Get((int)startX + xx, (int)startY + yy));
                }
            }
        }

        public void DrawText(int x, int y, string text, Character color)
        {
            foreach (char c in text)
            {
                Set(x, y, new Character(c, color.Foreground, color.Background));
                x++;
            }
        }

        // TODO: drawtext with formatting characters

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
