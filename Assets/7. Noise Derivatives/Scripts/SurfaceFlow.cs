using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SurfaceFlow : MonoBehaviour {

	public SurfaceCreator surface;
	public float flowStrength;

	private ParticleSystem _system;
	private ParticleSystem.Particle[] _particles;

	private void LateUpdate () {
		if (_system == null)
			_system = GetComponent<ParticleSystem>();
		
		if (_particles == null || _particles.Length < _system.maxParticles)
			_particles = new ParticleSystem.Particle[_system.maxParticles];

		int particleCount = _system.GetParticles(_particles);
		PositionParticles();
		_system.SetParticles(_particles, particleCount);
	}
	
	private void PositionParticles () {
		Quaternion q = Quaternion.Euler(surface.rotation);
		Quaternion qInv = Quaternion.Inverse(q);
		NoiseMethod2 method = Noise2.noiseMethods[(int)surface.type][surface.dimensions - 1];
		float amplitude = surface.damping ? surface.strength / surface.frequency : surface.strength;
		for (int i = 0; i < _particles.Length; i++) {
			Vector3 position = _particles[i].position;
			Vector3 point = q * new Vector3(position.x, position.z) + surface.offset;
			NoiseSample sample = Noise2.Sum(method, point,
				surface.frequency, surface.octaves, surface.lacunarity, surface.persistence);
			sample = surface.type == NoiseMethodType2.Value ? (sample - 0.5f) : (sample * 0.5f);
			sample *= amplitude;
			sample.derivative = qInv * sample.derivative;
			Vector3 curl = new Vector3(sample.derivative.y, 0f, -sample.derivative.x);
			position += curl * Time.deltaTime * flowStrength;
			position.y = sample.value + _system.startSize;
			_particles[i].position = position;
		}
	}
}