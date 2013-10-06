using System;
using System.IO;
using SFML.Graphics;

namespace Texter
{
    public partial class TextDisplay
    {
        static string _paletteFile;
        static Texture _fontTexture;
        static string _displayVertexSource;
        static string _displayFragmentSource;

        public static uint CharacterWidth { get; private set; }
        public static uint CharacterHeight { get; private set; }

        public static void Initialize(uint characterWidth = 8, uint characterHeight = 12, string dataFolder = "Data/")
        {
            if (_fontTexture != null)
                throw new Exception("TextDisplay.Initialize was already called");

            CharacterWidth = characterWidth;
            CharacterHeight = characterHeight;

            _paletteFile = Path.Combine(dataFolder, "palette.png");

            _fontTexture = new Texture(Path.Combine(dataFolder, "font.png"));

            _displayVertexSource = File.ReadAllText(Path.Combine(dataFolder, "texter.vert"));
            _displayFragmentSource = File.ReadAllText(Path.Combine(dataFolder, "texter.frag"))
                    .Replace("#W#", CharacterWidth.ToString("G"))
                    .Replace("#H#", CharacterHeight.ToString("G"));
        }
    }
}
