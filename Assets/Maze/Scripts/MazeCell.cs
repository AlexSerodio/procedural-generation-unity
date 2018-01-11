using UnityEngine;

public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;
	private int _initializedEdgeCount;

	private MazeCellEdge[] _edges = new MazeCellEdge[MazeDirections.COUNT];

	public MazeCellEdge GetEdge (MazeDirection direction) {
		return _edges[(int)direction];
	}
	
	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
		_edges[(int)direction] = edge;
		_initializedEdgeCount++;
	}

	public bool IsFullyInitialized {
		get {
			return _initializedEdgeCount == MazeDirections.COUNT;
		}
	}

	public MazeDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range(0, MazeDirections.COUNT - _initializedEdgeCount);
			for (int i = 0; i < MazeDirections.COUNT; i++) {
				if (_edges[i] == null) {
					if (skips == 0)
						return (MazeDirection)i;
					skips--;
				}
			}
			throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
		}
	}

}
