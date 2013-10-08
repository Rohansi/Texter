#version 110

const vec2 charSize    = vec2(#W#, #H#); // Size of characters in the font texture
const vec2 fontSizeC   = vec2(16, 16);   // Layout of characters
const vec2 fontSize    = vec2(charSize.x * fontSizeC.x, charSize.y * fontSizeC.y);

uniform sampler2D   data;
uniform vec2        dataSize;
uniform sampler2D   font;
uniform sampler2D   palette;

vec4 texelGet(sampler2D tex, vec2 size, vec2 coord) {
    return texture2D(tex, vec2(coord.x / size.x,
                               coord.y / size.y));
}

void main() {
    vec2 chPos = vec2(floor(gl_TexCoord[0].x * (dataSize.x * charSize.x) / charSize.x),
                      floor(gl_TexCoord[0].y * (dataSize.y * charSize.y) / charSize.y));

    // r - character
    // g - foreground color
    // b - background color
    vec4 chData = texelGet(data, dataSize - 1.0, chPos);
    
    float ch = floor(chData.r * 255.0);
    
    vec2 fnPos = vec2(floor(mod(ch, fontSizeC.x)) * charSize.x,
                      floor(ch / fontSizeC.y) * charSize.y);
    
    vec2 offset = vec2(mod(floor(gl_TexCoord[0].x * (dataSize.x * charSize.x)), charSize.x),
                       mod(floor(gl_TexCoord[0].y * (dataSize.y * charSize.y)), charSize.y));
    
    vec4 fnCol = texelGet(font, fontSize - 1.0, fnPos + offset);
    
    vec4 foreCol = texelGet(palette, vec2(255.0, 1.0), vec2(floor(chData.g * 255.0), 0.0));
    vec4 backCol = texelGet(palette, vec2(255.0, 1.0), vec2(floor(chData.b * 255.0), 0.0));

    gl_FragColor = fnCol * foreCol + (1.0 - fnCol) * backCol;
}