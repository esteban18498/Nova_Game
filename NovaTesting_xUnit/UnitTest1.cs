using System.Runtime.InteropServices;
using System.Xml.Linq;
using NovaGame;
using NovaGame.Engine;
using SDL2;
using System.Numerics;

namespace NovaTesting_xUnit
{
    public class UnitTest1
    {
        [Fact]
        public void AnglesNormalization()
        {
           
            Assert.Equal(0 , NovaMath.NormalizeAngle(MathF.PI*2) );
            Assert.Equal(MathF.PI , NovaMath.NormalizeAngle(MathF.PI ));
            
                                                                           //  |--> equal Assert Presicion for floats e10-6
            Assert.Equal(MathF.PI*1f , NovaMath.NormalizeAngle(MathF.PI * 3f), 6);

        }


        [Fact]
        public void NovaGLTest()
        {
            NovaEngine.Init();

            int version = NovaGL.CheckOpenGLVersion();
            Assert.True(version >= 303, "OpenGL version is less than 3.0");
        }


        [Fact]
        public void TestPlayer()
        {
            //NovaEngine.Init();

            Scene scene = new ();

            Player player = new (scene);

            Assert.NotNull(player);
            Assert.Equal(new Vector2(0,0), player.Transform.Position);
            Assert.Equal(100, player.Health);
        }

    }
}
