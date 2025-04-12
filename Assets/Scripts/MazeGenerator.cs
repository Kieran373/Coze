using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    // https://youtu.be/_aeYq5BmDMg?si=FaSx4kuFiwcCUxcp

    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private int _mazeWidthX;

    [SerializeField]
    private int _mazeDepthZ;

    [SerializeField]
    private int _mazeCellBasePlateSize;

    private MazeCell[,] _mazeGrid;

    private IEnumerator Start()
    {
        _mazeGrid = new MazeCell[_mazeWidthX, _mazeDepthZ];

        for (int x = 0; x < _mazeWidthX; x++)
        {
            for (int z = 0; z < _mazeDepthZ; z++)
            {
                _mazeGrid[x, z] = Instantiate(
                    _mazeCellPrefab,
                    new Vector3((x - ((float)_mazeWidthX - 1) / 2) * _mazeCellBasePlateSize, 0, (-1) * (z - ((float)_mazeDepthZ - 1) / 2) * _mazeCellBasePlateSize),
                    Quaternion.identity // Quaternion.identity to specify no rotation
                );
            }
        }

        yield return GenerateMaze(null, _mazeGrid[0, 0]);
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        yield return new WaitForSeconds(0.5f);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                yield return GenerateMaze(currentCell, nextCell);
            }

        } while (nextCell != null);
        
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        IEnumerable<MazeCell> unvisitedCells = GetUnvisitedCells(currentCell);

        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        int arrayX = (int)(x / _mazeCellBasePlateSize + ((float)_mazeWidthX - 1) / 2);
        int arrayZ = (int)((-1) * z / _mazeCellBasePlateSize + ((float)_mazeDepthZ - 1) / 2);

        // is the next cell to the right within the bounds of the grid?
        if (arrayX + 1 < _mazeWidthX)
        {
            MazeCell cellToRight = _mazeGrid[arrayX + 1, arrayZ];

            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (arrayX - 1 >= 0)
        {
            MazeCell cellToLeft = _mazeGrid[arrayX - 1, arrayZ];

            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (arrayZ + 1 < _mazeDepthZ)
        {
            MazeCell cellToFront = _mazeGrid[arrayX, arrayZ + 1];

            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (arrayZ - 1 >= 0)
        {
            MazeCell cellToBack = _mazeGrid[arrayX, arrayZ - 1];

            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }
}
