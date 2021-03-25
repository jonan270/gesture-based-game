using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Bjorn : Character
{

    protected override void Start()
    {
        base.Start();
        Element = ElementState.Water;
        StrongAgainst = ElementState.Fire;
        WeakAgainst = ElementState.Wind;
        Name = "Bjorn";
        attackValue = 20;
        //MaterialType = Resources.Load("Materials/waterMat.mat", typeof(Material)) as Material;
        descriptionTextCard1 = "Bjorn will go berserk";
        descriptionTextCard2 = "Björn will drink mead and ...";
        descriptionTextCard3 = "Björn will do cleave and hurt multiple enemies.";
    }

}

