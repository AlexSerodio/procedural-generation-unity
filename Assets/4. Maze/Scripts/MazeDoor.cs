using UnityEngine;

public class MazeDoor : MazePassage {

	public Transform hinge;
	private static Quaternion _normalRotation = Quaternion.Euler(0f, -90f, 0f);
	private static Quaternion _mirroredRotation = Quaternion.Euler(0f, 90f, 0f);
	private bool _isMirrored;

	private MazeDoor OtherSideOfDoor {
		get {
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}

	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
		if (OtherSideOfDoor != null) {
			_isMirrored = true;
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			if (child != hinge)
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
		}
	}

	public override void OnPlayerEntered () {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation =
			_isMirrored ? _mirroredRotation : _normalRotation;
		OtherSideOfDoor.cell.room.Show();
	}
	
	public override void OnPlayerExited () {
		OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
		OtherSideOfDoor.cell.room.Hide();
	}
}
