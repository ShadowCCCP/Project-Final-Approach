using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class EndScreen : AnimationSprite
    {
        public EndScreen(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base("sketchbackgroundENDS.png", 7, 2)
        {
            MyGame myGame = (MyGame)game;
            myGame.soundCollection.PlayNarration(33);
        }

        void Update()
        {
            SetCycle(0, 13);
            if (currentFrame < 12)
            {
                
                Animate(0.05f);
            }
            
        }
    }
}
