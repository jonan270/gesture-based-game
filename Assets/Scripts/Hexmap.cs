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
    const int width = 7;
    const int height = 7;
    
    // Hextiles is an array containing all gameobjects at index [x,y]
    public Hextile[,] hexTiles = new Hextile[width,height];

    // Each tile is a hexPrefab
    public Hextile hexPrefab;
    public Raycasthandler rayhandler;

    // Offset values
    float xoff = 0.8f;//0.8f;
    //int xoff = 1;
    float zoff = 0.46f;//0.46f;

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
        float zSwitch = zoff;
        float xSwitch = xoff;
        Hextile instantiated;
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                instantiated = Instantiate(hexPrefab, new Vector3(x * xoff, 0, 2*zoff*z + zSwitch), Quaternion.identity);
                instantiated.transform.localScale = Vector3.one;
                hexTiles[x, z] = instantiated;
            }
            if(x % 2 == 0)
            {
                zSwitch = 0.0f;
            }
            else
            {
                zSwitch = zoff;
            }
            xSwitch += xoff;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Example: rotate tile at index 2,2 every 1000 frames.
        ++count;
        if (count % 1000 == 0)
        {
            //hexTiles[2, 2].spinTile();
            //hexTiles[2, 1].spinTile(); // +0 -1
            //hexTiles[2, 0].spinTile(); // +0 -2
            //hexTiles[2, 3].spinTile(); // +0 +1
            //hexTiles[2, 4].spinTile(); // +0 +2

            //hexTiles[1, 3].spinTile(); // -1 +2
            //hexTiles[1, 1].spinTile(); // -1 -1

            hexTiles[0, 0].spinTile();
            hexTiles[1, 0].spinTile();
            hexTiles[0, 6].spinTile();
            hexTiles[3, 6].spinTile();
        }

    }


    void areaTarget(int xcord, int ycord, int radius)
    {
        
    }
}
