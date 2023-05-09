using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    class BallNew : AnimationSprite
    {

        public Vec2 velocity;
        public Vec2 position;

        public readonly int radius;
        public readonly bool moving;

        MyGame myGame;
        Vec2 _oldPosition;
        Arrow _velocityIndicator;

        protected List<CollisionInfo> collisions;

        public BallNew(string filename = "", int cols = 0, int rows = 0, TiledObject obj = null) : base(filename, cols, rows, -1, false, false)
        {

        }

        private CollisionInfo FindEarliestCollision()
        {
            collisions = new List<CollisionInfo>();

            BallCollision();
            LineSegmentCollision();
            BlockCollision();

            return EarliestCollision();

        }

        public void Step()
        {
            _oldPosition = position;
            position += velocity;

            UpdateScreenPosition();

            CollisionInfo firstCollision = FindEarliestCollision();
            if (firstCollision != null)
            {
                ResolveCollision(firstCollision);
            }
        }

        private void ResolveCollision(CollisionInfo col)
        {
            position = _oldPosition + col.timeOfImpact * velocity;
            velocity.Reflect(col.normal);
        }

        private void UpdateScreenPosition()
        {
            x = position.x;
            y = position.y;
        }

        private void BallCollision()
        {
            BallNew[] moverList = FindObjectsOfType<BallNew>();

            for (int i = 0; i < moverList.Length; i++)
            {
                BallNew mover = moverList[i];
                if (mover != this)
                {
                    Vec2 u = _oldPosition - mover.position;
                    float a = velocity.Length() * velocity.Length();
                    float b = 2 * u.Dot(velocity);
                    float c = u.Length() * u.Length() - (radius + mover.radius) * (radius + mover.radius);

                    if (c < 0)
                    {
                        if (b < 0)
                        {
                            collisions.Add(new CollisionInfo(u.Normalized(), mover, 0));
                        }
                    }

                    if (a > 0.00001f)
                    {
                        float D = b * b - 4 * a * c;

                        if (D > 0)
                        {
                            float toi = (-b - Mathf.Sqrt(D)) / (2 * a);
                            if (0 <= toi && toi < 1)
                            {
                                collisions.Add(new CollisionInfo(u.Normalized(), mover, toi));
                            }
                        }
                    }
                }
            }
        }

        private CollisionInfo EarliestCollision()
        {
            foreach (CollisionInfo col in collisions)
            {
                bool earliest = true;
                foreach (CollisionInfo other in collisions)
                {
                    if (col != other)
                    {
                        if (col.timeOfImpact > other.timeOfImpact)
                        {
                            earliest = false;
                        }
                    }
                }
                if (earliest)
                {
                    return col;
                }
            }

            return null;
        }

        private void LineSegmentCollision()
        {
            for (int i = 0; i < myGame.GetNumberOfLines(); i++)
            {
                Vec2 difference = position - myGame.GetLine(i).end;

                // Bottom
                LineSegmentTopOrBottom(true, difference, i);
                // Top
                LineSegmentTopOrBottom(false, difference, i);
            }
        }

        private void LineSegmentTopOrBottom(bool bottom, Vec2 difference, int i)
        {
            Vec2 bottomOrTop;

            if (bottom)
            {
                bottomOrTop = myGame.GetLine(i).end - myGame.GetLine(i).start;
            }
            else
            {
                bottomOrTop = myGame.GetLine(i).start - myGame.GetLine(i).end;
            }

            float toi = 2;
            float a = difference.Dot(bottomOrTop.Normal()) - radius;
            float b = (_oldPosition - position).Dot(bottomOrTop.Normal());

            if (b > 0 && a >= 0)
            {
                toi = a / b;
            }
            else if (b > 0 && a >= -radius)
            {
                toi = 0;
            }

            if (toi >= 0 && toi <= 1)
            {
                Vec2 poi = _oldPosition + velocity * toi;
                Vec2 poiDifference;
                if (bottom)
                {
                    poiDifference = poi - myGame.GetLine(i).start;
                }
                else
                {
                    poiDifference = poi - myGame.GetLine(i).end;
                }
                float lineDistance = poiDifference.Dot(bottomOrTop.Normalized());
                if (lineDistance >= 0 && lineDistance <= bottomOrTop.Length())
                {
                    collisions.Add(new CollisionInfo(bottomOrTop.Normal(), null, toi));
                }
            }
        }

        private void BlockCollision()
        {
            Square[] rectangles = FindObjectsOfType<Square>();
            for (int i = 0; i < rectangles.Length; i++)
            {
                Vec2 check = new Vec2(x, y);

                // left edge
                if (x < rectangles[i].x)
                {
                    check.x = rectangles[i].x;
                }
                // right edge
                else if (x > rectangles[i].x + rectangles[i].width)
                {
                    check.x = rectangles[i].x + rectangles[i].width;
                }

                // top edge
                if (y < rectangles[i].y)
                {
                    check.y = rectangles[i].y;
                }
                // bottom edge
                else if (y > rectangles[i].y + rectangles[i].height)
                {
                    check.y = rectangles[i].y + rectangles[i].height;
                }

                Vec2 dist = new Vec2(position.x - check.x, position.y - check.y);
                float distance = dist.Length();
                if (distance <= radius) // This is the correct discrete collision check 
                {
                    // Check whether the calculated point is on a corner or not
                    if (check.Approximate(rectangles[i].topLeftCorner) || check.Approximate(rectangles[i].topRightCorner) ||
                        check.Approximate(rectangles[i].bottomLeftCorner) || check.Approximate(rectangles[i].bottomRightCorner))
                    {
                        Vec2 normal = (position - check).Normalized();
                        float overlap = radius - distance;
                        position += normal * overlap;
                        velocity.Reflect(normal);
                    }
                    else
                    {
                        if (Math.Abs(dist.x) < Math.Abs(dist.y))
                        {
                            if (y < rectangles[i].y)
                            {
                                //top
                                float impactY = rectangles[i].y - radius;

                                position.y = impactY;
                                velocity.y *= -1;
                            }
                            else
                            {
                                // bottom
                                float impactY = rectangles[i].y + rectangles[i].height + radius;

                                position.y = impactY;
                                velocity.y *= -1;
                            }
                        }
                        else
                        {
                            if (x < rectangles[i].x)
                            {
                                // left
                                float impactX = rectangles[i].x - radius;

                                position.x = impactX;
                                velocity.x *= -1;
                            }
                            else
                            {
                                // right
                                float impactX = rectangles[i].x + rectangles[i].width + radius;

                                position.x = impactX;
                                velocity.x *= -1;
                            }
                        }
                    }
                }
            }
        }
    }
}
