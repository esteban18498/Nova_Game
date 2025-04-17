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
        static TextRenderer text;
        static bool running = true;

        static void Main()
        {
            if (NovaEngine.Init(1600, 900) == -1)
            {
                Console.WriteLine("Error initializing NovaEngine");
                return;
            }

            player = new Player();
            text = new TextRenderer("Hello, World!", new Transform(0,0), new SDL.SDL_Color { r=255, g=255, b=255, a=255 });

            // Main loop
            while (running)
            {   
                running = NovaEngine.ManageEvents();

                NovaEngine.Update();

                // Ejemplo: Comprobar si una tecla está presionada
                if (NovaEngine.IsKeyPressed(NovaEngine.KEY_W))
                {
                    Console.WriteLine("Tecla W presionada");
                }


                player.Update();

                // Clear Frame
                NovaEngine.Clear();
                
                // Render
                player.Render();
                text.Render();


                // Swap/Show Frame
                NovaEngine.Show();
            }


            // Clean up
            player.Clean();
            NovaEngine.Clean();

        }





    }
}