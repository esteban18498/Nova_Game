#version 330 core

in vec2 TexCoord;                        // Texture coordinates from the vertex shader
in vec2 FragPos;                         // Fragment position in screen space


uniform vec2 uViewportSize;  

uniform vec2 uCenter;                    // Center of the circle in screen space
uniform float uRadius;                   // Radius of the circle in pixels
uniform float uThickness;
uniform vec4 uColor;                     // Color of the circle

out vec4 FragColor;                      // Final color output

void main()
{
    // Calculate the distance from the fragment to the center of the circle
    vec4 color=uColor/255;
    
    vec2 reMap;
    reMap.x = abs( (2*TexCoord.x ) - 1) ;  // -1 to 1 (left to right)
    reMap.y = abs((2*TexCoord.y ) - 1 );  // -1 to 1 (bottom to top)


    vec4 reMapColor = vec4(reMap.x, reMap.y, 0, 255); // Default color
    float glow =1;
    
    float innerDistance=0;
    float alpha=0;

    float outerDistance = (length(reMap)-uRadius/100);
    if(uThickness!=0){
        innerDistance = abs(outerDistance) - uThickness/100;
        alpha =  innerDistance / (fwidth(outerDistance)*glow);
    }else{
        alpha = alpha =  outerDistance / (fwidth(outerDistance)*glow);
    }

    color.w*=1-(alpha+innerDistance );
    

    FragColor = color; // Set the fragment color
    
}