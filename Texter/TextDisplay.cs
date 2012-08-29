using System;
using System.IO;
using SFML.Window;
using SFML.Graphics;

namespace Texter
{
	public partial class TextDisplay : TextRenderer
	{
		Image data;
		Texture dataTexture;

		RenderTexture display;
		Shader renderer;

		Color color = new Color(0, 0, 0);

		public TextDisplay(uint width, uint height)
		{
			Width = width;
			Height = height;

			data = new Image(width, height, Color.Black);
			dataTexture = new Texture(data);

			display = new RenderTexture((uint)width * CharacterWidth, (uint)height * CharacterHeight);

			renderer = Shader.FromString(DisplayVertexShader, DisplayFragmentShader);
			renderer.SetParameter("data", dataTexture);
			renderer.SetParameter("dataSize", width, height);
			renderer.SetParameter("font", FontTexture);
			renderer.SetParameter("palette", PaletteTexture);
		}

		public void Draw(RenderTarget renderTarget, Vector2f position)
		{
			PaletteTexture.Update(Palette);
			dataTexture.Update(data);

			Sprite s = new Sprite(display.Texture);
			s.Position = position;
			renderTarget.Draw(s, new RenderStates(renderer));
		}

		public override void Set(int x, int y, Character character)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return;
			color.R = (byte)character.Char;
			color.G = character.ForegroundColor;
			color.B = character.BackgroundColor;
			data.SetPixel((uint)x, (uint)y, color);
		}

		public override Character Get(int x, int y)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return Character.Create(0, 0, 0);
			Color p = data.GetPixel((uint)x, (uint)y);
			return Character.Create(p.R, p.G, p.B);
		}
	}
}
