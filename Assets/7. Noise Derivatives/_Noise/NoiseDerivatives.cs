﻿using UnityEngine;

public delegate float NoiseDerivativesMethod (Vector3 point, float frequency);

public enum NoiseDerivativesMethodType {
	Value,
	Perlin
}

public static class NoiseDerivatives {

	public static NoiseDerivativesMethod[] valueMethods = {
		Value1D,
		Value2D,
		Value3D
	};

	public static NoiseDerivativesMethod[] perlinMethods = {
		Perlin1D,
		Perlin2D,
		Perlin3D
	};

	/* represents a permutation table used as a hash function, that is, 
	   transforms a value into some other value. */
	private static int[] _hash = {
		151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
		140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
		247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
		 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
		 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
		 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
		 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
		200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
		 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
		207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
		119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
		129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
		218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
		 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
		184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
		222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180,

		151,160,137, 91, 90, 15,131, 13,201, 95, 96, 53,194,233,  7,225,
		140, 36,103, 30, 69,142,  8, 99, 37,240, 21, 10, 23,190,  6,148,
		247,120,234, 75,  0, 26,197, 62, 94,252,219,203,117, 35, 11, 32,
		 57,177, 33, 88,237,149, 56, 87,174, 20,125,136,171,168, 68,175,
		 74,165, 71,134,139, 48, 27,166, 77,146,158,231, 83,111,229,122,
		 60,211,133,230,220,105, 92, 41, 55, 46,245, 40,244,102,143, 54,
		 65, 25, 63,161,  1,216, 80, 73,209, 76,132,187,208, 89, 18,169,
		200,196,135,130,116,188,159, 86,164,100,109,198,173,186,  3, 64,
		 52,217,226,250,124,123,  5,202, 38,147,118,126,255, 82, 85,212,
		207,206, 59,227, 47, 16, 58, 17,182,189, 28, 42,223,183,170,213,
		119,248,152,  2, 44,154,163, 70,221,153,101,155,167, 43,172,  9,
		129, 22, 39,253, 19, 98,108,110, 79,113,224,232,178,185,112,104,
		218,246, 97,228,251, 34,242,193,238,210,144, 12,191,179,162,241,
		 81, 51,145,235,249, 14,239,107, 49,192,214, 31,181,199,106,157,
		184, 84,204,176,115,121, 50, 45,127,  4,150,254,138,236,205, 93,
		222,114, 67, 29, 24, 72,243,141,128,195, 78, 66,215, 61,156,180
	};
	private const int HASH_MASK = 255;

	private static float[] _gradients1D = {
		1f, -1f
	};
	private const int GRADIENT_MASK_1D = 1;

	private static Vector2[] _gradients2D = {
		new Vector2( 1f, 0f),
		new Vector2(-1f, 0f),
		new Vector2( 0f, 1f),
		new Vector2( 0f,-1f),
		new Vector2( 1f, 1f).normalized,
		new Vector2(-1f, 1f).normalized,
		new Vector2( 1f,-1f).normalized,
		new Vector2(-1f,-1f).normalized
	};
	private const int GRADIENT_MASK_2D = 7;

	private static float _sqr2 = Mathf.Sqrt(2f);

		private static Vector3[] _gradients3D = {
		new Vector3( 1f, 1f, 0f),
		new Vector3(-1f, 1f, 0f),
		new Vector3( 1f,-1f, 0f),
		new Vector3(-1f,-1f, 0f),
		new Vector3( 1f, 0f, 1f),
		new Vector3(-1f, 0f, 1f),
		new Vector3( 1f, 0f,-1f),
		new Vector3(-1f, 0f,-1f),
		new Vector3( 0f, 1f, 1f),
		new Vector3( 0f,-1f, 1f),
		new Vector3( 0f, 1f,-1f),
		new Vector3( 0f,-1f,-1f),
		
		new Vector3( 1f, 1f, 0f),
		new Vector3(-1f, 1f, 0f),
		new Vector3( 0f,-1f, 1f),
		new Vector3( 0f,-1f,-1f)
	};
	private const int GRADIENT_MASK_3D = 15;

	public static NoiseDerivativesMethod[][] noiseMethods = {
		valueMethods,
		perlinMethods
	};

