using System;
using System.IO;
using SFML.Graphics;

namespace Texter
{
    public partial class TextDisplay
    {
        static string paletteFile;
        static Texture fontTexture;
        static string displayVertexSource;
        static string displayFragmentSource;

        public static uint CharacterWidth { get; private set; }
        public static uint CharacterHeight { get; private set; }

        public static void Initialize(uint characterWidth = 8, uint characterHeight = 12, string dataFolder = "Data/")
        {
            if (fontTexture != null)
                throw new Exception("TextDisplay.Initialize was already called");

            CharacterWidth = characterWidth;
            CharacterHeight = characterHeight;

            paletteFile = Path.Combine(dataFolder, "palette.png");

            fontTexture = new Texture(Path.Combine(dataFolder, "font.png"));

            displayVertexSource = File.ReadAllText(Path.Combine(dataFolder, "texter.vert"));
            displayFragmentSource = File.ReadAllText(Path.Combine(dataFolder, "texter.frag"))
                    .Replace("#W#", CharacterWidth.ToString("G"))
                    .Replace("#H#", CharacterHeight.ToString("G"));
        }
    }
}
