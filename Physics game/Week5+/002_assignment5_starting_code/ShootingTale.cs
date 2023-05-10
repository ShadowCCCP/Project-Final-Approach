using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    public class ShootingTale : Square
    {
        float rotationSpeed = 5;
        Vec2 position;

        public ShootingTale(Vec2 pPosition, string filename = "DebugPlayer.png", int cols = 1, int rows = 1) : base(filename, cols, rows)
        {
            SetOrigin(width/2, height/2);
            position = pPosition;
        }

        protected override void Update()
        {
            FollowMouse();
        }

        private void FollowMouse()
        {
            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);

            Vec2 cPos = mousePos - (position - height/2);
            Console.WriteLine(rotation);
            float targetRotation = cPos.GetAngleDegrees() - rotation;
            float distance = (targetRotation - rotation + 540) % 360 - 180;
            Console.WriteLine("distance: " + distance);
            if (distance > rotationSpeed)
            {
                rotation += rotationSpeed;
            }
            else if (distance < -rotationSpeed)
            {
                rotation -= rotationSpeed;
            }
            else
            {
                rotation += distance;
            }
        }
    }
}
