using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using NovaGame.Engine.Components;
using NovaGame.Engine;

namespace NovaGame
{
    public enum gameStatus
    {
        menu, game, lose
    }

    public class GameManager
    {
        private static GameManager instance;
        private gameStatus gameStage = gameStatus.menu;    // 0-Menu     1-Game    2-Lose

        private SpriteRenderer mainMenuScreen; //= Engine.LoadImage("assets/MainMenu.png");
        private Transform screenTransform;

        private Transform titleTransform;
        private TextRenderer titleText;

        private Transform playTextTransform;
        private TextRenderer playText;

        private SurvivalScene survivalScene;

        private LoseScene loseScene;

        public const byte PLAYER_LAYER = 0b10000000;
        public const byte BULLET_LAYER = 0b01000000;
        public const byte ENEMY_LAYER = 0b00100000;
        //public static readonly byte enemyBulletLayer = 0b00010000;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public void Initialize()
        {
            //levelController = new LevelController();
            screenTransform = new Transform(0, 0);
            mainMenuScreen = new SpriteRenderer("assets/Screens/MainMenu.png", screenTransform);
            
            titleTransform = new Transform(0,NovaEngine.ScreenHeight/3);
            titleText = new TextRenderer(titleTransform, "NovaGame");

            playTextTransform = new Transform(0, NovaEngine.ScreenHeight / 4);
            playText = new TextRenderer(playTextTransform, "Press 'SPACE' to Play");

            survivalScene = new SurvivalScene();
            loseScene = new LoseScene();
            


        }
        public void Update()
        {
            switch (gameStage)
            {
                case gameStatus.menu:
                    if (NovaEngine.IsKeyPressed(NovaEngine.KEY_ESP))
                    {
                        gameStage = gameStatus.game;
                    }
                    break;
                case gameStatus.game:
                    survivalScene.Update();
                    break;
                case gameStatus.lose:
                    loseScene.Update();
                    break;
            }
        }

        public void Render()
        {
            NovaEngine.Clear();
            switch (gameStage)
            {
                case gameStatus.menu:

                    mainMenuScreen.Render();
                    titleText.Render();
                    playText.Render();

                    break;
                case gameStatus.game:

                    survivalScene.Render();

                    break;
                case gameStatus.lose:
                    loseScene.Render();

                    break;
            }

            NovaEngine.Show();
        }

        public void ChangeGameStatus(gameStatus status)
        {
            if (gameStage == gameStatus.lose && status == gameStatus.game)
            {
                survivalScene.Reset();
            }
            gameStage = status;
        }
    }




}
