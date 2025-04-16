using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NovaGame.Engine
{
    public class Time
    {
        private static float _deltaTime;
        private static DateTime _startTime;
        private static float _lastFrameTime;

        public static float DeltaTime=>_deltaTime;
        public static DateTime StartTime=>_startTime;
        public static float LastFrameTime=>_lastFrameTime;

        public static float Now => (float)(DateTime.Now - _startTime).TotalSeconds;

        private Time()
        {
            Init();
        }


        public static void Init()
        {
            _startTime = DateTime.Now;
        }
        public static void UpdateTime()
        {
            float currentTime = (float)(DateTime.Now - _startTime).TotalSeconds;
            _deltaTime = currentTime - _lastFrameTime;
            _lastFrameTime = currentTime;
        }

    }
}
