using System;
using System.IO;
using SFML.Graphics;

namespace Texter
{
	public partial class TextDisplay : TextRenderer
	{
		#region Shaders
		static string DisplayVertexShader = @"
void main() {
	gl_TexCoord[0] = gl_TextureMatrix[0] * gl_MultiTexCoord0;
	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}";

		static string DisplayFragmentShader = @"
#version 130
#extension GL_EXT_gpu_shader4 : enable

const ivec2 charSize	= ivec2(#W#, #H#);
const ivec2 fontSizeC	= ivec2(16, 16);
const ivec2 fontSize	= ivec2(charSize.x * fontSizeC.x, charSize.y * fontSizeC.y);

uniform sampler2D	data;
uniform vec2		dataSize;
uniform sampler2D	font;
uniform sampler2D	palette;

vec4 texelGet(sampler2D tex, ivec2 size, ivec2 coord) {
	return texture2D(tex, vec2(float(coord.x) / float(size.x),
						       float(coord.y) / float(size.y)));
}

void main() {
	ivec2 chPos = ivec2(int(gl_TexCoord[0].x * (dataSize.x * charSize.x)) / charSize.x,
						int(gl_TexCoord[0].y * (dataSize.y * charSize.y)) / charSize.y);

	// r - character
	// g - foreground color
	// b - background color
	vec4 chData = texelGet(data, ivec2(dataSize) - 1, chPos);

	int ch = int(chData.r * 255);

	ivec2 fnPos = ivec2((ch % fontSizeC.x) * charSize.x, (ch / fontSizeC.y) * charSize.y);

	ivec2 offset = ivec2(int(gl_TexCoord[0].x * (dataSize.x * charSize.x)) % charSize.x, 
						 int(gl_TexCoord[0].y * (dataSize.y * charSize.y)) % charSize.y);

	vec4 fnCol = texelGet(font, fontSize - 1, fnPos + offset);

	if (fnCol.rgb == 1) {
		gl_FragColor = texelGet(palette, ivec2(255, 1), ivec2(int(chData.g * 255), 0));
	} else if (fnCol.rgb == 0) {
		gl_FragColor = texelGet(palette, ivec2(255, 1), ivec2(int(chData.b * 255), 0));
	}
}";
		#endregion

		public static uint CharacterWidth { get; private set; }
		public static uint CharacterHeight { get; private set; }

		static Image Palette;
		static Texture PaletteTexture;
		static Texture FontTexture;

		public static void Initialize(uint characterWidth = 8, uint characterHeight = 12, string dataFolder = "Data/")
		{
			if (Palette != null)
				throw new Exception("Initialize was already called");

			CharacterWidth = characterWidth;
			CharacterHeight = characterHeight;

			Palette = new Image(Path.Combine(dataFolder, "Palette.png"));
			PaletteTexture = new Texture(Palette);

			FontTexture = new Texture(Path.Combine(dataFolder, "Font.png"));

			DisplayFragmentShader = DisplayFragmentShader
					.Replace("#W#", CharacterWidth.ToString())
					.Replace("#H#", CharacterHeight.ToString());
		}

		public static void PaletteSet(byte index, Color color)
		{
			Palette.SetPixel((uint)index, 0, color);
		}

		public static Color PaletteGet(byte index)
		{
			return Palette.GetPixel((uint)index, 0);
		}
	}
}
