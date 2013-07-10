#version 130
#extension GL_EXT_gpu_shader4 : enable

const ivec2 charSize    = ivec2(#W#, #H#); // Size of characters in the font texture
const ivec2 fontSizeC   = ivec2(16, 16);   // Layout of characters, default font has 16x16 characters
const ivec2 fontSize    = ivec2(charSize.x * fontSizeC.x, charSize.y * fontSizeC.y);

uniform sampler2D   data;
uniform vec2        dataSize;
uniform sampler2D   font;
uniform sampler2D   palette;

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
}