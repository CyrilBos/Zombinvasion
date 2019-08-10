using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject verticalRoad, horizontalRoad, crossRoad;

    [SerializeField]
    private List<GameObject> buildings;

    [SerializeField]
    private int mapGridSize = 10;
    private int[,] grid;

    public void GenerateLevel()
    {
        grid = new int[mapGridSize, mapGridSize];
        for (int i = 0; i < mapGridSize; i++)
        {
            for (int j = 0; j < mapGridSize; j++)
            {
                if (grid[i, j] == 0)
                {
                    // if we are on the first row and there is a building on the left column,
                    // then we can make a vetical road
                    if (i == 0 && j > 0 && grid[i, j-1] == 1)
                    {
                        if (IsARoad())
                        {
                            for (int k = i; k < mapGridSize; k++)
                            {
                                // vertical road
                                grid[k, j] -= 1;
                            }
                        }
                        else
                        {
                            grid[i, j] = 1;
                        }
                    }
                    // if we are on the first column and there is a building above,
                    // then we can make a horizontal road
                    else if (j == 0 && i > 0 && grid[i - 1, j] == 1)
                    {
                        if (IsARoad())
                        {
                            for (int k = j; k < mapGridSize; k++)
                            {
                                grid[i, k] -= 1;
                            }
                        } else
                        {
                            grid[i, j] = 1;
                        }
                    }
                    else
                    {
                        grid[i, j] = 1;
                    }
                }
             
            }
        }

        Vector3 position = new Vector3(0, 0, 0);
        
        string map = "";
        for (int i = 0; i < mapGridSize; i++)
        {
            for (int j = 0; j < mapGridSize; j++)
            {
                map += grid[i, j];
                if (grid[i, j] == 1)
                {
                    InstantiateBuilding(position);
                } else if (grid[i, j] == -1) {
                    if (i == 0 && j == 0)
                    {
                        if (grid[i+1, j] == -1)
                        {
                            InstantiateVerticalRoad(position);
                        } else if (grid[i, j+1] == -1)
                        {
                            InstantiateHorizontalRoad(position);
                        }
                    }
                    if (i > 0 && grid[i-1, j] == -1)
                    {
                        InstantiateVerticalRoad(position);
                    } else if (j > 0 && grid[i, j-1] == -1)
                    {
                        InstantiateHorizontalRoad(position);
                    }
                } else if (grid[i, j] == -2)
                {
                    InstantiateCrossRoad(position);
                }
            }
            position.x = 0;
            map += "\n";
        }

        Debug.Log(map);
        
    }

    private bool IsARoad()
    {
        return Random.Range(0, 100) < 50;
    }

    private void InstantiateBuilding(Vector3 position)
    {
        int index = Random.Range(0, buildings.Count - 1);
        Instantiate(buildings[index], position, Quaternion.identity);
    }

    private void InstantiateVerticalRoad(Vector3 position)
    {
        Instantiate(verticalRoad, position, verticalRoad.transform.rotation);
    }

    private void InstantiateHorizontalRoad(Vector3 position)
    {
        Instantiate(horizontalRoad, position, horizontalRoad.transform.rotation);
    }

    private void InstantiateCrossRoad(Vector3 position)
    {
        Instantiate(crossRoad, position, crossRoad.transform.rotation);
    }
}
