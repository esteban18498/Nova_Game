using System;
using System.Runtime.InteropServices;
using NovaGame.Engine;
using NovaGame.Engine.Components;
using SDL2;

namespace NovaGame
{
    class Program
    {
        //static Player player;
        //static TextRenderer text;


        static bool running = true;

        static void Main()
        {
            if (NovaEngine.Init(1600, 900) == -1)
            {
                Console.WriteLine("Error initializing NovaEngine");
                return;
            }

            GameManager.Instance.Initialize();

            //player = new Player();
            // text = new TextRenderer(new Transform(0,0));

            // Main loop
            while (running)
            {   
                NovaEngine.Update();
                running = NovaEngine.ManageEvents();
                GameManager.Instance.Update();
                GameManager.Instance.Render();
                /*

                // Ejemplo: Comprobar si una tecla está presionada
                if (NovaEngine.IsKeyPressed(NovaEngine.KEY_W))
                {
                    Console.WriteLine("Tecla W presionada");
                    text.SetMessage("Tecla W presionada");
                }


                player.Update();

                // Clear Frame
                NovaEngine.Clear();
                
                // Render
                player.Render();
                text.Render();


                // Swap/Show Frame
                NovaEngine.Show();
                */
            }


            // Clean up
            //player.Clean();
            NovaEngine.Clean();

        }





    }
}