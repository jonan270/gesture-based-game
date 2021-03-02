using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Hextile contains basic information about a specific tile and provides
 * functions for modifying that specific tile
 */

public class Hextile : MonoBehaviour
{
    public bool spin; // Should the tile be rotating?
    int angleCount = 0; // The angle of the tile during rotation.

    // Start is called before the first frame update
    void Start()
    {
        spin = false;
    }

    // Update checks what needs to be done to the tile in each frame
    void Update()
    {
        if (spin)
            rotateHex();
    }

    public Vector3 getPosition()
    {
        //Debug.Log(transform.position);
        return transform.position;
    }

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

    public void spinTile()
    {
        spin = true;
    }
}
