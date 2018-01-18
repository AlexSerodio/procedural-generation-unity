using UnityEngine;
using System.Collections.Generic;

public class MazeRoom : ScriptableObject {

	public int settingsIndex;
	public MazeRoomSettings settings;
	
	private List<MazeCell> _cells = new List<MazeCell>();
	
	public void Add (MazeCell cell) {
		cell.room = this;
		_cells.Add(cell);
	}

	public void Assimilate (MazeRoom room) {
		for (int i = 0; i < room._cells.Count; i++)
			Add(room._cells[i]);
	}

	public void Hide () {
		for (int i = 0; i < _cells.Count; i++)
			_cells[i].Hide();
	}
	
	public void Show () {
		for (int i = 0; i < _cells.Count; i++)
			_cells[i].Show();
	}
}