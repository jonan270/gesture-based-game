using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Hextile contains basic information about a specific tile and provides
 * functions for modifying that specific tile
 */

public class Hextile : MonoBehaviour
{
    /********************************************************************
     * tileType contains information of what element the tile belongs to.
     * Enables checks such as " if (hexTile[0,0].tileType == "water") "
     * 
     * tileType can be: "grass", "dessert", "water" or "woods".
     ********************************************************************/
    public string tileType;

    public bool spin; // Should the tile be rotating?
    int angleCount = 0; // The angle of the tile during rotation.

    public Material matgrass;
    public Material matdessert;
    public Material matwater;
    public Material matwoods;

    // Awake runs before start
    void Awake()
    {
        spin = true;
        randomizeType(); //Set to random
    }

    // Update checks what needs to be done to the tile in each frame
    void Update()
    {
        if (spin)
            rotateHex();
    }
    // Takes an argument of type string to control which action the tile should take
    public void affectTile(string effect)
    {
        if (effect == "spin")
            spinTile();
        else if (effect == "typeGrass" || effect == "typeDessert" || effect == "typeWater" || effect == "typeWoods")
            makeType(effect);
        else if (effect == "typeRandom")
            randomizeType();
    }

    public Vector3 getPosition()
    {
        //Debug.Log(transform.position);
        return transform.position;
    }

    // Tells update to initiate spinning state
    private void spinTile()
    {
        spin = true;
    }

    // Spin until 1 rotation has been completed
    void rotateHex()
    {
        angleCount++;
        transform.localEulerAngles = new Vector3(-angleCount, 0, 0);
        if (angleCount == 360)
        {
            spin = false;
            angleCount = 0;
        }
    }

    private void randomizeType()
    {
        int randT = Random.Range(0, 4);
        string type;

        if (randT == 1)
            type = "typeDessert";
        else if (randT == 2)
            type = "typeWater";
        else if (randT == 3)
            type = "typeWoods";
        else
            type = "typeGrass";
        makeType(type);
    }

    // Takes argument of type string to convert tileType and rendering material
    private void makeType(string type)
    {
        spinTile();
        if(type == "typeGrass")
        {
            tileType = "grass";
            GetComponentsInChildren<MeshRenderer>()[0].material = matgrass;
        }
        else if(type == "typeDessert")
        {
            tileType = "dessert";
            GetComponentsInChildren<MeshRenderer>()[0].material = matdessert;
        }
        else if(type == "typeWater")
        {
            tileType = "water";
            GetComponentsInChildren<MeshRenderer>()[0].material = matwater;
        }
        else if (type == "typeWoods")
        {
            tileType = "woods";
            GetComponentsInChildren<MeshRenderer>()[0].material = matwoods;
        }
    }
}
