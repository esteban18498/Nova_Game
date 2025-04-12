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
        static bool running = true;

        static void Main()
        {
            NovaEngine.Init(1600,900);

            player = new Player();



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
                

                // Swap/Show Frame
                NovaEngine.Show();
            }


            // Clean up
            player.Clean();
            NovaEngine.Clean();

        }





    }
}