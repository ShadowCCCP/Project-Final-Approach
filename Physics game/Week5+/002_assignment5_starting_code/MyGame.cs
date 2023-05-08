using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime;
using System.Reflection.Emit;

public class MyGame : Game
{
    private string nextLevel;
    bool _stepped = false;
	bool _paused = false;
	int _stepIndex = 0;

	Canvas _lineContainer = null;

	public List<Ball> _balls;
    List<LineSegment> _lines;
    List<Block> _blocks;

	public float boundaryLeft;
	public float boundaryRight;

    public MyGame() : base(800, 600, false, false)
    {
        // Tiled loading
        LoadLevel("MainMenu.tmx");
        OnAfterStep += CheckLoadLevel;



        _lineContainer = new Canvas(width, height);
        AddChild(_lineContainer);

        targetFps = 60;

        _balls = new List<Ball>();
        _lines = new List<LineSegment>();
        _blocks = new List<Block>();

        LoadScene();

        PrintInfo();
    }




    // Tiled loading
    private void DestroyAll()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.Destroy();
        }
    }

    public void LoadLevel(string filename)
    {
        nextLevel = filename;
    }

    private void CheckLoadLevel()
    {
        if (nextLevel != null)
        {
            DestroyAll();
            AddChild(new Level(nextLevel));
            nextLevel = null;
        }
    }








    void LoadScene()
    {
        // remove previous scene:
        foreach (Ball mover in _balls)
        {
            mover.Destroy();
        }
        _balls.Clear();
        foreach (LineSegment line in _lines)
        {
            line.Destroy();
        }
        _lines.Clear();
        foreach (Block block in _blocks)
        {
            block.Destroy();
        }
        _blocks.Clear();

        // Static Balls
        AddTargetBall(500, 250, 20, 3); // right on line segment
        AddTargetBall(100, 250, 20, 4); // top center 
        AddTargetBall(450, 75, 20, 2); // middle left
        // Blocks
        AddBlock(20, new Vec2(600, 250)); // middle right
        AddBlock(20, new Vec2(300, 150), 5); // left on line segment
        AddBlock(20, new Vec2(200, 400), 2); // bottom left
        AddBlock(20, new Vec2(700, 100), 1); // top right
        AddBlock(20, new Vec2(100, 100), 4); // top left
        // Boundary lines
        AddLine(new Vec2(20, height - 20), new Vec2(20, 20)); // left
        AddLine(new Vec2(20, 20), new Vec2(width - 20, 20)); // top
        AddLine(new Vec2(width - 20, 20), new Vec2(width - 20, height - 20));  //right
        boundaryLeft = 20;
        boundaryRight = width - 20;
        // angle top
        AddLine(new Vec2(120, 20), new Vec2(20, 120)); // left-angle
        AddLine(new Vec2(width - 120, 20), new Vec2(width - 20, 120)); // right-angle
        // Angled linesegment
        AddLine(new Vec2(290, 250), new Vec2(455, 350));
        // Moving Line (Player)
        AddMovingLine(new Vec2(550, 450), new Vec2(550, 550));

        foreach (Ball b in _balls)
        {
            AddChild(b);
        }
    }

    public int GetNumberOfBlocks()
    {
        return _blocks.Count;
    }

    public Block GetBlock(int index)
    {
        if (index >= 0 && index < _blocks.Count)
        {
            return _blocks[index];
        }
        return null;
    }

	public void RemoveBlock(Block block)
	{
		_blocks.Remove(block);
	}

	public void RemoveBall(Ball ball)
	{
		_balls.Remove(ball);
	}

    public int GetNumberOfLines()
    {
        return _lines.Count;
    }

    public LineSegment GetLine(int index)
    {
        if (index >= 0 && index < _lines.Count)
        {
            return _lines[index];
        }
        return null;
    }

    public int GetNumberOfBalls()
    {
        return _balls.Count;
    }

    public Ball GetBalls(int index)
    {
        if (index >= 0 && index < _balls.Count)
        {
            return _balls[index];
        }
        return null;
    }

    public void DrawLine(Vec2 start, Vec2 end)
    {
        _lineContainer.graphics.DrawLine(Pens.White, start.x, start.y, end.x, end.y);
    }

    private void AddBlock(int radius, Vec2 position, int live = 3)
	{
		Block block = new Block(radius, position, live);
		AddChild(block);
		_blocks.Add(block);
	}

    private void AddTargetBall(float x, float y, int radius, int live)
    {
        BallTarget ball = new BallTarget(radius, new Vec2(x, y), live);
        AddChild(ball);
        _balls.Add(ball);
    }

    void AddLine (Vec2 start, Vec2 end) {
		LineSegment line = new LineSegment (start, end, 0xff00ff00, 4);
		AddChild (line);
		_lines.Add (line);
	}

	void AddMovingLine(Vec2 start, Vec2 end)
    {
        MovingLine line = new MovingLine(start, end, 0xff00ff00, 4);
        AddChild(line);
        _lines.Add(line);
    }

	/****************************************************************************************/

	void PrintInfo() {
		Console.WriteLine("Hold spacebar to slow down the frame rate.");
		Console.WriteLine("Press S to toggle stepped mode.");
		Console.WriteLine("Press P to toggle pause.");
		Console.WriteLine("Press 1 to draw debug lines.");
		Console.WriteLine("Press C to clear all debug lines.");
		Console.WriteLine("Press R to reset scene, and numbers to load different scenes.");
        Console.WriteLine("Press 0 to toggle mouse-target aiming");
	}

	void HandleInput() {
		targetFps = Input.GetKey(Key.SPACE) ? 5 : 60;

		if (Input.GetKeyDown (Key.S)) {
			_stepped ^= true;
		}
		if (Input.GetKeyDown (Key.ONE)) {
			Ball.drawDebugLine ^= true;
		}
		if (Input.GetKeyDown (Key.P)) {
			_paused ^= true;
		}
		if (Input.GetKeyDown (Key.C)) {
			_lineContainer.graphics.Clear (Color.Black);
		}
		if (Input.GetKeyDown (Key.R)) {
			LoadScene();
		}
	}

	void StepThroughMovers() {
		if (_stepped) { // move everything step-by-step: in one frame, only one mover moves
			_stepIndex++;
			if (_stepIndex >= _balls.Count) {
				_stepIndex = 0;
			}
			if (_balls [_stepIndex].moving) {
				_balls [_stepIndex].Step ();
			}
		} else { // move all movers every frame
			foreach (Ball mover in _balls) {
				if (mover.moving) {
					mover.Step ();
				}
			}
		}
	}

	void Update () {
		HandleInput();
		if (!_paused) {
			StepThroughMovers ();
		}
    }

	static void Main() {
		new MyGame().Start();
	}
}