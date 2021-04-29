using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Valve.VR;
using System.Linq;


public class RayCastFromHand : MonoBehaviour
{
    public Transform start;
    public LineRenderer lineRenderer;
    public RayCastFromHand otherHand;
    private CharacterSelector characterSelector;
    private PathCreator pathCreator;
    [SerializeField] private List<Hextile> tilesSelected = new List<Hextile>();
    private Character selectedCharacter { get { return PlayerManager.Instance.selectedCharacter.GetComponent<Character>(); } }
    
    Camera cam;
    private PlayerState PlayerState { get { return PlayerManager.Instance.PlayerState; } }
    private Hextile previoustile;
    private Hextile singleTile;
    private static int maxNrTilesToWalk = 6;
    private int displayTilesLeft;
    // Start is called before the first frame update
    private void Start()
    {
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
        cam = FindObjectOfType<Camera>();
        characterSelector = GetComponent<CharacterSelector>();
        pathCreator = FindObjectOfType<PathCreator>();
        displayTilesLeft = 0;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (characterSelector.IsHandFree)
        {
            //Choosing a character
            if (PlayerState == PlayerState.chooseFriendlyCharacter || PlayerState == PlayerState.chooseEnemyCharacter)
            {
                if (PlayerState == PlayerState.chooseFriendlyCharacter)
                    UIText.Instance.DisplayText("Choose friendly character");
                else
                    UIText.Instance.DisplayText("Choose enemy character");

                GetCharacter();
            }
            //Single tile for an ability
            if (PlayerState == PlayerState.chooseTile)
            {
                UIText.Instance.DisplayText("Find a tile");

                GetTile();
            }
            //Drawing a path for a character
            if(PlayerState == PlayerState.drawPath)
            {
                displayTilesLeft = maxNrTilesToWalk - tilesSelected.Count + 1;
                UIText.Instance.DisplayText("Draw a path for the character. \n Tiles left to walk: " + displayTilesLeft);

                ScanForTiles();
            }
            //We release the button and finish our drawing
            if (PlayerState == PlayerState.drawPath)
            {
                if (SteamVR.active)
                {
                    if (SteamVR_Actions.default_GrabPinch.GetStateUp(characterSelector.source))
                        FinishPath();
                    else if (displayTilesLeft == 0)
                    {
                        FinishPath();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonUp(0))
                        FinishPath();

                    else if(displayTilesLeft == 0)
                    {
                        FinishPath();
                    }

                }
            }

            //Quick fix for a bug when player switches from drawing a path to making a gesture for example
            if(PlayerState == PlayerState.makeGesture)
            {
                StopRayCast();
            }
        }
    }


