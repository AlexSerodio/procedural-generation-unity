using UnityEngine;
using System.Collections.Generic;

public class MazeRoom : ScriptableObject {

	public int settingsIndex;
	public MazeRoomSettings settings;
	
	private List<MazeCell> cells = new List<MazeCell>();
	
	public void Add (MazeCell cell) {
		cell.room = this;
		cells.Add(cell);
	}

	public void Assimilate (MazeRoom room) {
		for (int i = 0; i < room.cells.Count; i++)
			Add(room.cells[i]);
	}
}