using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using GXPEngine;

    public class Level: GameObject
    {

    MyGame myGame;
    public HUD blockHUD;

    public int blockLimit;
    public int rightSlopeLimit;
    public int leftSlopeLimit;
    public int topRightSlopeLimit;
    public int topLeftSlopeLimit;

    public GameObject[] _blockers;

    public Canvas _lineContainer = null;



    public void DrawLine(Vec2 start, Vec2 end)
    {
        _lineContainer.graphics.DrawLine(Pens.White, start.x, start.y, end.x, end.y);
    }

    public Level(int pBlockLimit = 1, int pRightSlopeLimit = 1, int pLeftSlopeLimit = 1, int pTopRightSlopeLimit = 1, int pTopLeftSlopeLimit = 1)
    {



        _lineContainer = new Canvas(game.width, game.height);
        AddChild(_lineContainer);
        

        blockLimit = pBlockLimit;
        rightSlopeLimit = pRightSlopeLimit;
        leftSlopeLimit = pLeftSlopeLimit;
        topRightSlopeLimit = pTopRightSlopeLimit;
        topLeftSlopeLimit = pTopLeftSlopeLimit;

        myGame = (MyGame)game;

        blockHUD = new HUD();
        
        //myGame.AddLine(new Vec2(game.width - 60, game.height - 38), new Vec2(60, game.height - 38));  //bottom
        myGame.AddLine(new Vec2(60, game.height - 38), new Vec2(60, 60));
        myGame.AddLine(new Vec2(60, 60), new Vec2(game.width - 60, 60));
        myGame.AddLine(new Vec2(game.width - 60, 60), new Vec2(game.width - 60, game.height - 38));  //right






        game.AddChild(blockHUD);
        _blockers = FindObjectsOfType<SquareBlocker>();
        Console.WriteLine(_blockers.Length);
    }



    //public virtual Grid[] GetGridArray() => blockHUD._gridArray.ToArray();

    }

