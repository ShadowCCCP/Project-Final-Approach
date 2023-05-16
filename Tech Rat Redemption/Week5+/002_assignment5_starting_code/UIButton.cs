using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class UIButton : Square
    {
        bool doOnce;
        public UIButton(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base("PlayButtonS.png", 2, 1)
        {

        }

        void Update()
        {
            int mouseX = Input.mouseX;
            int mouseY = Input.mouseY;
            if (mouseX > x - width/2 && mouseX < x + width/2 && mouseY > y - height/3 && mouseY < y +height/3) 
            {
                SetFrame(1);
                
                if(Input.GetMouseButton(0) && !doOnce)
                {
                    MyGame myGame = (MyGame)game;
                    myGame.LoadLevel("Level2.tmx");
                    doOnce = true;  
                }
            }
            else { SetFrame(0); }
        }
         
}

    }

