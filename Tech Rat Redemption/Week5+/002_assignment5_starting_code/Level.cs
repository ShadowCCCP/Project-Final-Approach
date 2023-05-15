using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class Level : GameObject
    {
        TiledLoader loader;
        string currentLevelName;

        // Use this to load level:
        // ((MyGame) game).LoadLevel("MainMenu.tmx");

        public Level(string filename)
        {
            currentLevelName = filename;
            loader = new TiledLoader(filename);
            CreateLevel();
        }

        private void Update()
        {
            RestartLevelCheck();
            SwitchNextLevel();
        }

        private void RestartLevelCheck()
        {
            if(Input.GetKeyDown(Key.R))
            {
                MyGame myGame = (MyGame)game;
                myGame.LoadLevel(currentLevelName);
            }
        }

        private void SwitchNextLevel()
        {
            int maxLevel = 12;

            // Go next
            if (Input.GetKeyDown(Key.RIGHT) || Input.GetKeyDown(Key.LEFT))
            {
                string cLevel = currentLevelName;
                if (cLevel.Length == 10)
                {
                    cLevel = cLevel.Substring(cLevel.IndexOf(".") - 1);
                }
                else
                {
                    cLevel = cLevel.Substring(cLevel.IndexOf(".") - 2);
                }
                cLevel = cLevel.Remove(cLevel.IndexOf("."));

                int currentLevel = Convert.ToInt32(cLevel);
                string loadLevel = "Level.tmx";

                if (Input.GetKeyDown(Key.RIGHT))
                {
                    if (currentLevel < maxLevel)
                    {
                        currentLevel++;
                    }
                    else
                    {
                        currentLevel = 1;
                    }
                    loadLevel = loadLevel.Insert(loadLevel.IndexOf("."), currentLevel.ToString());

                    MyGame myGame = (MyGame)game;
                    myGame.LoadLevel(loadLevel);
                }
                // Go back
                else if (Input.GetKeyDown(Key.LEFT))
                {
                    if (currentLevel > 1)
                    {
                        currentLevel--;
                    }
                    else
                    {
                        currentLevel = maxLevel;
                    }
                    loadLevel = loadLevel.Insert(loadLevel.IndexOf("."), currentLevel.ToString());

                    MyGame myGame = (MyGame)game;
                    myGame.LoadLevel(loadLevel);
                }

            }


        }

        /*private void SwitchNextLevel()
        {
            // Go next
            if(Input.GetKeyDown(Key.RIGHT) || Input.GetKeyDown(Key.LEFT))
            {
                int i = 1;
                string cLevel = currentLevelName;
                cLevel = cLevel.Substring(cLevel.IndexOf(".") - i);
                cLevel = cLevel.Remove(cLevel.IndexOf("."));

                int currentLevel = Convert.ToInt32(cLevel);
                string loadLevel = "Level.tmx";

                if (Input.GetKeyDown(Key.RIGHT))
                {
                    if(currentLevel < 20)
                    {
                        currentLevel++;
                        if (currentLevel > 10)
                        {
                            loadLevel = "Level1.tmx";
                            i = 2;
                        }
                    }
                    else
                    {
                        currentLevel = 1;
                    }
                    loadLevel = loadLevel.Insert(loadLevel.IndexOf("."), currentLevel.ToString());
                    Console.WriteLine(loadLevel);

                    MyGame myGame = (MyGame)game;
                    myGame.LoadLevel(loadLevel);
                    
                }
                // Go back
                else if (Input.GetKeyDown(Key.LEFT))
                {
                    if (currentLevel > 1)
                    {
                        currentLevel--;
                    }
                    else
                    {
                        currentLevel = 20;
                    }
                    loadLevel = loadLevel.Insert(loadLevel.IndexOf("."), currentLevel.ToString());

                    MyGame myGame = (MyGame)game;
                    myGame.LoadLevel(loadLevel);


                }
                
            }
            

        }*/

        private void CreateLevel(bool includeImageLayer = true)
        {
            try
            {
                loader.rootObject = this;
                loader.addColliders = false;
                loader.LoadImageLayers();
                loader.LoadTileLayers(0);
                loader.autoInstance = true;
                loader.LoadObjectGroups();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load level: " + e);
            }
        }


       
    }
}
