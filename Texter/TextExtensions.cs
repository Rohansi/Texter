
namespace Texter
{
    public static class TextExtensions
    {
        public const string SingleBox = "\xDA\xC4\xBF\xB3 \xB3\xC0\xC4\xD9";
        public const string DoubleBox = "\xC9\xCD\xBB\xBA \xBA\xC8\xCD\xBC";

        /// <summary>
        /// Access a subregion of this renderer.
        /// </summary>
        public static TextRegion Region(this ITextRenderer renderer, int x, int y, uint w, uint h)
        {
            return new TextRegion(renderer, x, y, w, h);
        }

        /// <summary>
        /// Access this renderer behind an effect function.
        /// </summary>
        public static TextEffect Effect(this ITextRenderer renderer, TextEffect.Func effect)
        {
            return new TextEffect(renderer, effect);
        }

        /// <summary>
        /// Clear this renderer to a character.
        /// </summary>
        public static void Clear(this ITextRenderer renderer, Character character, bool useBlending = false)
        {
            for (int y = 0; y < renderer.Height; y++)
            {
                for (int x = 0; x < renderer.Width; x++)
                {
                    renderer.Set(x, y, character, useBlending);
                }
            }
        }

        /// <summary>
        /// Draw a renderer to this renderer.
        /// </summary>
        public static void DrawImage(this ITextRenderer renderer, int x, int y, ITextRenderer image)
        {
            if (x < -image.Width || x > renderer.Width || y < -image.Height || y > renderer.Height)
                return;

            for (int yy = 0; yy < image.Height; yy++)
            {
                for (int xx = 0; xx < image.Width; xx++)
                {
                    renderer.Set(x + xx, y + yy, image.Get(xx, yy));
                }
            }
        }

        /// <summary>
        /// Draw part of a renderer to this renderer.
        /// </summary>
        public static void DrawImagePartial(this ITextRenderer renderer, int x, int y, ITextRenderer image, uint startX, uint startY, uint width, uint height)
        {
            if (x < -width || x > renderer.Width || y < -height || y > renderer.Height)
                return;

            for (int yy = 0; yy < height; yy++)
            {
                for (int xx = 0; xx < width; xx++)
                {
                    renderer.Set(x + xx, y + yy, image.Get((int)startX + xx, (int)startY + yy));
                }
            }
        }

        /// <summary>
        /// Draw unformatted text. Will not handle control characters.
        /// </summary>
        public static void DrawText(this ITextRenderer renderer, int x, int y, string text, Character color)
        {
            foreach (char c in text)
            {
                renderer.Set(x, y, new Character(c, color.Foreground, color.Background));
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
        public static void DrawBox(this ITextRenderer renderer, int x, int y, uint w, uint h, string box, Character color)
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

                    renderer.Set(putX, putY, new Character(c, color.Foreground, color.Background));
                }
            }
        }
    }
}
