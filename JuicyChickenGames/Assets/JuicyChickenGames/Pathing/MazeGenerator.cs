using System;
using System.Collections.Generic;

namespace JuicyChickenGames.Pathing
{
	public class MazeGenerator
	{
		private readonly int width;
		private readonly int height;
		private readonly bool[,] visited;
		private readonly Random random;
		public MazeCell[,] Maze;

		private static readonly Dictionary<MazeDirection, (int x, int y)> DirectionOffset = new Dictionary<MazeDirection, (int x, int y)>()
		{
			{ MazeDirection.Up, (0, 1) },
			{ MazeDirection.Right, (1, 0) },
			{ MazeDirection.Down, (0, -1) },
			{ MazeDirection.Left, (-1, 0) },
		};

		public MazeGenerator(int width, int height, Random random = null)
		{
			this.width = width;
			this.height = height;
			this.random = random ?? new Random();
			visited = new bool[width, height];
			Maze = new MazeCell[width, height];
		}

		public void GenerateMaze()
		{
			InitializeMaze();
			GeneratePath((0, 0));
		}

		private void InitializeMaze()
		{
			// Initialize maze with walls
			for (int i = 0; i < Maze.GetLength(0); i++)
			{
				for (int j = 0; j < Maze.GetLength(1); j++)
				{
					Maze[i, j] = MazeCell.Wall;
				}
			}
		}

		private void GeneratePath((int x, int y) position)
		{
			visited[position.x, position.y] = true;
			Maze[position.x, position.y] = MazeCell.Path;

			MazeDirection[] directions =
			{
				MazeDirection.Up,
				MazeDirection.Right,
				MazeDirection.Down,
				MazeDirection.Left
			};
			Shuffle(directions);

			foreach (MazeDirection dir in directions)
			{
				var offset = DirectionOffset[dir];
				var newPosition = (position.x + offset.x, position.y + offset.y);
				var newPosition2 = (newPosition.x + offset.x, newPosition.y + offset.y);

				if (IsInBounds(newPosition2) && !visited[newPosition2.x, newPosition2.y])
				{
					Maze[newPosition.x, newPosition.y] = MazeCell.Path;
					GeneratePath(newPosition2);
				}
			}
		}

		private bool IsInBounds((int x, int y) position)
		{
			return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
		}

		private void Shuffle(MazeDirection[] array)
		{
			for (int i = array.Length - 1; i > 0; i--)
			{
				int j = random.Next(i + 1);
				(array[i], array[j]) = (array[j], array[i]);
			}
		}
	}

	public enum MazeCell
	{
		Path,
		Wall
	}

	public enum MazeDirection
	{
		Up,
		Right,
		Down,
		Left,
	}
}
