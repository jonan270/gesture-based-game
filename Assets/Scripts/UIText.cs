using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public static UIText Instance { get; private set; }
    [SerializeField] TextMeshProUGUI uiText;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Displays the text for a set amount of seconds
    /// </summary>
    /// <param name="_text">text to display</param>
    /// <param name="turnoffin">How long the msg should be visible</param>
    public void DisplayText(string _text, float turnoffin = 5f)
    {
        if (uiText.text == _text)
            return;
        StopAllCoroutines();
        uiText.enabled = true;
        uiText.text = _text;
        StartCoroutine(TurnOffTextIn(turnoffin));
    }

    private IEnumerator TurnOffTextIn(float time)
    {
        yield return new WaitForSeconds(time);
        uiText.text = "";
        uiText.enabled = false;
    }


}
