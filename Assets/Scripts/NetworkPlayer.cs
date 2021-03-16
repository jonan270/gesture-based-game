using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Valve.VR;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform lefthand;
    public Transform righthand;
    
    private PhotonView photonView;

    private Transform xrHead, xrLeft, xrRight;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            head.gameObject.SetActive(false);
            lefthand.gameObject.SetActive(false);
            righthand.gameObject.SetActive(false);
        }
        //Fallback if no vr headset is connected make sure we still track and update the position of the other player
        if(!SteamVR.active)
        {
            xrHead = GameObject.Find("Main Camera").transform;
            xrLeft = GameObject.Find("LeftHand Controller").transform;
            xrRight = GameObject.Find("RightHand Controller").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            //if we have VR headset connected track 
            if (SteamVR.active)
            {
                MapPosition(head, XRNode.Head);
                MapPosition(lefthand, XRNode.LeftHand);
                MapPosition(righthand, XRNode.RightHand);
            }//track ingame gameobjects
            else
            {
                MapPosition(head, xrHead);
                MapPosition(lefthand, xrLeft);
                MapPosition(righthand, xrRight);
            }

        }
    }
    /// <summary>
    /// Track position and rotation of VR controllers 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="node"></param>
    void MapPosition(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = position;
        target.rotation = rotation;
    }
    /// <summary>
    /// Track position and rotation without VR controllers
    /// </summary>
    /// <param name="target"></param>
    /// <param name="track"></param>
    void MapPosition(Transform target, Transform track)
    {
        target.position = track.position;
        target.rotation = track.rotation;
    }
}
