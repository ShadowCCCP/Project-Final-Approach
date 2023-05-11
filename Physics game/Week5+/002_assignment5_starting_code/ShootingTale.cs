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
        Vec2 playerPos;
        bool doOnce;
        Bullet bullet;
        int maxCollisions;

        public ShootingTale(Vec2 pPosition, int pMaxCollisions, string filename = "RatTail.png", int cols = 1, int rows = 1) : base(filename, cols, rows)
        {
            SetOrigin(width - 117, height - 28);
            playerPos = pPosition;
            maxCollisions = pMaxCollisions;
        }

        private void Update()
        {
            Shoot();
            FollowMouse();
        }

        private void FollowMouse()
        {
            
            Vec2 mousePos = new Vec2(Input.mouseX, Input.mouseY);
            Vec2 cPos = mousePos - playerPos;
            //onsole.WriteLine(rotation);
            float targetRotation = cPos.GetAngleDegrees() + 45;
            float distance = (targetRotation - rotation + 540) % 360 - 180;

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
                rotation = targetRotation;
            }
        }

        private void Shoot()
        {
            Vec2 velocityDirection = new Vec2(143, -91);
            Vec2 laserTip = new Vec2(width - 25, -height + 25);
            Vec2 currentPos = new Vec2(x, y);

            Vec2 spawnPosition = currentPos + laserTip.RotateDegrees(rotation);
            Vec2 velocity = velocityDirection.RotateDegrees(rotation).Normalized() * 40;

            Gizmos.DrawRay(spawnPosition.x, spawnPosition.y, velocity.x * 10, velocity.y * 10, null, 0xffff0000);

            if (Input.GetMouseButton(0) && !doOnce) //left click
            {
                bullet = new Bullet(spawnPosition, velocity, maxCollisions);
                game.AddChild(bullet);
                doOnce = true;
            }
            else if (doOnce)
            {
                if(bullet.collisions >= bullet.maxCollisions)
                {
                    myGame.RemoveBall(bullet);
                    bullet.LateDestroy();
                    doOnce = false;
                }
            }
        }
    }
}
