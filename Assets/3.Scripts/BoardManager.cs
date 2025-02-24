using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable;
    }

    #region private
    private CellData[,] m_BoardData;
    private Tilemap m_Tilemap;
    private Grid m_Grid;
    #endregion

    #region public
    public int Width;
    public int Heigth;
    public Tile[] GroundTiles;
    public Tile[] wallTiles;
    public PlayerController Player;
    #endregion
    void Start()
    {
        m_Grid = GetComponentInChildren<Grid>();
        m_Tilemap = GetComponentInChildren<Tilemap>();

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
                }
                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        Player.Spawn(this, new Vector2Int(1, 1));
    }


    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
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
}
