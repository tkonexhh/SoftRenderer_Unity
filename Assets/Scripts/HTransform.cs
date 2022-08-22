using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTransform
{
    //Unity 里面就是 Position Rotation Scale来构建这个矩阵 后面这里可以拆分下更好理解
    // ModelMatrix，就是将模型坐标变换到WorldMatrix的Matrix，WorldMatrix = Mt * Mr * Ms  ModleMatrix =  Mt * Mr * Ms
    public HMatrix ModleMat;
    // 世界坐标转到视锥体 转成相机坐标 View矩阵做转换
    public HMatrix ViewMat;
    // 投影矩阵 视锥体坐标乘以这个投影矩阵 就得到屏幕坐标
    public HMatrix ProjectionMat;
    //MVP 矩阵就是 ModleMat *  ViewMat * ProjectionMat
    public HMatrix MVPMat;


    public HTransform()
    {
        Init();
    }

    void Init()
    {
        int ScreenWidth = HScreenDevice.S.ScreenWidth;
        int ScreenHeight = HScreenDevice.S.ScreenHeight;
        this.ModleMat = HMatrix.identity;
        HVector camera = new HVector(5, 0, 0, 1);
        HVector at = new HVector(0, 0, 0, 1);
        HVector up = HVector.up;
        this.ViewMat = MathHelper.GetLookAtMatrix(camera, up, up);
        //fov = 90 /0.5π
        ProjectionMat = MathHelper.GetPerspectiveMatrix(3.1415926f * 0.5f, (float)ScreenWidth / (float)ScreenHeight, 1.0f, 500.0f);
        UpdateMVPMatrix();
    }

    public void UpdateMVPMatrix()
    {
        MVPMat = ModleMat.Mul(ViewMat).Mul(ProjectionMat);
    }


    //坐标转化到屏幕坐标
    public HVector MulMVPMatrix(HVector origin)
    {
        return origin.MulMatrix(MVPMat);
    }

    //归一化 且屏幕坐标
    //宽->
    //高↓
    public HVector HomogenizeToScreenCoord(HVector Origin)
    {
        int ScreenWidth = HScreenDevice.S.ScreenWidth;
        int ScreenHeight = HScreenDevice.S.ScreenHeight;
        float rhw = 1.0f / Origin.w;
        HVector vecRet = new HVector();
        vecRet.x = (Origin.x * rhw + 1.0f) * ScreenWidth * 0.5f;
        vecRet.y = (1.0f - Origin.y * rhw) * ScreenHeight * 0.5f;
        vecRet.z = Origin.z * rhw;
        vecRet.w = 1.0f;
        return vecRet;
    }
}
