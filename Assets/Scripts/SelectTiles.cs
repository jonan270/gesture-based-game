using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

public class SelectTiles : MonoBehaviour
{
    public GameObject leftHand, rightHand;
    public Character currentCharacter;
    public LineRenderer leftLine, rightLine;
    public Transform hexTiles;
    public List<Hextile> tilesSelected;
    public PathCreator pathCreator;
    // Start is called before the first frame update
    void Start()
    {
        rightLine = rightHand.GetComponent<LineRenderer>();
        pathCreator = FindObjectOfType<PathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand))
        {
            scanForTiles();
            rightLine.enabled = true;
        }
        if (SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            rightLine.enabled = false;
            //This is where the final list will be sent to the manager, but for now it will just be reset.
            if (tilesSelected.Count != 0)
            {
                pathCreator.FinishPath(currentCharacter.gameObject);
                tilesSelected.Clear();
            }
            //resetTiles();
        }
        if (SteamVR_Actions.default_GrabGrip.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            pathCreator.ClearPath();
            tilesSelected.Clear();
        }
    }

    void scanForTiles()
    {
        RaycastHit hit;

        //Adjust the angle of the ray in order to send it forward.
        Vector3 rotation = rightHand.transform.forward + new Vector3(0, -0.5f, 0);
        if (rotation.y < -1.0f) //The rotation goes from -1 to 1, but loops at those points. The angle goes from -0.9 to -1 to -0.9 again. This makes any overshoot wrap correctly.
        {
            rotation.y -= rotation.y + 1;
        }
        Ray handRay = new Ray(rightHand.transform.position, rotation);
        Vector3[] laserPositions = new Vector3[2] { rightHand.transform.position, rightHand.transform.position + (rotation * 5.0f) };
        //Debug.Log(rightHand.transform.forward + " " + rotation);
        //TODO: Add canceling of path drawing
        if (Physics.Raycast(handRay, out hit, Mathf.Infinity) && hit.collider.gameObject.transform.IsChildOf(hexTiles))
        {
            laserPositions[1] = hit.point;
            rightLine.SetPositions(laserPositions);
            Hextile currentObject = hit.collider.gameObject.GetComponent<Hextile>();
            if ((tilesSelected.Count == 0 && areTilesAdjacent(currentObject, currentCharacter.CurrentTile)) || (tilesSelected.Count != 0 && 
                !tilesSelected.Contains(currentObject) && areTilesAdjacent(currentObject, tilesSelected.Last())))
            {
                tilesSelected.Add(currentObject);
                pathCreator.AddTile(currentObject);
            }
            //Add a check if the hex is adjacent to the last hex here.
            //hit.collider.enabled = false; //This solution is terrible, fix something else
            Debug.Log("woooo");
        }
    }

    bool areTilesAdjacent(Hextile a, Hextile b)
    {
        return (Mathf.Abs(a.tileIndex.x - b.tileIndex.x) <= 1 && Mathf.Abs(a.tileIndex.y - b.tileIndex.y) <= 1);
    }

    //void resetTiles() //Borde nog flyttas till ett annat script som sitter i HexTiles men lägger den här så länge
    //{
    //    foreach (Transform hex in hexTiles)
    //    {
    //        hex.gameObject.GetComponent<Collider>().enabled = true;
    //    }
    //}
}
