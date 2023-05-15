using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class RayBall : GameObject
    {
        MyGame myGame;

        public Vec2 acceleration;
        public Vec2 velocity;
        public Vec2 position;

        public float _density = 1;
        public float bounciness = 0.9f;

        protected float bouncyPlatformVelocity = 1.5f;

        public float Mass
        {
            get
            {
                return radius * radius * _density;
            }
        }

        public int radius;
        public readonly bool moving;

        bool posFix;
        Vec2 _oldPosition;

        protected List<CollisionInfo> collisions;

        public RayBall(Vec2 pPosition, Vec2 pVelocity) : base()
        {
            myGame = (MyGame)game;
            position = pPosition;
            velocity = pVelocity;
            radius = 100;
        }

        protected CollisionInfo FindEarliestCollision()
        {
            collisions = new List<CollisionInfo>();

            BlockCollision();
            LineSegmentCollision();

            return EarliestCollision();

        }

        protected virtual void Update()
        {
            Console.WriteLine(position);
            PositionFix();
            _oldPosition = position;
            position += velocity;

            UpdateScreenPosition();

            CollisionInfo firstCollision = FindEarliestCollision();
            if (firstCollision != null)
            {
                ResolveCollision(firstCollision);
            }
            velocity += acceleration;
        }

        protected virtual void ResolveCollision(CollisionInfo col)
        {
            position = _oldPosition + col.timeOfImpact * velocity;
            if (col.other != null)
            {
                BallNew other = col.other as BallNew;
                if (other.lineBall)
                {
                    velocity.Reflect(col.normal, bounciness);
                }
                else
                {
                    velocity.Reflect(col.normal, col.velCOM, bounciness);
                    other.velocity.Reflect(col.normal, col.velCOM, bounciness);
                }
            }
            else
            {
                velocity.Reflect(col.normal, bounciness);
            }
        }

        protected void PositionFix()
        {
            if (!posFix)
            {
                position.x = x;
                position.y = y;

                posFix = true;
            }
        }

        protected void UpdateScreenPosition()
        {
            x = position.x;
            y = position.y;
        }


        protected CollisionInfo EarliestCollision()
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

        protected void LineSegmentCollision()
        {
            for (int i = 0; i < myGame.NumberOfAngles(); i++)
            {
                Vec2 difference = position - myGame.GetAngle(i).end;

                // Bottom
                LineSegmentTopOrBottom(true, difference, i);
                // Top
                LineSegmentTopOrBottom(false, difference, i);
            }
        }

        protected void LineSegmentTopOrBottom(bool bottom, Vec2 difference, int i)
        {
            Vec2 bottomOrTop;

            if (bottom)
            {
                bottomOrTop = myGame.GetAngle(i).end - myGame.GetAngle(i).start;
            }
            else
            {
                bottomOrTop = myGame.GetAngle(i).start - myGame.GetAngle(i).end;
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
                    poiDifference = poi - myGame.GetAngle(i).start;
                }
                else
                {
                    poiDifference = poi - myGame.GetAngle(i).end;
                }
                float lineDistance = poiDifference.Dot(bottomOrTop.Normalized());
                if (lineDistance >= 0 && lineDistance <= bottomOrTop.Length())
                {
                    collisions.Add(new CollisionInfo(bottomOrTop.Normal(), null, toi));
                }
            }
        }

        protected virtual void BlockCollision()
        {
            for (int i = 0; i < myGame.NumberOfSquares(); i++)
            {
                Vec2 check = new Vec2(x, y);

                // left edge
                if (x < myGame.GetSquare(i).x - myGame.GetSquare(i).width / 2)
                {
                    check.x = myGame.GetSquare(i).x - myGame.GetSquare(i).width / 2;
                }
                // right edge
                else if (x > myGame.GetSquare(i).x + myGame.GetSquare(i).width / 2)
                {
                    check.x = myGame.GetSquare(i).x + myGame.GetSquare(i).width / 2;
                }

                // top edge
                if (y < myGame.GetSquare(i).y - myGame.GetSquare(i).height / 2)
                {
                    check.y = myGame.GetSquare(i).y - myGame.GetSquare(i).height / 2;
                }
                // bottom edge
                else if (y > myGame.GetSquare(i).y + myGame.GetSquare(i).height / 2)
                {
                    check.y = myGame.GetSquare(i).y + myGame.GetSquare(i).height / 2;
                }

                Vec2 dist = new Vec2(position.x - check.x, position.y - check.y);
                float distance = dist.Length();
                if (distance <= radius)
                {
                    // Check whether the calculated point is on a corner or not
                    if (check.Approximate(myGame.GetSquare(i).topLeftCorner) || check.Approximate(myGame.GetSquare(i).topRightCorner) ||
                        check.Approximate(myGame.GetSquare(i).bottomLeftCorner) || check.Approximate(myGame.GetSquare(i).bottomRightCorner))
                    {
                        Vec2 normal = (position - check).Normalized();
                        float overlap = radius - distance;
                        position += normal * overlap;
                        Console.WriteLine(position);
                        velocity.Reflect(normal, bounciness);
                    }
                    else
                    {
                        if (Math.Abs(dist.x) < Math.Abs(dist.y))
                        {
                            if (y < myGame.GetSquare(i).y)
                            {
                                //top
                                float impactY = myGame.GetSquare(i).y - (myGame.GetSquare(i).height / 2 + radius + 1);

                                position.y = impactY;
                                velocity.y *= -bounciness;

                                Console.WriteLine(impactY);
                            }
                            else
                            {
                                // bottom
                                float impactY = myGame.GetSquare(i).y + (myGame.GetSquare(i).height / 2 + radius + 1);

                                position.y = impactY;
                                velocity.y *= -bounciness;

                                Console.WriteLine(impactY);
                            }

                        }
                        else
                        {
                            if (x < myGame.GetSquare(i).x)
                            {
                                // left
                                float impactX = myGame.GetSquare(i).x - (myGame.GetSquare(i).width / 2 + radius + 1);

                                position.x = impactX;
                                velocity.x *= -bounciness;

                                Console.WriteLine(impactX);
                            }
                            else
                            {
                                // right
                                float impactX = myGame.GetSquare(i).x + (myGame.GetSquare(i).width / 2 + radius + 1);

                                position.x = impactX;
                                velocity.x *= -bounciness;

                                Console.WriteLine(impactX);
                            }
                        }
                    }
                }
            }
        }
    }
}
