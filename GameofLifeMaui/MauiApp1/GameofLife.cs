using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
public class GameofLife
{
    private readonly int x;
    private readonly int y;
    private int count = 1;
    private String[,] oldGrid;
    //Check that there are live cells in the grid. Returns true if at least one cell is alive
    private static bool IsAlive(String[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == "#") return true;
            }
        }
        return false;
    }

    //Check that there is change between generations. Returns false if a change is found
    private static bool IsEqual(String[,] grid, String[,] newGrid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] != newGrid[i, j]) return false;
            }
        }
        return true;
    }

    //Find number of neighbours for a given cell
    private static int FindNeighbours(String[,] grid, int x, int y)
    {
        int liveNeighbours = 0;
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                //If the cell is inside the grid
                if ((i >= 0 && i < grid.GetLength(0)) &&
                    (j >= 0 && j < grid.GetLength(1)))
                {
                    if (grid[i, j] == "#") liveNeighbours++;
                }
            }
        }
        return liveNeighbours;
    }

    public GameofLife(int rows, int colums)
    {
        x = rows;
        y = colums;

        //Populate grid which holds the -1 generation
        oldGrid = new string[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                oldGrid[i, j] = ".";
            }
        }
    }

    public Tuple<String[,], int, bool> NextGen(String[,] grid)
    {
        //Grid to be updated with the new generation
        String[,] newGrid = new String[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                int neightbours = GameofLife.FindNeighbours(grid, i, j);
                //If less than 2 or more than 3 neighbours, a live cell dies (-1 to account for itself being alive)
                if (grid[i, j] == "#" && (neightbours - 1 < 2 || neightbours - 1 > 3)) newGrid[i, j] = ".";
                //If exactly 3 live neighbours, a dead cell is revived
                else if (grid[i, j] == "." && neightbours == 3) newGrid[i, j] = "#";
                //else it stays the same
                else newGrid[i, j] = grid[i, j];
            }
        }

        //If the grid is empty, or generations are repeating, return with a false flag representing the end of the game
        count++;
        if (!GameofLife.IsAlive(newGrid) || GameofLife.IsEqual(grid, newGrid) || GameofLife.IsEqual(oldGrid, newGrid))
        {
            return Tuple.Create(newGrid, count, false);
        }

        //Store the current grid
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                oldGrid[i, j] = grid[i, j];
            }
        }

        //Return the grid, generation counter, and a flag representing the continued game
        return Tuple.Create(newGrid, count, true);
    }
}
}