#version 110

const vec2 charSize    = vec2(#W#, #H#); // Size of characters in the font texture
const vec2 fontSizeC   = vec2(16, 16);   // Layout of characters
const vec2 fontSize    = vec2(charSize.x * fontSizeC.x, charSize.y * fontSizeC.y);
const vec3 eps         = vec3(0.005, 0.005, 0.005);

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
    
    float ch = floor(chData.r * 256.0);
    
    vec2 fnPos = vec2(floor(mod(ch, fontSizeC.x)) * charSize.x,
                      floor(ch / fontSizeC.y) * charSize.y);
    
    vec2 offset = vec2(mod(floor(gl_TexCoord[0].x * (dataSize.x * charSize.x)), charSize.x),
                       mod(floor(gl_TexCoord[0].y * (dataSize.y * charSize.y)), charSize.y));
    
    vec4 fnCol = texelGet(font, fontSize - 1.0, fnPos + offset);

    if (all(greaterThanEqual(fnCol.rgb, vec3(1.0) - eps)))
        gl_FragColor = texelGet(palette, vec2(256.0, 1.0), vec2(floor(chData.g * 256.0), 0.0));
    else if (all(lessThanEqual(fnCol.rgb, eps)))
        gl_FragColor = texelGet(palette, vec2(256.0, 1.0), vec2(floor(chData.b * 256.0), 0.0));
    else
        gl_FragColor = fnCol;
}
