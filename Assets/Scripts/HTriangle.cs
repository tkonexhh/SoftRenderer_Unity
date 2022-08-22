using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//屏幕三角形
public class HTriangle
{
    //三个顶点的数据
    public HVertex p1InObjectSpace, p2InObjectSpace, p3InObjectSpace;
    //中间忽略了世界坐标、相机坐标 直接乘MVP就得到ClipSpace坐标

    //三个顶点坐标转化的裁剪空间的坐标
    public HVertex p1InClipSpace, p2InClipSpace, p3InClipSpace;
    //三个顶点坐标转化的屏幕空间的坐标
    public HVertex p1InScreenSpace, p2InScreenSpace, p3InScreenSpace;

    public int CalculateTrap(List<HTrapezoid> trapezoid)
    {
        HVertex p;
        float k, x;

        //顶点排序
        if (p1InScreenSpace.pos.y > p2InScreenSpace.pos.y)
        {
            p = p1InScreenSpace;
            p1InScreenSpace = p2InScreenSpace;
            p2InScreenSpace = p;
        }

        if (p1InScreenSpace.pos.y > p3InScreenSpace.pos.y)
        {
            p = p1InScreenSpace;
            p1InScreenSpace = p3InScreenSpace;
            p3InScreenSpace = p;
        }

        if (p2InScreenSpace.pos.y > p3InScreenSpace.pos.y)
        {
            p = p2InScreenSpace;
            p2InScreenSpace = p3InScreenSpace;
            p3InScreenSpace = p;
        }
        if (p1InScreenSpace.pos.y == p2InScreenSpace.pos.y && p1InScreenSpace.pos.y == p3InScreenSpace.pos.y)
            return 0;
        if (p1InScreenSpace.pos.x == p2InScreenSpace.pos.x && p1InScreenSpace.pos.x == p3InScreenSpace.pos.x)
            return 0;

        if (p1InScreenSpace.pos.y == p2InScreenSpace.pos.y)
        {   // triangle down
            if (p1InScreenSpace.pos.x > p2InScreenSpace.pos.x)
            {
                p = p1InScreenSpace;
                p1InScreenSpace = p2InScreenSpace;
                p2InScreenSpace = p;
            }
            trapezoid[0].top = p1InScreenSpace.pos.y;
            trapezoid[0].bottom = p3InScreenSpace.pos.y;
            trapezoid[0].left.v1 = p1InScreenSpace;
            trapezoid[0].left.v2 = p3InScreenSpace;
            trapezoid[0].right.v1 = p2InScreenSpace;
            trapezoid[0].right.v2 = p3InScreenSpace;
            return (trapezoid[0].top < trapezoid[0].bottom) ? 1 : 0;
        }

        if (p2InScreenSpace.pos.y == p3InScreenSpace.pos.y)
        {   // triangle up
            if (p2InScreenSpace.pos.x > p3InScreenSpace.pos.x)
            {
                p = p2InScreenSpace;
                p2InScreenSpace = p3InScreenSpace;
                p3InScreenSpace = p;
            }
            trapezoid[0].top = p1InScreenSpace.pos.y;
            trapezoid[0].bottom = p3InScreenSpace.pos.y;
            trapezoid[0].left.v1 = p1InScreenSpace;
            trapezoid[0].left.v2 = p2InScreenSpace;
            trapezoid[0].right.v1 = p1InScreenSpace;
            trapezoid[0].right.v2 = p3InScreenSpace;
            return (trapezoid[0].top < trapezoid[0].bottom) ? 1 : 0;
        }

        trapezoid[0].top = p1InScreenSpace.pos.y;
        trapezoid[0].bottom = p2InScreenSpace.pos.y;
        trapezoid[1].top = p2InScreenSpace.pos.y;
        trapezoid[1].bottom = p3InScreenSpace.pos.y;

        k = (p3InScreenSpace.pos.y - p1InScreenSpace.pos.y) / (p2InScreenSpace.pos.y - p1InScreenSpace.pos.y);
        x = p1InScreenSpace.pos.x + (p2InScreenSpace.pos.x - p1InScreenSpace.pos.x) * k;

        if (x <= p3InScreenSpace.pos.x)
        {       // triangle left
            trapezoid[0].left.v1 = p1InScreenSpace;
            trapezoid[0].left.v2 = p2InScreenSpace;
            trapezoid[0].right.v1 = p1InScreenSpace;
            trapezoid[0].right.v2 = p3InScreenSpace;
            trapezoid[1].left.v1 = p2InScreenSpace;
            trapezoid[1].left.v2 = p3InScreenSpace;
            trapezoid[1].right.v1 = p1InScreenSpace;
            trapezoid[1].right.v2 = p3InScreenSpace;
        }
        else
        {                   // triangle right
            trapezoid[0].left.v1 = p1InScreenSpace;
            trapezoid[0].left.v2 = p3InScreenSpace;
            trapezoid[0].right.v1 = p1InScreenSpace;
            trapezoid[0].right.v2 = p2InScreenSpace;
            trapezoid[1].left.v1 = p1InScreenSpace;
            trapezoid[1].left.v2 = p3InScreenSpace;
            trapezoid[1].right.v1 = p2InScreenSpace;
            trapezoid[1].right.v2 = p3InScreenSpace;
        }
        return 2;
    }
}
