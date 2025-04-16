using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace NovaGame.Engine.Components
{
    public class AnimationController
    {
        private Transform transform;
        private SpriteRenderer spriteRenderer;
        private List<uint> sprites = new();

        public AnimationController(Transform transform, SpriteRenderer spriteRenderer, string AnimationsPath, int lenght)
        {
            this.transform = transform;
            this.spriteRenderer = spriteRenderer;

            LoadSprites(AnimationsPath, lenght);


        }


        private void LoadSprites(string path, int lenght)
        {
            List<nint> surfaces = new();
            for (int i = 0; i < lenght; i++) {
                surfaces.Add(SDL_image.IMG_Load(path + $"/{i}"));
            
            
            }
            Console.WriteLine(surfaces.ToString());
        }

    }
}
