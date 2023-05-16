using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Bullet : BallNew
    {
        public int maxCollisions;
        public int collisionCount;
        int startTimer;
        int soundCooldown;

        public Bullet(Vec2 pPosition, Vec2 pVelocity, int pMaxCollisions) : base("Bullet.png", 1, 1)
        {
            soundCooldown = Time.now;
            startTimer = Time.now;
            maxCollisions = pMaxCollisions;
            SetOrigin(width / 2, height / 2);
            position = pPosition;
            velocity = pVelocity;
            UpdatePosition();
            radius = 128;
            //_density = 0.140625f; // Wilhelms value
            _density = 0.25f;
        }

        protected override void BlockCollision()
        {
            int cTime = Time.now;
            if(cTime - startTimer > 45)
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
                        collisionCount++;
                        // Check whether the calculated point is on a corner or not
                        if (check.Approximate(myGame.GetSquare(i).topLeftCorner) || check.Approximate(myGame.GetSquare(i).topRightCorner) ||
                            check.Approximate(myGame.GetSquare(i).bottomLeftCorner) || check.Approximate(myGame.GetSquare(i).bottomRightCorner))
                        {
                            PlaySound();
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
                                    PlaySound();
                                    float impactY = myGame.GetSquare(i).y - (myGame.GetSquare(i).height / 2 + radius + 1);

                                    position.y = impactY;
                                    velocity.y *= -1;
                                }
                                else
                                {
                                    // bottom
                                    PlaySound();
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
                                    PlaySound();
                                    float impactX = myGame.GetSquare(i).x - (myGame.GetSquare(i).width / 2 + radius + 1);

                                    position.x = impactX;
                                    velocity.x *= -1;
                                }
                                else
                                {
                                    // right
                                    PlaySound();
                                    float impactX = myGame.GetSquare(i).x + (myGame.GetSquare(i).width / 2 + radius + 1);

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


        void PlaySound(bool energyCol = false)
        {
            Random rdm = new Random();
            if (!energyCol)
            {
                int cTime = Time.now;
                if (cTime - soundCooldown > 300)
                {
                    switch (rdm.Next(4))
                    {
                        case 0:
                            {
                                myGame.soundCollection.PlaySound(19);
                                break;
                            }
                        case 1:
                            {
                                myGame.soundCollection.PlaySound(20);
                                break;
                            }
                        case 2:
                            {
                                myGame.soundCollection.PlaySound(21);
                                break;
                            }
                        case 3:
                            {
                                myGame.soundCollection.PlaySound(22);
                                break;
                            }
                       /* case 4:
                            {
                                myGame.soundCollection.PlaySound(23);
                                break;
                            }*/
                    }

                }
            }
            else
            {
                myGame.soundCollection.PlaySound(24);
            }
        }

        private void UpdatePosition()
        {
            x = position.x;
            y = position.y;
        }

        protected override void ResolveCollision(CollisionInfo col)
        {
            base.ResolveCollision(col);
            collisionCount++;
            if(col.other is EnergyBall)
            {
                PlaySound(true);
            }
        }
    }
}
