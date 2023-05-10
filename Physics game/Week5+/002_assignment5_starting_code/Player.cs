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

        public Player(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows, obj)
        {
            myGame = (MyGame)game;

            myGame.AddSquare(this);
        }

        private void Update()
        {
            PositionFix();
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
            Console.WriteLine(position2);
            AddChild(shootTale);
        }
    }
}
