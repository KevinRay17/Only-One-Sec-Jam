using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    public TileType[] tileTypes;

    private int[,] tiles;

    private int mapSizeX = 10;

    private int mapSizeY = 10;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 1;
            }
        }

        //Map Pathing
        tiles[4, 4] = 0;
        tiles[5, 4] = 0;
        tiles[6, 4] = 0;
        tiles[7, 4] = 0;
        tiles[8, 4] = 0;
        
        tiles[4, 5] = 0;
        tiles[4, 6] = 0;
        tiles[8, 5] = 0;
        tiles[8, 6] = 0;

        GenerateMap();

    }

    void GenerateMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                Instantiate(tt.tileVisualPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }  
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
