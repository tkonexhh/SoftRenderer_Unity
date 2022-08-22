using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//纹理 这里没有去读文件 直接在代码里面赋值了
public class HTexture
{
    public int width, height;
    public int[,] Texture = new int[256, 256];

    public HTexture()
    {
        this.width = 256;
        this.height = 256;

        int i, j;
        for (j = 0; j < height; j++)
        {
            for (i = 0; i < width; i++)
            {
                int x = i / 32, y = j / 32;
                Texture[j, i] = (int)((((x + y) % 2) > 0) ? 0xffffffff : 0x3fbcefff);
            }
        }
    }

    //纹理采样：参考https://gameinstitute.qq.com/community/detail/115739
    //1、这里可以实现：最近点采样、Bilinear 4个点取均值、Trilinear加了MipMap这个维度
    public int ReadTexture(float u, float v)
    {
        int x, y;
        u = u * width;
        v = v * height;
        x = (int)(u + 0.5f);
        y = (int)(v + 0.5f);
        x = MathHelper.Clamp(x, 0, width - 1);
        y = MathHelper.Clamp(y, 0, height - 1);
        return Texture[y, x];
    }

}
