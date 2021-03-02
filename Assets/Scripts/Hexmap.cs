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
    const int width = 20;
    const int height = 20;
    
    // Hextiles is an array containing all gameobjects at index [x,y]
    public Hextile[,] hexTiles = new Hextile[width,height];

    // Each tile is a hexPrefab
    public Hextile hexPrefab;
    public Raycasthandler rayhandler;

    // Offset values
    float xoff = 0.8f;//0.8f;
    float zoff = 0.46f;//0.46f;

    void Awake()
    {
        //rayhandler.controls.Player.Tilespin.performed += ctx => Test();
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

            //hexTiles[0, 0].spinTile();
            //hexTiles[1, 0].spinTile();
            //hexTiles[0, 6].spinTile();
            //hexTiles[3, 6].spinTile();
            affectRadius(5, 5, 3);
            affectRadius(10, 12, 2);
        }

    }

    // This function is completely unreadable. Good luck! Mvh Jonathan vid klockan 23:16
    // Should be changed some day. Preferably before 21:00.

    //TODO: Check for out of bounds
    void affectRadius(int xCord, int yCord, int radius)
    {
        int nAffect = 0; // Number of tiles on each side of center to affect

        // is center of first row consisting of 1 or 2 tiles?
        bool symmetric;
        if (radius % 2 == 0)
        {
            symmetric = true;
            nAffect++;
        }
        else
            symmetric = false;

        // Affect a growing amount of points around centerpoints.
        for(int xi = -radius; xi <= radius; xi++)
        {
            if (xi <= 0)
                nAffect += 1; 
            else
                nAffect -= 1;

            int range = nAffect / 2 + (radius - 1) / 2;

            if (symmetric)
            {   
                for(int yi = -range; yi <= range; yi++)
                {
                    hexTiles[xCord + xi, yCord + yi].spinTile();
                }
                symmetric = false;
            }
            else
            {
                // Weird correction necessary? I dont know.
                int correction = 0;
                if (xCord % 2 != 0)
                    correction = 1;

                for (int yi = -range; yi <= range; yi++)
                {
                    hexTiles[xCord + xi, yCord + yi - correction].spinTile();
                    if(yi > 0)
                    {
                        hexTiles[xCord + xi, yCord + yi + 1 - correction].spinTile();
                    }
                }
                symmetric = true;
            }
        }
    }
}
