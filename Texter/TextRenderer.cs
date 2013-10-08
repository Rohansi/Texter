
namespace Texter
{
    public abstract class TextRenderer
    {
        public const string SingleBox = "\xDA\xC4\xBF\xB3 \xB3\xC0\xC4\xD9";
        public const string DoubleBox = "\xC9\xCD\xBB\xBA \xBA\xC8\xCD\xBC";

        public uint Width { get; protected set; }
        public uint Height { get; protected set; }

        public abstract void Set(int x, int y, Character character, bool useBlending = true);
        public abstract Character Get(int x, int y);

        /// <summary>
        /// Access a subregion of this renderer.
        /// </summary>
        public TextRegion Region(int x, int y, uint w, uint h)
        {
            return new TextRegion(this, x, y, w, h);
        }

        /// <summary>
        /// Access this renderer behind an effect.
        /// </summary>
        public TextEffect Effect(TextEffect.Func effect)
        {
            return new TextEffect(this, effect);
        }

        /// <summary>
        /// Clear the renderer to a character.
        /// </summary>
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

        /// <summary>
        /// Draw an renderer to this renderer.
        /// </summary>
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

        /// <summary>
        /// Draw part of a renderer to this renderer.
        /// </summary>
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

        /// <summary>
        /// Draw unformatted text. Will not handle control characters.
        /// </summary>
        public void DrawText(int x, int y, string text, Character color)
        {
            foreach (char c in text)
            {
                Set(x, y, new Character(c, color.Foreground, color.Background));
                x++;
            }
        }

        // TODO: drawtext with formatting characters

        /// <summary>
        /// Draw a 9-patch box.
        /// 
        /// Box indices:
        /// 012
        /// 345
        /// 678
        /// </summary>
        public void DrawBox(int x, int y, uint w, uint h, string box, Character color)
        {
            for (int putY = y, yy = 0; putY < y + h; putY++, yy++)
            {
                for (int putX = x, xx = 0; putX < x + w; putX++, xx++)
                {
                    char c;
                    if (yy == 0)
                    {
                        if (xx == 0)
                            c = box[0];
                        else if (xx == w - 1)
                            c = box[2];
                        else
                            c = box[1];
                    }
                    else if (yy == h - 1)
                    {
                        if (xx == 0)
                            c = box[6];
                        else if (xx == w - 1)
                            c = box[8];
                        else
                            c = box[7];
                    }
                    else
                    {
                        if (xx == 0)
                            c = box[3];
                        else if (xx == w - 1)
                            c = box[5];
                        else
                            c = box[4];
                    }

                    Set(putX, putY, new Character(c, color.Foreground, color.Background));
                }
            }
        }
    }
}
