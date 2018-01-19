using UnityEngine;

public static class Bezier {

	/// <summary>
	/// Uses the quadratic Beziér curve polynomial function (it needs three points), 
	/// B(t) = (1 - t)² P0 + 2(1 - t)t P1 + t² P2, 
	/// to calculate the interpolation between points of the curve.
	/// Where t is a value between 0 and 1.
	/// </summary>
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
		
		//is the same as
		//return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
	}

	/// <summary>
	/// Uses the first derivative of the quadratic Beziér curve (it needs three points), 
	/// B'(t) = 2(1 - t)(P1 - P0) + 2t(P2 - P1), to produce lines tangent to the curve,  
	/// which can be interpreted as the speed with which it moves along the curve.
	/// </summary>
	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, float t) {
		return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
	}

	/// <summary>
	/// Uses the cubic Beziér curve function (it needs four points), 
	/// B(t) = (1 - t)³P0 + 3(1 - t)² t P1 + 3(1 - t)t² P2 + t³ P3, 
	/// to calculate the interpolation between points of the curve.
	/// Where t is a value between 0 and 1.
	/// </summary>
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}
	
	/// <summary>
	/// Uses the first derivative of the cubic Beziér curve (it needs four points), 
	/// B'(t) = 3(1 - t)² (P1 - P0) + 6 (1 - t) t (P2 - P1) + 3t² (P3 - P2), 
	/// to produce lines tangent to the curve, which can be interpreted as
	/// the speed with which it moves along the curve.
	/// </summary>
	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return 3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}
}