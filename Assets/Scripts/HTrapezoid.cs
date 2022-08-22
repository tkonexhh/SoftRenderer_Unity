using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//边 线段
public class HEdge
{
    //起点 终点
    public HVertex v1, v2;
    public HVertex v;//临时变量
};

//梯形 有个理论是 所有的三角形都可以拆分成一个平底三角形和平顶三角形
public class HTrapezoid
{
    public float top, bottom;
    public HEdge left = new HEdge();
    public HEdge right = new HEdge();

    public void EdgeInterp(float y)
    {
        float s1 = left.v2.pos.y - left.v1.pos.y;
        float s2 = right.v2.pos.y - right.v1.pos.y;
        float t1 = (y - left.v1.pos.y) / s1;
        float t2 = (y - right.v1.pos.y) / s2;

        /*
		 *  根据y值左边两个点left v1 v2 插值得到left v  同理right
		 *  
		 *       /--------\
		 *      /          \
		 *     y------------y
		 *    /              \
		 *   /----------------\
		 *   
		 */

        //根据Y坐标 得到左右两边的点
        left.v = left.v1.InterpVertex(left.v2, t1);
        right.v = right.v1.InterpVertex(right.v2, t2);
    }

}
