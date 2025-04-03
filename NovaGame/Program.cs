using System;
using System.Runtime.InteropServices;
using SDL2;


namespace NovaGame
{
    class Program
    {
        static IntPtr window;
        static IntPtr glContext;

        static void Main()
        {
            // Inicializar SDL con OpenGL
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
            {
                Console.WriteLine("Error al inicializar SDL: " + SDL.SDL_GetError());
                return;
            }
            

            // Configurar atributos de OpenGL
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 3);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);

            // Crear ventana con OpenGL
            window = SDL.SDL_CreateWindow("NovaGame", 100, 100, 800, 600,
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            if (window == IntPtr.Zero)
            {
                Console.WriteLine("Error al crear ventana: " + SDL.SDL_GetError());
                SDL.SDL_Quit();
                return;
            }

            // Crear contexto OpenGL
            glContext = SDL.SDL_GL_CreateContext(window);
            SDL.SDL_GL_MakeCurrent(window, glContext);
            SDL.SDL_GL_SetSwapInterval(1); // Activar V-Sync

            // Cargar funciones de OpenGL manualmente
            LoadOpenGLFunctions();

            // Configurar OpenGL
            GL.glClearColor(0.1f, 0.2f, 0.3f, 1.0f);



            var image = SDL_image.IMG_Load("assets/player.png");

            // Bucle principal
            bool running = true;
            while (running)
            {
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        running = false;
                }

                // Renderizar
                GL.glClear(GL.GL_COLOR_BUFFER_BIT);
                SDL.SDL_GL_SwapWindow(window);
            }

            // Liberar recursos
            SDL.SDL_GL_DeleteContext(glContext);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_Quit();
        }

        // Cargar funciones de OpenGL manualmente
        static void LoadOpenGLFunctions()
        {
            GL.LoadFunctionPointers(name => SDL.SDL_GL_GetProcAddress(name));
        }
    }

    // Definir funciones OpenGL manualmente
    static class GL
    {
        public const uint GL_COLOR_BUFFER_BIT = 0x00004000;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glClearColorDelegate(float r, float g, float b, float a);
        private static glClearColorDelegate _glClearColor;
        public static void glClearColor(float r, float g, float b, float a) => _glClearColor(r, g, b, a);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glClearDelegate(uint mask);
        private static glClearDelegate _glClear;
        public static void glClear(uint mask) => _glClear(mask);

        public static void LoadFunctionPointers(Func<string, IntPtr> getProcAddress)
        {
            _glClearColor = Marshal.GetDelegateForFunctionPointer<glClearColorDelegate>(getProcAddress("glClearColor"));
            _glClear = Marshal.GetDelegateForFunctionPointer<glClearDelegate>(getProcAddress("glClear"));
        }
    }
}
