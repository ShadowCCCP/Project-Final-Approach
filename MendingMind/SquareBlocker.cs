using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

    public class SquareBlocker: GameObject
    {

    public const int BLOCK_OFFSET = 75;

    const int BLOCK_SQUARE = 1;
    const int RIGHT_SLOPE = 2;
    const int LEFT_SLOPE = 3;
    const int TOP_RIGHT_SLOPE = 4;
    const int TOP_LEFT_SLOPE = 5;
    public int index;

    public LineSegment line;
    public Vec2 position;
    public List<LineSegment> _pLines;
    public float offset;
    public bool lvlComp;

    public List<Sprite> _pImages;

    public SquareBlocker(Vec2 pPosition, int blockIndex, bool pLvlComp = false)
    {
        position = pPosition;
        index = blockIndex;
        offset = BLOCK_OFFSET;
        lvlComp = pLvlComp;

        _pLines = new List<LineSegment>();
        _pImages = new List<Sprite>();

        drawShape();
        //Console.WriteLine(_images.Count());
    }

    public void drawShape()
    {
        switch (index)
        {
            case BLOCK_SQUARE:

                AddLine(new Vec2(position.x + BLOCK_OFFSET, position.y), position);
                AddLine(new Vec2(position.x + BLOCK_OFFSET, position.y + BLOCK_OFFSET), new Vec2(position.x + BLOCK_OFFSET, position.y));
                AddLine(new Vec2(position.x, position.y + BLOCK_OFFSET), new Vec2(position.x + BLOCK_OFFSET, position.y + BLOCK_OFFSET));
                AddLine(position, new Vec2(position.x, position.y + BLOCK_OFFSET));

                

                break;
            
            case RIGHT_SLOPE:

                AddLine(new Vec2(position.x + BLOCK_OFFSET + 1, position.y + BLOCK_OFFSET + 1), position);
                AddLine(new Vec2(position.x, position.y + BLOCK_OFFSET), new Vec2(position.x + BLOCK_OFFSET, position.y + BLOCK_OFFSET));
                AddLine(position, new Vec2(position.x, position.y + BLOCK_OFFSET));

                

                break;
            
            case LEFT_SLOPE:
                
                    AddLine(new Vec2(position.x + BLOCK_OFFSET + 1, position.y + 1), new Vec2(position.x + 1, position.y + BLOCK_OFFSET + 1));
                    AddLine(new Vec2(position.x + BLOCK_OFFSET, position.y + BLOCK_OFFSET), new Vec2(position.x + BLOCK_OFFSET, position.y));
                    AddLine(new Vec2(position.x, position.y + BLOCK_OFFSET), new Vec2(position.x + BLOCK_OFFSET, position.y + BLOCK_OFFSET));


                break;

                    

            case TOP_RIGHT_SLOPE:

                AddLine(new Vec2(position.x + BLOCK_OFFSET, position.y), position);
                AddLine(position, new Vec2(position.x, position.y + BLOCK_OFFSET));
                AddLine(new Vec2(position.x + 1, position.y + BLOCK_OFFSET + 1), new Vec2(position.x + BLOCK_OFFSET + 1, position.y + 1));


                break;

            case TOP_LEFT_SLOPE:
                AddLine(new Vec2(position.x + BLOCK_OFFSET, position.y), position);
                AddLine(new Vec2(position.x + BLOCK_OFFSET, position.y + BLOCK_OFFSET), new Vec2(position.x + BLOCK_OFFSET, position.y));
                AddLine(position + new Vec2(1, 1), new Vec2(position.x + BLOCK_OFFSET + 1, position.y + BLOCK_OFFSET + 1));


                break;
        }
    }



    void AddLine(Vec2 start, Vec2 end)
    {
        line = new LineSegment(start, end, 0xffffffff, 4);
        AddChild(line);
        _pLines.Add(line);
    }
}

