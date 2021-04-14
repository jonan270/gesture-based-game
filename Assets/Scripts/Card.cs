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

    [SerializeField] public GameObject model;
    [SerializeField] public GameObject model2;

    public Image gesture;

    public GestureType gestureType;

    public void setText(string desc, string name)
    {
        description.SetText(desc);
        nameText.SetText(name);
    }


    public Vector3 cardPosition()
    {
        return this.transform.position;
    }


}

