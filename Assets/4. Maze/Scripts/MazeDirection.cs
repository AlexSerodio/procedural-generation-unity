using UnityEngine;

/// <summary>
/// Contains four directions in which the maze can expand.
/// </summary>
public enum MazeDirection
{
    North,
	East,
	South,
	West
}

/// <summary>
/// Auxiliar class with extension methods that handle the MazeDirection enum.
/// </summary>
public static class MazeDirections
{
    public const int COUNT = 4;

    private static IntVector2[] _vectors = {
        new IntVector2(0, 1),
        new IntVector2(1, 0),
        new IntVector2(0, -1),
        new IntVector2(-1, 0)
    };

	/// <summary>
	/// Same as MazeDirection but inheritdoc the opposite order.
	/// </summary>
    private static MazeDirection[] _opposites = {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East
    };

    private static Quaternion[] _rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };

    /// <summary>
    /// Generates a random MazeDirection.
    /// </summary>
    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, COUNT);
        }
    }

    /// <summary>
    /// Transforms a MazeDirection value in to an IntVector2.
    /// </summary>
    public static IntVector2 ToIntVector2(this MazeDirection direction)
    {
        return _vectors[(int)direction];
    }

    public static MazeDirection GetOpposite(this MazeDirection direction)
    {
        return _opposites[(int)direction];
    }

    public static Quaternion ToRotation(this MazeDirection direction)
    {
        return _rotations[(int)direction];
    }

    public static MazeDirection GetNextClockwise(this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + 1) % COUNT);
    }

    public static MazeDirection GetNextCounterclockwise(this MazeDirection direction)
    {
        return (MazeDirection)(((int)direction + COUNT - 1) % COUNT);
    }
}