    /// <summary>
    /// We have a successful hit renders a cyan laser
    /// </summary>
    /// <param name="hit"></param>
    private void HitSomething(RaycastHit hit)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, start.position + start.TransformDirection(Vector3.forward) * hit.distance);
    }
    /// <summary>
    /// Unsuccessful hit, render a red laser
    /// </summary>
    private void DidNotHit()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.SetPosition(1, start.position + start.TransformDirection(Vector3.forward) * 10);
    }

    private void GetTile()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(start.position, start.TransformDirection(Vector3.forward), 100.0f);

        bool foundTile = false;
        foreach(var hit in hits)
        {
            GameObject obj = hit.transform.gameObject;

            Hextile tile = obj.GetComponent<Hextile>();
            if (tile != null) //if we find a tile
            {
                //deselect previous tile 
                if (previoustile != tile)
                {
                    if (previoustile != null)
                        previoustile.DeselectTile();
                    previoustile = tile;
                }
                //select new tile
                tile.OnSelectedTile();
                HitSomething(hit);
                foundTile = true;
                singleTile = tile;

            }
        }
        if (!foundTile)
            DidNotHit();

        //raycast from mouse to find a tile: TODO: move this function to the hands instead and raycast from the wand for example. 
        if (Input.GetMouseButtonDown(0) || SteamVR_Actions.default_GrabPinch.GetStateDown(characterSelector.source)) //when the player presses left mouse btn invoke function
        {
            PlayerManager.Instance.tileTargetHandler.Invoke(singleTile);
            //UIText.Instance.SetActive(false);
            StopRayCast();
        }
    }

    private void GetCharacter()
    {
        //raycast from mouse to find a character
        RaycastHit hit;
        if (Physics.Raycast(start.position, start.TransformDirection(Vector3.forward), out hit)) //raycast into the world
        {
            GameObject obj = hit.transform.gameObject;
            bool targetFriendly = PlayerState == PlayerState.chooseFriendlyCharacter;
            PhotonView pv = obj.GetComponent<PhotonView>();
            if (pv != null && (pv.IsMine == targetFriendly)) //if we find a character 
            {
                PlayerManager.Instance.DeselectCharacters(); // deselect previous (only happens if we go directly from 1 character to another, this way both characters will not be lit)
                HitSomething(hit);
                obj.GetComponent<Outline>().enabled = true;
                if (Input.GetMouseButtonDown(0) || SteamVR_Actions.default_GrabPinch.GetStateDown(characterSelector.source)) //when the player presses left mouse btn invoke function
                {
                    PlayerManager.Instance.DeselectCharacters();
                    PlayerManager.Instance.characterTargetHandler.Invoke(obj.GetComponent<Character>());
                    //UIText.Instance.SetActive(false);
                    StopRayCast();
                }

            }
            else
            {
                PlayerManager.Instance.DeselectCharacters();
                DidNotHit();
            }
        }
        else
        {
            DidNotHit();
        }
    }


    private void ScanForTiles()
    {
        if (tilesSelected.Count == 0)
        {
            
            tilesSelected.Add(selectedCharacter.CurrentTile);
            pathCreator.AddTile(selectedCharacter.CurrentTile);
            selectedCharacter.CurrentTile.OnSelectedTile();
        }

        RaycastHit[] hits;
        hits = Physics.RaycastAll(start.position, start.TransformDirection(Vector3.forward), 100.0f);
        bool validTile = false;

        foreach (var hit in hits)
        {
            Hextile currentTile = hit.collider.gameObject.GetComponent<Hextile>();
            if (currentTile != null) //we hit a tile
            {
                //tile has to be empty or can be occupied by enemy character 
                if (currentTile.occupant == null || !currentTile.occupant.photonView.IsMine)
                {
                    HitSomething(hit);
                    validTile = true;
                    if (SteamVR.active) //using vr controller
                    {
                        if (SteamVR_Actions.default_GrabPinch.GetState(characterSelector.source))
                            TryAddTile(currentTile);
                    }
                    else //fallback using mouse + keyboard
                    {
                        if (Input.GetMouseButton(0))
                            TryAddTile(currentTile);
                    }
                }
            }
        }
        if (!validTile)
            DidNotHit();
    }
    
    private void TryAddTile(Hextile currentTile)
    {
        if ((tilesSelected.Count > 0 && tilesSelected.Count <= maxNrTilesToWalk && !tilesSelected.Contains(currentTile) && AreTilesAdjacent(currentTile, tilesSelected.Last())))
        {

            //Debug.Log("Adding tile to list");
            UIText.Instance.DisplayText("Tiles left to walk: " + (maxNrTilesToWalk - tilesSelected.Count));
            tilesSelected.Add(currentTile);
            pathCreator.AddTile(currentTile);
            currentTile.OnSelectedTile();
        }
    }

    private void FinishPath()
    {
        if (tilesSelected.Count > 1)
        {
            pathCreator.FinishPath(selectedCharacter.gameObject);
            tilesSelected.Clear();
            Debug.Log("Released he should walk now");
            StopRayCast();
        }
    }

    /// <summary>
    /// Stops rendering the laser
    /// </summary>
    public void StopRayCast()
    {
        lineRenderer.positionCount = 0;
        otherHand.StopOtherRayCast();
    }

    public void StopOtherRayCast()
    {
        lineRenderer.positionCount = 0;
    }
    /// <summary>
    /// Checks if two tiles are at most 1 index from eachother
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    bool AreTilesAdjacent(Hextile a, Hextile b)
    {
        return (Mathf.Abs(a.tileIndex.x - b.tileIndex.x) <= 1 && Mathf.Abs(a.tileIndex.y - b.tileIndex.y) <= 1);
    }
}