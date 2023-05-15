using System;
using GXPEngine;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime;
using System.Reflection.Emit;

public class MyGame : Game
{
    private string nextLevel;

    public float boundaryLeft;
    public float boundaryRight;

    public bool ButtonPressed=false;
    public bool BouncyPlatformAnim=false;


    // Tiled loading
    List<Square> _squares;
    List<BallNew> _balls;
    List<AngledLine> _angles;

    public SoundCollection soundCollection;

    /**/
    public MyGame() : base(6144, 4096, false, false, 1536, 1024)
    {
        soundCollection = new SoundCollection();
        

        // Tiled loading
        LoadLevel("Level1.tmx");
        OnAfterStep += CheckLoadLevel;
        _balls = new List<BallNew>();
        _squares = new List<Square>();
        _angles = new List<AngledLine>();
    }
    //*/

    private void DestroyAll()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.Destroy();
        }
        _balls.Clear();
        _squares.Clear();
        _angles.Clear();
    }

    public void LoadLevel(string filename)
    {
        nextLevel = filename;
        musicChanger(filename);
    }

    void musicChanger(string cLevel)
    {
        soundCollection.StopMusic();
        cLevel = cLevel.Substring(cLevel.IndexOf(".") - 1);
        cLevel = cLevel.Remove(cLevel.IndexOf("."));

        int currentLevel = Convert.ToInt32(cLevel);
        switch (currentLevel)
        {
            case 1:
                {
                    soundCollection.PlayMusic(13);
                    break;
                }
            case 2:
                {
                    soundCollection.PlaySound(14);
                    break;
                }
            case 3:
                {
                    soundCollection.PlaySound(15);
                    break;
                }

        }
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

    public void AddSquare(Square square)
    {
        _squares.Add(square);
    }

    public int NumberOfSquares()
    {
        return _squares.Count;
    }

    public void RemoveSquare(Square square)
    {
        _squares.Remove(square);
    }

    public Square GetSquare(int index)
    {
        if (index >= 0 && index < _squares.Count)
        {
            return _squares[index];
        }
        return null;
    }

    public void AddBall(BallNew ball)
    {
        _balls.Add(ball);
    }

    public int NumberOfBalls()
    {
        return _balls.Count;
    }
    public void RemoveBallNew(BallNew ball)
    {
        _balls.Remove(ball);
    }


    public BallNew GetBall(int index)
    {
        if (index >= 0 && index < _balls.Count)
        {
            return _balls[index];
        }
        return null;
    }

    public void RemoveBall(BallNew ball)
    {
        _balls.Remove(ball);
    }

    public void AddAngle(AngledLine angle)
    {
        _angles.Add(angle);
    }

    public int NumberOfAngles()
    {
        return _angles.Count;
    }

    public AngledLine GetAngle(int index)
    {
        if (index >= 0 && index < _angles.Count)
        {
            return _angles[index];
        }
        return null;
    }

	static void Main() {
		new MyGame().Start();
	}
}