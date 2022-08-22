using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//向量或者是点都是这个类便是
public class HVector
{
    //表示向量时w无用  w=0 向量 w=1 点
    public float x, y, z, w;

    public HVector()
    {
        w = 1;
    }

    public HVector(float xp, float yp, float zp, float wp)
    {
        x = xp;
        y = yp;
        z = zp;
        w = wp;
    }

    //向量长度
    public float Length() { return (float)Math.Sqrt(x * x + y * y + z * z); }


    //向量加法
    public HVector Add(HVector vec)
    {
        HVector vecRet = new HVector();
        vecRet.w = 1;
        vecRet.x = x + vec.x;
        vecRet.y = y + vec.y;
        vecRet.z = z + vec.z;
        return vecRet;
    }

    //向量减法
    public HVector Sub(HVector vec)
    {
        HVector vecRet = new HVector();
        vecRet.w = 1;
        vecRet.x = x - vec.x;
        vecRet.y = y - vec.y;
        vecRet.z = z - vec.z;
        return vecRet;
    }

    //向量点乘 返回是值 A x B = |A||B|Cos 
    public float DotProduct(HVector vec)
    {
        return x * vec.x + y * vec.y + z * vec.z;
    }

    //向量叉乘 返回向量 右手螺旋决定方向
    public HVector CrossProduct(HVector vec)
    {
        HVector vecRet = new HVector();
        float m1, m2, m3;
        m1 = y * vec.z - z * vec.y;
        m2 = z * vec.x - x * vec.z;
        m3 = x * vec.y - y * vec.x;
        vecRet.x = m1;
        vecRet.y = m2;
        vecRet.z = m3;
        vecRet.w = 1.0f;
        return vecRet;
    }

    //向量插值
    public HVector InterpVec(HVector vec, float t)
    {
        HVector vecRet = new HVector();
        vecRet.x = MathHelper.Interp(x, vec.x, t);
        vecRet.y = MathHelper.Interp(y, vec.y, t);
        vecRet.z = MathHelper.Interp(z, vec.z, t);
        vecRet.w = 1.0f;
        return vecRet;
    }

    //向量归一
    public HVector Normalize()
    {
        HVector vecRet = new HVector();
        float len = Length();
        if (len != 0.0f)
        {
            vecRet.x = x / len;
            vecRet.y = y / len;
            vecRet.z = z / len;
        }
        return vecRet;
    }

    //向量乘矩阵
    public HVector MulMatrix(HMatrix mat)
    {
        HVector vec = new HVector();
        float X = x, Y = y, Z = z, W = w;
        vec.x = X * mat.m[0, 0] + Y * mat.m[1, 0] + Z * mat.m[2, 0] + W * mat.m[3, 0];
        vec.y = X * mat.m[0, 1] + Y * mat.m[1, 1] + Z * mat.m[2, 1] + W * mat.m[3, 1];
        vec.z = X * mat.m[0, 2] + Y * mat.m[1, 2] + Z * mat.m[2, 2] + W * mat.m[3, 2];
        vec.w = X * mat.m[0, 3] + Y * mat.m[1, 3] + Z * mat.m[2, 3] + W * mat.m[3, 3];
        return vec;
    }

    /// <summary>
    ///  检查齐次裁剪坐标 cvv canonical view volume
    /// </summary>
    /// <returns></returns>
    public bool CheckInCVV()
    {
        int check = 0;
        if (z < 0.0f) check |= 1;
        if (z > w) check |= 2;
        if (x < -w) check |= 4;
        if (x > w) check |= 8;
        if (y < -w) check |= 16;
        if (y > w) check |= 32;
        return check == 0;
    }


    public static HVector up => new HVector(0, 1, 0, 1);

}
