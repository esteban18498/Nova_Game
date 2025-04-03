using System;
using System.Runtime.InteropServices;
using SDL2;

namespace NovaGame
{
    class Program
    {
        static IntPtr window;
        static IntPtr glContext;
        static uint textureID;
        static int shaderProgram;

        static uint VAO, VBO, EBO;
        static float quadWidth, quadHeight;

        static void Main()
        {
            // Initialize SDL with OpenGL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Error initializing SDL: " + SDL.SDL_GetError());
                return;
            }

            // Initialize SDL_image
            if (SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0)
            {
                Console.WriteLine("Error initializing SDL_image: " + SDL.SDL_GetError());
                return;
            }

            // Configure OpenGL attributes
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);

            // Create window with OpenGL
            window = SDL.SDL_CreateWindow("NovaGame", 100, 100, 800, 600,
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            if (window == IntPtr.Zero)
            {
                Console.WriteLine("Error creating window: " + SDL.SDL_GetError());
                SDL.SDL_Quit();
                return;
            }

            // Create OpenGL context
            glContext = SDL.SDL_GL_CreateContext(window);
            SDL.SDL_GL_MakeCurrent(window, glContext);
            SDL.SDL_GL_SetSwapInterval(1); // Enable V-Sync

            // Load OpenGL functions
            LoadOpenGLFunctions();

            // Configure OpenGL
            GL.glClearColor(0.1f, 0.2f, 0.3f, 1.0f);
            GL.glEnable(GL.GL_BLEND);
            GL.glBlendFunc(GL.GL_SRC_ALPHA, GL.GL_ONE_MINUS_SRC_ALPHA);

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

                // Render
                GL.glClear(GL.GL_COLOR_BUFFER_BIT);

                // Use shader
                GL.glUseProgram(shaderProgram);

                // Bind texture
                GL.glActiveTexture(GL.GL_TEXTURE0);
                GL.glBindTexture(GL.GL_TEXTURE_2D, textureID);
                GL.glUniform1i(GL.glGetUniformLocation(shaderProgram, "texture1"), 0);

                // Draw quad
                GL.glBindVertexArray(VAO);
                GL.glDrawElements(GL.GL_TRIANGLES, 6, GL.GL_UNSIGNED_INT, IntPtr.Zero);

                SDL.SDL_GL_SwapWindow(window);
            }

            // Cleanup
            GL.glDeleteVertexArrays(1, ref VAO);
            GL.glDeleteBuffers(1, ref VBO);
            GL.glDeleteBuffers(1, ref EBO);
            GL.glDeleteProgram(shaderProgram);
            GL.glDeleteTextures(1, ref textureID);

            SDL.SDL_GL_DeleteContext(glContext);
            SDL.SDL_DestroyWindow(window);
            SDL_image.IMG_Quit();
            SDL.SDL_Quit();
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
            uint glFormat = GL.GL_RGB;
            if (format == 376840196)// SDL_PIXELFORMAT_RGBA32??
                glFormat = GL.GL_RGBA;
            else if (format == SDL.SDL_PIXELFORMAT_RGB24)
                glFormat = GL.GL_RGB;

            // Create texture
            uint texture;
            GL.glGenTextures(1, out texture);
            GL.glBindTexture(GL.GL_TEXTURE_2D, texture);

