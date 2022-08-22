using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//形状接口
public interface HShape
{
    void Draw();
}


public class a2v
{
    public a2v(HVector posPara)
    {
        pos = posPara;
    }
    public HVector pos = new HVector();     //模型坐标
    public HVector normal = new HVector();      //法线坐标
    public HTexcoord uv = new HTexcoord();      //uv坐标
    public HColor color = new HColor();		//顶点颜色
};

public class v2f
{
    public HVector pos = new HVector();     //模型坐标
    public HTexcoord uv = new HTexcoord();		//uv坐标
};

//立方体
public class HCube : HShape
{
    //坐标变换
    public HTransform Transform = new HTransform();
    //纹理
    public HTexture Texture = new HTexture();
    //mesh
    //8个顶点  前面4个顶点是正方体的前面  后面4个顶点是正方体的后面
    public List<HVertex> mesh = new List<HVertex>();

    public HCube()
    {
        mesh.Add(new HVertex(-1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.2f, 0.2f, 1f));
        mesh.Add(new HVertex(1, -1, 1, 1, 0, 1, 0.2f, 1.0f, 0.2f, 1));
        mesh.Add(new HVertex(1, 1, 1, 1, 1, 1, 0.2f, 0.2f, 1.0f, 1));
        mesh.Add(new HVertex(-1, 1, 1, 1, 1, 0, 1.0f, 0.2f, 1.0f, 1));
        mesh.Add(new HVertex(-1, -1, -1, 1, 0, 0, 1.0f, 1.0f, 0.2f, 1));
        mesh.Add(new HVertex(1, -1, -1, 1, 0, 1, 0.2f, 1.0f, 1.0f, 1));
        mesh.Add(new HVertex(1, 1, -1, 1, 1, 1, 1.0f, 0.3f, 0.3f, 1));
        mesh.Add(new HVertex(-1, 1, -1, 1, 1, 0, 0.2f, 1.0f, 0.3f, 1));
    }

    //更新MVP矩阵
    void UpdateMVPMat()
    {
        HMatrix mat = MathHelper.GetRotateMat(0, 0.8f, 0.8f);
        Transform.ModleMat = mat;
        Transform.UpdateMVPMatrix();
    }

    //计算三角形三个顶点的裁剪空间的坐标
    void InitTriangleClipSpacePos(HTriangle Triangle)
    {
        //三角形的模型坐标乘以MVP矩阵 得到投影坐标（相机空间）
        Triangle.p1InClipSpace = Triangle.p1InObjectSpace.Copy();
        Triangle.p2InClipSpace = Triangle.p2InObjectSpace.Copy();
        Triangle.p3InClipSpace = Triangle.p3InObjectSpace.Copy();
    }

    //计算三角形三个顶点的裁剪空间的坐标
    void CalTriangleScreenSpacePos(HTriangle Triangle)
    {
        //顶点的其他数据
        Triangle.p1InScreenSpace = Triangle.p1InObjectSpace.Copy();
        Triangle.p2InScreenSpace = Triangle.p2InObjectSpace.Copy();
        Triangle.p3InScreenSpace = Triangle.p3InObjectSpace.Copy();
        //归一化然后乘宽高
        Triangle.p1InScreenSpace.pos = Transform.HomogenizeToScreenCoord(Triangle.p1InClipSpace.pos);
        Triangle.p2InScreenSpace.pos = Transform.HomogenizeToScreenCoord(Triangle.p2InClipSpace.pos);
        Triangle.p3InScreenSpace.pos = Transform.HomogenizeToScreenCoord(Triangle.p3InClipSpace.pos);
        //保存Z信息
        Triangle.p1InScreenSpace.pos.w = Triangle.p1InClipSpace.pos.w;
        Triangle.p2InScreenSpace.pos.w = Triangle.p2InClipSpace.pos.w;
        Triangle.p3InScreenSpace.pos.w = Triangle.p3InClipSpace.pos.w;
    }

    //检查三角形是否在裁剪
    bool CheckTriangleInCVV(HTriangle Triangle)
    {
        if (Triangle.p1InClipSpace.pos.CheckInCVV() == false) return false;
        if (Triangle.p2InClipSpace.pos.CheckInCVV() == false) return false;
        if (Triangle.p3InClipSpace.pos.CheckInCVV() == false) return false;
        return true;
    }


    //深度Z初始化顶点插值信息
    void InitTriangleInterpn(HTriangle Triangle)
    {
        Triangle.p1InScreenSpace.Initrhw();
        Triangle.p2InScreenSpace.Initrhw();
        Triangle.p3InScreenSpace.Initrhw();
    }


    //简单的顶点着色器
    v2f vert(a2v v)
    {
        v2f output = new v2f();
        output.pos = Transform.MulMVPMatrix(v.pos);
        return output;
    }

    //简单的片元着色器
    int frag(v2f f)
    {
        int color = Texture.ReadTexture(f.uv.u, f.uv.v);
        return color;
    }


    public void Draw()
    {
        DrawBox();
    }

    //画立方体
    void DrawBox()
    {
        DrawPlane(0, 1, 2, 3);//前面
        DrawPlane(7, 6, 5, 4);//后面
        DrawPlane(0, 4, 5, 1);//下面
        DrawPlane(1, 5, 6, 2);//右面
        DrawPlane(2, 6, 7, 3);//上面
        DrawPlane(3, 7, 4, 0);//左面
    }

