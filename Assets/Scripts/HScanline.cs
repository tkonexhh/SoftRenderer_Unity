using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//光栅化的时候 三角形遍历的时候去生成图元的过程 用扫描线
public class HScanline
{
    public HVertex v, step;
    public float x, y, width;
};
