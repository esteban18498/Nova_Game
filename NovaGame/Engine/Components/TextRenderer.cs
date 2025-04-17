using System;
using System.Numerics;
using System.Runtime.InteropServices;
using NovaGame.Engine;
using NovaGame.Engine.Components;
using NovaGame.Engine.Shaders;
using SDL2;
using static SDL2.SDL;

public class TextRenderer
{
    private Transform transform;
    private uint textureId;
    private int texWidth, texHeight;
    private int textWidth, textHeight;
    private SDL_Color color;
    private string message;
    private bool isLoaded = false;

    uint VAO, VBO, EBO;
    float quadWidth, quadHeight;

    public TextRenderer(string message, Transform transform, SDL_Color color)
    {
        this.message = message;
        this.transform = transform;
        this.color = color;
        LoadText();
    }

    private void LoadText()
    {
        IntPtr font = NovaEngine.NovaFont;
        if (font == IntPtr.Zero)
            throw new Exception("Font not initialized in NovaEngine");

        IntPtr surface = SDL_ttf.TTF_RenderUTF8_Blended(font, message, color);

        if (surface == IntPtr.Zero)
            throw new Exception("Failed to render text: " + SDL_ttf.TTF_GetError());

        SDL.SDL_Surface s = Marshal.PtrToStructure<SDL.SDL_Surface>(surface);

        texWidth = NextPowerOfTwo(s.w);
        texHeight = NextPowerOfTwo(s.h);
        textWidth = s.w;
        textHeight = s.h;

        IntPtr convertedSurface = SDL.SDL_CreateRGBSurface(0, texWidth, texHeight, 32,
            0x00FF0000, 0x0000FF00, 0x000000FF, 0xFF000000);

        SDL.SDL_BlitSurface(surface, IntPtr.Zero, convertedSurface, IntPtr.Zero);

        SDL.SDL_Surface cs = Marshal.PtrToStructure<SDL.SDL_Surface>(convertedSurface);

        NovaGL.glGenTextures(1, out textureId);
        NovaGL.glBindTexture(NovaGL.GL_TEXTURE_2D, textureId);
        NovaGL.glTexImage2D(NovaGL.GL_TEXTURE_2D, 0, 4, texWidth, texHeight, 0, NovaGL.GL_RGBA, NovaGL.GL_UNSIGNED_BYTE, cs.pixels);
        NovaGL.glTexParameteri(NovaGL.GL_TEXTURE_2D, NovaGL.GL_TEXTURE_MIN_FILTER, (int)NovaGL.GL_LINEAR);
        NovaGL.glTexParameteri(NovaGL.GL_TEXTURE_2D, NovaGL.GL_TEXTURE_MAG_FILTER, (int)NovaGL.GL_LINEAR);

        SDL.SDL_FreeSurface(surface);
        SDL.SDL_FreeSurface(convertedSurface);
        SetupRenderQuad(textWidth, textHeight);

        isLoaded = true;
    }

    public void Render()
    {
        if (!isLoaded) return;

        // Use ShaderProgram
        SpriteShader shader = NovaGL.SpriteShader;
        shader.Use();

        shader.Use();
        shader.SetRotation(transform.Rotation);
        shader.SetPosition(transform.Position.X, transform.Position.Y);
        shader.SetScale(1.0f, 1.0f); // Default scale
        shader.SetViewportSize(NovaEngine.ScreenWidth, NovaEngine.ScreeHeight);
        shader.SetTexture(0); // Texture unit 0

        NovaGL.glActiveTexture(NovaGL.GL_TEXTURE0);
        NovaGL.glBindTexture(NovaGL.GL_TEXTURE_2D, textureId);
        NovaGL.glBindVertexArray(VAO);
        NovaGL.glDrawElements(NovaGL.GL_TRIANGLES, 6, NovaGL.GL_UNSIGNED_INT, IntPtr.Zero);
    }

    void SetupRenderQuad(int imageWidth, int imageHeight)
    {
        // Calculate aspect-correct quad dimensions
        float imageAspect = (float)imageWidth / imageHeight;
        quadWidth = (float)imageWidth;
        quadHeight = (float)imageHeight;

        float[] vertices = {
        // Positions        // Texture coords
         quadWidth,  quadHeight,  1.0f, 0.0f,  // top right
         quadWidth, -quadHeight,  1.0f, 1.0f,  // bottom right
        -quadWidth, -quadHeight,  0.0f, 1.0f,  // bottom left
        -quadWidth,  quadHeight,  0.0f, 0.0f   // top left
            };


        uint[] indices = {
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
            };

        // Generate buffers
        NovaGL.glGenVertexArrays(1, out VAO);
        NovaGL.glGenBuffers(1, out VBO);
        NovaGL.glGenBuffers(1, out EBO);

        NovaGL.glBindVertexArray(VAO);

        NovaGL.glBindBuffer(NovaGL.GL_ARRAY_BUFFER, VBO);
        NovaGL.glBufferData(NovaGL.GL_ARRAY_BUFFER, vertices.Length * sizeof(float), vertices, NovaGL.GL_STATIC_DRAW);

        NovaGL.glBindBuffer(NovaGL.GL_ELEMENT_ARRAY_BUFFER, EBO);
        NovaGL.glBufferData(NovaGL.GL_ELEMENT_ARRAY_BUFFER, indices.Length * sizeof(uint), indices, NovaGL.GL_STATIC_DRAW);

        // Position attribute
        NovaGL.glVertexAttribPointer(0, 2, NovaGL.GL_FLOAT, false, 4 * sizeof(float), IntPtr.Zero);
        NovaGL.glEnableVertexAttribArray(0);

        // Texture coordinate attribute
        NovaGL.glVertexAttribPointer(1, 2, NovaGL.GL_FLOAT, false, 4 * sizeof(float), (IntPtr)(2 * sizeof(float)));
        NovaGL.glEnableVertexAttribArray(1);

        NovaGL.glBindBuffer(NovaGL.GL_ARRAY_BUFFER, 0);
        NovaGL.glBindVertexArray(0);
    }

    public void Clean()
    {
        if (isLoaded)
        {
            NovaGL.glDeleteTextures(1, ref textureId);
            isLoaded = false;
        }
    }

    private int NextPowerOfTwo(int x)
    {
        return (int)Math.Pow(2, Math.Ceiling(Math.Log(x) / Math.Log(2)));
    }
}
