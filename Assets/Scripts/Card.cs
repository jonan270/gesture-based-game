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
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI nameText;
    //public TextMeshProUGUI costGemstones;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material neutralMaterial;


    [SerializeField] private GameObject fireSymbol;
    [SerializeField] private GameObject waterSymbol;
    [SerializeField] private GameObject earthSymbol;

    //public Image gesture;


    public GestureType gestureType;


    private void Start()
    {
        ResetCard();
    }

    private void SetText(string desc, string name)
    {
        description.SetText(desc);
        nameText.SetText(name);
    }


    public Vector3 CardPosition()
    {
        return transform.position;
    }

    public void SetCardData(AbilityData data, Material newMaterial)
    {
        SetText("Cost: " + data.gemsCost + "\n" + data.abilityDescription, data.abilityName);
        SetElementSymbol(data.abilityElement);
        meshRenderer.material = newMaterial;
    }

    private void SetElementSymbol(ElementState element)
    {
        if (element == ElementState.Fire)
        {
            fireSymbol.SetActive(true);

        }
        else if (element == ElementState.Earth)
        {
            earthSymbol.SetActive(true);
        }
        else if (element == ElementState.Water)
        {
            waterSymbol.SetActive(true);
        }
    }

    public void ResetCard()
    {
        SetText("  ", "  ");
        fireSymbol.SetActive(false);
        waterSymbol.SetActive(false);
        earthSymbol.SetActive(false);
        meshRenderer.material = neutralMaterial;
    }
}

