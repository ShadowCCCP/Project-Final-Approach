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
    public bool playerLaser;


    // Tiled loading
    List<Square> _squares;
    List<BallNew> _balls;
    List<AngledLine> _angles;

    public SoundCollection soundCollection;

    public Vec2 mfirst;
    public Vec2 msecond;

    public Vec2 raySpawnPos;

    /**/
    public MyGame() : base(6144, 4096, true, false, 1536, 1024)
    {
        soundCollection = new SoundCollection();

        // Tiled loading
        LoadLevel("Level1.tmx");
        OnAfterStep += CheckLoadLevel;
        _balls = new List<BallNew>();
        _squares = new List<Square>();
        _angles = new List<AngledLine>();

        mfirst = new Vec2(0, 0);
        msecond = new Vec2(0, 0);
    }
    //*/

    private void Update()
    {
        if (!mfirst.Approximate(new Vec2(0, 0)))
        {
            if(playerLaser)
            {
                Gizmos.DrawLine(raySpawnPos.x, raySpawnPos.y, mfirst.x, mfirst.y, null, 0xffff0000);
            }
        }
        if (!msecond.Approximate(new Vec2(0, 0)))
        {
            //Gizmos.DrawLine(mfirst.x, mfirst.y, msecond.x, msecond.y, null, 0xffff0000);
        }
    }

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
        playerLaser = false;
        nextLevel = filename;
        musicChanger(filename);
    }

    void musicChanger(string cLevel)
    {
        soundCollection.StopMusic();
        if(cLevel.Length == 10)
                {
                    cLevel = cLevel.Substring(cLevel.IndexOf(".") - 1);
                }
                else
                {
                    cLevel = cLevel.Substring(cLevel.IndexOf(".") - 2);
                }
        cLevel = cLevel.Remove(cLevel.IndexOf("."));

        int currentLevel = Convert.ToInt32(cLevel);
        Console.Write(currentLevel);
        switch (currentLevel)
        {
            case 1:
                {
                    soundCollection.PlayMusic(34);
                    break;
                }
            case 2:
                {
                    soundCollection.PlayNarration(26);
                    break;
                }
            case 3:
                {
                    soundCollection.PlayNarration(27);
                    break;
                }
            case 4:
                {
                    soundCollection.PlayNarration(28);
                    break;
                }
            case 5:
                {
                    soundCollection.PlayNarration(29);
                    break;
                }
            case 6:
                {
                    soundCollection.PlayNarration(30);
                    break;
                }
            case 7:
                {
                    soundCollection.PlayMusic(13);
                    break;
                }
            case 8:
                {
                    soundCollection.PlayMusic(14, true);
                    break;
                }
            case 10:
                {
                    soundCollection.PlayNarration(31);
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