using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using GXPEngine;
using GXPEngine.Core;
using TiledMapParser;

public class Ball : EasyDraw
{
	// These four public static fields are changed from MyGame, based on key input (see Console):
	public static bool drawDebugLine = false;

	public Vec2 velocity;
	public Vec2 position;

	public readonly int radius;
	public readonly bool moving;

    MyGame myGame;
    Vec2 _oldPosition;
	Arrow _velocityIndicator;

    protected List<CollisionInfo> collisions;

    protected bool collided;

    // int pRadius, Vec2 pPosition, Vec2 pVelocity=new Vec2(), bool moving=true
    public Ball (int pRadius, Vec2 pPosition, Vec2 pVelocity=new Vec2(), bool moving=true) : base (pRadius*2 + 1, pRadius*2 + 1)
	{
        myGame = (MyGame)game;

        radius = pRadius;
		position = pPosition;
		velocity = pVelocity;
		this.moving = moving;


        position = pPosition;
		UpdateScreenPosition ();
		SetOrigin (radius, radius);

		Draw (230, 200, 0);

		_velocityIndicator = new Arrow(position, new Vec2(0,0), 10);
		AddChild(_velocityIndicator);
	}

	public void Step () {
		_oldPosition = position;
		position += velocity;	

        UpdateScreenPosition();

        CollisionInfo firstCollision = FindEarliestCollision();
        if (firstCollision != null)
        {
            ResolveCollision(firstCollision);
        }

        ShowDebugInfo();
    }

    public void SetCollisionTrue()
    {
        collided = true;
    }

    private void Draw(byte red, byte green, byte blue)
    {
        Fill(red, green, blue);
        Stroke(red, green, blue);
        Ellipse(radius, radius, 2 * radius, 2 * radius);

    }

    private CollisionInfo FindEarliestCollision()
    {
        collisions = new List<CollisionInfo>();

        BallCollision();
        LineSegmentCollision();
        BlockCollision();

        return EarliestCollision();

    }

    private void ResolveCollision(CollisionInfo col)
    {
        position = _oldPosition + col.timeOfImpact * velocity;
        velocity.Reflect(col.normal);
        // Discrete Ball/Ball collision
        /**
        Ball other = (Ball)col.other;
        Vec2 relativePosition = other.position - position;
        float overlap = radius + other.radius - relativePosition.Length();
        position -= col.normal * overlap;
        velocity.Reflect(col.normal);
        //*/
    }

    private void UpdateScreenPosition()
    {
        x = position.x;
        y = position.y;
    }

    private void BallCollision()
    {
        for (int i = 0; i < myGame.GetNumberOfBalls(); i++)
        {
            Ball mover = myGame.GetBalls(i);
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
                        mover.SetCollisionTrue();
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
                            mover.SetCollisionTrue();
                            collisions.Add(new CollisionInfo(u.Normalized(), mover, toi));
                        }
                    }
                }

                /**
                Vec2 relativePosition = mover.position - position;
				if (relativePosition.Length () < radius + mover.radius) {

                    // Discrete Ball/Ball collision...
                    /**
					Vec2 collisionNormal = relativePosition.Normalized();
                    return new CollisionInfo (collisionNormal, mover, 0);
					
				}
				//*/
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
            if(bottom)
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
        for (int i = 0; i < myGame.GetNumberOfBlocks(); i++)
        {
            Vec2 check = new Vec2(x, y);

            // left edge
            if (x < myGame.GetBlock(i).x)
            {
                check.x = myGame.GetBlock(i).x;
            }
            // right edge
            else if (x > myGame.GetBlock(i).x + myGame.GetBlock(i).width)
            {
                check.x = myGame.GetBlock(i).x + myGame.GetBlock(i).width;
            }

            // top edge
            if (y < myGame.GetBlock(i).y)
            {
                check.y = myGame.GetBlock(i).y;
            }
            // bottom edge
            else if (y > myGame.GetBlock(i).y + myGame.GetBlock(i).height)
            {
                check.y = myGame.GetBlock(i).y + myGame.GetBlock(i).height;
            }

            // Left it here because of the for-loop
            if(this is ShootingBall)
            {
                Gizmos.DrawPlus(check.x, check.y, 20);
            }

            Vec2 dist = new Vec2(position.x - check.x, position.y - check.y);
            float distance = dist.Length();
            if (distance <= radius) // This is the correct discrete collision check 
            {
                // Check whether the calculated point is on a corner or not
                if(check.Approximate(myGame.GetBlock(i).topLeftCorner) || check.Approximate(myGame.GetBlock(i).topRightCorner) || 
                    check.Approximate(myGame.GetBlock(i).bottomLeftCorner) || check.Approximate(myGame.GetBlock(i).bottomRightCorner))
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
                        if (y < myGame.GetBlock(i).y)
                        {
                            //top
                            float impactY = myGame.GetBlock(i).y - radius;

                            position.y = impactY;
                            velocity.y *= -1;
                        }
                        else
                        {
                            // bottom
                            float impactY = myGame.GetBlock(i).y + myGame.GetBlock(i).height + radius;

                            position.y = impactY;
                            velocity.y *= -1;
                        }
                    }
                    else
                    {
                        if (x < myGame.GetBlock(i).x)
                        {
                            // left
                            float impactX = myGame.GetBlock(i).x - radius;

                            position.x = impactX;
                            velocity.x *= -1;
                        }
                        else
                        {
                            // right
                            float impactX = myGame.GetBlock(i).x + myGame.GetBlock(i).width + radius;

                            position.x = impactX;
                            velocity.x *= -1;
                        }
                    }
                }

                myGame.GetBlock(i).Collision();
            }
        }
    }

    void ShowDebugInfo() {
		    if (drawDebugLine) {
			    ((MyGame)game).DrawLine (_oldPosition, position);
		    }
		    _velocityIndicator.startPoint = position;
		    _velocityIndicator.vector = velocity;
	    }
    }

