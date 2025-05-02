#version 330 core

layout (location = 0) in vec2 aPos;        // Local coordinates (e.g., -0.5 to 0.5 for a quad)
layout (location = 1) in vec2 aTexCoord;  // Texture coordinates

uniform vec2 uViewportSize;               // Viewport size [width, height] in pixels

out vec2 TexCoord;                        // Pass texture coordinates to the fragment shader
out vec2 FragPos;                         // Pass fragment position to the fragment shader

void main()
{
    // Convert to OpenGL normalized coordinates (-1 to 1)
    vec2 normalizedPos;
    normalizedPos.x = (aPos.x / uViewportSize.x) * 2.0 ;  // -1 to 1 (left to right)
    normalizedPos.y = (aPos.y / uViewportSize.y) * 2.0 ;  // -1 to 1 (bottom to top)

    gl_Position = vec4(normalizedPos, 0.0, 1.0);  // Final position in clip space
    TexCoord = aTexCoord;                         // Pass texture coordinates
    FragPos = aPos;                               // Pass the position to the fragment shader
}