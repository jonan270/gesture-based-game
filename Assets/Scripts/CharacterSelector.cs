using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;
//TODO: visualize selected character on hovering for example with an outline shader

public class CharacterSelector : MonoBehaviour
{
    //alias for this transform
    [SerializeField] private Transform hand;

    public GameObject selectedCharacter;

    private bool hasTarget = false;

    private Vector3 originalPosition, originalScale;
    private Quaternion originalRotation;

    private void Start()
    {
        if(hand == null) 
            hand = transform;   
    }


    void Update()
    {
        //We have a target following
        if (hasTarget)
        {
            FollowHand();

            //Check for button release
            if (SteamVR.active) //check if we are in VR or not
            {
                if (!SteamVR_Actions.default_GrabGrip.GetState(SteamVR_Input_Sources.Any)) //is VR button released
                    ReleaseCharacter();
            }
            else
            {
                if (!Input.GetKey(KeyCode.F)) //is keyboard button released
                    ReleaseCharacter();

            }
        } 
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Hand collided with" + collider.transform.root.name);
    }

    void OnTriggerStay(Collider collider)
    {
        if (hasTarget)
            return;
        //TODO: check player state can pickup
        if (Input.GetKey(KeyCode.F) || SteamVR_Actions.default_GrabGrip.GetState(SteamVR_Input_Sources.Any)) {
            GameObject character = collider.transform.root.gameObject;
            if(character != null && character.GetComponent<Character>() != null && character.GetComponent<PhotonView>().IsMine)
            {
                selectedCharacter = character;
                hasTarget = true;
                CopyTransform(selectedCharacter.transform);
                CharacterControl.SelectedCharacter = selectedCharacter;
                Debug.Log("Selected character is " + selectedCharacter.name);
            }
        }
    }

    /// <summary>
    /// Copy target transform
    /// </summary>
    /// <param name="target"></param>
    private void CopyTransform(Transform target)
    {
        originalPosition = target.position;
        originalRotation = target.rotation;
        originalScale = target.localScale;
    }
    /// <summary>
    /// Selected character follows the hand, TODO: follow a point transform in the hand for better visual effect
    /// </summary>
    private void FollowHand()
    {
        selectedCharacter.transform.position = hand.position;
        selectedCharacter.transform.rotation = hand.rotation;
        selectedCharacter.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    /// <summary>
    /// Releases the character back to the board, reset its transform to before pickup
    /// </summary>
    private void ReleaseCharacter()
    {
        hasTarget = false;
        selectedCharacter.transform.position = originalPosition;
        selectedCharacter.transform.rotation = originalRotation;
        selectedCharacter.transform.localScale = originalScale;
        selectedCharacter = null;
        CharacterControl.SelectedCharacter = null;
        Debug.Log("No seleced character");
    }
}
