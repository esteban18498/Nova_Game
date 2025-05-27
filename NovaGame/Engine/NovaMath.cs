using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaGame.Engine
{
    public static class NovaMath
    {
        public static float LerpAngle(float startAngle, float endAngle, float t)
        {
            // Normalize angles within 0 to 2PI 
            startAngle = NormalizeAngle(startAngle);
            endAngle = NormalizeAngle(endAngle);

            // Calculate difference
            float delta = endAngle - startAngle;

            // If difference greater than PI, take the shorter path
            if (delta > MathF.PI)
            {
                delta -= 2 * MathF.PI;
            }
            else if (delta < -MathF.PI)
            {
                delta += 2 * MathF.PI;
            }

            // interpolation
            float result = startAngle + delta * t;

            return NormalizeAngle(result);
        }

        public static float NormalizeAngle(float angle)
        {
            while (angle < 0) angle += 2 * MathF.PI;
            while (angle >= 2 * MathF.PI) angle -= 2 * MathF.PI;
            return angle;
        }
    }
}
