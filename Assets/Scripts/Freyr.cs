using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Freyr : Character
{

    protected override void Start()
    {
        base.Start();
        Element = ElementState.Earth;
        StrongAgainst = ElementState.Wind;
        WeakAgainst = ElementState.Fire;
        Name = "Freyr";
        attackValue = 15;
        //MaterialType = Resources.Load("Materials/waterMat.mat", typeof(Material)) as Material;
        /*descriptionTextCard1 = "Bjorn will go berserk";
        descriptionTextCard2 = "Björn will drink mead and ...";
        descriptionTextCard3 = "Björn will do cleave and hurt multiple enemies.";*/
    }

}

