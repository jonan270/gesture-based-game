using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexmap : MonoBehaviour
{
    public GameObject hexPrefab;


    // Map size in terms of hexes
    int width = 11;
    int height = 22;

    // Offset values
    float xoff = 0.8f;
    float zoff = 0.46f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Do not offset
                if(j % 2 == 0)
                {
                    Instantiate(hexPrefab, new Vector3(2*i*xoff, 0, j*zoff), Quaternion.identity);
                }
                // Else offset
                else
                {
                    Instantiate(hexPrefab, new Vector3(2*i*xoff+xoff, 0, j*zoff), Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
