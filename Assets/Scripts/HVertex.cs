using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//颜色 RGBA
public class HColor
{
    public float r, g, b, a;
};

//纹理坐标 uc
public class HTexcoord
{
    public float u, v;
};

public class HVertex
{
    public HVector pos = new HVector();
    public HTexcoord uv = new HTexcoord();
    public HColor color = new HColor();
    public float rhw;

    public HVertex()
    {

    }

    public HVertex(float x, float y, float z, float w, float u, float v, float r, float g, float b, float rhwp)
    {
        pos.x = x;
        pos.y = y;
        pos.z = z;
        pos.w = w;
        uv.u = u;
        uv.v = v;
        color.r = r;
        color.g = g;
        color.b = b;
        color.a = 1;
        rhw = rhwp;
    }


    public HVertex Copy()
    {
        HVertex HVertexRet = new HVertex();

        HVertexRet.pos.x = pos.x;
        HVertexRet.pos.y = pos.y;
        HVertexRet.pos.z = pos.z;
        HVertexRet.pos.w = pos.w;
        HVertexRet.uv.u = uv.u;
        HVertexRet.uv.v = uv.v;
        HVertexRet.color.r = color.r;
        HVertexRet.color.g = color.g;
        HVertexRet.color.b = color.b;
        HVertexRet.color.a = color.a;
        HVertexRet.rhw = rhw;

        return HVertexRet;
    }

    //屏幕坐标的三角形插值的时候要初始化 rhw 做透视校正用 真正取值的时候乘以w 
    public void Initrhw()
    {
        rhw = 1.0f / pos.w;

        uv.u *= rhw;
        uv.v *= rhw;

        color.r *= rhw;
        color.g *= rhw;
        color.b *= rhw;
        color.a *= rhw;
    }


    //插值屏幕坐标的顶点信息
    public HVertex InterpVertex(HVertex vertex, float t)
    {
        HVertex HVertexRet = new HVertex();
        HVertexRet.pos = pos.InterpVec(vertex.pos, t);
        HVertexRet.uv.u = MathHelper.Interp(uv.u, vertex.uv.u, t);
        HVertexRet.uv.v = MathHelper.Interp(uv.v, vertex.uv.v, t);
        HVertexRet.color.r = MathHelper.Interp(color.r, vertex.color.r, t);
        HVertexRet.color.g = MathHelper.Interp(color.g, vertex.color.g, t);
        HVertexRet.color.b = MathHelper.Interp(color.b, vertex.color.b, t);
        HVertexRet.rhw = MathHelper.Interp(rhw, vertex.rhw, t);
        return HVertexRet;
    }

    // Step 1/d 的步伐 
    public HVertex Step(HVertex vertex, float d)
    {
        HVertex HVertexRet = new HVertex();
        if (d == 0.0f)
        {
            return HVertexRet;
        }

        float inv = 1.0f / d;
        HVertexRet.pos.x = (vertex.pos.x - pos.x) * inv;
        HVertexRet.pos.y = (vertex.pos.y - pos.y) * inv;
        HVertexRet.pos.z = (vertex.pos.z - pos.z) * inv;
        HVertexRet.pos.w = (vertex.pos.w - pos.w) * inv;
        HVertexRet.uv.u = (vertex.uv.u - uv.u) * inv;
        HVertexRet.uv.v = (vertex.uv.v - uv.v) * inv;
        HVertexRet.color.r = (vertex.color.r - color.r) * inv;
        HVertexRet.color.g = (vertex.color.g - color.g) * inv;
        HVertexRet.color.b = (vertex.color.b - color.b) * inv;
        HVertexRet.rhw = (vertex.rhw - rhw) * inv;

        return HVertexRet;
    }

    //顶点加法
    public HVertex Add(HVertex vertex)
    {
        HVertex HVertexRet = new HVertex();
        HVertexRet.pos.x = pos.x + vertex.pos.x;
        HVertexRet.pos.y = pos.y + vertex.pos.y;
        HVertexRet.pos.z = pos.z + vertex.pos.z;
        HVertexRet.pos.w = pos.w + vertex.pos.w;
        HVertexRet.rhw = rhw + vertex.rhw;
        HVertexRet.uv.u = uv.u + vertex.uv.u;
        HVertexRet.uv.v = uv.v + vertex.uv.v;
        HVertexRet.color.r = color.r + vertex.color.r;
        HVertexRet.color.g = color.g + vertex.color.g;
        HVertexRet.color.b = color.b + vertex.color.b;

        return HVertexRet;
    }
}
