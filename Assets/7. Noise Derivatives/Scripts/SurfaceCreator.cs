﻿using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour {

	[Range(1, 200)]
	public int resolution = 10;
	public Vector3 offset;
	public Vector3 rotation;
	public float frequency = 1f;
	[Range(1, 8)]
	public int octaves = 1;
	[Range(1f, 4f)]
	public float lacunarity = 2f;
	[Range(0f, 1f)]
	public float persistence = 0.5f;
	[Range(1, 3)]
	public int dimensions = 3;
	public NoiseMethodType type;
	public Gradient coloring;

	private int _currentResolution;
	private Mesh _mesh;
	private Vector3[] _vertices;
	private Vector3[] _normals;
	private Color[] _colors;

	private void OnEnable () {
		if (_mesh == null) {
			_mesh = new Mesh();
			_mesh.name = "Surface Mesh";
			GetComponent<MeshFilter>().mesh = _mesh;
		}
		Refresh();
	}
	
	public void Refresh () {
		if (resolution != _currentResolution)
			CreateGrid();
		
		Quaternion q = Quaternion.Euler(rotation);
		Vector3 point00 = q * new Vector3(-0.5f,-0.5f) + offset;
		Vector3 point10 = q * new Vector3( 0.5f,-0.5f) + offset;
		Vector3 point01 = q * new Vector3(-0.5f, 0.5f) + offset;
		Vector3 point11 = q * new Vector3( 0.5f, 0.5f) + offset;
		
		NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1];
		float stepSize = 1f / resolution;
		for (int v = 0, y = 0; y <= resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, y * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, y * stepSize);
			for (int x = 0; x <= resolution; x++, v++) {
				Vector3 point = Vector3.Lerp(point0, point1, x * stepSize);
				float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
				sample = type == NoiseMethodType.Value ? (sample - 0.5f) : (sample * 0.5f);
				_vertices[v].y = sample;
				_colors[v] = coloring.Evaluate(sample + 0.5f);
			}
		}
		_mesh.vertices = _vertices;
		_mesh.colors = _colors;
		_mesh.RecalculateNormals();
	}

	private void CreateGrid () {
		_currentResolution = resolution;
		_mesh.Clear();
		_vertices = new Vector3[(resolution + 1) * (resolution + 1)];
		_colors = new Color[_vertices.Length];
		_normals = new Vector3[_vertices.Length];
		Vector2[] uv = new Vector2[_vertices.Length];
		float stepSize = 1f / resolution;
		for (int v = 0, z = 0; z <= resolution; z++) {
			for (int x = 0; x <= resolution; x++, v++) {
				_vertices[v] = new Vector3(x * stepSize - 0.5f, 0f, z * stepSize - 0.5f);
				_colors[v] = Color.black;
				_normals[v] = Vector3.up;
				uv[v] = new Vector2(x * stepSize, z * stepSize);
			}
		}
		_mesh.vertices = _vertices;
		_mesh.colors = _colors;
		_mesh.normals = _normals;
		_mesh.uv = uv;

		int[] triangles = new int[resolution * resolution * 6];
		for (int t = 0, v = 0, y = 0; y < resolution; y++, v++) {
			for (int x = 0; x < resolution; x++, v++, t += 6) {
				triangles[t] = v;
				triangles[t + 1] = v + resolution + 1;
				triangles[t + 2] = v + 1;
				triangles[t + 3] = v + 1;
				triangles[t + 4] = v + resolution + 1;
				triangles[t + 5] = v + resolution + 2;
			}
		}
		_mesh.triangles = triangles;
	}
}