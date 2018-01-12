using UnityEngine;

/// <summary>
/// Represents a maze cell with scale of (1,1,1).
/// </summary>
public class MazeCell : MonoBehaviour {

	public IntVector2 coordinates;
	private int _initializedEdgeCount;

	private MazeCellEdge[] _edges = new MazeCellEdge[MazeDirections.COUNT];

	/// <summary>
	/// Gets a specific cell edge.
	/// </summary>
	public MazeCellEdge GetEdge (MazeDirection direction) {
		return _edges[(int)direction];
	}
	
	/// <summary>
	/// Sets a new cell edge.
	/// </summary>
	public void SetEdge (MazeDirection direction, MazeCellEdge edge) {
		_edges[(int)direction] = edge;
		_initializedEdgeCount++;
	}

	/// <summary>
	/// Checks if the cell already has all the edges inicialized.
	/// </summary>
	public bool IsFullyInitialized {
		get {
			return _initializedEdgeCount == MazeDirections.COUNT;
		}
	}

	/// <summary>
	/// Returns a random uninitialized edge direction.
	/// </summary>
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
