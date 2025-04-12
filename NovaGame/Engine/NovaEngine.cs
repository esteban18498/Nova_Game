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

        public static void Init(int width = 800, int height = 600)
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



        // Diccionario para rastrear el estado de las teclas
        private static Dictionary<SDL.SDL_Keycode, bool> _keyStates = new();

        public static bool IsKeyPressed(SDL.SDL_Keycode key)
        {
            return _keyStates.ContainsKey(key) && _keyStates[key];
        }

        public static void HandleInput(SDL.SDL_Event e)
        {
            switch (e.type)
            {
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    if (e.key.repeat == 0) // Ignorar repeticiones automáticas
                    _keyStates[e.key.keysym.sym] = true;
                    break;

                case SDL.SDL_EventType.SDL_KEYUP:
                    _keyStates[e.key.keysym.sym] = false;
                    break;
            }
        }

        public static bool ManageEvents()
        {
            while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
            {
                if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    return false;
                // Manejar entrada de teclado
                HandleInput(e);
            }
            return true;
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


        // Definiciones de teclas
        public static SDL.SDL_Keycode KEY_ESC = SDL.SDL_Keycode.SDLK_ESCAPE;
        public static SDL.SDL_Keycode KEY_ESP = SDL.SDL_Keycode.SDLK_SPACE;
        public static SDL.SDL_Keycode KEY_A = SDL.SDL_Keycode.SDLK_a;
        public static SDL.SDL_Keycode KEY_B = SDL.SDL_Keycode.SDLK_b;
        public static SDL.SDL_Keycode KEY_C = SDL.SDL_Keycode.SDLK_c;
        public static SDL.SDL_Keycode KEY_D = SDL.SDL_Keycode.SDLK_d;
        public static SDL.SDL_Keycode KEY_E = SDL.SDL_Keycode.SDLK_e;
        public static SDL.SDL_Keycode KEY_F = SDL.SDL_Keycode.SDLK_f;
        public static SDL.SDL_Keycode KEY_G = SDL.SDL_Keycode.SDLK_g;
        public static SDL.SDL_Keycode KEY_H = SDL.SDL_Keycode.SDLK_h;
        public static SDL.SDL_Keycode KEY_I = SDL.SDL_Keycode.SDLK_i;
        public static SDL.SDL_Keycode KEY_J = SDL.SDL_Keycode.SDLK_j;
        public static SDL.SDL_Keycode KEY_K = SDL.SDL_Keycode.SDLK_k;
        public static SDL.SDL_Keycode KEY_L = SDL.SDL_Keycode.SDLK_l;
        public static SDL.SDL_Keycode KEY_M = SDL.SDL_Keycode.SDLK_m;
        public static SDL.SDL_Keycode KEY_N = SDL.SDL_Keycode.SDLK_n;
        public static SDL.SDL_Keycode KEY_O = SDL.SDL_Keycode.SDLK_o;
        public static SDL.SDL_Keycode KEY_P = SDL.SDL_Keycode.SDLK_p;
        public static SDL.SDL_Keycode KEY_Q = SDL.SDL_Keycode.SDLK_q;
        public static SDL.SDL_Keycode KEY_R = SDL.SDL_Keycode.SDLK_r;
        public static SDL.SDL_Keycode KEY_S = SDL.SDL_Keycode.SDLK_s;
        public static SDL.SDL_Keycode KEY_T = SDL.SDL_Keycode.SDLK_t;
        public static SDL.SDL_Keycode KEY_U = SDL.SDL_Keycode.SDLK_u;
        public static SDL.SDL_Keycode KEY_V = SDL.SDL_Keycode.SDLK_v;
        public static SDL.SDL_Keycode KEY_W = SDL.SDL_Keycode.SDLK_w;
        public static SDL.SDL_Keycode KEY_X = SDL.SDL_Keycode.SDLK_x;
        public static SDL.SDL_Keycode KEY_Y = SDL.SDL_Keycode.SDLK_y;
        public static SDL.SDL_Keycode KEY_Z = SDL.SDL_Keycode.SDLK_z;
        public static SDL.SDL_Keycode KEY_1 = SDL.SDL_Keycode.SDLK_1;
        public static SDL.SDL_Keycode KEY_2 = SDL.SDL_Keycode.SDLK_2;
        public static SDL.SDL_Keycode KEY_3 = SDL.SDL_Keycode.SDLK_3;
        public static SDL.SDL_Keycode KEY_4 = SDL.SDL_Keycode.SDLK_4;
        public static SDL.SDL_Keycode KEY_5 = SDL.SDL_Keycode.SDLK_5;
        public static SDL.SDL_Keycode KEY_6 = SDL.SDL_Keycode.SDLK_6;
        public static SDL.SDL_Keycode KEY_7 = SDL.SDL_Keycode.SDLK_7;
        public static SDL.SDL_Keycode KEY_8 = SDL.SDL_Keycode.SDLK_8;
        public static SDL.SDL_Keycode KEY_9 = SDL.SDL_Keycode.SDLK_9;
        public static SDL.SDL_Keycode KEY_0 = SDL.SDL_Keycode.SDLK_0;
        public static SDL.SDL_Keycode KEY_UP = SDL.SDL_Keycode.SDLK_UP;
        public static SDL.SDL_Keycode KEY_DOWN = SDL.SDL_Keycode.SDLK_DOWN;
        public static SDL.SDL_Keycode KEY_RIGHT = SDL.SDL_Keycode.SDLK_RIGHT;
        public static SDL.SDL_Keycode KEY_LEFT = SDL.SDL_Keycode.SDLK_LEFT;

        /*
        public static SDL.SDL_Keycode MOUSE_LEFT = SDL.SDL_Keycode.SDL_BUTTON_LEFT;
        public static SDL.SDL_Keycode MOUSE_RIGHT = SDL.SDL_Keycode.SDL_BUTTON_RIGHT;

        public static SDL.SDL_Keycode MOUSE_WHEELDOWN = SDL.SDL_Keycode.SDL_BUTTON_WHEELDOWN;
        public static SDL.SDL_Keycode MOUSE_WHEELUP = SDL.SDL_Keycode.SDL_BUTTON_WHEELUP;
                
        public static SDL.SDL_Keycode MOUSE_MIDDLE = SDL.SDL_Keycode.SDL_BUTTON_MIDDLE;
        */
    }
}
