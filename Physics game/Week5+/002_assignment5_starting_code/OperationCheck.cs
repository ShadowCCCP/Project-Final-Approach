using System;

namespace GXPEngine
{
    public class OperationCheck
    {
        public OperationCheck()
        {
            // Week 1
            Console.WriteLine("\nWeek 1 \n----------------------");
            // default (2, 3)
            Vec2 myVec = new Vec2(2, 3);

            // Multiplication
            Vec2 result = myVec * 3;
            Console.WriteLine("Scalar multiplication right ok?: " +
             (result.x == 6 && result.y == 9 && myVec.x == 2 && myVec.y == 3));
            result = 4 * myVec;
            Console.WriteLine("Scalar multiplication left ok?: " +
             (result.x == 8 && result.y == 12 && myVec.x == 2 && myVec.y == 3));

            // Addition
            result = myVec + myVec;
            Console.WriteLine("Addition with Vec2 ok?: " +
             (result.x == 4 && result.y == 6 && myVec.x == 2 && myVec.y == 3));

            result = myVec + 5;
            Console.WriteLine("Addition with float ok?: " +
             (result.x == 7 && result.y == 8 && myVec.x == 2 && myVec.y == 3));

            // Subtraction
            result = myVec * 2 - myVec;
            Console.WriteLine("Subtraction with Vec2 ok?: " +
             (result.x == 2 && result.y == 3 && myVec.x == 2 && myVec.y == 3));

            result = myVec * 2 - 5;
            Console.WriteLine("Subtraction with float ok?: " +
             (result.x == -1 && result.y == 1 && myVec.x == 2 && myVec.y == 3));


            myVec = new Vec2(4, 3);
            // Get Length
            Console.WriteLine("Length: {0} (should be 5) ok?: {1}", myVec.Length(),
                myVec.Length() == 5);

            // Return normalized vector
            Console.WriteLine("Normalized: {0} (should be (0.8, 0,6)) ok?: {1}", myVec.Normalized(),
                myVec.Normalized().Approximate(new Vec2(0.8f, 0.6f)));

            // Make vector normalized
            myVec.Normalize();
            Console.WriteLine("Setting vector to normalized: {0} (should be (0.8, 0.6)) ok?: {1}", myVec,
                myVec.Approximate(new Vec2(0.8f, 0.6f)));

            // Set XY of Vec2
            myVec.SetXY(100, 100);
            Console.WriteLine("Vector X: {0} / Y: {1} (should be (100, 100)) ok?: {2}", myVec.x, myVec.y,
                myVec.Approximate(new Vec2(100, 100)));



            // Week 2
            Console.WriteLine("\nWeek 2 \n----------------------");

            // Degrees to Radians
            float degRad = 180;
            Console.WriteLine("Radians: {0} (should be 1 PI) ok?: {1}", Vec2.Deg2Rad(degRad),
                Vec2.Deg2Rad(degRad) == Mathf.PI);

            // Radians to Degrees
            degRad = 1;
            Console.WriteLine("Degrees: {0} (should be 180) ok?: {1}", Vec2.Rad2Deg(degRad),
                Vec2.Rad2Deg(degRad) == 180);

            // Return Vector that points in given direction in degrees
            degRad = 90;
            Console.WriteLine("Vector direction by degrees: {0} (should be (0, 1)) ok?: {1}", Vec2.GetUnitVectorDeg(degRad),
                Vec2.GetUnitVectorDeg(degRad).Approximate(new Vec2(0, 1)));

            // Return Vector that points in given direction in radians
            degRad = 0.5f;
            Console.WriteLine("Vector direction by radians: {0} (should be (0, 1)) ok?: {1}", Vec2.GetUnitVectorRad(degRad),
                Vec2.GetUnitVectorRad(degRad).Approximate(new Vec2(0, 1)));

            // Return vector pointing into a random direction
            Console.WriteLine("Vector direction random: {0}", Vec2.RandomUnitVector());

            // Set angle of vector by degrees
            myVec = new Vec2(0, -1);
            degRad = 180;
            myVec.SetAngleDegrees(degRad);
            Console.WriteLine("Vector angle set by degrees: {0} (should be (-1, 0)) ok?: {1}", myVec,
                myVec.Approximate(new Vec2(-1, 0)));

            // Set angle of vector by radians
            myVec = new Vec2(0, -1);
            degRad = 1;
            myVec.SetAngleRadians(degRad);
            Console.WriteLine("Vector angle set by radians: {0} (should be (-1, 0)) ok?: {1}", myVec,
                myVec.Approximate(new Vec2(-1, 0)));

            // Get angle of vector in radians
            myVec = new Vec2(0, 1);
            Console.WriteLine("Get vector angle in radians: {0} (should be (0.5)) ok?: {1}", myVec.GetAngleRadians(),
                myVec.GetAngleRadians() == 0.5);

            // Get angle of vector in degrees
            Console.WriteLine("Get vector angle in degrees: {0} (should be (90)) ok?: {1}", myVec.GetAngleDegrees(),
                myVec.GetAngleDegrees() == 90);

            // Rotate vector by degrees
            degRad = 270;
            Console.WriteLine("Rotate vector by degrees: {0} (should be (1, 0)) ok?: {1}", myVec.RotateDegrees(degRad),
                myVec.RotateDegrees(degRad).Approximate(new Vec2(1, 0)));

            // Rotate vector by radians
            degRad = 1.5f;
            Console.WriteLine("Rotate vector by radians: {0} (should be (1, 0)) ok?: {1}", myVec.RotateRadians(degRad),
                myVec.RotateRadians(degRad).Approximate(new Vec2(1, 0)));

            // Rotate vector around point by degrees
            myVec = new Vec2(4, 6);
            Vec2 other = new Vec2(2, 1);
            degRad = 90;

            Console.WriteLine("Rotate vector around point by degrees: {0} (should be (-3, 3)) ok?: {1}", myVec.RotateAroundDegrees(other, degRad),
                myVec.Approximate(new Vec2(-3, 3)));

            // Rotate vector around point by radians
            myVec = new Vec2(4, 6);
            other = new Vec2(2, 1);
            degRad = 0.5f;
            Console.WriteLine("Rotate vector around point by radians: {0} (should be (-3, 3)) ok?: {1}", myVec.RotateAroundRadians(other, degRad),
                myVec.Approximate(new Vec2(-3, 3)));



            // Week 4
            Console.WriteLine("\nWeek 4 \n----------------------");

            // Dot product
            myVec = new Vec2(8, 6);
            other = new Vec2(11, 2);
            Console.WriteLine("Dot product of current and other vector: {0} (should be 100) ok?: {1}", myVec.Dot(other),
                myVec.Dot(other) == 100);

            // Normal
            myVec = new Vec2(8, 6);
            Console.WriteLine("Getting unit normal of vector: {0} (should be (-6, 8)) ok?: {1}", myVec.Normal(),
                myVec.Normal().Approximate(new Vec2(-0.6f, 0.8f)));

            myVec = new Vec2(0, 5);
            other = new Vec2(400, 300);
            myVec.Reflect(other.Normal());
            Console.WriteLine("Reflecting vector depending on line normal: {0} (should be (4.8, -1.4)) ok?: {1}", myVec,
                myVec.Approximate(new Vec2(4.8f, -1.4f)));

            Console.WriteLine("\n----------------------\nEND OF OPERATIONCHECK\n----------------------\n");
        }
    }
}