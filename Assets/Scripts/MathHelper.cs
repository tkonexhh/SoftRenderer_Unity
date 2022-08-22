using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MathHelper
{
    public static void GetRGBAFromInt(int color, ref byte r, ref byte g, ref byte b, ref byte a)
    {
        r = (byte)(color / 255 / 255 / 255);
        g = (byte)((color - r * 255 * 255 * 255) / (255 * 255));
        b = (byte)((color % (255 * 255)) / 255);
        a = (byte)(color % (255));
    }


    //插值函数   t为[0,1]之间
    public static float Interp(float x1, float x2, float t)
    {
        return x1 + (x2 - x1) * t;
    }


    //Clamp函数 Value
    public static int Clamp(int x, int min, int max)
    {
        return (x < min) ? min : ((x > max) ? max : x);
    }



    //获取旋转矩阵 X轴旋转
    // https://blog.csdn.net/csxiaoshui/article/details/65446125
    public static HMatrix GetRotateMatX(float x)
    {
        HMatrix matRet = HMatrix.identity;//单位矩阵
        float SinValue = (float)Math.Sin(x);
        float CosValue = (float)Math.Cos(x);

        matRet.m[0, 0] = 1; matRet.m[1, 0] = 0; matRet.m[2, 0] = 0; matRet.m[3, 0] = 0;
        matRet.m[0, 1] = 0; matRet.m[1, 1] = CosValue; matRet.m[2, 1] = -SinValue; matRet.m[3, 1] = 0;
        matRet.m[0, 2] = 0; matRet.m[1, 2] = SinValue; matRet.m[2, 2] = CosValue; matRet.m[3, 2] = 0;
        matRet.m[0, 3] = 0; matRet.m[1, 3] = 0; matRet.m[2, 3] = 0; matRet.m[3, 3] = 1;

        return matRet;
    }

    //获取旋转矩阵 Y轴旋转
    // https://blog.csdn.net/csxiaoshui/article/details/65446125
    public static HMatrix GetRotateMatY(float y)
    {
        HMatrix matRet = HMatrix.identity;//单位矩阵
        float SinValue = (float)Math.Sin(y);
        float CosValue = (float)Math.Cos(y);

        matRet.m[0, 0] = CosValue; matRet.m[1, 0] = 0; matRet.m[2, 0] = SinValue; matRet.m[3, 0] = 0;
        matRet.m[0, 1] = 0; matRet.m[1, 1] = 1; matRet.m[2, 1] = 0; matRet.m[3, 1] = 0;
        matRet.m[0, 2] = -SinValue; matRet.m[1, 2] = 0; matRet.m[2, 2] = CosValue; matRet.m[3, 2] = 0;
        matRet.m[0, 3] = 0; matRet.m[1, 3] = 0; matRet.m[2, 3] = 0; matRet.m[3, 3] = 1;

        return matRet;
    }

    //获取旋转矩阵 Z轴旋转
    // https://blog.csdn.net/csxiaoshui/article/details/65446125
    public static HMatrix GetRotateMatZ(float z)
    {
        HMatrix matRet = HMatrix.identity;//单位矩阵
        float SinValue = (float)Math.Sin(z);
        float CosValue = (float)Math.Cos(z);

        matRet.m[0, 0] = CosValue; matRet.m[1, 0] = -SinValue; matRet.m[2, 0] = 0; matRet.m[3, 0] = 0;
        matRet.m[0, 1] = SinValue; matRet.m[1, 1] = CosValue; matRet.m[2, 1] = 0; matRet.m[3, 1] = 0;
        matRet.m[0, 2] = 0; matRet.m[1, 2] = 0; matRet.m[2, 2] = 1; matRet.m[3, 2] = 0;
        matRet.m[0, 3] = 0; matRet.m[1, 3] = 0; matRet.m[2, 3] = 0; matRet.m[3, 3] = 1;

        return matRet;
    }

    //获取旋转矩阵 XYZ轴旋转
    public static HMatrix GetRotateMat(float x, float y, float z)
    {
        //X Y Z矩阵相乘 这里是为了好理解 但是这样做效率有浪费 6次三角函数 2次矩阵乘法
        HMatrix matRet = GetRotateMatX(x).Mul(GetRotateMatY(y)).Mul(GetRotateMatZ(z));
        return matRet;
    }


    //获取LookAt矩阵
    //参数: 相机的位置 相机的看着那个位置（决定相机方向） 相机上方位置
    // see:https://zhuanlan.zhihu.com/p/66384929
    // Rx Ry Rz 0
    // Ux Uy Uz 0
    // Dx Dy Dz 0
    // 0  0  0  1 相机空间是左手系 
    public static HMatrix GetLookAtMatrix(HVector camera, HVector at, HVector up)
    {
        HMatrix matRet = new HMatrix();
        HVector CameraXAxis, CameraYAxis, CameraZAxis;
        CameraZAxis = at.Sub(camera);
        CameraZAxis = CameraZAxis.Normalize();
        CameraYAxis = up.Normalize();
        CameraXAxis = CameraZAxis.CrossProduct(CameraYAxis);
        CameraXAxis = CameraXAxis.Normalize();

        matRet.m[0, 0] = CameraXAxis.x;
        matRet.m[1, 0] = CameraXAxis.y;
        matRet.m[2, 0] = CameraXAxis.z;
        matRet.m[3, 0] = -CameraXAxis.DotProduct(camera);

        matRet.m[0, 1] = CameraYAxis.x;
        matRet.m[1, 1] = CameraYAxis.y;
        matRet.m[2, 1] = CameraYAxis.z;
        matRet.m[3, 1] = -CameraYAxis.DotProduct(camera);

        matRet.m[0, 2] = CameraZAxis.x;
        matRet.m[1, 2] = CameraZAxis.y;
        matRet.m[2, 2] = CameraZAxis.z;
        matRet.m[3, 2] = -CameraZAxis.DotProduct(camera);

        matRet.m[0, 3] = matRet.m[1, 3] = matRet.m[2, 3] = 0.0f;
        matRet.m[3, 3] = 1.0f;
        return matRet;
    }

    //获取投影矩阵 乘以这个矩阵之后得到的是相机空间的坐标
    public static HMatrix GetPerspectiveMatrix(float fovy, float aspect, float zn, float zf)
    {
        float fax = 1.0f / (float)Math.Tan(fovy * 0.5f);

        HMatrix matRet = HMatrix.zero;
        matRet.m[0, 0] = (float)(fax / aspect);
        matRet.m[1, 1] = (float)(fax);
        matRet.m[2, 2] = zf / (zf - zn);
        matRet.m[3, 2] = -zn * zf / (zf - zn);
        matRet.m[2, 3] = 1;
        return matRet;
    }
}
