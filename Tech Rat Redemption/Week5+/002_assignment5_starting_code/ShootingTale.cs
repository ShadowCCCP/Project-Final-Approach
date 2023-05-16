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
        SoundCollection sounds = new SoundCollection();

        public ShootingTale(Vec2 pPosition, int pMaxCollisions, string filename = "RatTail.png", int cols = 1, int rows = 1) : base(filename, cols, rows)
        {
            myGame.playerLaser = true;
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
            myGame.raySpawnPos = spawnPosition;
            Vec2 velocity = velocityDirection.RotateDegrees(rotation).Normalized() * 80;

            Vec2 rayVelocity = velocityDirection.RotateDegrees(rotation).Normalized() * 120;

            RayBall test = new RayBall(spawnPosition, rayVelocity, 2);
            game.AddChild(test);

            if (Input.GetMouseButton(0) && !doOnce) //left click
            {
                shootSound();
                bullet = new Bullet(spawnPosition, velocity, maxCollisions);
                game.AddChild(bullet);
                doOnce = true;
            }
            else if (doOnce)
            {
                if (bullet.collisionCount >= bullet.maxCollisions)
                {
                    myGame.RemoveBall(bullet);
                    bullet.LateDestroy();
                    doOnce = false;
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    shootSound();
                    myGame.RemoveBall(bullet);
                    bullet.LateDestroy();
                    bullet = new Bullet(spawnPosition, velocity, maxCollisions);
                    game.AddChild(bullet);
                }
            }
        }

        private void shootSound()
        {
            Random rdm = new Random();
            switch (rdm.Next(5))
            {
                case 0:
                    {
                        sounds.PlaySound(4);
                        break;
                    }
                case 1:
                    {
                        sounds.PlaySound(15);
                        break;
                    }
                case 2:
                    {
                        sounds.PlaySound(16);
                        break;
                    }
                case 3: 
                    {
                        sounds.PlaySound(17);
                        break;
                    }
                case 4:
                    {
                        sounds.PlaySound(18);
                        break;
                    }
            }
        }
    }
}