using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class Card : MonoBehaviour // Shows a card specifik for the character
{
    //public Image cardFront;
    //public Plane plane;
    public TextMeshProUGUI description;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costGemstones;

    public Image gesture;

    //public bool cardShown;


   // public GameObject prefabHoriz;
   // public GameObject prefabCir;
   // public GameObject prefabVert;

    public void setText(string desc, string name)
    {
        description.text = desc;
        nameText.text = name;
    }

    public Vector3 cardPosition()
    {
        return this.transform.position;
    }


}

