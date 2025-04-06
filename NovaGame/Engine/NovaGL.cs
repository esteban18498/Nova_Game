using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NovaGame.Engine.Shaders;

namespace NovaGame.Engine
{
    public static class NovaGL
    {
        // Constants
        public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
        public const uint GL_BLEND = 0x0BE2;
        public const uint GL_SRC_ALPHA = 0x0302;
        public const uint GL_ONE_MINUS_SRC_ALPHA = 0x0303;
        public const uint GL_TEXTURE_2D = 0x0DE1;
        public const uint GL_RGB = 0x1907;
        public const uint GL_RGBA = 0x1908;
        public const uint GL_UNSIGNED_BYTE = 0x1401;
        public const uint GL_TEXTURE_WRAP_S = 0x2802;
        public const uint GL_TEXTURE_WRAP_T = 0x2803;
        public const uint GL_REPEAT = 0x2901;
        public const uint GL_TEXTURE_MIN_FILTER = 0x2801;
        public const uint GL_TEXTURE_MAG_FILTER = 0x2800;
        public const uint GL_LINEAR = 0x2601;
        public const uint GL_VERTEX_SHADER = 0x8B31;
        public const uint GL_FRAGMENT_SHADER = 0x8B30;
        public const uint GL_COMPILE_STATUS = 0x8B81;
        public const uint GL_LINK_STATUS = 0x8B82;
        public const uint GL_ARRAY_BUFFER = 0x8892;
        public const uint GL_ELEMENT_ARRAY_BUFFER = 0x8893;
        public const uint GL_STATIC_DRAW = 0x88E4;
        public const uint GL_FLOAT = 0x1406;
        public const uint GL_TRIANGLES = 0x0004;
        public const uint GL_UNSIGNED_INT = 0x1405;
        public const uint GL_TEXTURE0 = 0x84C0;

#nullable disable

        // Delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glClearColorDelegate(float r, float g, float b, float a);
        private static glClearColorDelegate _glClearColor;
        public static void glClearColor(float r, float g, float b, float a) => _glClearColor(r, g, b, a);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glClearDelegate(uint mask);
        private static glClearDelegate _glClear;
        public static void glClear(uint mask) => _glClear(mask);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glEnableDelegate(uint cap);
        private static glEnableDelegate _glEnable;
        public static void glEnable(uint cap) => _glEnable(cap);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBlendFuncDelegate(uint sfactor, uint dfactor);
        private static glBlendFuncDelegate _glBlendFunc;
        public static void glBlendFunc(uint sfactor, uint dfactor) => _glBlendFunc(sfactor, dfactor);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenTexturesDelegate(int n, out uint textures);
        private static glGenTexturesDelegate _glGenTextures;
        public static void glGenTextures(int n, out uint textures) => _glGenTextures(n, out textures);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindTextureDelegate(uint target, uint texture);
        private static glBindTextureDelegate _glBindTexture;
        public static void glBindTexture(uint target, uint texture) => _glBindTexture(target, texture);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glTexParameteriDelegate(uint target, uint pname, int param);
        private static glTexParameteriDelegate _glTexParameteri;
        public static void glTexParameteri(uint target, uint pname, int param) => _glTexParameteri(target, pname, param);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glTexImage2DDelegate(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels);
        private static glTexImage2DDelegate _glTexImage2D;
        public static void glTexImage2D(uint target, int level, int internalformat, int width, int height, int border, uint format, uint type, IntPtr pixels) => _glTexImage2D(target, level, internalformat, width, height, border, format, type, pixels);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenerateMipmapDelegate(uint target);
        private static glGenerateMipmapDelegate _glGenerateMipmap;
        public static void glGenerateMipmap(uint target) => _glGenerateMipmap(target);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint glCreateShaderDelegate(uint shaderType);
        private static glCreateShaderDelegate _glCreateShader;
        public static uint glCreateShader(uint shaderType) => _glCreateShader(shaderType);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glShaderSourceDelegate(uint shader, int count, string[] source, int[] length);
        private static glShaderSourceDelegate _glShaderSource;
        public static void glShaderSource(uint shader, int count, string[] source, int[] length) => _glShaderSource(shader, count, source, length);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glCompileShaderDelegate(uint shader);
        private static glCompileShaderDelegate _glCompileShader;
        public static void glCompileShader(uint shader) => _glCompileShader(shader);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetShaderivDelegate(uint shader, uint pname, out int success);
        private static glGetShaderivDelegate _glGetShaderiv;
        public static void glGetShaderiv(uint shader, uint pname, out int success) => _glGetShaderiv(shader, pname, out success);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetShaderInfoLogDelegate(uint shader, int maxLength, out int length, string infoLog);
        private static glGetShaderInfoLogDelegate _glGetShaderInfoLog;
        public static void glGetShaderInfoLog(uint shader, int maxLength, out int length, string infoLog) => _glGetShaderInfoLog(shader, maxLength, out length, infoLog);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int glCreateProgramDelegate();
        private static glCreateProgramDelegate _glCreateProgram;
        public static int glCreateProgram() => _glCreateProgram();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glAttachShaderDelegate(int program, uint shader);
        private static glAttachShaderDelegate _glAttachShader;
        public static void glAttachShader(int program, uint shader) => _glAttachShader(program, shader);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glLinkProgramDelegate(int program);
        private static glLinkProgramDelegate _glLinkProgram;
        public static void glLinkProgram(int program) => _glLinkProgram(program);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetProgramivDelegate(int program, uint pname, out int success);
        private static glGetProgramivDelegate _glGetProgramiv;
        public static void glGetProgramiv(int program, uint pname, out int success) => _glGetProgramiv(program, pname, out success);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGetProgramInfoLogDelegate(int program, int maxLength, out int length, string infoLog);
        private static glGetProgramInfoLogDelegate _glGetProgramInfoLog;
        public static void glGetProgramInfoLog(int program, int maxLength, out int length, string infoLog) => _glGetProgramInfoLog(program, maxLength, out length, infoLog);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteShaderDelegate(uint shader);
        private static glDeleteShaderDelegate _glDeleteShader;
        public static void glDeleteShader(uint shader) => _glDeleteShader(shader);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUseProgramDelegate(int program);
        private static glUseProgramDelegate _glUseProgram;
        public static void glUseProgram(int program) => _glUseProgram(program);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenVertexArraysDelegate(int n, out uint arrays);
        private static glGenVertexArraysDelegate _glGenVertexArrays;
        public static void glGenVertexArrays(int n, out uint arrays) => _glGenVertexArrays(n, out arrays);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glGenBuffersDelegate(int n, out uint buffers);
        private static glGenBuffersDelegate _glGenBuffers;
        public static void glGenBuffers(int n, out uint buffers) => _glGenBuffers(n, out buffers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindVertexArrayDelegate(uint array);
        private static glBindVertexArrayDelegate _glBindVertexArray;
        public static void glBindVertexArray(uint array) => _glBindVertexArray(array);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBindBufferDelegate(uint target, uint buffer);
        private static glBindBufferDelegate _glBindBuffer;
        public static void glBindBuffer(uint target, uint buffer) => _glBindBuffer(target, buffer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBufferDataDelegate(uint target, int size, float[] data, uint usage);
        private static glBufferDataDelegate _glBufferData;
        public static void glBufferData(uint target, int size, float[] data, uint usage) => _glBufferData(target, size, data, usage);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glBufferDataIntDelegate(uint target, int size, uint[] data, uint usage);
        private static glBufferDataIntDelegate _glBufferDataInt;
        public static void glBufferData(uint target, int size, uint[] data, uint usage) => _glBufferDataInt(target, size, data, usage);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glVertexAttribPointerDelegate(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);
        private static glVertexAttribPointerDelegate _glVertexAttribPointer;
        public static void glVertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer) => _glVertexAttribPointer(index, size, type, normalized, stride, pointer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glEnableVertexAttribArrayDelegate(uint index);
        private static glEnableVertexAttribArrayDelegate _glEnableVertexAttribArray;
        public static void glEnableVertexAttribArray(uint index) => _glEnableVertexAttribArray(index);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDrawElementsDelegate(uint mode, int count, uint type, IntPtr indices);
        private static glDrawElementsDelegate _glDrawElements;
        public static void glDrawElements(uint mode, int count, uint type, IntPtr indices) => _glDrawElements(mode, count, type, indices);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteVertexArraysDelegate(int n, ref uint arrays);
        private static glDeleteVertexArraysDelegate _glDeleteVertexArrays;
        public static void glDeleteVertexArrays(int n, ref uint arrays) => _glDeleteVertexArrays(n, ref arrays);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteBuffersDelegate(int n, ref uint buffers);
        private static glDeleteBuffersDelegate _glDeleteBuffers;
        public static void glDeleteBuffers(int n, ref uint buffers) => _glDeleteBuffers(n, ref buffers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteTexturesDelegate(int n, ref uint textures);
        private static glDeleteTexturesDelegate _glDeleteTextures;
        public static void glDeleteTextures(int n, ref uint textures) => _glDeleteTextures(n, ref textures);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glDeleteProgramDelegate(int program);
        private static glDeleteProgramDelegate _glDeleteProgram;
        public static void glDeleteProgram(int program) => _glDeleteProgram(program);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glActiveTextureDelegate(uint texture);
        private static glActiveTextureDelegate _glActiveTexture;
        public static void glActiveTexture(uint texture) => _glActiveTexture(texture);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int glGetUniformLocationDelegate(int program, string name);
        private static glGetUniformLocationDelegate _glGetUniformLocation;
        public static int glGetUniformLocation(int program, string name) => _glGetUniformLocation(program, name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUniform1iDelegate(int location, int value);
        private static glUniform1iDelegate _glUniform1i;
        public static void glUniform1i(int location, int value) => _glUniform1i(location, value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUniform1fDelegate(int location, float value);
        private static glUniform1fDelegate _glUniform1f;
        public static void glUniform1f(int location, float value) => _glUniform1f(location, value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void glUniform2fvDelegate(int location, int count, float[] value);
        private static glUniform2fvDelegate _glUniform2fv;
        public static void glUniform2fv(int location, int count, float[] value) => _glUniform2fv(location, count, value);

#nullable restore

        public static void LoadFunctionPointers(Func<string, IntPtr> getProcAddress)
        {
            _glClearColor = Marshal.GetDelegateForFunctionPointer<glClearColorDelegate>(getProcAddress("glClearColor"));
            _glClear = Marshal.GetDelegateForFunctionPointer<glClearDelegate>(getProcAddress("glClear"));
            _glEnable = Marshal.GetDelegateForFunctionPointer<glEnableDelegate>(getProcAddress("glEnable"));
            _glBlendFunc = Marshal.GetDelegateForFunctionPointer<glBlendFuncDelegate>(getProcAddress("glBlendFunc"));
            _glGenTextures = Marshal.GetDelegateForFunctionPointer<glGenTexturesDelegate>(getProcAddress("glGenTextures"));
            _glBindTexture = Marshal.GetDelegateForFunctionPointer<glBindTextureDelegate>(getProcAddress("glBindTexture"));
            _glTexParameteri = Marshal.GetDelegateForFunctionPointer<glTexParameteriDelegate>(getProcAddress("glTexParameteri"));
            _glTexImage2D = Marshal.GetDelegateForFunctionPointer<glTexImage2DDelegate>(getProcAddress("glTexImage2D"));
            _glGenerateMipmap = Marshal.GetDelegateForFunctionPointer<glGenerateMipmapDelegate>(getProcAddress("glGenerateMipmap"));
            _glCreateShader = Marshal.GetDelegateForFunctionPointer<glCreateShaderDelegate>(getProcAddress("glCreateShader"));
            _glShaderSource = Marshal.GetDelegateForFunctionPointer<glShaderSourceDelegate>(getProcAddress("glShaderSource"));
            _glCompileShader = Marshal.GetDelegateForFunctionPointer<glCompileShaderDelegate>(getProcAddress("glCompileShader"));
            _glGetShaderiv = Marshal.GetDelegateForFunctionPointer<glGetShaderivDelegate>(getProcAddress("glGetShaderiv"));
            _glGetShaderInfoLog = Marshal.GetDelegateForFunctionPointer<glGetShaderInfoLogDelegate>(getProcAddress("glGetShaderInfoLog"));
            _glCreateProgram = Marshal.GetDelegateForFunctionPointer<glCreateProgramDelegate>(getProcAddress("glCreateProgram"));
            _glAttachShader = Marshal.GetDelegateForFunctionPointer<glAttachShaderDelegate>(getProcAddress("glAttachShader"));
            _glLinkProgram = Marshal.GetDelegateForFunctionPointer<glLinkProgramDelegate>(getProcAddress("glLinkProgram"));
            _glGetProgramiv = Marshal.GetDelegateForFunctionPointer<glGetProgramivDelegate>(getProcAddress("glGetProgramiv"));
            _glGetProgramInfoLog = Marshal.GetDelegateForFunctionPointer<glGetProgramInfoLogDelegate>(getProcAddress("glGetProgramInfoLog"));
            _glDeleteShader = Marshal.GetDelegateForFunctionPointer<glDeleteShaderDelegate>(getProcAddress("glDeleteShader"));
            _glUseProgram = Marshal.GetDelegateForFunctionPointer<glUseProgramDelegate>(getProcAddress("glUseProgram"));
            _glGenVertexArrays = Marshal.GetDelegateForFunctionPointer<glGenVertexArraysDelegate>(getProcAddress("glGenVertexArrays"));
            _glGenBuffers = Marshal.GetDelegateForFunctionPointer<glGenBuffersDelegate>(getProcAddress("glGenBuffers"));
            _glBindVertexArray = Marshal.GetDelegateForFunctionPointer<glBindVertexArrayDelegate>(getProcAddress("glBindVertexArray"));
            _glBindBuffer = Marshal.GetDelegateForFunctionPointer<glBindBufferDelegate>(getProcAddress("glBindBuffer"));
            _glBufferData = Marshal.GetDelegateForFunctionPointer<glBufferDataDelegate>(getProcAddress("glBufferData"));
            _glBufferDataInt = Marshal.GetDelegateForFunctionPointer<glBufferDataIntDelegate>(getProcAddress("glBufferData"));
            _glVertexAttribPointer = Marshal.GetDelegateForFunctionPointer<glVertexAttribPointerDelegate>(getProcAddress("glVertexAttribPointer"));
            _glEnableVertexAttribArray = Marshal.GetDelegateForFunctionPointer<glEnableVertexAttribArrayDelegate>(getProcAddress("glEnableVertexAttribArray"));
            _glDrawElements = Marshal.GetDelegateForFunctionPointer<glDrawElementsDelegate>(getProcAddress("glDrawElements"));
            _glDeleteVertexArrays = Marshal.GetDelegateForFunctionPointer<glDeleteVertexArraysDelegate>(getProcAddress("glDeleteVertexArrays"));
            _glDeleteBuffers = Marshal.GetDelegateForFunctionPointer<glDeleteBuffersDelegate>(getProcAddress("glDeleteBuffers"));
            _glDeleteTextures = Marshal.GetDelegateForFunctionPointer<glDeleteTexturesDelegate>(getProcAddress("glDeleteTextures"));
            _glDeleteProgram = Marshal.GetDelegateForFunctionPointer<glDeleteProgramDelegate>(getProcAddress("glDeleteProgram"));
            _glActiveTexture = Marshal.GetDelegateForFunctionPointer<glActiveTextureDelegate>(getProcAddress("glActiveTexture"));
            _glGetUniformLocation = Marshal.GetDelegateForFunctionPointer<glGetUniformLocationDelegate>(getProcAddress("glGetUniformLocation"));
            _glUniform1i = Marshal.GetDelegateForFunctionPointer<glUniform1iDelegate>(getProcAddress("glUniform1i"));
            _glUniform1f = Marshal.GetDelegateForFunctionPointer<glUniform1fDelegate>(getProcAddress("glUniform1f"));
            _glUniform2fv = Marshal.GetDelegateForFunctionPointer<glUniform2fvDelegate>(getProcAddress("glUniform2fv"));
        }


        //Shaders
#nullable disable
        private static SpriteShader _spriteShader;
        public static SpriteShader SpriteShader=>_spriteShader;
#nullable enable
        public static void CompileShaders()
        {
            _spriteShader=new SpriteShader();
        }

        public static void CleanShaders()
        {
            _spriteShader.DeleteShaderProgram();
        }
    }

}
