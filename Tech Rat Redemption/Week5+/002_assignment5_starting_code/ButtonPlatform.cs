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
        bool soundPlayed;
        public ButtonPlatform (string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base("ButtonS.png", 4, 1) 
        {

        }
            
        


        void Update()
        {
            myGame = (MyGame)game;
            if (myGame.ButtonPressed)
            {
                animFinished = false;
                if (!soundPlayed)
                {
                    myGame.soundCollection.PlaySound(2);
                    soundPlayed = true;
                }
            }

            if (!animFinished && myGame.ButtonPressed)
            {
                counter++;

                if (counter > 5) // animation
                {
                    counter = 0;
                    frame++;
                    if (frame == 5)
                    {
                        animFinished = true;
                        myGame.ButtonPressed = false;
                        soundPlayed = false;
                        frame =0;
                    }
                }
            }
            SetFrame(frame);

        }







     
    }
}
