using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor {

	private BezierSpline _spline;
	private Transform _handleTransform;
	private Quaternion _handleRotation;
	private const int _stepsPerCurve = 10;
	private const float _directionScale = 0.5f;
	private const float _handleSize = 0.04f;
	private const float _pickSize = 0.06f;
	
	private int _selectedIndex = -1;

	private void OnSceneGUI () {
		_spline = target as BezierSpline;

		//references to line position and rotation
		_handleTransform = _spline.transform;
		_handleRotation = Tools.pivotRotation == PivotRotation.Local ?
			_handleTransform.rotation : Quaternion.identity;

		Vector3 p0 = ShowPoint(0);
		for (int i = 1; i < _spline.points.Length; i += 3) {
			Vector3 p1 = ShowPoint(i);
			Vector3 p2 = ShowPoint(i+1);
			Vector3 p3 = ShowPoint(i+2);

			Handles.color = Color.gray;
			Handles.DrawLine(p0, p1);
			Handles.DrawLine(p2, p3);

			Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
			p0 = p3;
		}
		ShowDirections();
	}

	private void ShowDirections () {
		Handles.color = Color.green;
		Vector3 point = _spline.GetPoint(0f);
		Handles.DrawLine(point, point + _spline.GetDirection(0f) * _directionScale);
		int steps = _stepsPerCurve * _spline.CurveCount;
		for (int i = 1; i <= steps; i++) {
			point = _spline.GetPoint(i / (float)steps);
			Handles.DrawLine(point, point + _spline.GetDirection(i / (float)steps) * _directionScale);
		}
	}

	private Vector3 ShowPoint (int index) {
		Vector3 point = _handleTransform.TransformPoint(_spline.points[index]);
		Handles.color = Color.white;

		float size = HandleUtility.GetHandleSize(point);
		if (Handles.Button(point, _handleRotation, size * _handleSize, size * _pickSize, Handles.DotHandleCap))
			_selectedIndex = index;
		
		if (_selectedIndex == index) {
			EditorGUI.BeginChangeCheck();
			point = Handles.DoPositionHandle(point, _handleRotation);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(_spline, "Move Point");
				EditorUtility.SetDirty(_spline);
				_spline.points[index] = _handleTransform.InverseTransformPoint(point);
			}
		}
		return point;
	}

	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		_spline = target as BezierSpline;
		if (GUILayout.Button("Add Curve")) {
			Undo.RecordObject(_spline, "Add Curve");
			_spline.AddCurve();
			EditorUtility.SetDirty(_spline);
		}
	}
}
