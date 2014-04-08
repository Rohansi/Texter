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

        private Color _color = new Color(0, 0, 0);

        public uint CharacterWidth { get; private set; }
        public uint CharacterHeight { get; private set; }

        public TextDisplay(uint width, uint height, string fontFile = "font.png", string paletteFile = "palette.png")
        {
            Width = width;
            Height = height;

            _data = new Image(width, height, Color.Black);
            _dataTexture = new Texture(_data);

            _fontTexture = new Texture(Path.Combine(DataFolder, fontFile));

            CharacterWidth = _fontTexture.Size.X / 16;
            CharacterHeight = _fontTexture.Size.Y / 16;

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

            _color.R = (byte)glyph;
            _color.G = (byte)fore;
            _color.B = (byte)back;
            _data.SetPixel((uint)x, (uint)y, _color);
        }

        public Character Get(int x, int y)
        {
            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return Character.Blank;

            Color p = _data.GetPixel((uint)x, (uint)y);
            return new Character(p.R, p.G, p.B);
        }

        public void PaletteSet(byte index, Color color)
        {
            _palette.SetPixel(index, 0, color);
        }

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
