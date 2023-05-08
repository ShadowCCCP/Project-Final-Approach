using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;

public class MyGame : Game
{	
	public bool _stepped = false;
	public bool _paused = true;
	public int _stepIndex = 0;
	public int _startSceneNumber = 0;


	public bool GameStarted = false;


	public List<Ball> _movers;
	public List<LineSegment> _lines;
	public List<Sprite> _images;
	public GameObject[] _blockers;

	public SquareBlocker squareBlock;
	ActiveBlock activeBlock;

	SceneManager sceneManager;

	public int GetNumberOfLines() {
		return _lines.Count;
	}

	public LineSegment GetLine(int index) {
		if (index >= 0 && index < _lines.Count) {
			return _lines [index];
		}
		return null;	
	}

	public int GetNumberOfMovers() {
		return _movers.Count;
	}

	public Ball GetMover(int index) {
		if (index >= 0 && index < _movers.Count) {
			return _movers [index];
		}
		return null;
	}
	


	public MyGame () : base(1920, 1080, false, false)
	{
		_blockers = FindObjectsOfType<SquareBlocker>();
		squareBlock = new SquareBlocker(new Vec2(-1000, -1000), 1);
		activeBlock = new ActiveBlock();
		AddChild(activeBlock);

		sceneManager = new SceneManager();
		AddChild(sceneManager);
		targetFps = 60;

		_movers = new List<Ball>();
		_lines = new List<LineSegment>();

		sceneManager.loadLevel("Level1");

	}
	
	public void AddLine (Vec2 start, Vec2 end) {
		LineSegment line = new LineSegment (start, end, 0x00ffffffff, 4);
		AddChild (line);
		_lines.Add (line);
	}

	public void AddBlocker(Vec2 position, int index, bool lvlComp = false)
	{
		SquareBlocker block = new SquareBlocker(position, index, lvlComp);
        foreach (var item in block._pLines)
        {
			_lines.Add(item);
        }
		AddChild(block);

	}

	/****************************************************************************************/



	void HandleInput() {
		if(sceneManager.currentLevel != null)
        {
			targetFps = Input.GetKey(Key.P) ? 5 : 60;
			if (Input.GetKeyDown(Key.SPACE))
			{
				_paused = false;
				
				//sceneManager.currentLevel.blockHUD.LateDestroy();
				GameStarted = true;
			}
            if (Input.GetKey(Key.C))
            {
                Console.WriteLine(_lines.Count);
            }
			if (!_paused)
			{
				if (Input.GetKeyDown(Key.S))
				{
					sceneManager.loadLevel(sceneManager.levelName);
				}
			}


			
		}

	}

	void StepThroughMovers() {
		if (_stepped) { // move everything step-by-step: in one frame, only one mover moves
			_stepIndex++;
			if (_stepIndex >= _movers.Count) {
				_stepIndex = 0;
			}
			if (_movers [_stepIndex].moving) {
				_movers [_stepIndex].Step ();
			}
		} else { // move all movers every frame
			foreach (Ball mover in _movers) {
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