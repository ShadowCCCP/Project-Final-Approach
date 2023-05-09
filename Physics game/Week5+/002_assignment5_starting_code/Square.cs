using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class Square : AnimationSprite
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

        public Square(string filename = "", int cols = 0, int rows = 0, TiledObject obj = null) : base(filename, cols, rows, -1, false, false)
        {
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

        void UpdateScreenPosition()
        {
            x = _position.x;
            y = _position.y;
        }
    }
}