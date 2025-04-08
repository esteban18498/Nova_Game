using System;
using System.Runtime.InteropServices;
using NovaGame.Engine;
using NovaGame.Engine.Components;
using SDL2;

namespace NovaGame
{
    class Program
    {
        static SpriteRenderer sprite;
        static Transform transform;

        static void Main()
        {
            NovaEngine.Init(1600,900);

            
            transform = new Transform(0.5f, 0.5f);
            sprite = new SpriteRenderer("assets/player.png", transform);
            



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

                transform.Rotate(1*Time.DeltaTime);
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