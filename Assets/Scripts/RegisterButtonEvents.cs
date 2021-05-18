using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterButtonEvents : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void RegisterHover()
    {
        button.image.color = button.colors.highlightedColor;
        //Debug.Log("ayo");
    }
    public void RegisterClick()
    {
        button.onClick.Invoke();
    }
    public void RemoveHover()
    {
        button.image.color = button.colors.normalColor;
        //Debug.Log("cya");
    }

    public void StartGame()
    {
        Debug.Log("this is where the game scene loads");
    }
    public void Options()
    {
        Debug.Log("this is where the game options change");
    }
    public void QuitGame()
    {
        Debug.Log("this is where the game quits");
        Application.Quit();
    }

    
}
