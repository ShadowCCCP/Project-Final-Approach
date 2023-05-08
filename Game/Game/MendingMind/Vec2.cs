using System;
using GXPEngine; // Allows using Mathf functions

public struct Vec2 
{
	public float x;
	public float y;

	public Vec2 (float pX = 0, float pY = 0) 
	{
		x = pX;
		y = pY;
	}

	public float Length()
	{
		
		return Mathf.Sqrt(x * x + y * y);

	}
	public Vec2 Normalized()
    {
		Vec2 temp = new Vec2(x, y);
		temp.Normalize();
		return temp;
    }
	public void Normalize()
    {
		float len = Length();
		if(len != 0)
        {

		x = x / len;
		y = y / len;
        }
    }
	public void SetXY(float px, float py)
    {
		this.x = px;
		this.y = py;
    }

	public float DistanceTo(Vec2 v)
    {
		float dx = v.x - x;
		float dy = v.y - y;
		return Mathf.Sqrt(dx * dx + dy * dy);
    }

	public static float Rad2Deg(float d) => d * (180 / Mathf.PI);
	public static float Deg2Rad(float d)
    {
		return d * (Mathf.PI / 180);
    }
	public static Vec2 GetUnitVectorDeg(float deg)
    {
		float px = Mathf.Cos(Deg2Rad(deg));
		float py = Mathf.Sin(Deg2Rad(deg));
		return new Vec2(px, py);
    }
	public static Vec2 GetUnitVectorRad(float rad) => new Vec2(Mathf.Cos(rad), Mathf.Sin(rad));
	public static Vec2 RandomUnitVector() => GetUnitVectorDeg(Utils.Random(0, 360));

	public void SetAngleDegrees(float deg)
    {
		float len = Length();
		x = Mathf.Cos(Deg2Rad(deg)) * len;
		y = Mathf.Sin(Deg2Rad(deg)) * len;
    }
	public void SetAngleRadians(float rad)
    {
		float len = Length();
		x = Mathf.Cos(rad) * len;
		y = Mathf.Sin(rad) * len;

    }

	public float GetAngleRadians() => Mathf.Atan2(y, x);
	public float GetAngleDegrees() => Rad2Deg(GetAngleRadians());

	public void RotateRadians(float rad)
    {
		float sin = Mathf.Sin(rad);
		float cos = Mathf.Cos(rad);

		float prevX = x;
		float prevY = y;
		x = prevX * cos - prevY * sin;
		y = prevX * sin + prevY * cos;
	}
	public void RotateDegrees(float deg) => RotateRadians(Deg2Rad(deg));

	public void RotateAroundRadians(float rad, Vec2 point) => RotateAroundDegrees(Rad2Deg(rad), point);
	public void RotateAroundDegrees(float deg, Vec2 point)
    {
		Vec2 translatedPos = this - point;
		translatedPos.RotateDegrees(deg);
		translatedPos += point;
		this = translatedPos;
    }
	public Vec2 Normal()
    {
        Vec2 normal = new Vec2(-y,x);
		return normal.Normalized();
    }

    public float Dot(Vec2 v2)
    {

		return x * v2.x + y * v2.y;
    }

	public void Reflect(Vec2 refOver, float bounciness = 1)
    {
		Vec2 reflected = this - (1 + bounciness) * (this.Dot(refOver)) * refOver;
		this = reflected;
    }

	public static Vec2 operator -(Vec2 v1, Vec2 v2) => new Vec2(v1.x - v2.x, v1.y - v2.y);

	public static Vec2 operator *(float f, Vec2 v2) => new Vec2(f * v2.x,f*v2.y);

	public static Vec2 operator *(Vec2 v1, float f) => new Vec2(f * v1.x, f * v1.y);

	public static Vec2 operator /(Vec2 v1, float f) => new Vec2(v1.x / f, v1.y / f);

	public static Vec2 operator+ (Vec2 left, Vec2 right) {
		return new Vec2(left.x+right.x, left.y+right.y);
	}

	public override string ToString () 
	{
		return String.Format ("({0},{1})", x, y);
	}
}

