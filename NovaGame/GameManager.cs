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
        private SpriteRenderer loseScreen;// = Engine.LoadImage("assets/Lose.png");
        private Transform screenTransform;

        private Transform titleTransform;
        private TextRenderer titleText;

        private Transform playTextTransform;
        private TextRenderer playText;

        private SurvivalScene survivalScene;

        private CircleRenderer circleRenderer;


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
            loseScreen = new SpriteRenderer("assets/Screens/Lose.png", screenTransform);

            titleTransform = new Transform(0,NovaEngine.ScreenHeight/3);
            titleText = new TextRenderer(titleTransform, "NovaGame");

            playTextTransform = new Transform(0, NovaEngine.ScreenHeight / 4);
            playText = new TextRenderer(playTextTransform, "Press 'SPACE' to Play");

            survivalScene = new SurvivalScene();

            circleRenderer = new CircleRenderer(new Transform(0, 0), 50f, 2);

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
                    //logica de derrota
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
                    //Engine.Draw(mainMenu, 0, 0);

                    break;
                case gameStatus.game:

                    survivalScene.Render();

                    break;
                case gameStatus.lose:

                    //Engine.Draw(loseScreen, 0, 0);
                    loseScreen.Render();

                    break;
            }

            circleRenderer.Render();
            NovaEngine.Show();
        }

        public void ChangeGameStatus(gameStatus status)
        {
            gameStage = status;
        }
    }




}
