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

    public GestureType gestureType;

    private Color alphaColor;
    public bool fadeOut;
    private float counter = 1.0f;

    float duration = 0f;

    void Update()
    {
        if(gestureType == GestureType.circle)
        {
            DrawCircle();
        }
    }

    public void setText(string desc, string name)
    {
        description.SetText(desc);
        nameText.SetText(name);
    }


    public Vector3 cardPosition()
    {
        return this.transform.position;
    }

    void DrawCircle()
    {
    }

    void DrawLine()
    {

    }


}

