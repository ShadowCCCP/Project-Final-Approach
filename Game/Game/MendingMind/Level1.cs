using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

public class Level1 : Level
{


    Vec2 ballSpeed = new Vec2(0, 0);

    Vec2 platformPos = new Vec2(400, 1050);

    SquareBlocker plat1;
    SquareBlocker plat2;
    SquareBlocker plat3;

    public Ball pong;

    MyGame myGame;
    public Level1() : base(99, 99, 99, 99, 99)
    {
        myGame = (MyGame)game;

        pong = new Ball(30, new Vec2(962.5f, 1000), ballSpeed);



        Ball.acceleration.SetXY(0, 1);
        myGame._movers.Add(pong);

        for(int i = 0; i < 13; i++) 
        {
            myGame.AddBlocker(new Vec2(62.5f, 62.5f + (i * 75)), 1, true); //left border
            myGame.AddBlocker(new Vec2(1787.5f, 62.5f + (i * 75)), 1, true); //right border
        }

        for (int i = 0; i < 22; i++) //top border
        {
            myGame.AddBlocker(new Vec2(137.5f + (i * 75), 62.5f), 1, true);
        }

        for (int i = 0; i < 3; i++) 
        {
            myGame.AddBlocker(new Vec2(287.5f - (i * 75), 137.5f + (i * 75)), 4, true); //left slope border
            myGame.AddBlocker(new Vec2(1562.5f + (i * 75), 137.5f + (i * 75)), 5, true); //right slope border
            myGame.AddBlocker(new Vec2(737.5f + (i * 75), 137.5f + (i * 75)), 5, true); //left slope center
            myGame.AddBlocker(new Vec2(1112.5f - (i * 75), 137.5f + (i * 75)), 4, true); //right slope center

        }


        for(int i = 0; i < 2; i++)
        {
            //left pyramid
            myGame.AddBlocker(new Vec2(587.5f + (i * 75), 962.5f), 1, true);
            myGame.AddBlocker(new Vec2(587.5f, 887.5f), 3, true);
            myGame.AddBlocker(new Vec2(662.5f, 887.5f), 2, true);


            //right pyramid
            myGame.AddBlocker(new Vec2(1187.5f + (i * 75), 962.5f), 1, true);
            myGame.AddBlocker(new Vec2(1187.5f, 887.5f), 3, true);
            myGame.AddBlocker(new Vec2(1262.5f, 887.5f), 2, true);

            //left bottom slope
            myGame.AddBlocker(new Vec2(137.5f + (i * 75), 887.5f + (i * 75)), 2, true);

            //left bottom slope
            myGame.AddBlocker(new Vec2(1637.5f + (i * 75), 962.5f - (i * 75)), 3, true);
        }

        for(int i = 0; i < 20; i++) //static balls
        {
            Ball ping = new Ball(25, new Vec2(Utils.Random(0, 10) * 150 + 250, Utils.Random(0, 3) * 150 + 400), new Vec2(0, 0), false);
            myGame._movers.Add(ping);
        }




        foreach (Ball b in myGame._movers)
        {
            AddChild(b);

        }

        
    }

    void Update()
    {

        ballSpeed = new Vec2((Input.mouseX - pong.x), (Input.mouseY - pong.y));
        if (myGame._paused)
        {
            if (Input.GetMouseButton(0))
            {

                pong.velocity = ballSpeed * 0.1f;
                pong.ShowDebugInfo();
            }

        }

        else
        {
            pong._velocityIndicator.LateDestroy();
        }


        if (pong.reset)
        {
            SceneManager.Instance.loadLevel("WinScreen");
        }
    }
}