	public static float Sum (NoiseDerivativesMethod method, Vector3 point, float frequency, int octaves, float lacunarity, float persistence) {
		float sum = method(point, frequency);
		float amplitude = 1f;
		float range = 1f;
		for (int o = 1; o < octaves; o++) {
			frequency *= lacunarity;
			amplitude *= persistence;
			range += amplitude;
			sum += method(point, frequency) * amplitude;
		}
		return sum / range;
	}

	/* 5th degree polynomial: 6t5 - 15t4 + 10t3 used to smooth interpolation.*/
	private static float Smooth (float t) {
		return t * t * t * (t * (t * 6f - 15f) + 10f);
	}

	public static float Value1D (Vector3 point, float frequency) {
		point *= frequency;
		int i0 = Mathf.FloorToInt(point.x);
		float t = point.x - i0;
		i0 &= HASH_MASK;
		int i1 = i0 + 1;

		int h0 = _hash[i0];
		int h1 = _hash[i1];

		t = Smooth(t);
		return Mathf.Lerp(h0, h1, t) * (1f / HASH_MASK);
	}

	public static float Value2D (Vector3 point, float frequency) {
		point *= frequency;
		int ix0 = Mathf.FloorToInt(point.x);
		int iy0 = Mathf.FloorToInt(point.y);
		float tx = point.x - ix0;
		float ty = point.y - iy0;
		ix0 &= HASH_MASK;
		iy0 &= HASH_MASK;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;

		int h0 = _hash[ix0];
		int h1 = _hash[ix1];
		int h00 = _hash[h0 + iy0];
		int h10 = _hash[h1 + iy0];
		int h01 = _hash[h0 + iy1];
		int h11 = _hash[h1 + iy1];

		tx = Smooth(tx);
		ty = Smooth(ty);
		return Mathf.Lerp(Mathf.Lerp(h00, h10, tx), Mathf.Lerp(h01, h11, tx), ty) * (1f / HASH_MASK);
	}

	public static float Value3D (Vector3 point, float frequency) {
		point *= frequency;
		int ix0 = Mathf.FloorToInt(point.x);
		int iy0 = Mathf.FloorToInt(point.y);
		int iz0 = Mathf.FloorToInt(point.z);
		float tx = point.x - ix0;
		float ty = point.y - iy0;
		float tz = point.z - iz0;
		ix0 &= HASH_MASK;
		iy0 &= HASH_MASK;
		iz0 &= HASH_MASK;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;
		int iz1 = iz0 + 1;

		int h0 = _hash[ix0];
		int h1 = _hash[ix1];
		int h00 = _hash[h0 + iy0];
		int h10 = _hash[h1 + iy0];
		int h01 = _hash[h0 + iy1];
		int h11 = _hash[h1 + iy1];
		int h000 = _hash[h00 + iz0];
		int h100 = _hash[h10 + iz0];
		int h010 = _hash[h01 + iz0];
		int h110 = _hash[h11 + iz0];
		int h001 = _hash[h00 + iz1];
		int h101 = _hash[h10 + iz1];
		int h011 = _hash[h01 + iz1];
		int h111 = _hash[h11 + iz1];

		tx = Smooth(tx);
		ty = Smooth(ty);
		tz = Smooth(tz);
		return Mathf.Lerp(
			Mathf.Lerp(Mathf.Lerp(h000, h100, tx), Mathf.Lerp(h010, h110, tx), ty),
			Mathf.Lerp(Mathf.Lerp(h001, h101, tx), Mathf.Lerp(h011, h111, tx), ty),
			tz) * (1f / HASH_MASK);
	}
	
	public static float Perlin1D (Vector3 point, float frequency) {
		point *= frequency;
		int i0 = Mathf.FloorToInt(point.x);
		float t0 = point.x - i0;
		float t1 = t0 - 1f;
		i0 &= HASH_MASK;
		int i1 = i0 + 1;
		
		float g0 = _gradients1D[_hash[i0] & GRADIENT_MASK_1D];
		float g1 = _gradients1D[_hash[i1] & GRADIENT_MASK_1D];

		float v0 = g0 * t0;
		float v1 = g1 * t1;

		float t = Smooth(t0);
		return Mathf.Lerp(v0, v1, t) * 2f;
	}
	
	private static float Dot (Vector2 g, float x, float y) {
		return g.x * x + g.y * y;
	}
	
