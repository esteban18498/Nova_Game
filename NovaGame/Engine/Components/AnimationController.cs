using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace NovaGame.Engine.Components
{
    public class AnimationController
    {
        private SpriteRenderer spriteRenderer;
        private List<uint> textureIDs = new();
        private int texturesIndex;


        private int frameCount;
        private float frameDuration;

        private float initTime;
        private float elapsedTime;

        public AnimationController(SpriteRenderer spriteRenderer, string AnimationsPath, int frameCount, float frameDuration)
        {
            this.spriteRenderer = spriteRenderer;
            this.frameCount = frameCount; 
            this.frameDuration = frameDuration;
            LoadSprites(AnimationsPath);

            texturesIndex = 0;

            initTime = Time.Now;
            elapsedTime = 0;
        }


        private void LoadSprites(string path)
        {
            bool error = false;
            // load surfaces
            List<nint> surfaces = new();
            for (int i = 0; i < frameCount; i++) {
                surfaces.Add(SDL_image.IMG_Load(path + $"/{i}.png"));
                if (surfaces[i] == IntPtr.Zero)
                {
                    Console.WriteLine("Failed to load texture: " + SDL.SDL_GetError());
                    error = true;
                }
            }

            if (error)
            {
                return;
            }

            for (int i = 0; i < frameCount ; i++)
            {
                SDL.SDL_Surface sdlSurface = Marshal.PtrToStructure<SDL.SDL_Surface>(surfaces[i]);
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
                NovaGL.glTexImage2D(NovaGL.GL_TEXTURE_2D, 0, (int)glFormat, spriteRenderer.Width, spriteRenderer.Height, 0,
                              glFormat, NovaGL.GL_UNSIGNED_BYTE, pixels);
                NovaGL.glGenerateMipmap(NovaGL.GL_TEXTURE_2D);

                textureIDs.Add(texture);

            }

            // Free surfaces
            for (int i = 0; i < frameCount; i++)
            {
                SDL.SDL_FreeSurface(surfaces[i]);
            }

        }

        public void Update()
        {
            elapsedTime += Time.DeltaTime;
            if (elapsedTime > frameDuration)
            {
                texturesIndex++;
                if (texturesIndex >= frameCount)
                {
                    texturesIndex= 0;
                }

                spriteRenderer.ModifiTextureID(textureIDs[texturesIndex]);

                elapsedTime = 0;
            }
        }


        public void Clean()
        {
            for (int i = 0; i < frameCount - 1; i++)
            {
                uint tex = textureIDs[i];
                NovaGL.glDeleteTextures(1, ref tex);
            }
        }
    }
}
