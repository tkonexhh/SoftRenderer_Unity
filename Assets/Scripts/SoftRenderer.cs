//软渲染器参考：https://github.com/skywind3000/mini3d  
//mini3d笔记：https://zhuanlan.zhihu.com/p/74510058
//图形学流水线文章：https://positiveczp.github.io/%E7%BB%86%E8%AF%B4%E5%9B%BE%E5%BD%A2%E5%AD%A6%E6%B8%B2%E6%9F%93%E7%AE%A1%E7%BA%BF.pdf
//意图：
//1、增加了注释为了更好的理解  mini3d  写的很精简 看懂了之后对渲染流水线更加深刻了
//2、同是对图形学感兴趣的朋友看了可以更好的理解，刚开始看图形学 没有代码总感觉很虚。
//备注：
//C++代码大部分是值传递 没有使用指针 纯为了展示mini3d的过程 

/*
 *  1、最简单的渲染流水线:
 *	   分成CPU阶段和GPU阶段
 *      +--------------+     +-------------+
 *      |              |     |             |
 *      |     CPU      +----->     GPU     |
 *      |              |     |             |
 *      +--------------+     +-------------+
 *
 *	2、其中CPU阶段就是Application 应用阶段  GPU阶段包括了几何阶段和光栅化阶段
 *      +--------------+     +-----------------+  +----------------+   +----------------+
 *      |              |     |                 |  |                |   |                |
 *      |   应用阶段   +----->     几何阶段    +-->      光栅化    +--->     像素处理   |
 *      |              |     |                 |  |                |   |                |
 *      +--------------+     +-----------------+  +----------------+   +----------------+
 *
 *  3、几何阶段：
 *		+--------------+     +-----------------+  +------------------+   +-------------+  +-------------+
 *      |              |     |                 |  |                  |   |             |  |             |
 *      |  顶点着色器  +-----> 曲面细分着色器  +-->   几何着色器     +--->    裁剪     |-->  屏幕投射   |
 *      |              |     |                 |  |                  |   |             |  |             |
 *      +--------------+     +-----------------+  +------------------+   +-------------+  +-------------+
 *
 *  4、光栅化阶段：
 *		+--------------+     +--------------+  +------------------+
 *      |              |     |              |  |                  |
 *      |  三角形遍历  +----->  三角形设置  +-->   片元着色器     |
 *      |              |     |              |  |                  |
 *      +--------------+     +--------------+  +------------------+
 *
 *  5、像素处理阶段：
 *		深度测试ZTest
 *		颜色混合
 *      模板测试（模板缓冲）
 *
 *	【说明】：下面的代码根据上面的流水线来讲解和划分
 *	绘制调用堆栈：
 *		HScreenDevice::Draw					
 *			HScreenDevice::ClearScreen									清屏
 *			HCube::Draw													Cube绘制
 *				HCube::DrawBox											立方体绘制
 *					HCube::DrawPlane									长方形绘制
 *						HCube::DrawTriangle								三角形绘制
 *						
 *							HCube::UpdateMVPMat()							1、更新MVP矩阵					   -|
 *							HCube::vert()									2、顶点着色器 之后就是裁剪空间坐标了		|
 *																			3、曲面细分着色器 几何着色器【TODO】		|--->几何阶段
 *							HCube::CheckTriangleInCVV()						4、裁剪 检查在不在裁剪空间里面			|
 *							HCube::CalTriangleScreenSpacePos()				5、屏幕投射						   -|
 *							
 *							HCube::InitTriangleInterpn()					1、插值初始化 后面透视校正用			   -|
 *							Triangle::CalculateTrap() DrawTrap DrawScanline 2、三角形设置、三角形遍历 得到片元信息		|--->光栅化阶段
 *							HCube::frag										3、片元着色器						   -|
 *							
 *							ZTest Zwrite
 *						
 *						
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SoftRenderer : MonoBehaviour
{
    public Image MainImage;
    public Texture2D MainTexture;
    private void Start()
    {
        //获取设备宽高 然后初始化
        int X = 700;
        int Y = 700;
        HScreenDevice.S.Init(X, Y);

        //MainTexture
        MainTexture = new Texture2D(X, Y, TextureFormat.RGBA32, false);
        //设置绘制一个Cube
        HScreenDevice.S.shape = new HCube();
    }

    private bool first = true;

    void Update()
    {
        if (first)
        {
            //first = false;
            HScreenDevice.S.Draw();

            for (int i = 0; i < HScreenDevice.S.ScreenHeight; i++)
            {
                for (int j = 0; j < HScreenDevice.S.ScreenWidth; j++)
                {
                    Color color = new Color();
                    color.r = HScreenDevice.S.FrameBuff[(i * HScreenDevice.S.ScreenWidth + j) * 4 + 0] / 255.0f;
                    color.g = HScreenDevice.S.FrameBuff[(i * HScreenDevice.S.ScreenWidth + j) * 4 + 1] / 255.0f;
                    color.b = HScreenDevice.S.FrameBuff[(i * HScreenDevice.S.ScreenWidth + j) * 4 + 2] / 255.0f;
                    color.a = 255 / 255.0f;
                    MainTexture.SetPixel(i, j, color);

                }
            }
            MainTexture.Apply();
            MainImage.sprite = Sprite.Create(MainTexture, new Rect(0, 0, MainTexture.width, MainTexture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
