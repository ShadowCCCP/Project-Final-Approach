using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class Player : Square
    {
        MyGame myGame;
        bool posFix;
        Vec2 position2;
        int counter = 0;
        int frame = 0;
        public Player(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows, obj)
        {
            myGame = (MyGame)game;

            isPlayer = true;

            myGame.AddSquare(this);
        }

        private void Update()
        {
            PositionFix();
            animation();
        }

        void animation()
        {
            counter++;

            if (counter > 10) // animation
            {
                 counter = 0;
                 frame++;
                 if (frame == 4)
                 { 
                      frame = 0;
                 }
            }
            
            SetFrame(frame);
        }

        protected void PositionFix()
        {
            if (!posFix)
            {
                position2.x = x;
                position2.y = y;

                CreateShootingTale();

                posFix = true;
            }
        }

        private void CreateShootingTale()
        {
            ShootingTale shootTale = new ShootingTale(position2);
            shootTale.SetXY(position2.x - width / 2 + 10, position2.y + height/4);
            myGame.AddChild(shootTale);
        }
    }
}
