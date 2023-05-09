using GXPEngine.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class ShootingBall : Ball
    {
        private MyGame myGame;
        private Vec2 lineEnd;
        private Vec2 lineStart;
        private Vec2 line;
        private float lineRotation;

        public bool doOnce;
        public bool shot;
            
        public ShootingBall(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool moving = true) : base(pRadius, pPosition, pVelocity)
        {
            myGame = (MyGame)game;
        }

        private void Update()
        {
            UpdateGizmo();
        }

        private void UpdateGizmo()
        {
            Gizmos.DrawArrow(x, y, line.Normal().x * 50, line.Normal().y * 50);
            // Pauls line
            Stroke(200, 0, 0);
            Line(0, height / 2, width, height / 2);
        }
    }
}
