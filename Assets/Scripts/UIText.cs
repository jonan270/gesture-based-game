using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public static UIText Instance { get; private set; }
    [SerializeField] TextMeshProUGUI uiText;
    private void Start()
    {
        Instance = this;
    }

    public void DisplayText(string _text)
    {
        uiText.text = _text;
    }

    public void SetActive(bool state)
    {
        uiText.enabled = state;
    }


}
