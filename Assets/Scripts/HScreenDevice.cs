using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//渲染设备
public class HScreenDevice
{
    //============单例 Begin============
    public static HScreenDevice s_Instance;
    public static HScreenDevice S
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = new HScreenDevice();
            }
            return s_Instance;
        }
    }
    //============单例 End============


    //渲染的形状
    public HShape shape;

    //屏幕分辨率宽高
    public int ScreenWidth, ScreenHeight;
    //屏幕缓冲
    public List<byte> FrameBuff = new List<byte>();
    //深度缓冲 绘制过程ZTest ZWrite
    public List<float> DepthBuff = new List<float>();

    public HScreenDevice()
    {
        shape = null;
    }

    public void Init(int width, int height)
    {
        //1、屏幕像素分辨率
        ScreenWidth = width;
        ScreenHeight = height;

        //2、屏幕缓冲和深度缓冲 
        ClearScreen();
    }

    // 清理屏幕
    public void ClearScreen()
    {
        FrameBuff.Clear();
        DepthBuff.Clear();
        for (int i = 0; i < ScreenWidth * ScreenHeight; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                FrameBuff.Add((byte)255);
            }
            DepthBuff.Add(0.0f);
        }
    }
    //=====================================================================
    // 主绘制函数
    //=====================================================================
    public void Draw()
    {
        //1、清理屏幕缓冲
        ClearScreen();
        //2、绘制一个图形
        if (shape != null)
        {
            shape.Draw();
        }
    }

}

