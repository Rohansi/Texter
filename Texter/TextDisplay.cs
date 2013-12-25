using System.IO;
using SFML.Graphics;
using SFML.Window;

namespace Texter
{
    public class TextDisplay : TextRenderer
    {
        public static string DataFolder = "Data/";
        
        Image _data;
        Texture _dataTexture;
        Texture _fontTexture;
        Image _palette;
        Texture _paletteTexture;

        RenderTexture _display;
        Sprite _displaySprite;
        Shader _renderer;

        Color _color = new Color(0, 0, 0);

        public uint CharacterWidth { get; private set; }
        public uint CharacterHeight { get; private set; }

        public TextDisplay(uint width, uint height, string fontFile = "font.png", uint characterWidth = 8, uint characterHeight = 12, string paletteFile = "palette.png")
        {
            Width = width;
            Height = height;

            CharacterWidth = characterWidth;
            CharacterHeight = characterHeight;

            _data = new Image(width, height, Color.Black);
            _dataTexture = new Texture(_data);

            _fontTexture = new Texture(Path.Combine(DataFolder, fontFile));

            _palette = new Image(Path.Combine(DataFolder, paletteFile));
            _paletteTexture = new Texture(_palette);

            // TODO: get rid of this useless texture
            _display = new RenderTexture(width * CharacterWidth, height * CharacterHeight);

            _displaySprite = new Sprite(_display.Texture);

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

        public void Draw(RenderTarget renderTarget, Vector2f position)
        {
            _paletteTexture.Update(_palette);
            _dataTexture.Update(_data);

            _displaySprite.Position = position;
            renderTarget.Draw(_displaySprite, new RenderStates(_renderer));
        }

        public override void Set(int x, int y, Character character, bool useBlending = true)
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

        public override Character Get(int x, int y)
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
    }
}
