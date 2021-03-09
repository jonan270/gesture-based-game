using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head;
    public Transform lefthand;
    public Transform righthand;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            head.gameObject.SetActive(false);
            lefthand.gameObject.SetActive(false);
            righthand.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            MapPosition(head, XRNode.Head);
            MapPosition(lefthand, XRNode.LeftHand);
            MapPosition(righthand, XRNode.RightHand);

        }
    }

    void MapPosition(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = position;
        target.rotation = rotation;

    }
}
