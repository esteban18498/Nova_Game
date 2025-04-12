using System;
using System.Runtime.InteropServices;
using NovaGame.Engine;
using NovaGame.Engine.Components;
using SDL2;

namespace NovaGame
{
    class Program
    {
        static Player player;

        static void Main()
        {
            NovaEngine.Init(1600,900);

            player = new Player();



            // Main loop
            bool running = true;
            while (running)
            {
                while (SDL.SDL_PollEvent(out SDL.SDL_Event e) != 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                        running = false;
                }


                NovaEngine.Update();
                player.Update();


                // Clear Frame
                NovaEngine.Clear();
                
                // Render
                player.Render();
                

                // Swap/Show Frame
                NovaEngine.Show();
            }


            // Clean up
            player.Clean();
            NovaEngine.Clean();

        }





    }
}