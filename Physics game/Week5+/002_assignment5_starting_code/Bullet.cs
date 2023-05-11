using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Bullet : BallNew
    {
        public int maxCollisions = 8;
        public int collisions;
        float bouncyPlatformVelocity = 1.5f;
        public Bullet(Vec2 pPosition, Vec2 pVelocity) : base("Bullet.png", 1, 1)
        {
            SetOrigin(width / 2, height / 2);
            position = pPosition;
            velocity = pVelocity;
            UpdatePosition();
            radius = 126;
            IsBullet = true;
        }

        protected override void BlockCollision()
        {
            for (int i = 0; i < myGame.NumberOfSquares(); i++)
            {
                if (myGame.GetSquare(i) is BulletThroughOnly)
                {
                    break;
                }
                if (myGame.GetSquare(i).isPlayer == false)
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
                            velocity.Reflect(normal);
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
                                    velocity.y *= -1;
                                }
                                else
                                {
                                    // bottom
                                    float impactY = myGame.GetSquare(i).y + (myGame.GetSquare(i).height / 2 + radius + 1);

                                    position.y = impactY;
                                    velocity.y *= -1;
                                }
                            }
                            else
                            {
                                if (x < myGame.GetSquare(i).x)
                                {
                                    // left
                                    float impactX = myGame.GetSquare(i).x - (myGame.GetSquare(i).width / 2 + radius + 1);

                                    position.x = impactX;
                                    velocity.x *= -1;
                                }
                                else
                                {
                                    // right
                                    float impactX = myGame.GetSquare(i).x + (myGame.GetSquare(i).width / 2 + radius + 1);

                                    position.x = impactX;
                                    velocity.x *= -1;
                                }
                            }
                            if (myGame.GetSquare(i) is BouncyPlatform)
                            {
                                myGame.BouncyPlatformAnim = true;
                                velocity = velocity * bouncyPlatformVelocity;
                            }
                            if (myGame.GetSquare(i) is ButtonPlatform)
                            {
                                myGame.ButtonPressed = true;
                            }
                            else
                            {
                                myGame.ButtonPressed = false;
                            }
                            collisions++;
                        }
                    }
                }
            }
        }

        public void Disappear()
        {

        }

        private void UpdatePosition()
        {
            x = position.x;
            y = position.y;
        }

        protected override void ResolveCollision(CollisionInfo col)
        {
            base.ResolveCollision(col);
            collisions++;
        }
    }
}
