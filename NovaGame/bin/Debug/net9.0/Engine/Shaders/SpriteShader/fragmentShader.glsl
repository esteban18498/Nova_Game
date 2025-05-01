#version 330 core

in vec2 TexCoord;                        // Texture coordinates from the vertex shader

uniform sampler2D uTexture;              // Texture sampler

out vec4 FragColor;                      // Final color output

void main()
{
    FragColor = texture(uTexture, TexCoord);  // Sample the texture at the given coordinates
}