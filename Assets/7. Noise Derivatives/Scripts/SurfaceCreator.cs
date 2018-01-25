using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour {

	[Range(1, 200)]
	public int resolution = 10;
	
	private int _currentResolution;
	private Mesh _mesh;

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
	}

	private void CreateGrid () {
		_currentResolution = resolution;
		_mesh.Clear();
		Vector3[] vertices = new Vector3[(resolution + 1) * (resolution + 1)];
		Color[] colors = new Color[vertices.Length];
		Vector3[] normals = new Vector3[vertices.Length];
		Vector2[] uv = new Vector2[vertices.Length];
		float stepSize = 1f / resolution;
		for (int v = 0, y = 0; y <= resolution; y++) {
			for (int x = 0; x <= resolution; x++, v++) {
				vertices[v] = new Vector3(x * stepSize - 0.5f, y * stepSize - 0.5f);
				colors[v] = Color.black;
				normals[v] = Vector3.back;
				uv[v] = new Vector2(x * stepSize, y * stepSize);
			}
		}
		_mesh.vertices = vertices;
		_mesh.colors = colors;
		_mesh.normals = normals;
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