	public static float Perlin2D (Vector3 point, float frequency) {
		point *= frequency;
		int ix0 = Mathf.FloorToInt(point.x);
		int iy0 = Mathf.FloorToInt(point.y);
		float tx0 = point.x - ix0;
		float ty0 = point.y - iy0;
		float tx1 = tx0 - 1f;
		float ty1 = ty0 - 1f;
		ix0 &= HASH_MASK;
		iy0 &= HASH_MASK;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;
		
		int h0 = _hash[ix0];
		int h1 = _hash[ix1];
		Vector2 g00 = _gradients2D[_hash[h0 + iy0] & GRADIENT_MASK_2D];
		Vector2 g10 = _gradients2D[_hash[h1 + iy0] & GRADIENT_MASK_2D];
		Vector2 g01 = _gradients2D[_hash[h0 + iy1] & GRADIENT_MASK_2D];
		Vector2 g11 = _gradients2D[_hash[h1 + iy1] & GRADIENT_MASK_2D];

		float v00 = Dot(g00, tx0, ty0);
		float v10 = Dot(g10, tx1, ty0);
		float v01 = Dot(g01, tx0, ty1);
		float v11 = Dot(g11, tx1, ty1);
		
		float tx = Smooth(tx0);
		float ty = Smooth(ty0);
		//return v00;
		return Mathf.Lerp(Mathf.Lerp(v00, v10, tx), Mathf.Lerp(v01, v11, tx), ty) * _sqr2;
	}
	
	private static float Dot (Vector3 g, float x, float y, float z) {
		return g.x * x + g.y * y + g.z * z;
	}
	
	public static float Perlin3D (Vector3 point, float frequency) {
		point *= frequency;
		int ix0 = Mathf.FloorToInt(point.x);
		int iy0 = Mathf.FloorToInt(point.y);
		int iz0 = Mathf.FloorToInt(point.z);
		float tx0 = point.x - ix0;
		float ty0 = point.y - iy0;
		float tz0 = point.z - iz0;
		float tx1 = tx0 - 1f;
		float ty1 = ty0 - 1f;
		float tz1 = tz0 - 1f;
		ix0 &= HASH_MASK;
		iy0 &= HASH_MASK;
		iz0 &= HASH_MASK;
		int ix1 = ix0 + 1;
		int iy1 = iy0 + 1;
		int iz1 = iz0 + 1;
		
		int h0 = _hash[ix0];
		int h1 = _hash[ix1];
		int h00 = _hash[h0 + iy0];
		int h10 = _hash[h1 + iy0];
		int h01 = _hash[h0 + iy1];
		int h11 = _hash[h1 + iy1];
		Vector3 g000 = _gradients3D[_hash[h00 + iz0] & GRADIENT_MASK_3D];
		Vector3 g100 = _gradients3D[_hash[h10 + iz0] & GRADIENT_MASK_3D];
		Vector3 g010 = _gradients3D[_hash[h01 + iz0] & GRADIENT_MASK_3D];
		Vector3 g110 = _gradients3D[_hash[h11 + iz0] & GRADIENT_MASK_3D];
		Vector3 g001 = _gradients3D[_hash[h00 + iz1] & GRADIENT_MASK_3D];
		Vector3 g101 = _gradients3D[_hash[h10 + iz1] & GRADIENT_MASK_3D];
		Vector3 g011 = _gradients3D[_hash[h01 + iz1] & GRADIENT_MASK_3D];
		Vector3 g111 = _gradients3D[_hash[h11 + iz1] & GRADIENT_MASK_3D];

		float v000 = Dot(g000, tx0, ty0, tz0);
		float v100 = Dot(g100, tx1, ty0, tz0);
		float v010 = Dot(g010, tx0, ty1, tz0);
		float v110 = Dot(g110, tx1, ty1, tz0);
		float v001 = Dot(g001, tx0, ty0, tz1);
		float v101 = Dot(g101, tx1, ty0, tz1);
		float v011 = Dot(g011, tx0, ty1, tz1);
		float v111 = Dot(g111, tx1, ty1, tz1);

		float tx = Smooth(tx0);
		float ty = Smooth(ty0);
		float tz = Smooth(tz0);
		return Mathf.Lerp(
			Mathf.Lerp(Mathf.Lerp(v000, v100, tx), Mathf.Lerp(v010, v110, tx), ty),
			Mathf.Lerp(Mathf.Lerp(v001, v101, tx), Mathf.Lerp(v011, v111, tx), ty),
			tz);
	}


}
