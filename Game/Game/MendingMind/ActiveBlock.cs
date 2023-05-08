using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

    public class ActiveBlock: GameObject
    {

    const int BLOCK_SQUARE = 1;
    const int RIGHT_SLOPE = 2;
    const int LEFT_SLOPE = 3;
    const int TOP_RIGHT_SLOPE = 4;
    const int TOP_LEFT_SLOPE = 5;
    public int index = BLOCK_SQUARE;

    public ActiveBlock()
    {

        
    }

    void Update()
    {

        if (Input.GetKeyDown(Key.Q))
        {
            index = BLOCK_SQUARE;
            //Console.WriteLine("Switched to block");
        }
        if (Input.GetKeyDown(Key.W))
        {
            index = RIGHT_SLOPE;
            //Console.WriteLine("Switched to right slope");
        }
        if (Input.GetKeyDown(Key.E))
        {
            index = LEFT_SLOPE;
            //Console.WriteLine("Switched to left slope");
        }
        if (Input.GetKeyDown(Key.R))
        {
            index = TOP_RIGHT_SLOPE;
            //Console.WriteLine("Switched to right slope");
        }
        if (Input.GetKeyDown(Key.T))
        {
            index = TOP_LEFT_SLOPE;
            //Console.WriteLine("Switched to left slope");
        }


    }
}

