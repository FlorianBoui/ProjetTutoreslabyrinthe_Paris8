using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Genlaby : MonoBehaviour
{
    public int width, height;
    public Material Wall;
    public GameObject floor, door, doorway, Torch, ceilling;
    private int[,] Maze;
    private List<Vector3> pathMazes = new List<Vector3>();
    private Stack<Vector2> _tiletoTry = new Stack<Vector2>();
    private List<Vector2> offsets = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    private System.Random rnd = new System.Random();
    private int _width, _height;
    private Vector2 _currentTile;
    private List<Vector2> off1 = new List<Vector2> { new Vector2(0, -1), new Vector2(0, 1) };
    private List<Vector2> off2 = new List<Vector2> { new Vector2(1, 0), new Vector2(-1, 0) };
    public Vector2 CurrentTile
    {
        get { return _currentTile; }
        private set
        {
            if (value.x < 1 || value.x >= this.width - 1 || value.y < 1 || value.y >= this.height - 1)
            {
                throw new ArgumentException("CurrentTile must be within the one tile border all around the maze");
            }
            if (value.x % 2 == 1 || value.y % 2 == 1)
            { _currentTile = value; }
            else
            {
                throw new ArgumentException("The current square must not be both on an even X-axis and an even Y-axis, to ensure we can get walls around all tunnels");
            }
        }
    }

    private static Genlaby instance;
    public static Genlaby Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        GenerateMaze();
    }

    void GenerateMaze()
    {
        Maze = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Maze[x, y] = 1;
            }
        }
        CurrentTile = Vector2.one;
        _tiletoTry.Push(CurrentTile);
        Maze = CreateMaze();
        GameObject ptype = null;
        GameObject ptypequad = null;
        GameObject Doorwayprefab;
        GameObject Doorprefab;
        GameObject Ceil;
        GameObject Floor;
        GameObject torch;
        Vector2 vall;
        int exit_door = rnd.Next(4);
        int random_exit = 0;

        if(exit_door == 0)
        {
            random_exit = rnd.Next(1,15);
            for(int i = (width / 2) - 1; i > 0; i--)
            {
                random_exit--;
                if (random_exit == 0 && Canbe_exit(new Vector2(0, i)) == true)
                {
                    Maze[0, i] = 3;
                }
                else
                {
                    if (random_exit == 0)
                        random_exit++;
                }
            }
            if(random_exit > 0)
            {
                for (int i = (width / 2) - 1; i > 0; i--)
                {
                    random_exit--;
                    if (random_exit == 0 && Canbe_exit(new Vector2(i, 0)) == true)
                    {
                        Maze[i, 0] = 3;
                    }
                    else
                    {
                        if (random_exit == 0)
                            random_exit++;
                    }
                }
            }
        }
        if (exit_door == 1)
        {
            random_exit = rnd.Next(1, 15);
            for (int i = width - 1; i > width / 2; i--)
            {
                random_exit--;
                if (random_exit == 0 && Canbe_exit(new Vector2(0, i)) == true)
                {
                    Maze[0, i] = 3;
                }
                else
                {
                    if (random_exit == 0)
                        random_exit++;
                }
            }
            if (random_exit > 0)
            {
                for (int i = (width / 2) - 1; i > 0; i--)
                {
                    random_exit--;
                    if (random_exit == 0 && Canbe_exit(new Vector2(i, width - 1)) == true)
                    {
                        Maze[i, width - 1] = 3;
                    }
                    else
                    {
                        if (random_exit == 0)
                            random_exit++;
                    }
                }
            }
        }
        if (exit_door == 2)
        {
            random_exit = rnd.Next(1, 15);
            for (int i = (width / 2) - 1; i > 0; i--)
            {
                random_exit--;
                if (random_exit == 0 && Canbe_exit(new Vector2(height - 1, i)) == true)
                {
                    Maze[height - 1, i] = 3;
                }
                else
                {
                    if (random_exit == 0)
                        random_exit++;
                }
            }
            if (random_exit > 0)
            {
                for (int i = width - 1; i > width / 2; i--)
                {
                    random_exit--;
                    if (random_exit == 0 && Canbe_exit(new Vector2(i, 0)) == true)
                    {
                        Maze[i, 0] = 3;
                    }
                    else
                    {
                        if (random_exit == 0)
                            random_exit++;
                    }
                }
            }
        }
        if (exit_door == 3)
        {
            random_exit = rnd.Next(1, 15);
            for (int i = width - 1; i > width / 2; i--)
            {
                random_exit--;
                if (random_exit == 0 && Canbe_exit(new Vector2(width - 1, i)) == true)
                {
                    Maze[width -1, i] = 3;
                }
                else
                {
                    if (random_exit == 0)
                        random_exit++;
                }
            }
            if (random_exit > 0)
            {
                for (int i = width - 1; i > width / 2; i--)
                {
                    random_exit--;
                    if (random_exit == 0 && Canbe_exit(new Vector2(i, width - 1)) == true)
                    {
                        Maze[i, width - 1] = 3;
                    }
                    else
                    {
                        if (random_exit == 0)
                            random_exit++;
                    }
                }
            }
        }

        int chose_door = rnd.Next(1,30);
        for (int p = 0; p < width / 2; p++)
        {
            for (int m = 0; m < height / 2; m++)
            {
                if (Maze[p, m] == 0)
                {

                    if (InTheCorner(new Vector2(p, m)) == true && HasThreeWallsIntact(new Vector2(p, m)) == false)
                    {
                        chose_door--;
                        if (chose_door == 0)
                        {
                            if(doororientation(new Vector2(p, m)).x == 0)
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3(p * Doorprefab.transform.localScale.x, 0.5f, (m * Doorprefab.transform.localScale.y) + 0.20f);
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                            }
                            else
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3((p * Doorprefab.transform.localScale.x) + 0.20f, 0.5f, (m * Doorprefab.transform.localScale.y));
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorprefab.transform.rotation = Doorprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab.transform.rotation = Doorwayprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                            }
                            

                        }
                    }
                    else
                    {
                        if (chose_door > 1)
                        {
                            chose_door--;
                        }
                    }
                }

            }
        }
        chose_door = rnd.Next(1,30);

        for (int p = 0; p < width / 2; p++)
        {
            for (int m =(height / 2) + 1; m < height; m++)
            {
                if (Maze[p, m] == 0)
                {

                    if (InTheCorner(new Vector2(p, m)) == true && HasThreeWallsIntact(new Vector2(p, m)) == false)
                    {
                        chose_door--;
                        if (chose_door == 0)
                        {

                            if (doororientation(new Vector2(p, m)).x == 0)
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3(p * Doorprefab.transform.localScale.x, 0.5f, (m * Doorprefab.transform.localScale.y) + 0.20f);
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                            }
                            else
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3((p * Doorprefab.transform.localScale.x) + 0.20f, 0.5f, (m * Doorprefab.transform.localScale.y));
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorprefab.transform.rotation = Doorprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab.transform.rotation = Doorwayprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                            }

                        }
                    }
                    else
                    {
                        if (chose_door > 1)
                        {
                            chose_door--;
                        }
                    }
                }

            }
        }


        chose_door = rnd.Next(1,30);

        for (int p = (width / 2) + 1; p < width; p++)
        {
            for (int m = 0; m < height / 2; m++)
            {
                if (Maze[p, m] == 0)
                {

                    if (InTheCorner(new Vector2(p, m)) == true && HasThreeWallsIntact(new Vector2(p, m)) == false)
                    {
                        chose_door--;
                        if (chose_door == 0)
                        {

                            if (doororientation(new Vector2(p, m)).x == 0)
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3(p * Doorprefab.transform.localScale.x, 0.5f, (m * Doorprefab.transform.localScale.y) + 0.20f);
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                            }
                            else
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3((p * Doorprefab.transform.localScale.x) + 0.20f, 0.5f, (m * Doorprefab.transform.localScale.y));
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorprefab.transform.rotation = Doorprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab.transform.rotation = Doorwayprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                            }
                        }
                    }
                    else
                    {
                        if (chose_door > 1)
                        {
                            chose_door--;
                        }
                    }
                }

            }
        }

        chose_door = rnd.Next(1,30);
 
        for (int p = (width / 2) + 1; p < width; p++)
        {
            for (int m = (height / 2) + 1; m < height; m++)
            {
                if (Maze[p, m] == 0)
                {


                    if (InTheCorner(new Vector2(p, m)) == true && HasThreeWallsIntact(new Vector2(p, m)) == false)
                    {
                        chose_door--;
                        if (chose_door == 0)
                        {

                            if (doororientation(new Vector2(p, m)).x == 0)
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3(p * Doorprefab.transform.localScale.x, 0.5f, (m * Doorprefab.transform.localScale.y) + 0.20f);
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                            }
                            else
                            {
                                Doorprefab = Instantiate(door);
                                Doorprefab.transform.position = new Vector3((p * Doorprefab.transform.localScale.x) +0.20f, 0.5f, (m * Doorprefab.transform.localScale.y));
                                Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorprefab.transform.rotation = Doorprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                                Doorwayprefab = Instantiate(doorway);
                                Doorwayprefab.transform.position = new Vector3(p * Doorwayprefab.transform.localScale.x, 0.5f, m * Doorwayprefab.transform.localScale.y);
                                Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                                Doorwayprefab.transform.rotation = Doorwayprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                            }

                        }
                    }
                    else
                    {
                        if (chose_door > 1)
                        {
                            chose_door--;
                        }
                    }
                }

            }
        }

        for (int i = 0; i <= Maze.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= Maze.GetUpperBound(1); j++)
            {
                if (Maze[i, j] == 1)
                {
                    ptype = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    ptype.transform.position = new Vector3(i * ptype.transform.localScale.x, 1.0f, j * ptype.transform.localScale.y);
                    if (Wall != null)
                    {
                        ptype.GetComponent<Renderer>().material = Wall;
                    }

                    ptype.transform.parent = transform;
                }
                else if (Maze[i, j] == 0)
                {
                    Ceil = Instantiate(ceilling);
                    Ceil.transform.position = new Vector3(i * Ceil.transform.localScale.x, 1.5f, j * Ceil.transform.localScale.y);
                    Ceil.transform.localScale = new Vector3(0.173f, 0.20f, 0.173f);
                    Floor = Instantiate(floor);
                    Floor.transform.position = new Vector3(i * ptype.transform.localScale.x, 0.5f, j * ptype.transform.localScale.y);
                    //Floor.transform.rotation = Quaternion.Euler(90, 0, 0);
                    Floor.transform.localScale = new Vector3(0.173f, 0.20f, 0.173f);
                    if (InTheCorner(new Vector2(i, j)) == false)
                    {
                        vall = GetFirstWall(new Vector2(i, j));
                        

                        if (vall == offsets[0])
                        {

                            torch = Instantiate(Torch);
                            torch.transform.position = new Vector3(i, 1.0f, j+0.5f);
                            torch.transform.rotation = Quaternion.Euler(0, 270, 0);
                            torch.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
                        }
                        else if (vall == offsets[1])
                        {
                            torch = Instantiate(Torch);
                            torch.transform.position = new Vector3(i, 1.0f, j-0.5f);
                            torch.transform.rotation = Quaternion.Euler(0, 90, 0);
                            torch.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
                        }
                        else if (vall == offsets[2])
                        {
                            torch = Instantiate(Torch);
                            torch.transform.position = new Vector3(i+0.5f, 1.0f, j);
                            torch.transform.rotation = Quaternion.Euler(0, 0, 0);
                            torch.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
                        }
                        else if (vall == offsets[3])
                        {
                            torch = Instantiate(Torch);
                            torch.transform.position = new Vector3(i-0.5f, 1.0f, j);
                            torch.transform.rotation = Quaternion.Euler(0, 180, 0);
                            torch.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
                        }


                    }
                }
                else if (Maze[i, j] == 3)
                {
                    if (i == 0 || i == width - 1)
                    {
                        Doorprefab = Instantiate(door);
                        Doorprefab.transform.position = new Vector3(i * Doorprefab.transform.localScale.x, 0.5f, (j * Doorprefab.transform.localScale.y) + 0.20f);
                        Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                        Doorwayprefab = Instantiate(doorway);
                        Doorwayprefab.transform.position = new Vector3(i * Doorwayprefab.transform.localScale.x, 0.5f, j * Doorwayprefab.transform.localScale.y);
                        Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                        Floor = Instantiate(floor);
                        Floor.transform.position = new Vector3(i * ptype.transform.localScale.x, 0.5f, j * ptype.transform.localScale.y);
                        //Floor.transform.rotation = Quaternion.Euler(90, 0, 0);
                        Floor.transform.localScale = new Vector3(0.173f, 0.20f, 0.173f);
                        Ceil = Instantiate(ceilling);
                        Ceil.transform.position = new Vector3(i * Ceil.transform.localScale.x, 1.5f, j * Ceil.transform.localScale.y);
                        Ceil.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);

                    }
                    else
                    {
                        Doorprefab = Instantiate(door);
                        Doorprefab.transform.position = new Vector3((i * Doorprefab.transform.localScale.x) + 0.20f, 0.5f, (j * Doorprefab.transform.localScale.y));
                        Doorprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                        Doorprefab.transform.rotation = Doorprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                        Doorwayprefab = Instantiate(doorway);
                        Doorwayprefab.transform.position = new Vector3(i * Doorwayprefab.transform.localScale.x, 0.5f, j * Doorwayprefab.transform.localScale.y);
                        Doorwayprefab.transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
                        Doorwayprefab.transform.rotation = Doorwayprefab.transform.rotation = Quaternion.Euler(0, 90, 0);
                        Floor = Instantiate(floor);
                        Floor.transform.position = new Vector3(i * ptype.transform.localScale.x, 0.5f, j * ptype.transform.localScale.y);
                        //Floor.transform.rotation = Quaternion.Euler(90, 0, 0);
                        Floor.transform.localScale = new Vector3(0.173f, 0.20f, 0.173f);
                        Ceil = Instantiate(ceilling);
                        Ceil.transform.position = new Vector3(i * Ceil.transform.localScale.x, 1.5f, j * Ceil.transform.localScale.y);
                        Ceil.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
                    }
                }

            }
        }
    }

    private bool InTheCorner(Vector2 WallToCheck)
    {
        int wallcount1 = 0;
        int wallcount2 = 0;

        foreach (var offset1 in off1)
        {
            Vector2 neighborCheck = new Vector2(WallToCheck.x + offset1.x, WallToCheck.y + offset1.y);
            if (IsInside(neighborCheck) && Maze[(int)neighborCheck.x, (int)neighborCheck.y] == 1)
            {
                wallcount1++;
            }
        }


        foreach (var offset2 in off2)
        {
            Vector2 neighborCheck = new Vector2(WallToCheck.x + offset2.x, WallToCheck.y + offset2.y);
            if (IsInside(neighborCheck) && Maze[(int)neighborCheck.x, (int)neighborCheck.y] == 1)
            {
                wallcount2++;
            }
        }

        if (wallcount1 == 2)
        {
            return true;
        }

        if (wallcount2 == 2)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private Vector2 doororientation(Vector2 WallToCheck)
    {
        int wallcount1 = 0;
        int wallcount2 = 0;

        foreach (var offset1 in off1)
        {
            Vector2 neighborCheck = new Vector2(WallToCheck.x + offset1.x, WallToCheck.y + offset1.y);
            if (IsInside(neighborCheck) && Maze[(int)neighborCheck.x, (int)neighborCheck.y] == 1)
            {
                wallcount1++;
            }
        }


        foreach (var offset2 in off2)
        {
            Vector2 neighborCheck = new Vector2(WallToCheck.x + offset2.x, WallToCheck.y + offset2.y);
            if (IsInside(neighborCheck) && Maze[(int)neighborCheck.x, (int)neighborCheck.y] == 1)
            {
                wallcount2++;
            }
        }

        if (wallcount1 == 2)
        {
            return off1[0];
        }

        if (wallcount2 == 2)
        {
            return off2[0];
        }
        return new Vector2(-1,-1);
    }




    public int[,] CreateMaze()
    {
        //local variable to store neighbors to the current square
        //as we work our way through the maze
        List<Vector2> neighbors;
        //as long as there are still tiles to try
        while (_tiletoTry.Count > 0)
        {
            //excavate the square we are on
            Maze[(int)CurrentTile.x, (int)CurrentTile.y] = 0;

            //get all valid neighbors for the new tile
            neighbors = GetValidNeighbors(CurrentTile);

            //if there are any interesting looking neighbors
            if (neighbors.Count > 0)
            {
                //remember this tile, by putting it on the stack
                _tiletoTry.Push(CurrentTile);
                //move on to a random of the neighboring tiles
                CurrentTile = neighbors[rnd.Next(neighbors.Count)];
            }
            else
            {
                //if there were no neighbors to try, we are at a dead-end
                //toss this tile out
                //(thereby returning to a previous tile in the list to check).
                CurrentTile = _tiletoTry.Pop();
            }
        }

        return Maze;
    }
    /// <summary>
    /// Get all the prospective neighboring tiles
    /// </summary>
    /// <param name="centerTile">The tile to test</param>
    /// <returns>All and any valid neighbors</returns>
    private List<Vector2> GetValidNeighbors(Vector2 centerTile)
    {

        List<Vector2> validNeighbors = new List<Vector2>();

        //Check all four directions around the tile
        foreach (var offset in offsets)
        {
            //find the neighbor's position
            Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);

            //make sure the tile is not on both an even X-axis and an even Y-axis
            //to ensure we can get walls around all tunnels
            if (toCheck.x % 2 == 1 || toCheck.y % 2 == 1)
            {
                //if the potential neighbor is unexcavated (==1)
                //and still has three walls intact (new territory)
                if (Maze[(int)toCheck.x, (int)toCheck.y] == 1 && HasThreeWallsIntact(toCheck))
                    {
                    //add the neighbor
                    validNeighbors.Add(toCheck);
                }
            }
        }

        return validNeighbors;
    }

    private bool Canbe_exit(Vector2 coord)
    {
        foreach (var offset in offsets)
        {
            Vector2 toCheck = new Vector2(coord.x + offset.x, coord.y + offset.y);
            if (IsInside(toCheck) && Maze[(int)toCheck.x, (int)toCheck.y] == 0)
            {
                return true;
            }

        }
        return false;
    }

    /// <summary>
    /// Counts the number of intact walls around a tile
    /// </summary>
    /// <param name="Vector2ToCheck">The coordinates of the tile to check</param>
    /// <returns>Whether there are three intact walls (the tile has not been dug into earlier.</returns>
    private bool HasThreeWallsIntact(Vector2 Vector2ToCheck)
    {
        int intactWallCounter = 0;

        //Check all four directions around the tile
        foreach (var offset in offsets)
        {
            //find the neighbor's position
            Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);

            //make sure it is inside the maze, and it hasn't been dug out yet
            if (IsInside(neighborToCheck) && Maze[(int)neighborToCheck.x, (int)neighborToCheck.y] == 1)
                {
                intactWallCounter++;
            }
        }

        //tell whether three walls are intact
        return intactWallCounter == 3;

    }

    private Vector2 GetFirstWall(Vector2 Vector2ToCheck)
    {
        foreach (var offset in offsets)
        {
            //find the neighbor's position
            Vector2 neighborToCheck = new Vector2(Vector2ToCheck.x + offset.x, Vector2ToCheck.y + offset.y);

            //make sure it is inside the maze, and it hasn't been dug out yet
            if ( Maze[(int)neighborToCheck.x, (int)neighborToCheck.y] == 1)
            {
                return offset;
            }
        }
        return new Vector2(0,0);
    }

    private bool IsInside(Vector2 p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
    }
}
