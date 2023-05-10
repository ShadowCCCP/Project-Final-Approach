using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class Gate : Square
    {
        int counter = 0;
        int frame = 0;

        bool animFinished = true;

        MyGame myGame;
        public Gate(string filename = "", int cols =1 ,int rows =1, TiledObject obj = null) : base("gate.png",6,1)
        {
           
        }

        void Update()
        {
            myGame = (MyGame)game;
            if (myGame.ButtonPressed)
            {
                animFinished = false;
            }

            if (!animFinished && myGame.ButtonPressed)
            {
                counter++;
                
                if (counter > 10 ) // animation
                {
                    counter = 0;
                    frame++;
                    if (frame == 5)
                    {
                        animFinished = true;

                        myGame.RemoveSquare(this);
                    }
                }
            }
            SetFrame(frame);
            
        }









        
    }
}
