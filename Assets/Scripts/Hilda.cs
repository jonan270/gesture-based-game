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

        Element = ElementState.Earth;
        Name = "Hilda";
        attackValue = 15;
        descriptionTextCard1 = "Hilda will conjure a health potion.";
        descriptionTextCard2 = "Hilda will see the future.";
        descriptionTextCard3 = "Hilda will summon a raven.";    
    }
}
