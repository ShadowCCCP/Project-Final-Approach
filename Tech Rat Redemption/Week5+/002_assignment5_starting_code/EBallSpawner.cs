using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class EBallSpawner : Square
    {
        bool doOnce;

        int spawnSide;
        int goalSide;
        float bounciness;
        float gravity;
        string nextLevel;
        float velocityX;
        float velocityY;

        public EBallSpawner(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows)
        {
            if (obj != null)
            {
                spawnSide = obj.GetIntProperty("spawnSide", 1);
                goalSide = obj.GetIntProperty("goalSide", 1);
                bounciness = obj.GetFloatProperty("bounciness", 0.3f);
                gravity = obj.GetFloatProperty("gravity", 0);
                nextLevel = obj.GetStringProperty("nextLevel", "Level1.tmx");
                velocityX = obj.GetFloatProperty("velocityX", 0);
                velocityY = obj.GetFloatProperty("velocityY", 0);
            }
        }

        void Update()
        {
            if(!doOnce)
            {
                Vec2 position = new Vec2(0, 0);

                switch(spawnSide)
                {
                    case 1:
                        {
                            position = new Vec2(x, y - height);
                            break;
                        }
                    case 2:
                        {
                            position = new Vec2(x + width, y);
                            break;
                        }
                    case 3:
                        {
                            position = new Vec2(x, y + height);
                            break;
                        }
                    case 4:
                        {
                            position = new Vec2(x - width, y);
                            break;
                        }
                }

                soundRandomizer();
                EnergyBall eBall = new EnergyBall(position, goalSide, bounciness, gravity, nextLevel, velocityX, velocityY);
                game.AddChild(eBall);
                doOnce = true;
            }
        }

        void soundRandomizer()
        {
            int i = Utils.Random(0, 2);
            myGame.soundCollection.PlaySound(5 + i);
        }
    }
}
