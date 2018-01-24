using UnityEngine;

public class TextureCreator : MonoBehaviour {

	[Range(2, 512)]
	public int resolution = 256;
	[Range(1, 3)]
	public int dimensions = 3;
	public float frequency = 1f;
	public NoiseMethodType type;
	private Texture2D _texture;

	void OnEnable () {
		if (_texture == null) {
			_texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
			_texture.name = "Procedural Texture";
			_texture.wrapMode = TextureWrapMode.Clamp;
			//_texture.filterMode = FilterMode.Point;
			//_texture.filterMode = FilterMode.Bilinear;
			_texture.filterMode = FilterMode.Trilinear;
			_texture.anisoLevel = 9;
			GetComponent<MeshRenderer>().material.mainTexture = _texture;
		}
		FillTexture();
	}

	private void Update () {
		if (transform.hasChanged) {
			transform.hasChanged = false;
			FillTexture();
		}
	}

	public void FillTexture () {
		if (_texture.width != resolution)
			_texture.Resize(resolution, resolution);
		
		Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f,-0.5f));
		Vector3 point10 = transform.TransformPoint(new Vector3( 0.5f,-0.5f));
		Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
		Vector3 point11 = transform.TransformPoint(new Vector3( 0.5f, 0.5f));

		NoiseMethod method = Noise.noiseMethods[(int)type][dimensions - 1];
		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
			Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
			Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);
			for (int x = 0; x < resolution; x++) {
				Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
				float sample = method(point, frequency);
				if (type != NoiseMethodType.Value) 
					sample = sample * 0.5f + 0.5f;
				_texture.SetPixel(x, y, Color.white * sample);
			}
		}
		_texture.Apply();
	}

}
