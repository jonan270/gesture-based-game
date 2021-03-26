using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * 
 * Hextile contains basic information about a specific tile and provides
 * functions for modifying that specific tile
 * 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
 [System.Serializable]
public class Hextile : MonoBehaviour
{
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
     * 
     * tileType contains information of what element the tile belongs to.
     * Enables checks such as " if (hexTile[0,0].tileType == "water") "
     * 
     * tileType can be: "grass", "dessert", "water" or "woods".
     * 
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    public string tileType;

    // Should the tile be rotating?
    public bool spin;

    // The angle of the tile during rotation.
    private int angleCount = 0;

    // Materials for the tilebase of different types
    [SerializeField]
    private Material matgrass;
    [SerializeField]
    private Material matdessert;
    [SerializeField]
    private Material matwater;
    [SerializeField]
    private Material matwoods;

    // Awake runs before start
    void Awake()
    {
        randomizeType(); //Set to random
    }

    // Update checks what needs to be done to the tile in each frame
    private void Update()
    {
        if (spin)
            rotateHex();
    }

    /// Spin until 1 rotation has been completed TODO: Make rotateHex timebased instead of framebased.
    private void rotateHex()
    {
        angleCount++;
        transform.localEulerAngles = new Vector3(-angleCount, 0, 0);
        if (angleCount == 360)
        {
            spin = false;
            angleCount = 0;
        }
    }

    /// Takes an argument of type string to control which action the tile should take
    public void affectTile(string effect)
    {
        if (effect == "spin")
            spinTile();
        else if (effect == "typeGrass" || effect == "typeDessert" || effect == "typeWater" || effect == "typeWoods")
            makeType(effect);
        else if (effect == "typeRandom")
            randomizeType();
    }

    /// Tells update to initiate spinning state
    public void spinTile()
    {
        spin = true;
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

    /// Takes argument of type string to convert tileType and rendering material
    private void makeType(string type)
    {
        if(type == "typeGrass")
        {
            tileType = "grass";
            GetComponentsInChildren<MeshRenderer>()[0].material = matgrass;
            showGrass(true);
            showTrees(false);
            showDunes(false);
            showWaves(false);
        }
        else if(type == "typeDessert")
        {
            tileType = "dessert";
            GetComponentsInChildren<MeshRenderer>()[0].material = matdessert;
            showGrass(false);
            showTrees(false);
            showDunes(true);
            showWaves(false);
        }
        else if(type == "typeWater")
        {
            tileType = "water";
            GetComponentsInChildren<MeshRenderer>()[0].material = matwater;
            showGrass(false);
            showTrees(false);
            showDunes(false);
            showWaves(true);
        }
        else if (type == "typeWoods")
        {
            tileType = "woods";
            GetComponentsInChildren<MeshRenderer>()[0].material = matwoods;
            showGrass(false);
            showTrees(true);
            showDunes(false);
            showWaves(false);
        }
        spinTile();
    }

    // TODO: Reimplement show functions to look for child names instead of indexes.

    /// Sets visibility of trees to true or false according to show
    private void showTrees(bool show)
    {
        if (show)
        {
            showSub(1);
            showSub(2);
            showSub(3);
        }
        else
        {
            hideSub(1);
            hideSub(2);
            hideSub(3);
        }
    }


    /// Sets visibility of dunes to true or false according to show
    private void showDunes(bool show)
    {
        if (show)
        {
            showSub(4);
            showSub(5);
        }
        else
        {
            hideSub(4);
            hideSub(5);
        }
    }


    /// Sets visibility of waves to true or false according to show
    private void showWaves(bool show)
    {
        if (show)
        {
            showSub(6);
        }
        else
        {
            hideSub(6);
        }
    }

    /// Sets visibility of waves to true or false according to show
    private void showGrass(bool show)
    {
        if (show)
        {
            showSub(7);
            showSub(8);
            showSub(9);
        }
        else
        {
            hideSub(7);
            hideSub(8);
            hideSub(9);

        }
    }

    /// Hides submodel for a given index
    private void hideSub(int index)
    {
        GetComponentsInChildren<Renderer>()[index].enabled = false;
    }

    /// Shows submodel for a given index
    private void showSub(int index)
    {
        GetComponentsInChildren<Renderer>()[index].enabled = true;
    }

    /// Get the position of a tile
    public Vector3 getPosition()
    {
        return transform.position;
    }
}
