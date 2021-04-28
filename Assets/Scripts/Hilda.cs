using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Photon.Pun;

public class Hilda : Character
{

    protected override void Awake()
    {
        base.Awake();
        Element = ElementState.Fire;
        StrongAgainst = ElementState.Earth;
        WeakAgainst = ElementState.Water;
    }

    protected override void Start()
    {
        base.Start();
        //Element = ElementState.Fire;
        //StrongAgainst = ElementState.Earth;
        //WeakAgainst = ElementState.Water;
        Name = "Hilda";
        BasicAttackValue = 15;
        //MaterialType = Resources.Load("Materials/RoseMat.mat", typeof(Material)) as Material;
        /*descriptionTextCard1 = "Hilda will conjure a health potion to be used on self or her teammates.";
        descriptionTextCard2 = "Hilda will conjure an Attack boost for her teammates.";
        descriptionTextCard3 = "Hilda will change tiles to match her own element.";*/
    }

    protected override void RPC_Cant_Handle_Inheritance()
    {
        photonView.RPC("RPC_UpdateStatus", RpcTarget.Others, IsAlive);
    }

    [PunRPC]
    void RPC_UpdateStatus(bool status)
    {
        IsAlive = status;
    }
}
