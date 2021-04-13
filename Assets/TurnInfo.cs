using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TurnInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;

    public void OnMyTurn()
    {
        turnText.enabled = true;
        turnText.text = "[Your turn]";

        StartCoroutine(FadeText(5.0f));
    }
    public void OpponentsTurn()
    {
        turnText.enabled = true;
        turnText.text = "[Opponents turn]";

        StartCoroutine(FadeText(5.0f));
    }


    private IEnumerator FadeText(float time)
    {
        yield return new WaitForSeconds(time);

        turnText.enabled = false;
    }
}
