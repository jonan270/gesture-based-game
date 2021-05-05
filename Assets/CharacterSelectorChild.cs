using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectorChild : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SendMessageUpwards("OnEnter", other, SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.SendMessageUpwards("OnExit", other, SendMessageOptions.DontRequireReceiver);

    }

    private void OnTriggerStay(Collider other)
    {
        gameObject.SendMessageUpwards("OnStay", other, SendMessageOptions.DontRequireReceiver);

    }
}
