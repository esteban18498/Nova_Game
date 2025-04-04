using System;
using System.Runtime.InteropServices;
using NovaGame.Engine;
using SDL2;

namespace NovaGame
{
    class Program
    {
        static SpriteRenderer sprite;

        static void Main()
        {
            NovaEngine.Init();

            sprite = new SpriteRenderer("assets/player.png");
            



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
                sprite.Render();
                

                // Swap/Show Frame
                NovaEngine.Show();
            }

            sprite.Clean();

            NovaEngine.Clean();

        }





    }
}