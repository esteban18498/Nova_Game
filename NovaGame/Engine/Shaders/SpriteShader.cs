using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaGame.Engine.Shaders
{
    public  class SpriteShader
    {
        public int shaderProgram;

        public SpriteShader() {
            shaderProgram = CreateShaderProgram();
        }

        int CreateShaderProgram()
        {
            // Vertex shader source
            string vertexShaderSource =
                "#version 330 core\n" +
                "layout (location = 0) in vec2 aPos;\n" +
                "layout (location = 1) in vec2 aTexCoord;\n" +
                "out vec2 TexCoord;\n" +
                "void main()\n" +
                "{\n" +
                "   gl_Position = vec4(aPos, 0.0, 1.0);\n" +
                "   TexCoord = aTexCoord;\n" +
                "}\0";

            // Fragment shader source
            string fragmentShaderSource =
                "#version 330 core\n" +
                "in vec2 TexCoord;\n" +
                "out vec4 FragColor;\n" +
                "uniform sampler2D texture1;\n" +
                "void main()\n" +
                "{\n" +
                "   FragColor = texture(texture1, TexCoord);\n" +
                "}\0";

            // Compile vertex shader
            uint vertexShader = NovaGL.glCreateShader(NovaGL.GL_VERTEX_SHADER);
            NovaGL.glShaderSource(vertexShader, 1, new string[] { vertexShaderSource }, null);
            NovaGL.glCompileShader(vertexShader);

            // Check for compilation errors
            int success;
            NovaGL.glGetShaderiv(vertexShader, NovaGL.GL_COMPILE_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                NovaGL.glGetShaderInfoLog(vertexShader, 512, out _, infoLog);
                Console.WriteLine("Vertex shader compilation failed: " + infoLog);
            }

            // Compile fragment shader
            uint fragmentShader = NovaGL.glCreateShader(NovaGL.GL_FRAGMENT_SHADER);
            NovaGL.glShaderSource(fragmentShader, 1, new string[] { fragmentShaderSource }, null);
            NovaGL.glCompileShader(fragmentShader);

            // Check for compilation errors
            NovaGL.glGetShaderiv(fragmentShader, NovaGL.GL_COMPILE_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                NovaGL.glGetShaderInfoLog(fragmentShader, 512, out _, infoLog);
                Console.WriteLine("Fragment shader compilation failed: " + infoLog);
            }

            // Link shaders
            int shaderProgram = NovaGL.glCreateProgram();
            NovaGL.glAttachShader(shaderProgram, vertexShader);
            NovaGL.glAttachShader(shaderProgram, fragmentShader);
            NovaGL.glLinkProgram(shaderProgram);

            // Check for linking errors
            NovaGL.glGetProgramiv(shaderProgram, NovaGL.GL_LINK_STATUS, out success);
            if (success == 0)
            {
                string infoLog = new string('\0', 512);
                NovaGL.glGetProgramInfoLog(shaderProgram, 512, out _, infoLog);
                Console.WriteLine("Shader program linking failed: " + infoLog);
            }

            // Clean up shaders
            NovaGL.glDeleteShader(vertexShader);
            NovaGL.glDeleteShader(fragmentShader);

            return shaderProgram;
        }

        public void DeleteShaderProgram()
        {
            NovaGL.glDeleteProgram(shaderProgram);
        }

    }
}
