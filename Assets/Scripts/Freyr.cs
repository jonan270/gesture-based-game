using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //MaterialType = Resources.Load("Materials/RoseMat.mat", typeof(Material)) as Material;
        /*descriptionTextCard1 = "Hilda will conjure a health potion to be used on self or her teammates.";
        descriptionTextCard2 = "Hilda will conjure an Attack boost for her teammates.";
        descriptionTextCard3 = "Hilda will change tiles to match her own element.";*/


    }
}
