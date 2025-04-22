using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using static SDL2.SDL;
using NovaGame.Engine.Shaders;

namespace NovaGame.Engine.Components
{
    public class SpriteRenderer
    {
        private Transform transform;

        private uint textureID;

        public void ModifiTextureID(uint textureID)
        {
            this.textureID = textureID;
        }

        private int _width;
        private int _height;
        
        public int Width => _width;
        public int Height => _height;

        uint VAO, VBO, EBO;
        float quadWidth, quadHeight;


        public SpriteRenderer(string spritePath, Transform transform)
        {
            this.transform = transform;
            textureID = LoadTexture(spritePath);
        }


        public void Render()
        {
            //this.SetupRenderQuad(width, height);


            // Use ShaderProgram
            SpriteShader shader = NovaGL.SpriteShader;
            shader.Use();

            shader.Use();
            shader.SetRotation(transform.Rotation );
            shader.SetPosition(transform.Position.X, transform.Position.Y);
            shader.SetScale(1.0f, 1.0f); // Default scale
            shader.SetViewportSize(NovaEngine.ScreenWidth, NovaEngine.ScreenHeight);
            shader.SetTexture(0); // Texture unit 0

            NovaGL.glActiveTexture(NovaGL.GL_TEXTURE0);
            NovaGL.glBindTexture(NovaGL.GL_TEXTURE_2D, textureID);
            NovaGL.glBindVertexArray(VAO);
            NovaGL.glDrawElements(NovaGL.GL_TRIANGLES, 6, NovaGL.GL_UNSIGNED_INT, IntPtr.Zero);

        }

        public void Clean()
        {
            NovaGL.glDeleteVertexArrays(1, ref VAO);
            NovaGL.glDeleteBuffers(1, ref VBO);
            NovaGL.glDeleteBuffers(1, ref EBO);
            NovaGL.glDeleteTextures(1, ref textureID);
        }

        uint LoadTexture(string path)
        {
            // Load image
            var surface = SDL_image.IMG_Load(path);
            if (surface == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load texture: " + SDL.SDL_GetError());
                return 0;
            }

            // Get surface info
            SDL.SDL_Surface sdlSurface = Marshal.PtrToStructure<SDL.SDL_Surface>(surface);
            _width = sdlSurface.w;
            _height = sdlSurface.h;

            this.SetupRenderQuad(_width, _height);

            IntPtr pixels = sdlSurface.pixels;
            SDL.SDL_PixelFormat surfaceFormat = Marshal.PtrToStructure<SDL.SDL_PixelFormat>(sdlSurface.format);
            uint format = surfaceFormat.format;


            // Determine OpenGL format
            uint glFormat = NovaGL.GL_RGB;
            if (format == 376840196)// SDL_PIXELFORMAT_RGBA32?? cambiar por equivalente en EXA
                glFormat = NovaGL.GL_RGBA;
            else if (format == SDL.SDL_PIXELFORMAT_RGB24)
                glFormat = NovaGL.GL_RGB;

            // Create texture
            uint texture;
            NovaGL.glGenTextures(1, out texture);
            NovaGL.glBindTexture(NovaGL.GL_TEXTURE_2D, texture);

            // Set texture parameters
            NovaGL.glTexParameteri(NovaGL.GL_TEXTURE_2D, NovaGL.GL_TEXTURE_WRAP_S, (int)NovaGL.GL_REPEAT);
            NovaGL.glTexParameteri(NovaGL.GL_TEXTURE_2D, NovaGL.GL_TEXTURE_WRAP_T, (int)NovaGL.GL_REPEAT);
            NovaGL.glTexParameteri(NovaGL.GL_TEXTURE_2D, NovaGL.GL_TEXTURE_MIN_FILTER, (int)NovaGL.GL_LINEAR);
            NovaGL.glTexParameteri(NovaGL.GL_TEXTURE_2D, NovaGL.GL_TEXTURE_MAG_FILTER, (int)NovaGL.GL_LINEAR);

            // Upload texture data
            NovaGL.glTexImage2D(NovaGL.GL_TEXTURE_2D, 0, (int)glFormat, _width, _height, 0,
                          glFormat, NovaGL.GL_UNSIGNED_BYTE, pixels);
            NovaGL.glGenerateMipmap(NovaGL.GL_TEXTURE_2D);

            // Free surface
            SDL.SDL_FreeSurface(surface);

            return texture;
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

       
    }
}
