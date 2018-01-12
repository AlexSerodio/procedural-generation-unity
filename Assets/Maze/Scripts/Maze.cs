﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	public IntVector2 size;
	public MazeCell cellPrefab;
	public float generationStepDelay = .01f;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;
	private MazeCell[,] _cells;

	public MazeCell GetCell (IntVector2 coordinates) {
		return _cells[coordinates.x, coordinates.z];
	}
	
	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public IEnumerator Generate () {
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		_cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);
		while (activeCells.Count > 0) {
			yield return delay;
			DoNextGenerationStep(activeCells);
		}
	}

	/// <summary>
	/// Checks if a specific coordinate is inside the maze limits.
	/// </summary>
	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	/// <summary>
	/// Creates the first maze cell and store it in a list.
	/// </summary>
	/// <param name="activeCells">A list of already instantiated cells.</param>
	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		activeCells.Add(CreateCell(RandomCoordinates));
	}

	/// <summary>
	/// Checks if the current cell (last cell from the activeCells list) still has
	/// some uninicialized edge. 
	/// If the cell edges already are fully inicialized, remove the current cell 
	/// from the activeCells list and try again.
	/// If the cell still has some uninicialized edge, checks if there is a cell in 
	/// this direction. If so, instantiate a wall between this 
	/// two cells, otherwise creates a new cell (neighbor) int that position 
	/// and instantiate a passage between the two.
	/// </summary>
	/// <param name="activeCells">A list of already instantiated cells.</param>
	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 nextCoordinates = currentCell.coordinates + direction.ToIntVector2();
		if (ContainsCoordinates(nextCoordinates)) {
			MazeCell neighbor = GetCell(nextCoordinates);
			if (neighbor == null) {
				neighbor = CreateCell(nextCoordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		_cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = 
			new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		return newCell;
	}
}