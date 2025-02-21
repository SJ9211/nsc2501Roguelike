using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private Tilemap m_Tilemap;

    public int Width;
    public int Heigth;
    public Tile[] GroundTiles;
    void Start()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();

        for (int y = 0; y < Heigth; y++)
        {
            for (int x =0; x < Width; x++)
            {
                int tileNumber = Random.Range(0, GroundTiles.Length);
                m_Tilemap.SetTile(new Vector3Int(x,y,0), GroundTiles[tileNumber]);
            }
        }
    }


    void Update()
    {
        
    }
}
