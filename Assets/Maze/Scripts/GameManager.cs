using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for starting and ending the game.
/// </summary>
public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	private Maze _mazeInstance;

	void Start () {
		BeginGame();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
			RestartGame();
	}

	/// <summary>
	/// Instantiates the maze cells.
	/// </summary>
	private void BeginGame () {
		_mazeInstance = Instantiate(mazePrefab) as Maze;
		StartCoroutine(_mazeInstance.Generate());
	}

	/// <summary>
	/// Destroys the current maze and starts a new one.
	/// </summary>
	private void RestartGame () {
		StopAllCoroutines();
		Destroy(_mazeInstance.gameObject);
		BeginGame();
	}

}
