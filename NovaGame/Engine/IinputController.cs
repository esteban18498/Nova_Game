using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Components;

namespace NovaGame.Engine
{
    public interface IinputController
    {
        public float GetRotationAngle();
        public bool IsFowardPress();
        public bool IsShotPress();

    }

    public class MouseInputController : IinputController
    {
        private Transform referenceTransform;
        public MouseInputController(Transform reference) {
            referenceTransform = reference;
        }

        public bool IsFowardPress() {
            
            return NovaEngine.IsMouseButtonPressed(NovaEngine.MouseButton.LEFT); 
        }

        public bool IsShotPress() {
            return NovaEngine.IsMouseButtonPressed(NovaEngine.MouseButton.RIGHT);
        }

        public float GetRotationAngle() {
            Vector2 mousePos = NovaEngine.GetMousePosition();
            mousePos = NovaEngine.ScreenToWorld(mousePos);
            Vector2 direction = mousePos - referenceTransform.Position;
            return MathF.Atan2(direction.Y, direction.X) - MathF.PI / 2;
        }
    }
}
