using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class EnergyBall : BallNew
    {
        float gravity;
        string nextLevel;
        int goalSide;

        Vec2 oldVelocity;
        bool timer;
        int time;
        bool timerStarted = false;
        bool ballDestroyed;

        int cooldown = Time.now;


        public EnergyBall(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, 3, 3)
        {
            radius = 128;

            if (obj != null)
            {
                goalSide = obj.GetIntProperty("goalSide", 1);
                bounciness = obj.GetFloatProperty("bounciness", 0.9f);
                gravity = obj.GetFloatProperty("gravity", 0);
                nextLevel = obj.GetStringProperty("nextLevel", "Level1.tmx");
            }

            acceleration = new Vec2(0, gravity);
        }

        public EnergyBall(Vec2 pPosition, int pGoalSide, float pBounciness, float pGravity, string pNextLevel, float velocityX, float velocityY, string filename = "EnergyOrbS.png", int cols = 3, int rows = 3) : base(filename, cols, rows)
        {
            
            radius = 128;

            goalSide = pGoalSide;
            bounciness = pBounciness;
            gravity = pGravity;
            nextLevel = pNextLevel;

            velocity = new Vec2(velocityX, velocityY);
            
            acceleration = new Vec2(0, gravity);

            SetXY(pPosition.x, pPosition.y);

            SetOrigin(width / 2, height / 2);
        }

        protected virtual void LineSegmentTopOrBottom(bool bottom, Vec2 difference, int i)
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

        protected override void ResolveCollision(CollisionInfo col)
        {
            base.ResolveCollision(col);
            int cTime = Time.now;
            Console.WriteLine("huhu");
            if (cTime - cooldown > 300)
            {
                Console.WriteLine("non");
                myGame.soundCollection.PlaySound(8);
                cooldown = Time.now;
            }
        }

        protected override void BlockCollision()
        {
            for (int i = 0; i < myGame.NumberOfSquares(); i++)
            {
                if ((myGame.GetSquare(i) is EBallThroughOnly && !IsBullet))
                {
                    break;
                }

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
                                checkGoalCollision(1, i);
                            }
                            else
                            {
                                // bottom
                                float impactY = myGame.GetSquare(i).y + (myGame.GetSquare(i).height / 2 + radius + 1);

                                position.y = impactY;
                                velocity.y *= -bounciness;
                                checkGoalCollision(3, i);
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
                                checkGoalCollision(4, i);
                            }
                            else
                            {
                                // right
                                float impactX = myGame.GetSquare(i).x + (myGame.GetSquare(i).width / 2 + radius + 1);

                                position.x = impactX;
                                velocity.x *= -bounciness;
                                checkGoalCollision(2, i);
                            }
                        }
                    }
                }
            }
        }

        private void checkGoalCollision(int goal, int i)
        {
            do
            {
                if (myGame.GetSquare(i) is EnergyReceptor)
                {
                    if (goal == goalSide)
                    {
                        //NEXT LEVEL
                        ballDestroyed = true;
                        myGame.soundCollection.PlaySound(0);
                        break;
                    }

                }

                if (myGame.GetSquare(i) is BouncyPlatform)
                {
                    bouncyPlatformVelocity = 8;
                    myGame.BouncyPlatformAnim = true;
                    oldVelocity = velocity;
                    velocity = velocity * bouncyPlatformVelocity;
                    timer = true;
                    break;
                }
                if (myGame.GetSquare(i) is ButtonPlatform)
                {
                    myGame.ButtonPressed = true;
                    Console.WriteLine("Button pressed");
                    break;
                }
                if (myGame.GetSquare(i) is Square)
                {
                    int cTime = Time.now;
                    if(cTime - cooldown > 300)
                    {
                        myGame.soundCollection.PlaySound(8);
                        cooldown = Time.now;
                    }
                }
            } while (false);

           

        }

        void timerBouncy()
        {
            if (timerStarted == false)
            {
                time = Time.time;
            }
            if (timer == true) //start timer
            {
                timerStarted = true;

                if (Time.time - time > 1500) //timer ends
                {
                    velocity = oldVelocity;
                    timer = false;
                    timerStarted = false;
                }
            }

        }
        int frame=0;
        int counter = 0;
        void destroyAnim()
        {

            if (ballDestroyed)
            {
                velocity = new Vec2(0, 0);
                counter++;

                if (counter > 10) // animation
                {
                    counter = 0;
                    frame++;
                    if (frame == 8)
                    {
                        myGame.LoadLevel(nextLevel);
                        Console.WriteLine("Next level");
                    }
                }
            }
            SetFrame(frame);
        }

        protected override void Update()
        {
            
            base.Update();
            timerBouncy();

            destroyAnim();

        }


    }
}
