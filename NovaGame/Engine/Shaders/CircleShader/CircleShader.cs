using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaGame.Engine.Shaders
{
    public class CircleShader : IDisposable
    {
        private int _program;
        private int _positionLoc;
        private int _radLoc;
        private int _thicknessLoc;
        private int _colorLoc;
        private int _viewportSizeLoc;

        string vertexShaderPath = "Engine\\Shaders\\CircleShader\\vertexShader.glsl";
        string fragmentShaderPath = "Engine\\Shaders\\CircleShader\\fragmentShader.glsl";



        public CircleShader() {

            // Leer los shaders desde los archivos
            string vertexShaderSource = File.ReadAllText(vertexShaderPath);
            string fragmentShaderSource = File.ReadAllText(fragmentShaderPath);

            // Create and compile shaders
            uint vertexShader = NovaGL.glCreateShader(NovaGL.GL_VERTEX_SHADER);
            NovaGL.glShaderSource(vertexShader, 1, new[] { vertexShaderSource }, null);
            NovaGL.glCompileShader(vertexShader);

            uint fragmentShader = NovaGL.glCreateShader(NovaGL.GL_FRAGMENT_SHADER);
            NovaGL.glShaderSource(fragmentShader, 1, new[] { fragmentShaderSource }, null);
            NovaGL.glCompileShader(fragmentShader);

            // Create shader program
            _program = NovaGL.glCreateProgram();
            NovaGL.glAttachShader(_program, vertexShader);
            NovaGL.glAttachShader(_program, fragmentShader);
            NovaGL.glLinkProgram(_program);

            // Clean up shaders
            NovaGL.glDeleteShader(vertexShader);
            NovaGL.glDeleteShader(fragmentShader);

            // Get uniform locations
            _positionLoc = NovaGL.glGetUniformLocation(_program, "uCenter");
            _radLoc = NovaGL.glGetUniformLocation(_program, "uRadius");
            _thicknessLoc = NovaGL.glGetUniformLocation(_program, "uThickness");
            _viewportSizeLoc = NovaGL.glGetUniformLocation(_program, "uViewportSize");
            _colorLoc = NovaGL.glGetUniformLocation(_program, "uColor");
        }


        public void Use()
        {
            NovaGL.glUseProgram(_program);
        }

        public void SetPosition(float x, float y)
        {
            // You'll need to implement glUniform2f in NovaGL
            // For now using the float array approach:
            float[] pos = { x, y };
            NovaGL.glUniform2fv(_positionLoc, 1, pos);
        }

        public void SetRadius(float rad)
        {
            NovaGL.glUniform1f(_radLoc, rad);
        }
        public void SetThickness(float thickness)
        {
            NovaGL.glUniform1f(_thicknessLoc, thickness);
        }

        public void SetViewportSize(float width, float height)
        {
            float[] size = { width, height };
            NovaGL.glUniform2fv(_viewportSizeLoc, 1, size);
        }

        public void SetCircleColor(float r, float g, float b, float a)
        {
            float[] color = { r, g, b, a };
            NovaGL.glUniform4fv(_colorLoc, 1, color);
        }



        public void DeleteShaderProgram()
        {
            NovaGL.glDeleteProgram(_program);
        }


        public void Dispose()
        {
            DeleteShaderProgram();
        }
    }
}
