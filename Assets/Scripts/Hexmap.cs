using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * 
 * Hexmap generates and keeps track of all generated map tiles.
 * It tells each tile to perform certain via function calls to
 * each specific tile.
 * 
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


//[ExecuteInEditMode] // DEBUG: Toggle this to display tiles in editor

public class Hexmap : MonoBehaviour
{
    public InputMaster controls;
    public PathDraw lineRenderer;

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

    private List<Hextile> tiles = new List<Hextile>();

    // Enable and disable controls when necessary
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Spacebutton.performed += ctx => randomizeHexmap(1000, 3);
    }

    // Start by generating tiles and making a randomized map-config
    void Start()
    {
        generateTiles();
        randomizeHexmap(500, 3);

        lineRenderer.addNodeToPath(hexTiles[4, 0]);
        lineRenderer.addNodeToPath(hexTiles[5, 0]);
        lineRenderer.addNodeToPath(hexTiles[5, 1]);
        lineRenderer.addNodeToPath(hexTiles[5, 2]);
        lineRenderer.addNodeToPath(hexTiles[5, 3]);
    }

    // Update is called once per frame
    void Update()
    {
        //hexTiles[4, 0].spinTile();
    }

    /* * * * * * * * * * * * * * * * * * * * * * * * * * * *
    *
    * Utilizes affectRadius() to generate a random hexmap
    * according to provided parameters.
    * 
    * More iterations generate a more varied map and higher
    * continuity makes larger areas of tiletypes.
    * 
    * For best result use a large number of iterations and 
    * a continuity that is smaller than 8.
    * 
    * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    void randomizeHexmap(int iterations, int continuity)
    {
        for (int i = 0; i < iterations; i++)
        {
            int randX = Random.Range(0, width);
            int randY = Random.Range(0, height);
            int randR = Random.Range(0, continuity);
            int randT = Random.Range(0, 4);
            string effect;

            if (randT == 1)
                effect = "typeDessert";
            else if (randT == 2)
                effect = "typeWater";
            else if (randT == 3)
                effect = "typeWoods";
            else
                effect = "typeGrass";

            affectRadius(randX, randY, randR, effect);
        }
    }

    // Applies a provided effect to a tile for given coordinates if within bounds
    void applyEffect(int x, int y, string effect)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            hexTiles[x,y].affectTile(effect);
    }

    // Generates all tiles and places them in the array.
    private void generateTiles()
    {
        float zSwitch = zoff;
        float xSwitch = xoff;
        Hextile instantiated;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                // Instantiate and add to array
                instantiated = Instantiate(hexPrefab, new Vector3(x * xoff, 0, 2 * zoff * z + zSwitch), Quaternion.identity);
                instantiated.transform.localScale = Vector3.one;
                instantiated.transform.parent = transform;
                hexTiles[x, z] = instantiated;
            }
            // Only offsett odd rows
            if (x % 2 == 0)
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

    // This function is completely unreadable. Good luck! Mvh Jonathan at 23:16
    //
    // Should be changed some day. Preferably before 21:00.
    // TODO: Redo the entire thing? (Atleast it works as intended I guess.)
    void affectRadius(int xCord, int yCord, int radius, string effect)
    {
        if (radius >= 1)
        {
            int nAffect = 0; // Number of tiles on each side of center to affect

            // is center of row consisting of 1 or 2 tiles?
            bool symmetric;
            if (radius % 2 == 0)
            {
                symmetric = true;
                nAffect++;
            }
            else
                symmetric = false;

            // Affect a growing amount of points around centerpoints.
            for (int xi = -radius; xi <= radius; xi++)
            {
                if (xi <= 0)
                    nAffect += 1;
                else
                    nAffect -= 1;

                int range = nAffect / 2 + (radius - 1) / 2;

                if (symmetric)
                {
                    for (int yi = -range; yi <= range; yi++)
                    {
                        applyEffect(xCord + xi, yCord + yi, effect);
                    }
                    symmetric = false;
                }
                else
                {
                    int correction = 0;
                    if (xCord % 2 != 0)
                        correction = 1;
                    if (radius == 1) // If radius is 1 top-center tile must be made implicitly
                        applyEffect(xCord + xi, yCord + 1 - correction, effect);
                    for (int yi = -range; yi <= range; yi++)
                    {
                        applyEffect(xCord + xi, yCord + yi - correction, effect);
                        if (yi > 0)
                        {
                            applyEffect(xCord + xi, yCord + yi + 1 - correction, effect);
                        }
                    }
                    symmetric = true;
                }
            }
        }
        //Radius = 0, just affect 1 tile
        else if(radius == 0)
            applyEffect(xCord, yCord, effect);
    }
}
