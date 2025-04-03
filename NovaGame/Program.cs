using System;
using System.Runtime.InteropServices;
using NovaGame.Engine;
using SDL2;

namespace NovaGame
{
    class Program
    {
        static uint textureID;
        static int shaderProgram;

        static uint VAO, VBO, EBO;
        static float quadWidth, quadHeight;

        static void Main()
        {
            NovaEngine.Init();

            
            // Load the image and create a texture
            textureID = LoadTexture("assets/player.png");


            // Create a simple shader program
            shaderProgram = CreateShaderProgram();



            // Main loop
            bool running = true;
            while (running)
            {
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        running = false;
                }

                // Clear Frame
                NovaEngine.Clear();


                // Update ?

                // Use ShaderProgram
                NovaGL.glUseProgram(shaderProgram);

                // Bind texture
                NovaGL.glActiveTexture(NovaGL.GL_TEXTURE0);
                NovaGL.glBindTexture(NovaGL.GL_TEXTURE_2D, textureID);
                NovaGL.glUniform1i(NovaGL.glGetUniformLocation(shaderProgram, "texture1"), 0);

                // Draw quad
                NovaGL.glBindVertexArray(VAO);
                NovaGL.glDrawElements(NovaGL.GL_TRIANGLES, 6, NovaGL.GL_UNSIGNED_INT, IntPtr.Zero);

                // Swap/Show Frame
                NovaEngine.Show();
            }

            // Cleanup
            NovaGL.glDeleteVertexArrays(1, ref VAO);
            NovaGL.glDeleteBuffers(1, ref VBO);
            NovaGL.glDeleteBuffers(1, ref EBO);
            NovaGL.glDeleteProgram(shaderProgram);
            NovaGL.glDeleteTextures(1, ref textureID);

            NovaEngine.Clean();

        }

        static uint LoadTexture(string path)
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
            int width = sdlSurface.w;
            int height = sdlSurface.h;

            SetupRenderQuad(width, height);

            IntPtr pixels = sdlSurface.pixels;
            SDL.SDL_PixelFormat surfaceFormat = Marshal.PtrToStructure<SDL.SDL_PixelFormat>(sdlSurface.format);
            uint format = surfaceFormat.format;


            // Determine OpenGL format
            uint glFormat = NovaGL.GL_RGB;
            if (format == 376840196)// SDL_PIXELFORMAT_RGBA32??
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
            NovaGL.glTexImage2D(NovaGL.GL_TEXTURE_2D, 0, (int)glFormat, width, height, 0,
                          glFormat, NovaGL.GL_UNSIGNED_BYTE, pixels);
            NovaGL.glGenerateMipmap(NovaGL.GL_TEXTURE_2D);

            // Free surface
            SDL.SDL_FreeSurface(surface);

            return texture;
        }

        static void SetupRenderQuad(int imageWidth, int imageHeight)
        {
            // Calculate aspect-correct quad dimensions
            float imageAspect = (float)imageWidth / imageHeight;
            quadWidth = 0.5f;  // Base size (covers half the screen width)
            quadHeight = quadWidth / imageAspect;

            float[] vertices = {
        // Positions        // Texture coords
         quadWidth,  quadHeight,  1.0f, 0.0f,  // top right
         quadWidth, -quadHeight,  1.0f, 1.0f,  // bottom right
        -quadWidth, -quadHeight,  0.0f, 1.0f,  // bottom left
        -quadWidth,  quadHeight,  0.0f, 0.0f   // top left
    };

            Console.WriteLine("Quad W: " + quadWidth + "\nQuad H:" + quadHeight);

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

        static int CreateShaderProgram()
        {
            // Vertex shader source
            string vertexShaderSource =
                "#version 330 core\n" +
                "layout (location = 0) in vec2 aPos;\n" +
                "layout (location = 1) in vec2 aTexCoord;\n" +
                "out vec2 TexCoord;\n" +
                "void main()\n" +
                "{\n" +
                "   gl_Position = vec4(aPos, 0.0, 1.0);\n" +
                "   TexCoord = aTexCoord;\n" +
                "}\0";

            // Fragment shader source
            string fragmentShaderSource =
                "#version 330 core\n" +
                "in vec2 TexCoord;\n" +
                "out vec4 FragColor;\n" +
                "uniform sampler2D texture1;\n" +
                "void main()\n" +
                "{\n" +
                "   FragColor = texture(texture1, TexCoord);\n" +
                "}\0";

            // Compile vertex shader
            uint vertexShader = NovaGL.glCreateShader(NovaGL.GL_VERTEX_SHADER);
            NovaGL.glShaderSource(vertexShader, 1, new string[] { vertexShaderSource }, null);
            NovaGL.glCompileShader(vertexShader);

            // Check for compilation errors
            int success;
            NovaGL.glGetShaderiv(vertexShader, NovaGL.GL_COMPILE_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                NovaGL.glGetShaderInfoLog(vertexShader, 512, out _, infoLog);
                Console.WriteLine("Vertex shader compilation failed: " + infoLog);
            }

            // Compile fragment shader
            uint fragmentShader = NovaGL.glCreateShader(NovaGL.GL_FRAGMENT_SHADER);
            NovaGL.glShaderSource(fragmentShader, 1, new string[] { fragmentShaderSource }, null);
            NovaGL.glCompileShader(fragmentShader);

            // Check for compilation errors
            NovaGL.glGetShaderiv(fragmentShader, NovaGL.GL_COMPILE_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                NovaGL.glGetShaderInfoLog(fragmentShader, 512, out _, infoLog);
                Console.WriteLine("Fragment shader compilation failed: " + infoLog);
            }

            // Link shaders
            int shaderProgram = NovaGL.glCreateProgram();
            NovaGL.glAttachShader(shaderProgram, vertexShader);
            NovaGL.glAttachShader(shaderProgram, fragmentShader);
            NovaGL.glLinkProgram(shaderProgram);

            // Check for linking errors
            NovaGL.glGetProgramiv(shaderProgram, NovaGL.GL_LINK_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                NovaGL.glGetProgramInfoLog(shaderProgram, 512, out _, infoLog);
                Console.WriteLine("Shader program linking failed: " + infoLog);
            }

            // Clean up shaders
            NovaGL.glDeleteShader(vertexShader);
            NovaGL.glDeleteShader(fragmentShader);

            return shaderProgram;
        }


    }
}