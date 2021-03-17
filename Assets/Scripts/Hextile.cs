using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * 
 * Hextile contains basic information about a specific tile and provides
 * functions for modifying that specific tile
 * 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
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
    //public string tileType;

    public ElementState tileType;

    // Should the tile be rotating?
    public bool spin;

    // The angle of the tile during rotation.
    private int angleCount = -180;

    // Materials for the tilebase of different types
    [SerializeField]
    private Material matgrass;
    [SerializeField]
    private Material matdessert;
    [SerializeField]
    private Material matwater;
    [SerializeField]
    private Material matwoods;

    public GameObject tile, forest, dessert, water, grass;

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
        transform.localEulerAngles = new Vector3(angleCount, 0, 0);
        if (angleCount == 0)
        {
            spin = false;
            angleCount = -180;
        }
    }

    ///// Takes an argument of type string to control which action the tile should take
    //public void affectTile(ElementState _element)
    //{
    //    // if (effect == "spin")
    //    //     spinTile();
    //    else if (_element == "typeGrass" || _element == "typeDessert" || _element == "typeWater" || _element == "typeWoods")
    //        makeType(_element);
    //    //else if (effect == "typeRandom")
    //    //    randomizeType();
    //}

    /// Tells update to initiate spinning state
    public void spinTile()
    {
        transform.localEulerAngles = new Vector3(-180, 0, 0);
        spin = true;
    }

    private void randomizeType()
    {
        int randT = Random.Range(0, 4);
        // string type;
        ElementState state;
        if (randT == 1)
            state = ElementState.Earth;
        else if (randT == 2)
            state = ElementState.Fire;
        else if (randT == 3)
            state = ElementState.Water;
        else
            state = ElementState.Wind;

        makeType(state);
    }

    /// Takes argument of type string to convert tileType and rendering material
    public void makeType(ElementState type)
    {
        if(type == ElementState.Wind)
        {
            tileType = ElementState.Wind;
            tile.GetComponent<MeshRenderer>().material = matgrass;
            ResetTiles();
            grass.SetActive(true);
        }
        else if(type == ElementState.Fire)
        {
            tileType = ElementState.Fire;
            tile.GetComponent<MeshRenderer>().material = matdessert;
            ResetTiles();
            dessert.SetActive(true);
        }
        else if(type == ElementState.Water)
        {
            tileType = ElementState.Water;
            tile.GetComponent<MeshRenderer>().material = matwater;
            ResetTiles();
            water.SetActive(true);
        }
        else if (type == ElementState.Earth)
        {
            tileType = ElementState.Earth;
            tile.GetComponent<MeshRenderer>().material = matwoods;
            ResetTiles();
            forest.SetActive(true);
        }
        spinTile();
    }

    private void ResetTiles() 
    {
        forest.SetActive(false);
        water.SetActive(false);
        grass.SetActive(false);
        dessert.SetActive(false);
    }

    /// Get the position of a tile
    public Vector3 Position {
        get { return transform.position; }
    }
}
