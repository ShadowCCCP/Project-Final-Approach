using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class BouncyPlatform : Square
    {
        MyGame myGame;
        bool animFinished = true;
        int counter = 0;
        int frame = 0;
        public BouncyPlatform(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base("JumpyPlatformS.png", 5, 1)
        {
            
        }


        
        void Update()
        {
            myGame = (MyGame)game;
            if (myGame.BouncyPlatformAnim)
            {
                animFinished = false;
            }

            if (!animFinished && myGame.BouncyPlatformAnim)
            {
                counter++;

                if (counter > 5) // animation
                {
                    counter = 0;
                    frame++;
                    if (frame == 3)
                    {
                        animFinished = true;
                        myGame.BouncyPlatformAnim = false;
                        frame = 0;
                    }
                }
            }
            SetFrame(frame);
        }




      
    }
}
