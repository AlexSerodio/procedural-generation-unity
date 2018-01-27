using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextureCreator2))]
public class TextureCreatorInspector2 : Editor {

	private TextureCreator2 _creator;

	private void OnEnable () {
		_creator = target as TextureCreator2;
		Undo.undoRedoPerformed += RefreshCreator;
	}

	private void OnDisable () {
		Undo.undoRedoPerformed -= RefreshCreator;
	}

	private void RefreshCreator () {
		if (Application.isPlaying)
			_creator.FillTexture();
	}

	public override void OnInspectorGUI () {
		EditorGUI.BeginChangeCheck();
		DrawDefaultInspector();
		if (EditorGUI.EndChangeCheck())
			RefreshCreator();
	}
}