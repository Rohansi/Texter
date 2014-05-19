using System.IO;
using SFML.Graphics;
using SFML.Window;

namespace Texter
{
    public class TextDisplay : Transformable, Drawable, ITextRenderer
    {
        public static string DataFolder = "Data/";

        public uint Width { get; private set; }
        public uint Height { get; private set; }

        private Image _data;
        private Texture _dataTexture;
        private Texture _fontTexture;
        private Image _palette;
        private Texture _paletteTexture;

        private VertexArray _display;
        private Shader _renderer;

        public uint CharacterWidth { get; private set; }
        public uint CharacterHeight { get; private set; }

        /// <summary>
        /// Constructs a text display.
        /// </summary>
        /// <param name="width">Width in characters</param>
        /// <param name="height">Height in characters</param>
        /// <param name="fontFile">Font texture</param>
        /// <param name="convertFont">Enable support for masked fonts</param>
        /// <param name="paletteFile">Palette texture</param>
        public TextDisplay(uint width, uint height, string fontFile = "font.png", bool convertFont = true, string paletteFile = "palette.png")
        {
            Width = width;
            Height = height;

            _data = new Image(width, height, Color.Black);
            _dataTexture = new Texture(_data);

            var fontImage = new Image(Path.Combine(DataFolder, fontFile));

            CharacterWidth = fontImage.Size.X / 16;
            CharacterHeight = fontImage.Size.Y / 16;

            if (convertFont)
            {
                var fontWidth = fontImage.Size.X;
                var fontHeight = fontImage.Size.Y;
                var fontPixels = fontImage.Pixels;

                fontImage.Dispose();

                // use top left pixel of space as mask color
                var maskX = (32 % 16) * CharacterWidth;
                var maskY = (32 / 16) * CharacterHeight * (fontWidth * 4);
                var maskI = maskX + maskY;
                var maskR = fontPixels[maskI + 0];
                var maskG = fontPixels[maskI + 1];
                var maskB = fontPixels[maskI + 2];
                var maskA = fontPixels[maskI + 3];

                for (int i = 0; i < fontPixels.Length; i += 4)
                {
                    var r = fontPixels[i + 0];
                    var g = fontPixels[i + 1];
                    var b = fontPixels[i + 2];
                    var a = fontPixels[i + 3];

                    if (r == maskR && g == maskG && b == maskB && a == maskA)
                    {
                        // mask color, set to transparent
                        fontPixels[i + 3] = 0;
                    }
                    else
                    {
                        // set alpha channel to average of rgb
                        var level = (r + b + g) / 3f;
                        var alpha = a / 256f;

                        fontPixels[i + 3] = (byte)(level * alpha);
                    }
                }

                fontImage = new Image(fontWidth, fontHeight, fontPixels);
                //fontImage.SaveToFile("result.png");
            }

            _fontTexture = new Texture(fontImage);

            _palette = new Image(Path.Combine(DataFolder, paletteFile));
            _paletteTexture = new Texture(_palette);

            _display = new VertexArray(PrimitiveType.Quads, 4);
            _display[0] = new Vertex(new Vector2f(0, 0), new Vector2f(0, 0));
            _display[1] = new Vertex(new Vector2f(width * CharacterWidth, 0), new Vector2f(1, 0));
            _display[2] = new Vertex(new Vector2f(width * CharacterWidth, height * CharacterHeight), new Vector2f(1, 1));
            _display[3] = new Vertex(new Vector2f(0, height * CharacterHeight), new Vector2f(0, 1));

            var displayVertexSource = File.ReadAllText(Path.Combine(DataFolder, "texter.vert"));
            var displayFragmentSource = File.ReadAllText(Path.Combine(DataFolder, "texter.frag"))
                    .Replace("#W#", CharacterWidth.ToString("D"))
                    .Replace("#H#", CharacterHeight.ToString("D"));

            _renderer = Shader.FromString(displayVertexSource, displayFragmentSource);
            _renderer.SetParameter("data", _dataTexture);
            _renderer.SetParameter("dataSize", width, height);
            _renderer.SetParameter("font", _fontTexture);
            _renderer.SetParameter("palette", _paletteTexture);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _paletteTexture.Update(_palette);
            _dataTexture.Update(_data);

            states.Transform *= Transform;
            states.Shader = _renderer;

            Texture.Bind(null);
            target.Draw(_display, states);
        }

        public void Set(int x, int y, Character character, bool useBlending = true)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return;

            int glyph = character.Glyph;
            int fore = character.Foreground;
            int back = character.Background;

            if (character.HasTransparentComponent)
            {
                if (useBlending)
                {
                    Character ch = Get(x, y);

                    if (glyph == -1)
                        glyph = ch.Glyph;

                    if (fore == -1)
                        fore = ch.Foreground;

                    if (back == -1)
                        back = ch.Background;
                }
                else
                {
                    if (glyph == -1)
                        glyph = 0;

                    if (fore == -1)
                        fore = 0;

                    if (back == -1)
                        back = 0;
                }
            }

            var color = new Color((byte)glyph, (byte)fore, (byte)back);
            _data.SetPixel((uint)x, (uint)y, color);
        }

        public Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return Character.Blank;

            Color p = _data.GetPixel((uint)x, (uint)y);
            return new Character(p.R, p.G, p.B);
        }

        /// <summary>
        /// Sets a color in the display's palette.
        /// </summary>
        public void PaletteSet(byte index, Color color)
        {
            _palette.SetPixel(index, 0, color);
        }

        /// <summary>
        /// Gets a color from the display's palette.
        /// </summary>
        public Color PaletteGet(byte index)
        {
            return _palette.GetPixel(index, 0);
        }

        public new void Dispose()
        {
            _data.Dispose();
            _dataTexture.Dispose();
            _fontTexture.Dispose();
            _palette.Dispose();
            _paletteTexture.Dispose();
            _display.Dispose();
            _renderer.Dispose();

            base.Dispose();
        }
    }
}
