using System;
using GXPEngine.Core;
using GXPEngine.OpenGL;

namespace GXPEngine
{
	/// <summary>
	/// Implements an OpenGL line
	/// </summary>
	public class LineSegment : GameObject
	{
		//public LineSegment ReversedLine { get { return reversedLine; } }
		//LineSegment reversedLine;
		public Vec2 start;
		public Vec2 end;
		public Vec2 normalLine { get { return (end - start).Normal(); } }
		public Ball[] Points = new Ball[2];
		
		Ball startPoint;
		Ball endPoint;


		public float lenght { get { return (end - start).Length(); } }
		public uint color = 0xffffffff;
		public uint lineWidth = 1;
		public bool IsOneSided { get; private set; }


		public LineSegment (float pStartX, float pStartY, float pEndX, float pEndY, uint pColor = 0xffffffff, uint pLineWidth = 1)
			: this (new Vec2 (pStartX, pStartY), new Vec2 (pEndX, pEndY), pColor, pLineWidth)
		{
		}

		public LineSegment (Vec2 pStart, Vec2 pEnd, uint pColor = 0xffffffff, uint pLineWidth = 1, bool pIsOneSided = true)
		{
			IsOneSided=pIsOneSided;
			startPoint = new Ball(1, pStart);
			endPoint = new Ball(1, pEnd);
			Points[0] = startPoint;
			Points[1] = endPoint;

			start = pStart;
			end = pEnd;
			color = pColor;
			lineWidth = pLineWidth;

			//reversedLine = new LineSegment (pEnd, pStart);

		}
	
		//------------------------------------------------------------------------------------------------------------------------
		//														RenderSelf()
		//------------------------------------------------------------------------------------------------------------------------
		override protected void RenderSelf(GLContext glContext) {
			if (game != null) {
				Gizmos.RenderLine(start.x, start.y, end.x, end.y, color, lineWidth);
			}
		}
	}
}

