using NovaGame.Engine;
namespace NovaGame.Engine.Shaders
{


public class SpriteShader : IDisposable
{
    private int _program;
    private int _rotationLoc;
    private int _positionLoc;
    private int _scaleLoc;
    private int _textureLoc;

    string vertexShaderSource =
    "#version 330 core\n" +
    "layout (location = 0) in vec2 aPos;\n" +
    "layout (location = 1) in vec2 aTexCoord;\n" +
    "uniform float uRotation;\n" +
    "uniform vec2 uPosition;\n" +
    "uniform vec2 uScale;\n" +
    "out vec2 TexCoord;\n" +
    "void main()\n" +
    "{\n" +
    "   float c = cos(uRotation);\n" +
    "   float s = sin(uRotation);\n" +
    "   vec2 rotatedPos = vec2(\n" +
    "       aPos.x * c - aPos.y * s,\n" +
    "       aPos.x * s + aPos.y * c\n" +
    "   ) * uScale;\n" +
    "   gl_Position = vec4(rotatedPos + uPosition, 0.0, 1.0);\n" +
    "   TexCoord = aTexCoord;\n" +
    "}\0";

    string fragmentShaderSource =
    "#version 330 core\n" +
    "in vec2 TexCoord;\n" +
    "out vec4 FragColor;\n" +
    "uniform sampler2D uTexture;\n" +
    "void main()\n" +
    "{\n" +
    "   FragColor = texture(uTexture, TexCoord);\n" +
    "}\0";



    public SpriteShader()
    {
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
        _rotationLoc = NovaGL.glGetUniformLocation(_program, "uRotation");
        _positionLoc = NovaGL.glGetUniformLocation(_program, "uPosition");
        _scaleLoc = NovaGL.glGetUniformLocation(_program, "uScale");
        _textureLoc = NovaGL.glGetUniformLocation(_program, "uTexture");
    }

    public void Use()
    {
        NovaGL.glUseProgram(_program);
    }

    public void SetRotation(float radians)
    {
        NovaGL.glUniform1f(_rotationLoc, radians);
    }

    public void SetPosition(float x, float y)
    {
        // You'll need to implement glUniform2f in NovaGL
        // For now using the float array approach:
        float[] pos = { x, y };
        NovaGL.glUniform2fv(_positionLoc, 1, pos);
    }

    public void SetScale(float x, float y)
    {
        float[] scale = { x, y };
        NovaGL.glUniform2fv(_scaleLoc, 1, scale);
    }

    public void SetTexture(int textureUnit)
    {
        NovaGL.glUniform1i(_textureLoc, textureUnit);
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