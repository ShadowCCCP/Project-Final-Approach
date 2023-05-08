using System;
using System.Threading;
using GXPEngine;


public class Block : EasyDraw
{
    /******* PUBLIC FIELDS AND PROPERTIES *********************************************************/

    // These four public static fields are changed from MyGame, based on key input (see Console):
    public static bool drawDebugLine = false;
    public readonly int radius;

    public Vec2 topEdge;
    public Vec2 bottomEdge;
    public Vec2 rightEdge;
    public Vec2 leftEdge;

    public Vec2 topRightCorner;
    public Vec2 topLeftCorner;
    public Vec2 bottomRightCorner;
    public Vec2 bottomLeftCorner;

    public Vec2 position
    {
        get
        {
            return _position;
        }
    }

    /******* PRIVATE FIELDS *******************************************************************/

    int live;

    Vec2 _position;

    /******* PUBLIC METHODS *******************************************************************/

    public Block(int pRadius, Vec2 pPosition, int pLive = 3) : base(pRadius * 2, pRadius * 2)
    {
        live = pLive;

        radius = pRadius;
        _position = pPosition;

        Draw();
        UpdateScreenPosition();

        topLeftCorner = new Vec2(_position.x, position.y);
        topRightCorner = new Vec2(_position.x + width, _position.y);
        bottomLeftCorner = new Vec2(_position.x, _position.y + height);
        bottomRightCorner = new Vec2(_position.x + width, _position.y + height);

        topEdge = topLeftCorner - topRightCorner;
        bottomEdge = bottomRightCorner - bottomLeftCorner;
        rightEdge = topRightCorner - bottomRightCorner;
        leftEdge = bottomLeftCorner - topLeftCorner;
     }

    private void Update()
    {
        UpdateColor();
    }

    public void Collision()
    {
        if(live > 1)
        {
            live--;
        }
        else
        {
            MyGame myGame = (MyGame)game;
            Destroy();
            myGame.RemoveBlock(this);
        }
    }

    /******* PRIVATE METHODS *******************************************************************/

    /******* THIS IS WHERE YOU SHOULD WORK: ***************************************************/

    /******* NO NEED TO CHANGE ANY OF THE CODE BELOW: **********************************************/

    void UpdateColor()
    {
        switch(live)
        {
            case 1:
                {
                    SetColor(1, 1, 1);
                    break;
                }
            case 2:
                {
                    SetColor(0, 1, 0);
                    break;
                }
            case 3:
                {
                    SetColor(0, 0, 1);
                    break;
                }
            case 4:
                {
                    SetColor(1, 0, 1);
                    break;
                }
            case 5:
                {
                    SetColor(1, 1, 0);
                    break;
                }
        }
    }

    void UpdateScreenPosition()
    {
        x = _position.x;
        y = _position.y;
    }

    void Draw()
    {
        Fill(200);
        NoStroke();
        ShapeAlign(CenterMode.Min, CenterMode.Min);
        Rect(0, 0, width, height);
    }
}
