using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

public class SelectTiles : MonoBehaviour
{
    public GameObject leftHand, rightHand, currentCharacter;
    public Transform hexTiles;
    public List<Hextile> tilesSelected;
    public PathCreator pathCreator;
    // Start is called before the first frame update
    void Start()
    {
        pathCreator = FindObjectOfType<PathCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SteamVR_Actions.default_GrabPinch.GetState(SteamVR_Input_Sources.RightHand))
            scanForTiles();
        if (SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            //This is where the final list will be sent to the manager, but for now it will just be reset.
            pathCreator.FinishPath(currentCharacter);
            tilesSelected.Clear();
            //resetTiles();
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
        //Debug.Log(rightHand.transform.forward + " " + rotation);

        if (Physics.Raycast(handRay, out hit, Mathf.Infinity) && hit.collider.gameObject.transform.IsChildOf(hexTiles))
        {
            Hextile currentObject = hit.collider.gameObject.GetComponent<Hextile>();
            if (tilesSelected.Count == 0 || (!tilesSelected.Contains(currentObject) && Mathf.Abs(currentObject.tileIndex.x - tilesSelected.LastOrDefault().tileIndex.x) <= 1 && 
                Mathf.Abs(currentObject.tileIndex.y - tilesSelected.LastOrDefault().tileIndex.y) <= 1))
            {
                tilesSelected.Add(currentObject);
                pathCreator.AddTile(currentObject);
            }
            //Add a check if the hex is adjacent to the last hex here.
            //hit.collider.enabled = false; //This solution is terrible, fix something else
            Debug.Log("woooo");
        }
    }

    //void resetTiles() //Borde nog flyttas till ett annat script som sitter i HexTiles men lägger den här så länge
    //{
    //    foreach (Transform hex in hexTiles)
    //    {
    //        hex.gameObject.GetComponent<Collider>().enabled = true;
    //    }
    //}
}
