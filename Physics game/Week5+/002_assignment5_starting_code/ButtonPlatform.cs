using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
     class ButtonPlatform : Square
    {
        MyGame myGame;
        int counter= 0;
        int frame = 1;
        bool animFinished = true;
        public ButtonPlatform (string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base("ButtonS.png", 4, 1) 
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

                if (counter > 10) // animation
                {
                    counter = 0;
                    frame++;
                    if (frame == 5)
                    {
                        animFinished = true;

                        frame=0;
                    }
                }
            }
            SetFrame(frame);

        }







     
    }
}
