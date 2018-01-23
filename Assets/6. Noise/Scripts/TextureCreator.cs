using UnityEngine;

public class TextureCreator : MonoBehaviour {

	[Range(2, 512)]
	public int resolution = 256;
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

	public void FillTexture () {
		if (_texture.width != resolution)
			_texture.Resize(resolution, resolution);
		
		float stepSize = 1f / resolution;
		for (int y = 0; y < resolution; y++) {
			for (int x = 0; x < resolution; x++) {
				//_texture.SetPixel(x, y, new Color(x * stepSize, y * stepSize, 0f));
				//_texture.SetPixel(x, y, new Color((x + 0.5f) * stepSize, (y + 0.5f) * stepSize, 0f));
				_texture.SetPixel(x, y, new Color((x + 0.5f) * stepSize % 0.1f, (y + 0.5f) * stepSize % 0.1f, 0f) * 10f);
			}
		}
		_texture.Apply();
	}

}
