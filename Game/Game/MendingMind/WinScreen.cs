using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

    public class WinScreen: GameObject
    {

    Button levelMenuButton;
    Button exitButton;

    private int buttonSelected = 1;
    public WinScreen()
    {
        Sprite backGround = new Sprite("endlevel.png");
        //AddChild(backGround);

        levelMenuButton = new Button(800, 600, 1, 1, "levels.png");
        AddChild(levelMenuButton);

        exitButton = new Button(1130, 600, 0.8f, 0.8f, "exit.png");
        AddChild(exitButton);
    }

    private void ButtonsSelected()
    {
        if (buttonSelected == 1)
        {
            levelMenuButton.alpha = 1;
            exitButton.alpha = 0.7f;

            if (Input.GetKeyDown(Key.ENTER))
            {
                SceneManager.Instance.loadLevel("Level1");
            }
        }

        if (buttonSelected == 2)
        {
            levelMenuButton.alpha = 0.7f;
            exitButton.alpha = 1;

            if (Input.GetKeyDown(Key.ENTER))
            {
                game.LateDestroy();
            }
        }
    }

    void Update()
    {
        SelectNumber();
        ButtonsSelected();
    }
    public void SelectNumber()
    {
        if (Input.GetKeyDown(Key.LEFT))
        {
            buttonSelected += 1;
        }

        if (Input.GetKeyUp(Key.RIGHT))
        {
            buttonSelected -= 1;
        }

        if (buttonSelected <= 1)
        {
            buttonSelected = 1;
        }

        if (buttonSelected >= 2)
            buttonSelected = 2;
    }
}

    

