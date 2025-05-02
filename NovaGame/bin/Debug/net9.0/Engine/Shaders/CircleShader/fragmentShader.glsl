#version 330 core

in vec2 TexCoord;                        // Texture coordinates from the vertex shader
in vec2 FragPos;                         // Fragment position in screen space

uniform vec2 uCenter;                    // Center of the circle in screen space
uniform float uRadius;                   // Radius of the circle in pixels
uniform float uThickness;
uniform vec4 uColor;                     // Color of the circle

out vec4 FragColor;                      // Final color output

void main()
{
    // Calculate the distance from the fragment to the center of the circle
    float distance = length(FragPos - uCenter);

    // If the distance is less than or equal to the radius, render the circle
    if (distance <= uRadius && distance >= uRadius-uThickness)
    {
        FragColor = uColor;
    }
    else
    {
        discard;  // Discard fragments outside the circle
    }
}