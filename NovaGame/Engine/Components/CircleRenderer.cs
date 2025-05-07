using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Shaders;

namespace NovaGame.Engine.Components
{
    public class CircleRenderer
    {
        private Transform transform;

        private float radius;
        public float Radius => radius;

        private float thickness;
        public float Thickness => thickness;

        private Vector4 color;
        public Vector4 Color => color;

        uint VAO, VBO, EBO;
        float quadWidth, quadHeight;

        public CircleRenderer(Transform transform, float radius, float thickness = 0 , Vector4? color = null)
        {
            this.transform = transform;
            this.radius = radius;
            this.thickness = thickness;
            if (color != null )
            {
                this.color = (Vector4)color;
            }
            else
            {
                this .color = new Vector4 (255,255,255,255);
            }
            SetupRenderQuad(radius);
        }


        public void Render()
        {
            // Use ShaderProgram
            CircleShader shader = NovaGL.CircleShader;
            shader.Use();

            shader.SetPosition(transform.Position.X, transform.Position.Y);
            shader.SetRadius(99- thickness);
            shader.SetThickness(thickness);
            shader.SetCircleColor(color.X, color.Z, color.Y, color.W);
            shader.SetViewportSize(NovaEngine.ScreenWidth, NovaEngine.ScreenHeight);



            NovaGL.glBindVertexArray(VAO);
            NovaGL.glDrawElements(NovaGL.GL_TRIANGLES, 6, NovaGL.GL_UNSIGNED_INT, IntPtr.Zero);

        }

        public void Clean()
        {
            NovaGL.glDeleteVertexArrays(1, ref VAO);
            NovaGL.glDeleteBuffers(1, ref VBO);
            NovaGL.glDeleteBuffers(1, ref EBO);
        }

        void SetupRenderQuad(float radius)
        {
            quadWidth = radius;
            quadHeight = radius;

            float[] vertices = {
        // Positions        // Texture coords
         quadWidth,  quadHeight,  1.0f, 0.0f,  // top right
         quadWidth, -quadHeight,  1.0f, 1.0f,  // bottom right
        -quadWidth, -quadHeight,  0.0f, 1.0f,  // bottom left
        -quadWidth,  quadHeight,  0.0f, 0.0f   // top left
            };
            uint[] indices = {
        0, 1, 3,   // first triangle
        1, 2, 3    // second triangle
            };

            // Create VAO, VBO, and EBO
            NovaGL.glGenVertexArrays(1, out VAO);
            NovaGL.glGenBuffers(1, out VBO);
            NovaGL.glGenBuffers(1, out EBO);
            // Bind VAO
            NovaGL.glBindVertexArray(VAO);
            // Bind and set VBO data
            NovaGL.glBindBuffer(NovaGL.GL_ARRAY_BUFFER, VBO);
            NovaGL.glBufferData(NovaGL.GL_ARRAY_BUFFER, vertices.Length * sizeof(float), vertices, NovaGL.GL_STATIC_DRAW);

            NovaGL.glBindBuffer(NovaGL.GL_ELEMENT_ARRAY_BUFFER, EBO);
            NovaGL.glBufferData(NovaGL.GL_ELEMENT_ARRAY_BUFFER, indices.Length * sizeof(uint), indices, NovaGL.GL_STATIC_DRAW);

            // Position attribute
            NovaGL.glVertexAttribPointer(0, 2, NovaGL.GL_FLOAT, false, 4 * sizeof(float), IntPtr.Zero);
            NovaGL.glEnableVertexAttribArray(0);

            // Texture coordinate attribute
            NovaGL.glVertexAttribPointer(1, 2, NovaGL.GL_FLOAT, false, 4 * sizeof(float), (IntPtr)(2 * sizeof(float)));
            NovaGL.glEnableVertexAttribArray(1);

            NovaGL.glBindBuffer(NovaGL.GL_ARRAY_BUFFER, 0);
            NovaGL.glBindVertexArray(0);
        }
    }


}
