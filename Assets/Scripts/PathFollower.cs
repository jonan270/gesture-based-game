using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is just a simple solution that should be reworked.
public class PathFollower : MonoBehaviour
{
    public List<Hextile> moveNodes = new List<Hextile>();
    public PathDraw pathDrawer;

    Vector3 currentPos;

    int moveCounter;
    int nodeIndex;
    const float stepSize = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        
        moveCounter = 0;
        nodeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moveNodes = pathDrawer.tilesToDraw;
        int size = moveNodes.Count;
        if (size > 0)
        {
            currentPos = transform.position;
            transform.position = Vector3.Lerp(currentPos, moveNodes[nodeIndex].getPosition(), moveCounter*stepSize);
            moveCounter++;
            //Debug.Log()
            if (moveCounter * stepSize >= 1)
            {
                moveCounter = 0;
                if(nodeIndex < size - 1)
                    nodeIndex++;
            }
        }
    }
}
