using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Hilda : Character
{
    protected override void Start()
    {
        base.Start();
        Element = ElementState.Fire;
        StrongAgainst = ElementState.Earth;
        WeakAgainst = ElementState.Water;
        Name = "Hilda";
        attackValue = 15;
        MaterialType = Resources.Load("Materials/RoseMat.mat", typeof(Material)) as Material;
        descriptionTextCard1 = "Hilda will conjure a health potion.";
        descriptionTextCard2 = "Hilda will see the future.";
        descriptionTextCard3 = "Hilda will summon a raven.";
        
        
    }
}
