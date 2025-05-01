#version 330 core

layout (location = 0) in vec2 aPos;  // Local coordinates (typically -0.5 to 0.5 for a quad)
layout (location = 1) in vec2 aTexCoord;

uniform float uRotation;
uniform vec2 uPosition; // Position in pixels (0,0 = center of screen)
uniform vec2 uScale;     // Scale in pixels
uniform vec2 uViewportSize; // Viewport size [width, height] in pixels

out vec2 TexCoord;

void main()
{
    // Apply rotation
    float c = cos(uRotation);
    float s = sin(uRotation);
    vec2 rotatedPos = vec2(
        aPos.x * c - aPos.y * s,
        aPos.x * s + aPos.y * c
    ) * uScale;

    // Convert to pixel coordinates (0,0 at center, y-up)
    vec2 pixelPos = rotatedPos + uPosition;

    // Convert to OpenGL normalized coordinates (-1 to 1)
    vec2 normalizedPos;
    normalizedPos.x = (pixelPos.x / uViewportSize.x) * 2.0;
    normalizedPos.y = (pixelPos.y / uViewportSize.y) * 2.0;

    gl_Position = vec4(normalizedPos, 0, 1);
    TexCoord = aTexCoord;
}