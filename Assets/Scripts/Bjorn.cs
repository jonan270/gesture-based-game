using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Photon.Pun;

public class Bjorn : Character
{

    protected override void Start()
    {
        base.Start();
        Element = ElementState.Water;
        StrongAgainst = ElementState.Fire;
        WeakAgainst = ElementState.Wind;
        Name = "Bjorn";
        BasicAttackValue = 20;
        //MaterialType = Resources.Load("Materials/waterMat.mat", typeof(Material)) as Material;
        /*descriptionTextCard1 = "Bjorn will go berserk";
        descriptionTextCard2 = "Björn will drink mead and ...";
        descriptionTextCard3 = "Björn will do cleave and hurt multiple enemies.";*/
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

    //public void BerserkShow(bool show)
    //{
    //    GameObject effect = GameObject.Find("BerserkSkull");
    //    effect.SetActive(show);
    //}
}

