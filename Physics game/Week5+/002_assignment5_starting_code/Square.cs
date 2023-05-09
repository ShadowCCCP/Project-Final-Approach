using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class Square : AnimationSprite
    {
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

        Vec2 _position;

        public Square(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows, -1, false, false)
        {
            MyGame myGame = (MyGame)game;

            myGame.AddSquare(this);

            UpdateScreenPosition();

            
            topLeftCorner = new Vec2(_position.x, position.y);
            topRightCorner = new Vec2(_position.x + width, _position.y);
            bottomLeftCorner = new Vec2(_position.x, _position.y + height);
            bottomRightCorner = new Vec2(_position.x + width, _position.y + height);
            

            /*
            topLeftCorner = new Vec2(_position.x - width / 2, position.y - height / 2);
            topRightCorner = new Vec2(_position.x + width / 2, _position.y - height / 2);
            bottomLeftCorner = new Vec2(_position.x - width / 2, _position.y + height / 2);
            bottomRightCorner = new Vec2(_position.x + width / 2, _position.y + height / 2);
            */

            topEdge = topLeftCorner - topRightCorner;
            bottomEdge = bottomRightCorner - bottomLeftCorner;
            rightEdge = topRightCorner - bottomRightCorner;
            leftEdge = bottomLeftCorner - topLeftCorner;
        }

        void UpdateScreenPosition()
        {
            x = _position.x;
            y = _position.y;
        }
    }
}