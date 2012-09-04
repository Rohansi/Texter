﻿using System;
using SFML.Graphics;
using SFML.Window;

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

			display = new RenderTexture(width * CharacterWidth, height * CharacterHeight);

			renderer = Shader.FromString(displayVertexShader, displayFragmentShader);
			renderer.SetParameter("data", dataTexture);
			renderer.SetParameter("dataSize", width, height);
			renderer.SetParameter("font", fontTexture);
			renderer.SetParameter("palette", paletteTexture);
		}

		public void Draw(RenderTarget renderTarget, Vector2f position)
		{
			paletteTexture.Update(palette);
			dataTexture.Update(data);

			Sprite s = new Sprite(display.Texture);
			s.Position = position;

			renderTarget.Draw(s, new RenderStates(renderer));
		}

		public override void Set(int x, int y, Character character, bool blend = true)
		{
			if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
				return;

			int glyph = character.Glyph;
			int fore = character.Foreground;
			int back = character.Background;

			if (character.HasTransparentComponent)
			{
				Character ch = Get(x, y);

				if (glyph == -1)
					glyph = ch.Glyph;

				if (fore == -1)
					fore = ch.Foreground;

				if (back == -1)
					back = ch.Background;
			}

			color.R = (byte)glyph;
			color.G = (byte)fore;
			color.B = (byte)back;
			data.SetPixel((uint)x, (uint)y, color);
		}

		public override Character Get(int x, int y)
		{
			if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
				return Character.Blank;

			Color p = data.GetPixel((uint)x, (uint)y);
			return Character.Create(p.R, p.G, p.B);
		}
	}
}