    //画面
    void DrawPlane(int p1_index, int p2_index, int p3_index, int p4_index)
    {
        HVertex p1 = mesh[p1_index];
        HVertex p2 = mesh[p2_index];
        HVertex p3 = mesh[p3_index];
        HVertex p4 = mesh[p4_index];

        //纹理绘制到这个面上面
        p1.uv.u = 0;
        p1.uv.v = 0;

        p2.uv.u = 0;
        p2.uv.v = 1;

        p3.uv.u = 1;
        p3.uv.v = 1;

        p4.uv.u = 1;
        p4.uv.v = 0;
        HTriangle T1 = new HTriangle();
        T1.p1InObjectSpace = p1;
        T1.p2InObjectSpace = p2;
        T1.p3InObjectSpace = p3;
        DrawTriangle(T1);


        HTriangle T2 = new HTriangle();
        T2.p1InObjectSpace = p3;
        T2.p2InObjectSpace = p4;
        T2.p3InObjectSpace = p1;
        DrawTriangle(T2);
    }

    //画三角形 传入的
    void DrawTriangle(HTriangle Triangle)
    {
        //1、更新立方体的MVP矩阵
        UpdateMVPMat();

        //2.1、初始化裁剪空间坐标
        InitTriangleClipSpacePos(Triangle);

        //2.2、 顶点着色器
        Triangle.p1InClipSpace.pos = vert(new a2v(Triangle.p1InObjectSpace.pos)).pos;
        Triangle.p2InClipSpace.pos = vert(new a2v(Triangle.p2InObjectSpace.pos)).pos;
        Triangle.p3InClipSpace.pos = vert(new a2v(Triangle.p3InObjectSpace.pos)).pos;
        //曲面细分着色器【TODO】
        //几何着色器【TODO】

        //3、用裁剪空间裁剪三角形（这里比较暴力  一个点不在CVV则剔除）
        if (CheckTriangleInCVV(Triangle) == false) return;

        //4、归一化然后计算得到屏幕坐标
        CalTriangleScreenSpacePos(Triangle);

        //5、插值初始化
        InitTriangleInterpn(Triangle);

        //6、屏幕坐标的三角形拆分三角形为0-2个梯形，并且返回可用梯形数量
        List<HTrapezoid> traps = new List<HTrapezoid>();
        traps.Add(new HTrapezoid());
        traps.Add(new HTrapezoid());
        int n = Triangle.CalculateTrap(traps);

        //7、梯形扫描 绘制梯形
        if (n >= 1) DrawTrap(traps[0]);
        if (n >= 2) DrawTrap(traps[1]);
    }

    // 根据左右两边的端点，初始化计算出扫描线的起点和步长
    HScanline GetScanline(HTrapezoid trap, int y)
    {
        HScanline scanlineRet = new HScanline();
        // 左右两点的 宽度
        float width = trap.right.v.pos.x - trap.left.v.pos.x;
        // 起点X 坐标
        scanlineRet.x = (int)(trap.left.v.pos.x + 0.5f);
        // 宽度
        scanlineRet.width = (int)(trap.right.v.pos.x + 0.5f) - scanlineRet.x;
        // y坐标
        scanlineRet.y = y;
        // 扫描起点
        scanlineRet.v = trap.left.v.Copy();
        // 
        if (trap.left.v.pos.x >= trap.right.v.pos.x) scanlineRet.width = 0;
        //计算步伐
        scanlineRet.step = trap.left.v.Step(trap.right.v, width);
        return scanlineRet;
    }

    void DrawScanline(HScanline scanline)
    {
        HScreenDevice ScreenDevice = HScreenDevice.S;
        List<byte> framebuffer = ScreenDevice.FrameBuff;
        List<float> zbuffer = ScreenDevice.DepthBuff;

        int x = (int)scanline.x;
        int y = (int)scanline.y;
        int scanlineWidth = (int)scanline.width;
        int ScreenWidth = ScreenDevice.ScreenWidth;
        int ScreenHeight = ScreenDevice.ScreenHeight;

        for (; scanlineWidth > 0; x++, scanlineWidth--)
        {
            if (x >= 0 && x < ScreenWidth)
            {
                float rhw = scanline.v.rhw;
                if (rhw >= zbuffer[x + y * ScreenWidth])
                {
                    float w = 1.0f / rhw;
                    zbuffer[x + y * ScreenWidth] = rhw;

                    float u = scanline.v.uv.u * w;
                    float v = scanline.v.uv.v * w;
                    v2f fragIn = new v2f();
                    fragIn.uv.u = u;
                    fragIn.uv.v = v;
                    int color = frag(fragIn);//片元着色器
                    byte r = (byte)0x0, g = (byte)0x0, b = (byte)0x0, a = (byte)0x0;
                    MathHelper.GetRGBAFromInt(color, ref r, ref g, ref b, ref a);
                    framebuffer[(x + y * ScreenWidth) * 4] = r;
                    framebuffer[(x + y * ScreenWidth) * 4 + 1] = g;
                    framebuffer[(x + y * ScreenWidth) * 4 + 2] = b;
                    framebuffer[(x + y * ScreenWidth) * 4 + 3] = a;
                }
            }
            scanline.v = scanline.v.Add(scanline.step);
            if (x >= ScreenWidth) break;
        }
    }

    // 主渲染函数
    void DrawTrap(HTrapezoid trap)
    {
        //HScanline scanline;
        int j, top, bottom;
        top = (int)(trap.top + 0.5f);
        bottom = (int)(trap.bottom + 0.5f);
        for (j = top; j < bottom; j++)
        {
            if (j >= 0 && j < HScreenDevice.S.ScreenHeight)
            {//todo
             //插值得到梯形的腰的两个点
                trap.EdgeInterp((float)j + 0.5f);
                //初始化扫描线
                HScanline scanline = GetScanline(trap, j);
                //绘制扫描线
                DrawScanline(scanline);
            }
        }
    }
}
