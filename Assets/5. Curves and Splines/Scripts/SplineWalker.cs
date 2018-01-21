using UnityEngine;

public enum SplineWalkerMode {
	Once,
	Loop,
	PingPong
}

public class SplineWalker : MonoBehaviour {

	public BezierSpline spline;
	public float duration;
	public bool lookForward;
	public SplineWalkerMode mode;

	private bool _goingForward = true;
	private float _progress;

	private void Update () {
		if (_goingForward) {
			_progress += Time.deltaTime / duration;
			if (_progress > 1f) {
				if (mode == SplineWalkerMode.Once) {
					_progress = 1f;
				}
				else if (mode == SplineWalkerMode.Loop) {
					_progress -= 1f;
				}
				else {
					_progress = 2f - _progress;
					_goingForward = false;
				}
			}
		}
		else {
			_progress -= Time.deltaTime / duration;
			if (_progress < 0f) {
				_progress = -_progress;
				_goingForward = true;
			}
		}
		Vector3 position = spline.GetPoint(_progress);
		transform.localPosition = position;
		if (lookForward)
			transform.LookAt(position + spline.GetDirection(_progress));
		
	}
}