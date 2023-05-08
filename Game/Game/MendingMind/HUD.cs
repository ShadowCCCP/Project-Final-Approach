using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using GXPEngine;

public class HUD : GameObject
{


    EasyDraw _text;
    int score;

    MyGame myGame;



        public HUD()
        {

         myGame = (MyGame)game;

        _text = new EasyDraw(250, 25);
        _text.TextAlign(CenterMode.Min, CenterMode.Min);
        AddChild(_text);


        










    }

        void Update()
        {
        foreach (Ball mover in myGame._movers)
        {
            if (mover.moving)
            {
                score = mover.score;
            }
        }
        _text.Clear(Color.Transparent);
        _text.Text("Score: " + score, 0, 0);


    }
    

}

