using System;
using GXPEngine; // For Mathf

public struct Vec2
{
    public float x;
    public float y;

    public Vec2(float pX = 0, float pY = 0)
    {
        x = pX;
        y = pY;
    }

    // Week 1
    public float Length()
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    public Vec2 Normalized()
    {
        return new Vec2(1 / Length() * x, 1 / Length() * y);
    }

    public void Normalize()
    {
        this = Normalized();
    }

    public void SetXY(float pX, float pY)
    {
        x = pX;
        y = pY;
    }


    // Operator overloads
    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x + right.x, left.y + right.y);
    }

    public static Vec2 operator +(Vec2 left, float right)
    {
        return new Vec2(left.x + right, left.y + right);
    }

    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x - right.x, left.y - right.y);
    }

    public static Vec2 operator -(Vec2 left, float right)
    {
        return new Vec2(left.x - right, left.y - right);
    }

    public static Vec2 operator *(Vec2 vector, float value)
    {
        return new Vec2(vector.x * value, vector.y * value);
    }

    public static Vec2 operator *(float value, Vec2 vector)
    {
        return new Vec2(vector.x * value, vector.y * value);
    }

    public static Vec2 operator /(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x / right.x, left.y / right.y);
    }

    public static Vec2 operator /(Vec2 left, float right)
    {
        return new Vec2(left.x / right, left.y / right);
    }

    public override string ToString()
    {
        return String.Format("({0} , {1})", x, y);
    }




    // Week 2
    public static float Deg2Rad(float degrees)
    {
        return degrees / 180 * Mathf.PI;
    }

    public static float Rad2Deg(float radians)
    {
        return radians * Mathf.PI * 180 / Mathf.PI;
    }

    public static Vec2 GetUnitVectorDeg(float degrees)
    {
        float radians = Deg2Rad(degrees);

        return new Vec2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    public static Vec2 GetUnitVectorRad(float radians)
    {
        radians = radians * Mathf.PI;

        return new Vec2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    public static Vec2 RandomUnitVector()
    {
        Random random = new Random();
        float degrees = random.Next(0, 360);
        float sinT = Mathf.Sin(Deg2Rad(degrees));
        float cosT = Mathf.Cos(Deg2Rad(degrees));

        return new Vec2(sinT, cosT);
    }

    public void SetAngleDegrees(float degrees)
    {
        float length = Length();

        SetXY(Mathf.Cos(Deg2Rad(degrees)) * length, Mathf.Sin(Deg2Rad(degrees)) * length);
    }

    public void SetAngleRadians(float radians)
    {
        float length = Length();

        SetXY(Mathf.Cos(radians * Mathf.PI) * length, Mathf.Sin(radians * Mathf.PI) * length);
    }

    public float GetAngleRadians()
    {
        return Mathf.Atan2(y, x) / Mathf.PI;
    }

    public float GetAngleDegrees()
    {
        return Rad2Deg(GetAngleRadians());
    }

    public Vec2 RotateDegrees(float degrees)
    {
        float sinT = Mathf.Sin(Deg2Rad(degrees));
        float cosT = Mathf.Cos(Deg2Rad(degrees));

        return new Vec2(x * cosT - y * sinT, x * sinT + y * cosT);
    }

    public Vec2 RotateRadians(float radians)
    {
        float sinT = Mathf.Sin(radians * Mathf.PI);
        float cosT = Mathf.Cos(radians * Mathf.PI);

        return new Vec2(x * cosT - y * sinT, x * sinT + y * cosT);
    }

    public Vec2 RotateAroundDegrees(Vec2 point, float degrees)
    {
        this -= point;
        this = RotateDegrees(degrees);
        this += point;
        return this;
    }

    public Vec2 RotateAroundRadians(Vec2 point, float radians)
    {
        this -= point;
        this = RotateRadians(radians);
        this += point;
        return this;
    }




    // Week 4
    public float Dot(Vec2 other)
    {
        return x * other.x + y * other.y;
    }

    public Vec2 Normal()
    {
        return new Vec2(-y, x).Normalized();
    }

    public void Reflect(Vec2 pNormal, float pBounciness = 1)
    {
        this = this - (1 + pBounciness) * Dot(pNormal) * pNormal;
    }


    // For comparing floats...
    public bool Approximate(Vec2 right, float precision = 0.00001f)
    {
        if (Mathf.Abs(x - right.x) < precision && Mathf.Abs(y - right.y) < precision)
        {
            return true;
        }

        return false;
    }
}