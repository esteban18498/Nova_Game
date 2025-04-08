using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace NovaGame.Engine
{
    public class NovaEngine
    {
        static IntPtr window;
        static int _width, _height;
        public static int ScreenWidth => _width;
        public static int ScreeHeight => _height;

        static IntPtr glContext;

        public static void Init(int width=800,int height=600)
        {
            _width = width;
            _height = height;


            // Initialize Time
            Time.Init();

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
            window = SDL.SDL_CreateWindow("NovaGame", 100, 100, _width, _height,
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

            NovaGL.CheckOpenGLVersion();


            // Configure OpenGL
            NovaGL.glClearColor(0.1f, 0.2f, 0.3f, 1.0f);
            NovaGL.glEnable(NovaGL.GL_BLEND);
            NovaGL.glBlendFunc(NovaGL.GL_SRC_ALPHA, NovaGL.GL_ONE_MINUS_SRC_ALPHA);
            NovaGL.CompileShaders();

        }

        static void LoadOpenGLFunctions()
        {
            NovaGL.LoadFunctionPointers(name => SDL.SDL_GL_GetProcAddress(name));
        }


        public static void Clear()
        {
            NovaGL.glClear(NovaGL.GL_COLOR_BUFFER_BIT);
        }

        public static void Update()
        {
            Time.UpdateTime();
        }
        public static void Show()
        {
            SDL.SDL_GL_SwapWindow(window);
        }

        public static void Clean()
        {
            NovaGL.CleanShaders();
            SDL.SDL_GL_DeleteContext(glContext);
            SDL.SDL_DestroyWindow(window);
            SDL_image.IMG_Quit();
            SDL.SDL_Quit();
        }
    }
}
