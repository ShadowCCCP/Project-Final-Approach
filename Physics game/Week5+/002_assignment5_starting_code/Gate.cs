using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GXPEngine.Core;

namespace GXPEngine
{
    class Gate : LineSegment
    {
        bool trueIfTop;
        bool opening = true;
        public Gate(Vec2 pStart, Vec2 pEnd, bool _trueIfTop, uint pColor = 0xffffffff, uint pLineWidth = 1) : base(pStart, pEnd, pColor = 0xffffffff, pLineWidth = 1)
        {
            start = pStart;
            end = pEnd;
            trueIfTop = _trueIfTop;
        }

        public void OpenGate()
        {
            opening = true;
        }

        void Update()
        {
            if (opening && start.y != end.y)
            {
                if (trueIfTop)
                {
                    end.y--;
                }
                else
                {
                    start.y++;
                }

            }

        }









        //------------------------------------------------------------------------------------------------------------------------
        //                                                        RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        override protected void RenderSelf(GLContext glContext)
        {
            if (game != null)
            {
                Gizmos.RenderLine(start.x, start.y, end.x, end.y, color, lineWidth);
            }
        }
    }
}
