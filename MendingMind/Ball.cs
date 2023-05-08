using GXPEngine;
using System;

public class Ball : EasyDraw
{

    public static bool drawDebugLine = false;
    public static bool wordy = false;
    public static float bounciness = 0.66f;

    public static Vec2 acceleration = new Vec2(0, 0);


    public Vec2 velocity;
    public Vec2 position;

    Sprite texture;

    public int score;

    public readonly int radius;
    public readonly bool moving;

    public bool reset = false;

    bool firstTime = true;

    public float Mass
    {
        get
        {
            return radius * radius * _density;
        }
    }

    Vec2 _oldPosition;
    public Arrow _velocityIndicator;

    float _density = 1;

    public Ball(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool moving = true) : base(pRadius * 2 + 1, pRadius * 2 + 1)
    {

        radius = pRadius;
        position = pPosition;
        velocity = pVelocity;
        this.moving = moving;
        SetOrigin(radius, radius);
        Draw(170, 50, 0);

        if (moving)
        {
            texture = new Sprite("ball.png", false, false);
            texture.SetOrigin(texture.width / 2, texture.height / 2);
            AddChild(texture);
            texture.SetXY(0, 0);

        }
        

        position = pPosition;
        UpdateScreenPosition();

        

        _velocityIndicator = new Arrow(position, new Vec2(0, 0), 10);
        AddChild(_velocityIndicator);
    }

    void Draw(byte red, byte green, byte blue)
    {
        if (moving)
        {
            Fill(red, green, blue, 0);
            Stroke(red, green, blue, 0);
        }
        else
        {
            Fill(red, green, blue, 255);
            Stroke(red, green, blue, 255);
        }
        
        Ellipse(radius, radius, 2 * radius, 2 * radius);
    }

    void UpdateScreenPosition()
    {
        x = position.x;
        y = position.y;

        
    }

    public void Step()
    {
        velocity += acceleration;
        _oldPosition = position;
        position += velocity;

        BoundaryWrapAround();

        CollisionInfo firstCollision = FindEarliestCollision();
        if (firstCollision != null)
        {
            ResolveCollision(firstCollision);
            if(firstCollision.timeOfImpact < 0.00002f && firstTime == true)
            {
                firstTime = false;
                velocity -= acceleration;
                    Step();
            }
        }
        firstTime = true;
        if(velocity.x > 0)
        {
            rotation += velocity.x / 3;
        }
        if (velocity.x < 0)
        {
            
            rotation -= -velocity.x / 3;
        }

        UpdateScreenPosition();
        velocity *= 0.99f;
    }

    CollisionInfo FindEarliestCollision()
    {
        MyGame myGame = (MyGame)game;
        Vec2 normal = new Vec2();
        CollisionInfo earliestCollision = null;
        float currentTimeOfImpact = 10;

        // Check other movers:			
        for (int i = 0; i < myGame.GetNumberOfMovers(); i++)
        {
            Ball mover = myGame.GetMover(i);
            if (mover != this)
            {
                Vec2 u = _oldPosition - (mover.position);
                float a = Mathf.Pow(velocity.Length(), 2);
                float b = u.Dot(velocity) * 2;
                float c = Mathf.Pow(u.Length(), 2) - Mathf.Pow(radius + mover.radius, 2);
                float D = Mathf.Pow(b, 2) - (4 * a * c);

                if (c < 0)
                {
                    if (b < 0)
                        return new CollisionInfo(normal, mover, 0);
                    else return null;
                }

                if (velocity.Length() != 0)
                {

                    if (D > 0)
                    {

                        float TOI1 = (-b - Mathf.Sqrt(D)) / (2 * a);

                        if (TOI1 < 1 && TOI1 >= 0)
                        {
                            if (currentTimeOfImpact > TOI1)
                            {
                                earliestCollision = new CollisionInfo(normal, mover, TOI1);
                                currentTimeOfImpact = TOI1;
                            }
                        }
                    }
                }
                
            }
            
        }
        //----------------------------------------------------

        for (int i = 0; i < myGame.GetNumberOfLines(); i++)
        {
            LineSegment line = myGame.GetLine(i);
            float distanceTo = (position - line.start).Dot(line.normalLine);


            if (distanceTo < radius)
            {
                float a = line.normalLine.Dot((_oldPosition - line.start)) - radius;
                float b = -line.normalLine.Dot(velocity);
                float TOI;
                if (b <= 0)
                {
                   // return null;

                }
                if(a >= 0)
                {
                        TOI = a / b;

                }else if(a >= -radius)
                {
                    TOI = 0;
                }else continue;
        

                    if (TOI <= 1)
                    {

                        float d = ((_oldPosition + velocity * TOI) - line.start).Dot((line.end - line.start).Normalized());
                        if (d >= 0 && d <= line.lenght)
                        {


                            if (currentTimeOfImpact > TOI)
                            {
                                currentTimeOfImpact = TOI;
                                earliestCollision = new CollisionInfo(line.normalLine, null, TOI);
                                    
                            }
                        }

                    }

            }
            foreach (Ball ball in line.Points)
            {

                float TimeOfImpactWithBall = CollisionWithPoint(this, ball);
                if (currentTimeOfImpact > TimeOfImpactWithBall)
                {
                    earliestCollision = new CollisionInfo(normal, ball, TimeOfImpactWithBall);
                    currentTimeOfImpact = TimeOfImpactWithBall;
                }
            }



        }
        return earliestCollision;
    }


    float CollisionWithPoint(Ball current, Ball other)
    {

        Vec2 u = _oldPosition - (other.position);
        float a = Mathf.Pow(velocity.Length(), 2);
        float b = u.Dot(velocity) * 2;
        float c = Mathf.Pow(u.Length(), 2) - Mathf.Pow(radius + other.radius, 2);
        float D = Mathf.Pow(b, 2) - (4 * a * c);
        if (c < 0)
        {
            if (b < 0)
                return 0;
            else return 10;
        }
        if (a < 0.01f) return 10;
        if (D > 0)
        {

            float TOI1 = (-b - Mathf.Sqrt(D)) / (2 * a);


            if (TOI1 < 1 && TOI1 >= 0)
            {
                return TOI1;
            }
        }

        return 10;
    }

    void ResolveCollision(CollisionInfo col)
    {


        if (col.other == null)
        {


            position = _oldPosition + velocity * col.timeOfImpact;
            velocity.Reflect(col.normal, bounciness);
        }
        if (col.other is Ball)
        {
            Ball otherBall = (Ball)col.other;

            Vec2 fromBallToThis = (position - otherBall.position);
            Vec2 normal = (otherBall.position - position).Normalized();


            

            position = _oldPosition + velocity * col.timeOfImpact;
            velocity *= 1.3f;
            velocity.Reflect((otherBall.position - position).Normalized(), bounciness);

            score += (100 + (int)velocity.Length() * 10) / 2;
            //Console.WriteLine(score);
        }

    }

    void BoundaryWrapAround()
    {
        if (position.y > game.height)
        {
            reset = true;
        }
    }

    
    public void ShowDebugInfo()
    {
        
        _velocityIndicator.startPoint = position;
        _velocityIndicator.vector = velocity;
    }
    
}

