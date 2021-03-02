using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Hexmap generates and keeps track of all generated map tiles
 * 
 * 
 */


//[ExecuteInEditMode] //DEBUG: Toggle this to display tiles in editor

public class Hexmap : MonoBehaviour
{
    // Map size in terms of hexes
    const int width = 11;
    const int height = 22;
    
    // Hextiles is an array containing all gameobjects at index [x,y]
    public GameObject[,] hexTiles = new GameObject[width,height];

    // Each tile is a hexPrefab
    public GameObject hexPrefab;

    // Offset values
    float xoff = 0.8f;
    float zoff = 0.46f;

    // Start generates all tiles and places them in the array.
    void Start()
    {
        GameObject instantiated;
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
                Debug.Log(hexTiles[i, j]);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
