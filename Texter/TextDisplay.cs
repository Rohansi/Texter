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

		public TextDisplay(int width, int height, uint characterWidth = DefaultCharacterWidth, uint characterHeight = DefaultCharacterHeight)
		{
			Width = width;
			Height = height;

			data = new Image((uint)width, (uint)height, Color.Black);
			dataTexture = new Texture(data);

			display = new RenderTexture((uint)width * characterWidth, (uint)height * characterHeight);

			string shader = DisplayFragmentShader
				.Replace("#W#", characterWidth.ToString())
				.Replace("#H#", characterHeight.ToString());

			renderer = Shader.FromString(DisplayVertexShader, shader);
			renderer.SetParameter("data", dataTexture);
			renderer.SetParameter("dataSize", width, height);
			renderer.SetParameter("font", FontTexture);
			renderer.SetParameter("palette", PaletteTexture);
		}

		public void Draw(RenderTarget rt, Vector2f position)
		{
			PaletteTexture.Update(Palette);
			dataTexture.Update(data);

			Sprite s = new Sprite(display.Texture);
			s.Position = position;
			rt.Draw(s, new RenderStates(renderer));
		}

		public override void Clear(Character character)
		{
			color.R = (byte)character.Char;
			color.G = character.Fore;
			color.B = character.Back;

			for (uint y = 0; y < Height; y++)
			{
				for (uint x = 0; x < Width; x++)
				{
					data.SetPixel(x, y, color);
				}
			}
		}

		public override void Set(int x, int y, Character character)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height)
				return;
			color.R = (byte)character.Char;
			color.G = character.Fore;
			color.B = character.Back;
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
