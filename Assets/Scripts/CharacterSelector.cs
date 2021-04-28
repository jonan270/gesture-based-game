using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;
//TODO: visualize selected character on hovering for example with an outline shader

public class CharacterSelector : MonoBehaviour
{
    public SteamVR_Input_Sources source;
    [SerializeField] CharacterSelector otherHand;
    [SerializeField] GameObject glove, brush, magicWand;
    
    //alias for this transform
    [SerializeField] private Transform followTransform;

    private GameObject selectedCharacter;

    /// <summary>
    /// returns true if the hand is not holding a character
    /// </summary>
    public bool IsHandFree { get { return !hasTarget; } }

    private bool hasTarget = false;

    private Vector3 originalPosition, originalScale;
    private Quaternion originalRotation;

    private void Start()
    {
        if(followTransform == null) 
            followTransform = transform;   
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
                if (!SteamVR_Actions.default_GrabPinch.GetState(source)) //is VR button released
                    ReleaseCharacter();
            }
            else
            {
                if (!Input.GetKey(KeyCode.F)) //is keyboard button released
                    ReleaseCharacter();

            }
        } 
    }
    /// <summary>
    /// Changes tool in hand (only the hand with no character in it can have a tool)
    /// </summary>
    /// <param name="state"></param>
    public void OnChangedTool(PlayerState state)
    {
        if(!hasTarget)
        {
            switch (state)
            {
                case PlayerState.drawPath:                     
                    brush.SetActive(true);
                    magicWand.SetActive(false);
                    break;
                case PlayerState.makeGesture:
                    magicWand.SetActive(true);
                    brush.SetActive(false);
                    break;
                default:
                    magicWand.SetActive(false);
                    brush.SetActive(false);
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(followTransform.name + " collided with " + collider.transform.root.name);
        if (hasTarget || otherHand.hasTarget)
            return;

        Character character = collider.transform.root.gameObject.GetComponent<Character>();
        if(character && character.photonView.IsMine)
            character.GetComponent<Outline>().enabled = true;
    }

    void OnTriggerExit(Collider collider)
    {
        //Debug.Log(followTransform.name + " collided with " + collider.transform.root.name);
        //if (hasTarget || otherHand.hasTarget)
        //    return;

        Character character = collider.transform.root.gameObject.GetComponent<Character>();
        if (character && character.photonView.IsMine)
            character.GetComponent<Outline>().enabled = false;
    }

    void OnTriggerStay(Collider collider)
    {
        if (hasTarget)
            return;
        //TODO: check player state can pickup
        if (Input.GetKey(KeyCode.F) || SteamVR_Actions.default_GrabPinch.GetState(source)) {
            if (CanPickUp())
            {
                GameObject obj = collider.transform.root.gameObject;
                Character character = obj.GetComponent<Character>();
                //we interacted with an available character and it is ours
                if (obj != null && character != null  && character.CanDoAction() && obj.GetComponent<PhotonView>().IsMine)
                {
                    PickupCharacter(obj);
                }
            }
            else
            {
                Debug.Log("Can't pickup character, not my turn or this character has already been played");
            }
        }
    }
    /// <summary>
    /// Is the player in a state where we can pickup a character
    /// </summary>
    /// <returns></returns>
    private bool CanPickUp()
    {
        //return PlayerManager.Instance.PlayerState != PlayerState.waitingForMyTurn && PlayerManager.Instance.PlayerState != PlayerState.characterWalking;
        return PlayerManager.Instance.PlayerState == PlayerState.idle && otherHand.IsHandFree;
    }
    /// <summary>
    /// Pickup the character and set text on cards
    /// </summary>
    /// <param name="character"></param>
    private void PickupCharacter(GameObject character)
    {
        selectedCharacter = character;
        hasTarget = true;
        CopyTransform(selectedCharacter.transform);
        character.GetComponent<Character>().SetState(Character.CharacterState.PickedUp);
        PlayerManager.Instance.selectedCharacter = selectedCharacter;

        var cards = FindObjectsOfType<cardDrawing>();
        foreach(var card in cards)
        {
            card.OnPickup();
        }
        
        //GameObject.Find("HandCards(Clone)").GetComponent<HandCards>().setCardType(true);
        HandCards.HandCardsInstance.setCardType(true);

        //Debug.Log("Selected character in hand is " + selectedCharacter.name);
        //Debug.Log("Selected character in player manager is " + PlayerManager.Instance.selectedCharacter.name);
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
        selectedCharacter.transform.position = followTransform.position;
        selectedCharacter.transform.rotation = followTransform.rotation;
        selectedCharacter.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        //GameObject effect = selectedCharacter.GetComponent<Character>().activeEffect;
        Character character = selectedCharacter.GetComponent<Character>();

        //if (effect)
        //    effect.SetActive(false); // temporarily hide effect
        foreach (var effect in character.turnBasedEffects)
        {
            //effect.MakeOrDestroy(character, false); // temporarily hide effect
            effect.visualEffect.SetActive(false);
        }
    }
    /// <summary>
    /// Releases the character back to the board, reset its transform to before pickup
    /// </summary>
    public void ReleaseCharacter()
    {
        if (selectedCharacter == null)
            return;

        Character character = selectedCharacter.GetComponent<Character>();
        foreach (var effect in character.turnBasedEffects)
        {
            //effect.MakeOrDestroy(character, true); // show effect when dropping
            effect.visualEffect.SetActive(true);
        }

        selectedCharacter.GetComponent<Character>().SetState(Character.CharacterState.CanDoAction);
        //GameObject.Find("HandCards(Clone)").GetComponent<HandCards>().setCardType(false);
        //HandCards.HandCardsInstance.setCardType(false);

        selectedCharacter.transform.position = originalPosition;
        selectedCharacter.transform.rotation = originalRotation;
        selectedCharacter.transform.localScale = originalScale;
        hasTarget = false;
        otherHand.OnReleasedCharacter();
        selectedCharacter = null;

        //var cards = FindObjectsOfType<cardDrawing>();
        //foreach (var card in cards)
        //{
        //    card.OnDropCharacter();
        //}
    }

    public void OnReleasedCharacter()
    {
        brush.SetActive(false);
        magicWand.SetActive(false);
    }
}
