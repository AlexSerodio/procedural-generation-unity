﻿/// <summary>
/// Represents a 2 dimensional point such as Vector2 but with int values instead of float.
/// </summary>
[System.Serializable]
public struct IntVector2
{

    public int x, z;

    public IntVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }
}