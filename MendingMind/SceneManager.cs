using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class SceneManager : GameObject
    {

        private static SceneManager _instance;

        MyGame myGame;
        
        public Level currentLevel { get; private set; }

        public String levelName;


        public static SceneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SceneManager();
                }
                return _instance;
            }
        }

        public SceneManager()
        {
            myGame = (MyGame)game;
            if (_instance != null)
            {
                Destroy();
                return;
            }
            _instance = this;
        }

        void Update()
        {
            
        }

        public void loadLevel(String LevelName)
        {
            myGame._paused = true;
            myGame.GameStarted = false;
            
            
            // remove previous scene:
            foreach (Ball mover in myGame._movers)
            {
                mover.Destroy();
            }
            myGame._movers.Clear();
            foreach (LineSegment line in myGame._lines)
            {
                line.Destroy();
            }
            myGame._lines.Clear();
            foreach (GameObject Child in GetChildren())
            {
                Child.Destroy();
            }
            
            foreach(SquareBlocker blocky in myGame._blockers)
            {
                blocky.Destroy();
            }
            



            switch (LevelName)
            {

                // BALL / BALL COLLISION SCENES:
                case "Level1": // one moving ball (medium speed), one fixed ball.
                    levelName = "Level1";
                    Level1 level1 = new Level1();
                    level1.pong.reset = false;
                    currentLevel = level1;
                    AddChild(level1);
                    break;


                case "WinScreen": 
                    WinScreen winScreen = new WinScreen();
                    AddChild(winScreen);
                    break;

                default: // one moving ball (low speed), one fixed ball.
                    Console.WriteLine($"{LevelName} is not supported");
                    break;
            }
            myGame._stepIndex = -1;
            
        }
    }
}

