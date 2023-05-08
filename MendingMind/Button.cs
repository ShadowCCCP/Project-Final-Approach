using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GXPEngine;

public class Button : Sprite
{

    public Button(float _x, float _y, float _scaleX, float _scaleY, string image) : base(image)
    {
        x = _x;
        y = _y;
        alpha = 0.7f;
        scaleX = _scaleX;
        scaleY = _scaleY;

        SetOrigin(width / 2, height / 2);
    }
}

