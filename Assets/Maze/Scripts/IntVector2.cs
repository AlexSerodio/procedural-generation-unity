﻿/// <summary>
/// Represents a 2 dimensional vector such as Vector2 but with int values.
/// </summary>
[System.Serializable]
public struct IntVector2 {

	public int x, z;
	
	public IntVector2 (int x, int z) {
		this.x = x;
		this.z = z;
	}

	/// <summary>
	/// Overwrites the plus (+) signal.
	/// </summary>
	public static IntVector2 operator + (IntVector2 a, IntVector2 b) {
		a.x += b.x;
		a.z += b.z;
		return a;
	}
}