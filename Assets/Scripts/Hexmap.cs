using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Hexmap generates and keeps track of all generated map tiles.
 * It tells each tile to perform certain via function calls to
 * each specific tile.
 */


//[ExecuteInEditMode] //DEBUG: Toggle this to display tiles in editor

public class Hexmap : MonoBehaviour
{
    int count;

    // Map size in terms of hexes
    const int width = 11;
    const int height = 22;
    
    // Hextiles is an array containing all gameobjects at index [x,y]
    public Hextile[,] hexTiles = new Hextile[width,height];

    // Each tile is a hexPrefab
    public Hextile hexPrefab;
    public Raycasthandler rayhandler;

    // Offset values
    float xoff = 0.8f;
    float zoff = 0.46f;

    void Awake()
    {
        //rayhandler.controls.Player.Tilespin.performed += ctx => Test();
    }

    void Test()
    {
        Debug.Log("yo");
    }

    // Start generates all tiles and places them in the array.
    void Start()
    {
        Hextile instantiated;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Do not offset
                if(j % 2 == 0)
                {
                    instantiated = Instantiate(hexPrefab, new Vector3(2 * i * xoff, 0, j * zoff), Quaternion.identity);
                }
                // Else offset
                else
                {
                     instantiated = Instantiate(hexPrefab, new Vector3(2 * i * xoff + xoff, 0, j * zoff), Quaternion.identity);
                }
                instantiated.transform.localScale = Vector3.one;
                hexTiles[i, j] = instantiated;
                //Debug.Log(hexTiles[i, j]);
            }
        }
        //hexTiles[2, 2].spinTile();
    }


    // Update is called once per frame
    void Update()
    {
        // Example: rotate tile at index 2,2 every 1000 frames.
        ++count;
        if (count % 1000 == 0)
        {
            hexTiles[2, 2].spinTile();
            hexTiles[2, 1].spinTile(); // +0 -1
            hexTiles[2, 0].spinTile(); // +0 -2
            hexTiles[2, 3].spinTile(); // +0 +1
            hexTiles[2, 4].spinTile(); // +0 +2

            hexTiles[1, 3].spinTile(); // -1 +2
            hexTiles[1, 1].spinTile(); // -1 -1

            //hexTiles[2, 3].spinTile();
            //hexTiles[2, 1].spinTile();
        }

    }

    void areaTarget(int radius)
    {

    }
}
