using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMatrix
{
    /*  数据图示
	 *  m[0,0],m[1,0],m[2,0],m[3,0],
	 *  m[0,1],m[1,1],m[2,1],m[3,1],
	 *  m[0,2],m[1,2],m[2,2],m[3,2],
	 *  m[0,3],m[1,3],m[2,3,m[3,3],
	 */
    public float[,] m = new float[4, 4];

    public HMatrix()
    {
        //默认是一个单位矩阵
        m[0, 0] = m[1, 1] = m[2, 2] = m[3, 3] = 1.0f;
        m[0, 1] = m[0, 2] = m[0, 3] = 0.0f;
        m[1, 0] = m[1, 2] = m[1, 3] = 0.0f;
        m[2, 0] = m[2, 1] = m[2, 3] = 0.0f;
        m[3, 0] = m[3, 1] = m[3, 2] = 0.0f;
    }

    //矩阵加法
    public HMatrix Add(HMatrix mat)
    {
        HMatrix matRet = new HMatrix();
        int i, j;
        for (i = 0; i < 4; i++)
        {
            for (j = 0; j < 4; j++)
            {
                matRet.m[i, j] = m[i, j] + mat.m[i, j];
            }
        }
        return matRet;
    }

    //矩阵减法
    public HMatrix Sub(HMatrix mat)
    {
        HMatrix matRet = new HMatrix();
        int i, j;
        for (i = 0; i < 4; i++)
        {
            for (j = 0; j < 4; j++)
            {
                matRet.m[i, j] = m[i, j] - mat.m[i, j];
            }
        }
        return matRet;
    }

    //矩阵缩放
    public HMatrix Scale(float f)
    {
        HMatrix matRet = new HMatrix();
        int i, j;
        for (i = 0; i < 4; i++)
        {
            for (j = 0; j < 4; j++)
            {
                matRet.m[i, j] = m[i, j] * f;
            }
        }
        return matRet;
    }

    //矩阵乘法
    public HMatrix Mul(HMatrix mat)
    {
        HMatrix matRet = new HMatrix();
        int i, j;
        for (i = 0; i < 4; i++)
        {
            for (j = 0; j < 4; j++)
            {
                matRet.m[i, j] = (m[i, 0] * mat.m[0, j]) +
                    (m[i, 1] * mat.m[1, j]) +
                    (m[i, 2] * mat.m[2, j]) +
                    (m[i, 3] * mat.m[3, j]);
            }
        }
        return matRet;
    }

    static public bool operator ==(HMatrix mat1, HMatrix mat2)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (mat1.m[i, j] != mat2.m[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    static public bool operator !=(HMatrix mat1, HMatrix mat2)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (mat1.m[i, j] != mat2.m[i, j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static HMatrix identity
    {
        get
        {
            HMatrix matRet = new HMatrix();
            matRet.m[0, 0] = matRet.m[1, 1] = matRet.m[2, 2] = matRet.m[3, 3] = 1.0f;
            matRet.m[0, 1] = matRet.m[0, 2] = matRet.m[0, 3] = 0.0f;
            matRet.m[1, 0] = matRet.m[1, 2] = matRet.m[1, 3] = 0.0f;
            matRet.m[2, 0] = matRet.m[2, 1] = matRet.m[2, 3] = 0.0f;
            matRet.m[3, 0] = matRet.m[3, 1] = matRet.m[3, 2] = 0.0f;
            return matRet;
        }
    }

    public static HMatrix zero
    {
        get
        {
            HMatrix matRet = new HMatrix();
            matRet.m[0, 0] = matRet.m[0, 1] = matRet.m[0, 2] = matRet.m[0, 3] = 0.0f;
            matRet.m[1, 0] = matRet.m[1, 1] = matRet.m[1, 2] = matRet.m[1, 3] = 0.0f;
            matRet.m[2, 0] = matRet.m[2, 1] = matRet.m[2, 2] = matRet.m[2, 3] = 0.0f;
            matRet.m[3, 0] = matRet.m[3, 1] = matRet.m[3, 2] = matRet.m[3, 3] = 0.0f;
            return matRet;
        }
    }

}

