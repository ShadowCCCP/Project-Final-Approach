using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class EnergyBall : BallNew
    {
        float gravity;
        string nextLevel;

        public EnergyBall(string filename = "", int cols = 1, int rows = 1, TiledObject obj = null) : base(filename, cols, rows)
        {
            if (obj != null)
            {
                gravity = obj.GetFloatProperty("gravity", 0.5f);
                nextLevel = obj.GetStringProperty("nextLevel", "Level1.tmx");
            }

            acceleration = new Vec2(0, gravity);
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
                            }
                            else
                            {
                                // bottom
                                float impactY = myGame.GetSquare(i).y + (myGame.GetSquare(i).height / 2 + radius + 1);

                                position.y = impactY;
                                velocity.y *= -bounciness;
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
                            }
                            else
                            {
                                // right
                                float impactX = myGame.GetSquare(i).x + (myGame.GetSquare(i).width / 2 + radius + 1);

                                position.x = impactX;
                                velocity.x *= -bounciness;
                            }
                        }
                        if (myGame.GetSquare(i) is EnergyReceptor)
                        {
                            //NEXT LEVEL
                            myGame.LoadLevel(nextLevel);
                            Console.WriteLine("Next level");
                        }


                    }
                }
            }
        }
    }
}