            // Set texture parameters
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_S, (int)GL.GL_REPEAT);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_WRAP_T, (int)GL.GL_REPEAT);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MIN_FILTER, (int)GL.GL_LINEAR);
            GL.glTexParameteri(GL.GL_TEXTURE_2D, GL.GL_TEXTURE_MAG_FILTER, (int)GL.GL_LINEAR);

            // Upload texture data
            GL.glTexImage2D(GL.GL_TEXTURE_2D, 0, (int)glFormat, width, height, 0,
                          glFormat, GL.GL_UNSIGNED_BYTE, pixels);
            GL.glGenerateMipmap(GL.GL_TEXTURE_2D);

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
            GL.glGenVertexArrays(1, out VAO);
            GL.glGenBuffers(1, out VBO);
            GL.glGenBuffers(1, out EBO);

            GL.glBindVertexArray(VAO);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, VBO);
            GL.glBufferData(GL.GL_ARRAY_BUFFER, vertices.Length * sizeof(float), vertices, GL.GL_STATIC_DRAW);

            GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, EBO);
            GL.glBufferData(GL.GL_ELEMENT_ARRAY_BUFFER, indices.Length * sizeof(uint), indices, GL.GL_STATIC_DRAW);

            // Position attribute
            GL.glVertexAttribPointer(0, 2, GL.GL_FLOAT, false, 4 * sizeof(float), IntPtr.Zero);
            GL.glEnableVertexAttribArray(0);

            // Texture coordinate attribute
            GL.glVertexAttribPointer(1, 2, GL.GL_FLOAT, false, 4 * sizeof(float), (IntPtr)(2 * sizeof(float)));
            GL.glEnableVertexAttribArray(1);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0);
            GL.glBindVertexArray(0);
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
            uint vertexShader = GL.glCreateShader(GL.GL_VERTEX_SHADER);
            GL.glShaderSource(vertexShader, 1, new string[] { vertexShaderSource }, null);
            GL.glCompileShader(vertexShader);

            // Check for compilation errors
            int success;
            GL.glGetShaderiv(vertexShader, GL.GL_COMPILE_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                GL.glGetShaderInfoLog(vertexShader, 512, out _, infoLog);
                Console.WriteLine("Vertex shader compilation failed: " + infoLog);
            }

            // Compile fragment shader
            uint fragmentShader = GL.glCreateShader(GL.GL_FRAGMENT_SHADER);
            GL.glShaderSource(fragmentShader, 1, new string[] { fragmentShaderSource }, null);
            GL.glCompileShader(fragmentShader);

            // Check for compilation errors
            GL.glGetShaderiv(fragmentShader, GL.GL_COMPILE_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                GL.glGetShaderInfoLog(fragmentShader, 512, out _, infoLog);
                Console.WriteLine("Fragment shader compilation failed: " + infoLog);
            }

            // Link shaders
            int shaderProgram = GL.glCreateProgram();
            GL.glAttachShader(shaderProgram, vertexShader);
            GL.glAttachShader(shaderProgram, fragmentShader);
            GL.glLinkProgram(shaderProgram);

            // Check for linking errors
            GL.glGetProgramiv(shaderProgram, GL.GL_LINK_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                GL.glGetProgramInfoLog(shaderProgram, 512, out _, infoLog);
                Console.WriteLine("Shader program linking failed: " + infoLog);
            }

            // Clean up shaders
            GL.glDeleteShader(vertexShader);
            GL.glDeleteShader(fragmentShader);

            return shaderProgram;
        }

        static void LoadOpenGLFunctions()
        {
            GL.LoadFunctionPointers(name => SDL.SDL_GL_GetProcAddress(name));
        }
    }

    static class GL
    {
        // Constants
        public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
        public const uint GL_BLEND = 0x0BE2;
        public const uint GL_SRC_ALPHA = 0x0302;
        public const uint GL_ONE_MINUS_SRC_ALPHA = 0x0303;
        public const uint GL_TEXTURE_2D = 0x0DE1;
        public const uint GL_RGB = 0x1907;
        public const uint GL_RGBA = 0x1908;
        public const uint GL_UNSIGNED_BYTE = 0x1401;
        public const uint GL_TEXTURE_WRAP_S = 0x2802;
        public const uint GL_TEXTURE_WRAP_T = 0x2803;
        public const uint GL_REPEAT = 0x2901;
        public const uint GL_TEXTURE_MIN_FILTER = 0x2801;
        public const uint GL_TEXTURE_MAG_FILTER = 0x2800;
        public const uint GL_LINEAR = 0x2601;
        public const uint GL_VERTEX_SHADER = 0x8B31;
        public const uint GL_FRAGMENT_SHADER = 0x8B30;
        public const uint GL_COMPILE_STATUS = 0x8B81;
        public const uint GL_LINK_STATUS = 0x8B82;
        public const uint GL_ARRAY_BUFFER = 0x8892;
        public const uint GL_ELEMENT_ARRAY_BUFFER = 0x8893;
        public const uint GL_STATIC_DRAW = 0x88E4;
        public const uint GL_FLOAT = 0x1406;
        public const uint GL_TRIANGLES = 0x0004;
        public const uint GL_UNSIGNED_INT = 0x1405;
        public const uint GL_TEXTURE0 = 0x84C0;

        // Delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glClearColorDelegate(float r, float g, float b, float a);
        private static glClearColorDelegate _glClearColor;
        public static void glClearColor(float r, float g, float b, float a) => _glClearColor(r, g, b, a);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glClearDelegate(uint mask);
        private static glClearDelegate _glClear;
        public static void glClear(uint mask) => _glClear(mask);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glEnableDelegate(uint cap);
        private static glEnableDelegate _glEnable;
        public static void glEnable(uint cap) => _glEnable(cap);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBlendFuncDelegate(uint sfactor, uint dfactor);
        private static glBlendFuncDelegate _glBlendFunc;
        public static void glBlendFunc(uint sfactor, uint dfactor) => _glBlendFunc(sfactor, dfactor);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenTexturesDelegate(int n, out uint textures);
        private static glGenTexturesDelegate _glGenTextures;
        public static void glGenTextures(int n, out uint textures) => _glGenTextures(n, out textures);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindTextureDelegate(uint target, uint texture);
        private static glBindTextureDelegate _glBindTexture;
        public static void glBindTexture(uint target, uint texture) => _glBindTexture(target, texture);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glTexParameteriDelegate(uint target, uint pname, int param);
        private static glTexParameteriDelegate _glTexParameteri;
        public static void glTexParameteri(uint target, uint pname, int param) => _glTexParameteri(target, pname, param);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glTexImage2DDelegate(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
        private static glTexImage2DDelegate _glTexImage2D;
        public static void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels) => _glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenerateMipmapDelegate(uint target);
        private static glGenerateMipmapDelegate _glGenerateMipmap;
        public static void glGenerateMipmap(uint target) => _glGenerateMipmap(target);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint glCreateShaderDelegate(uint shaderType);
        private static glCreateShaderDelegate _glCreateShader;
        public static uint glCreateShader(uint shaderType) => _glCreateShader(shaderType);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glShaderSourceDelegate(uint shader, int count, string[] source, int[] length);
        private static glShaderSourceDelegate _glShaderSource;
        public static void glShaderSource(uint shader, int count, string[] source, int[] length) => _glShaderSource(shader, count, source, length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glCompileShaderDelegate(uint shader);
        private static glCompileShaderDelegate _glCompileShader;
        public static void glCompileShader(uint shader) => _glCompileShader(shader);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetShaderivDelegate(uint shader, uint pname, out int success);
        private static glGetShaderivDelegate _glGetShaderiv;
        public static void glGetShaderiv(uint shader, uint pname, out int success) => _glGetShaderiv(shader, pname, out success);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetShaderInfoLogDelegate(uint shader, int maxLength, out int length, string infoLog);
        private static glGetShaderInfoLogDelegate _glGetShaderInfoLog;
        public static void glGetShaderInfoLog(uint shader, int maxLength, out int length, string infoLog) => _glGetShaderInfoLog(shader, maxLength, out length, infoLog);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int glCreateProgramDelegate();
        private static glCreateProgramDelegate _glCreateProgram;
        public static int glCreateProgram() => _glCreateProgram();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glAttachShaderDelegate(int program, uint shader);
        private static glAttachShaderDelegate _glAttachShader;
        public static void glAttachShader(int program, uint shader) => _glAttachShader(program, shader);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glLinkProgramDelegate(int program);
        private static glLinkProgramDelegate _glLinkProgram;
        public static void glLinkProgram(int program) => _glLinkProgram(program);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetProgramivDelegate(int program, uint pname, out int success);
        private static glGetProgramivDelegate _glGetProgramiv;
        public static void glGetProgramiv(int program, uint pname, out int success) => _glGetProgramiv(program, pname, out success);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetProgramInfoLogDelegate(int program, int maxLength, out int length, string infoLog);
        private static glGetProgramInfoLogDelegate _glGetProgramInfoLog;
        public static void glGetProgramInfoLog(int program, int maxLength, out int length, string infoLog) => _glGetProgramInfoLog(program, maxLength, out length, infoLog);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteShaderDelegate(uint shader);
        private static glDeleteShaderDelegate _glDeleteShader;
        public static void glDeleteShader(uint shader) => _glDeleteShader(shader);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUseProgramDelegate(int program);
        private static glUseProgramDelegate _glUseProgram;
        public static void glUseProgram(int program) => _glUseProgram(program);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenVertexArraysDelegate(int n, out uint arrays);
        private static glGenVertexArraysDelegate _glGenVertexArrays;
        public static void glGenVertexArrays(int n, out uint arrays) => _glGenVertexArrays(n, out arrays);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenBuffersDelegate(int n, out uint buffers);
        private static glGenBuffersDelegate _glGenBuffers;
        public static void glGenBuffers(int n, out uint buffers) => _glGenBuffers(n, out buffers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindVertexArrayDelegate(uint array);
        private static glBindVertexArrayDelegate _glBindVertexArray;
        public static void glBindVertexArray(uint array) => _glBindVertexArray(array);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindBufferDelegate(uint target, uint buffer);
        private static glBindBufferDelegate _glBindBuffer;
        public static void glBindBuffer(uint target, uint buffer) => _glBindBuffer(target, buffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBufferDataDelegate(uint target, int size, float[] data, uint usage);
        private static glBufferDataDelegate _glBufferData;
        public static void glBufferData(uint target, int size, float[] data, uint usage) => _glBufferData(target, size, data, usage);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBufferDataIntDelegate(uint target, int size, uint[] data, uint usage);
        private static glBufferDataIntDelegate _glBufferDataInt;
        public static void glBufferData(uint target, int size, uint[] data, uint usage) => _glBufferDataInt(target, size, data, usage);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glVertexAttribPointerDelegate(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);
        private static glVertexAttribPointerDelegate _glVertexAttribPointer;
        public static void glVertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer) => _glVertexAttribPointer(index, size, type, normalized, stride, pointer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glEnableVertexAttribArrayDelegate(uint index);
        private static glEnableVertexAttribArrayDelegate _glEnableVertexAttribArray;
        public static void glEnableVertexAttribArray(uint index) => _glEnableVertexAttribArray(index);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDrawElementsDelegate(uint mode, int count, uint type, IntPtr indices);
        private static glDrawElementsDelegate _glDrawElements;
        public static void glDrawElements(uint mode, int count, uint type, IntPtr indices) => _glDrawElements(mode, count, type, indices);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteVertexArraysDelegate(int n, ref uint arrays);
        private static glDeleteVertexArraysDelegate _glDeleteVertexArrays;
        public static void glDeleteVertexArrays(int n, ref uint arrays) => _glDeleteVertexArrays(n, ref arrays);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteBuffersDelegate(int n, ref uint buffers);
        private static glDeleteBuffersDelegate _glDeleteBuffers;
        public static void glDeleteBuffers(int n, ref uint buffers) => _glDeleteBuffers(n, ref buffers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteTexturesDelegate(int n, ref uint textures);
        private static glDeleteTexturesDelegate _glDeleteTextures;
        public static void glDeleteTextures(int n, ref uint textures) => _glDeleteTextures(n, ref textures);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteProgramDelegate(int program);
        private static glDeleteProgramDelegate _glDeleteProgram;
        public static void glDeleteProgram(int program) => _glDeleteProgram(program);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glActiveTextureDelegate(uint texture);
        private static glActiveTextureDelegate _glActiveTexture;
        public static void glActiveTexture(uint texture) => _glActiveTexture(texture);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int glGetUniformLocationDelegate(int program, string name);
        private static glGetUniformLocationDelegate _glGetUniformLocation;
        public static int glGetUniformLocation(int program, string name) => _glGetUniformLocation(program, name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUniform1iDelegate(int location, int value);
        private static glUniform1iDelegate _glUniform1i;
        public static void glUniform1i(int location, int value) => _glUniform1i(location, value);

        public static void LoadFunctionPointers(Func<string, IntPtr> getProcAddress)
        {
            _glClearColor = Marshal.GetDelegateForFunctionPointer<glClearColorDelegate>(getProcAddress("glClearColor"));
            _glClear = Marshal.GetDelegateForFunctionPointer<glClearDelegate>(getProcAddress("glClear"));
            _glEnable = Marshal.GetDelegateForFunctionPointer<glEnableDelegate>(getProcAddress("glEnable"));
            _glBlendFunc = Marshal.GetDelegateForFunctionPointer<glBlendFuncDelegate>(getProcAddress("glBlendFunc"));
            _glGenTextures = Marshal.GetDelegateForFunctionPointer<glGenTexturesDelegate>(getProcAddress("glGenTextures"));
            _glBindTexture = Marshal.GetDelegateForFunctionPointer<glBindTextureDelegate>(getProcAddress("glBindTexture"));
            _glTexParameteri = Marshal.GetDelegateForFunctionPointer<glTexParameteriDelegate>(getProcAddress("glTexParameteri"));
            _glTexImage2D = Marshal.GetDelegateForFunctionPointer<glTexImage2DDelegate>(getProcAddress("glTexImage2D"));
            _glGenerateMipmap = Marshal.GetDelegateForFunctionPointer<glGenerateMipmapDelegate>(getProcAddress("glGenerateMipmap"));
            _glCreateShader = Marshal.GetDelegateForFunctionPointer<glCreateShaderDelegate>(getProcAddress("glCreateShader"));
            _glShaderSource = Marshal.GetDelegateForFunctionPointer<glShaderSourceDelegate>(getProcAddress("glShaderSource"));
            _glCompileShader = Marshal.GetDelegateForFunctionPointer<glCompileShaderDelegate>(getProcAddress("glCompileShader"));
            _glGetShaderiv = Marshal.GetDelegateForFunctionPointer<glGetShaderivDelegate>(getProcAddress("glGetShaderiv"));
            _glGetShaderInfoLog = Marshal.GetDelegateForFunctionPointer<glGetShaderInfoLogDelegate>(getProcAddress("glGetShaderInfoLog"));
            _glCreateProgram = Marshal.GetDelegateForFunctionPointer<glCreateProgramDelegate>(getProcAddress("glCreateProgram"));
            _glAttachShader = Marshal.GetDelegateForFunctionPointer<glAttachShaderDelegate>(getProcAddress("glAttachShader"));
            _glLinkProgram = Marshal.GetDelegateForFunctionPointer<glLinkProgramDelegate>(getProcAddress("glLinkProgram"));
            _glGetProgramiv = Marshal.GetDelegateForFunctionPointer<glGetProgramivDelegate>(getProcAddress("glGetProgramiv"));
            _glGetProgramInfoLog = Marshal.GetDelegateForFunctionPointer<glGetProgramInfoLogDelegate>(getProcAddress("glGetProgramInfoLog"));
            _glDeleteShader = Marshal.GetDelegateForFunctionPointer<glDeleteShaderDelegate>(getProcAddress("glDeleteShader"));
            _glUseProgram = Marshal.GetDelegateForFunctionPointer<glUseProgramDelegate>(getProcAddress("glUseProgram"));
            _glGenVertexArrays = Marshal.GetDelegateForFunctionPointer<glGenVertexArraysDelegate>(getProcAddress("glGenVertexArrays"));
            _glGenBuffers = Marshal.GetDelegateForFunctionPointer<glGenBuffersDelegate>(getProcAddress("glGenBuffers"));
            _glBindVertexArray = Marshal.GetDelegateForFunctionPointer<glBindVertexArrayDelegate>(getProcAddress("glBindVertexArray"));
            _glBindBuffer = Marshal.GetDelegateForFunctionPointer<glBindBufferDelegate>(getProcAddress("glBindBuffer"));
            _glBufferData = Marshal.GetDelegateForFunctionPointer<glBufferDataDelegate>(getProcAddress("glBufferData"));
            _glBufferDataInt = Marshal.GetDelegateForFunctionPointer<glBufferDataIntDelegate>(getProcAddress("glBufferData"));
            _glVertexAttribPointer = Marshal.GetDelegateForFunctionPointer<glVertexAttribPointerDelegate>(getProcAddress("glVertexAttribPointer"));
            _glEnableVertexAttribArray = Marshal.GetDelegateForFunctionPointer<glEnableVertexAttribArrayDelegate>(getProcAddress("glEnableVertexAttribArray"));
            _glDrawElements = Marshal.GetDelegateForFunctionPointer<glDrawElementsDelegate>(getProcAddress("glDrawElements"));
            _glDeleteVertexArrays = Marshal.GetDelegateForFunctionPointer<glDeleteVertexArraysDelegate>(getProcAddress("glDeleteVertexArrays"));
            _glDeleteBuffers = Marshal.GetDelegateForFunctionPointer<glDeleteBuffersDelegate>(getProcAddress("glDeleteBuffers"));
            _glDeleteTextures = Marshal.GetDelegateForFunctionPointer<glDeleteTexturesDelegate>(getProcAddress("glDeleteTextures"));
            _glDeleteProgram = Marshal.GetDelegateForFunctionPointer<glDeleteProgramDelegate>(getProcAddress("glDeleteProgram"));
            _glActiveTexture = Marshal.GetDelegateForFunctionPointer<glActiveTextureDelegate>(getProcAddress("glActiveTexture"));
            _glGetUniformLocation = Marshal.GetDelegateForFunctionPointer<glGetUniformLocationDelegate>(getProcAddress("glGetUniformLocation"));
            _glUniform1i = Marshal.GetDelegateForFunctionPointer<glUniform1iDelegate>(getProcAddress("glUniform1i"));
        }
    }
}