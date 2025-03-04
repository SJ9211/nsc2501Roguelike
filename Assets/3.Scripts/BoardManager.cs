using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
        public CellObject ContainedObject;
    }

    #region private
    private CellData[,] m_BoardData;
    private Tilemap m_Tilemap;
    private Grid m_Grid;
    private List<Vector2Int> m_EmptyCellsList;
    #endregion

    #region public
    public int Width;
    public int Heigth;
    public Tile[] GroundTiles;
    public Tile[] wallTiles; // 화면의 테두리
    public PlayerController Player;
    public FoodObject[] FoodPrefab;
    public WallObject[] WallPrefab; // 부셔지는 벽
    public ExitCellObject ExitCellPrefab;
    public int minFood;
    public int maxFood;
    #endregion

    public void Init()
    {
        m_Grid = GetComponentInChildren<Grid>();
        m_Tilemap = GetComponentInChildren<Tilemap>();
        // Initialize the list
        m_EmptyCellsList = new List<Vector2Int>();

        m_BoardData = new CellData[Width, Heigth];

        for (int y = 0; y < Heigth; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Tile tile;
                m_BoardData[x, y] = new CellData();
                if (x == 0 || y == 0 || x == Width - 1 || y == Heigth - 1)
                {
                    // wallTile
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    m_BoardData[x, y].Passable = false;
                }
                else
                {
                    // GroundTile
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x, y].Passable = true;
                    // 비어있는 타일이므로, 빈타일리스트에 넣어준다.
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        Player.Spawn(this, new Vector2Int(1, 1));
        // 플레이어가 등장하는 위치는 빈타일이 아니므로 빼준다.
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));

        // Exit 만드는 방법
        Vector2Int endCorrd = new Vector2Int(Width - 2, Heigth - 2);
        AddObject(Instantiate(ExitCellPrefab), endCorrd);
        m_EmptyCellsList.Remove(endCorrd);

        GenerateWall();
        GenerateFood();
    }

    public void Clean()
    {
        if (m_BoardData == null) return;

        for (int y = 0; y < Heigth; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var CellData = m_BoardData[x, y];
                if (CellData.ContainedObject != null)
                {
                    // 
                    Destroy(CellData.ContainedObject.gameObject);
                }
                SetCellTile(new Vector2Int(x, y), null);
            }
        }
    }


    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile);
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>
                (new Vector3Int(cellIndex.x, cellIndex.y, 0));
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width
             || cellIndex.y < 0 || cellIndex.y >= Heigth)
        {
            return null;
        }

        return m_BoardData[cellIndex.x, cellIndex.y];
    }

    private void GenerateFood()
    {
        int foodCount = Random.Range(minFood, maxFood);

        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            int foodType = Random.Range(0, FoodPrefab.Length); // Prefab 자체를 배열로만듬 
            FoodObject newFood = Instantiate(FoodPrefab[foodType]);
            AddObject(newFood, coord);

        }
    }

    private void GenerateWall()
    {
        int wallCount = Random.Range(6, 10);
        for (int i = 0; i < wallCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];

            m_EmptyCellsList.RemoveAt(randomIndex);
            int wallType = Random.Range(0, WallPrefab.Length);
            WallObject newWall = Instantiate(WallPrefab[wallType]);
            AddObject(newWall, coord);
        }
    }

    private void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainedObject = obj;
        obj.Init(coord);
    }
}
