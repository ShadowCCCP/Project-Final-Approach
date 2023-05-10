using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Core;
using TiledMapParser;

namespace GXPEngine
{
    class Gate : Square
    {
        int counter = 0;
        int frame = 1;

        bool animFinished = false;
        bool opening = true;

        MyGame myGame;
        public Gate(string filename = "", int cols =1 ,int rows =1, TiledObject obj = null) : base("gate.png",6,1)
        {
           
        }

        public void OpenGate()
        {
            opening = true;
        }

        void Update()
        {
            if (opening && !animFinished)
            {
                counter++;
                
                if (counter > 10 ) // animation
                {
                    counter = 0;
                    frame++;
                    if (frame == 6)
                    {
                        animFinished = true;

                        myGame = (MyGame)game;
                        myGame.RemoveSquare(this);
                    }
                }
            }
            SetFrame(frame);
            
        }









        
    }
}
