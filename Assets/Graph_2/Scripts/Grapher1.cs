using UnityEngine;

public class Grapher1 : MonoBehaviour {
	
	public enum FunctionOption {
		Linear,
		Exponential,
		Parabola,
		Sine
	}

	private delegate float FunctionDelegate (float x);
	private static FunctionDelegate[] functionDelegates = {
		Linear,
		Exponential,
		Parabola,
		Sine
	};

	[Range(10, 100)]
	public int resolution = 10;
	public FunctionOption function;

	private int currentResolution;
	private ParticleSystem.Particle[] points;

	private void CreatePoints () {
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		float increment = 1f / (resolution - 1);
		for(int i = 0; i < resolution; i++){
			float x = i * increment;
			points[i].position = new Vector3(x, 0f, 0f);
			points[i].startColor = new Color(x, 0f, 0f);
			points[i].startSize = 0.1f;
		}
	}

	void Update () {
		if (currentResolution != resolution || points == null)
			CreatePoints();
		
		FunctionDelegate f = functionDelegates[(int)function];
		for (int i = 0; i < resolution; i++) {
			Vector3 p = points[i].position;
			p.y = f(p.x);
			points[i].position = p;
			Color c = points[i].startColor;
			c.g = p.y;
			points[i].startColor = c;
		}
		GetComponent<ParticleSystem>().SetParticles(points, points.Length);
	}

	private static float Linear (float x) {
		return x;
	}

	private static float Exponential (float x) {
		return x * x;
	}

	private static float Parabola (float x) {
		x = 2f * x - 1f;
		return x * x;
	}

	private static float Sine (float x) {
		return 0.5f + 0.5f * Mathf.Sin(2 * Mathf.PI * x + Time.timeSinceLevelLoad);
	}
}