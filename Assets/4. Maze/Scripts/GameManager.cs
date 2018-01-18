using System.Collections;
using UnityEngine;

/// <summary>
/// Responsible for starting and ending the game.
/// </summary>
public class GameManager : MonoBehaviour {

	public Maze mazePrefab;
	public Player playerPrefab;
	private Maze _mazeInstance;
	private Player _playerInstance;

	void Start () {
		StartCoroutine(BeginGame());
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
			RestartGame();
	}

	/// <summary>
	/// Instantiates the maze cells and the player.
	/// </summary>
	private IEnumerator BeginGame () {
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
		_mazeInstance = Instantiate(mazePrefab) as Maze;
		yield return StartCoroutine(_mazeInstance.Generate());
		_playerInstance = Instantiate(playerPrefab) as Player;
		_playerInstance.SetLocation(_mazeInstance.GetCell(_mazeInstance.RandomCoordinates));
		Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
	}

	/// <summary>
	/// Destroys the current maze, the player and starts a new game.
	/// </summary>
	private void RestartGame () {
		StopAllCoroutines();
		Destroy(_mazeInstance.gameObject);
		if (_playerInstance != null)
			Destroy(_playerInstance.gameObject);
		StartCoroutine(BeginGame());
	}

}
