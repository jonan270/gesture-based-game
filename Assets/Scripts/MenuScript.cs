using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class MenuScript : MonoBehaviour
{
    public LineRenderer lineRender;
    public GameObject activeTarget;
    public Transform source;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        source = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRender.SetPosition(0, source.position);
        lineRender.SetPosition(1, source.position + source.TransformDirection(Vector3.forward) * 10);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            activeTarget = hit.transform.gameObject;
            RegisterButtonEvents buttonEvent = activeTarget.GetComponent<RegisterButtonEvents>();
            if (buttonEvent != null)
            {
                buttonEvent.RegisterHover();
                if (SteamVR.active && SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
                    buttonEvent.RegisterClick();
                lineRender.SetPosition(0, source.position);
                lineRender.SetPosition(1, source.position + source.TransformDirection(Vector3.forward) * hit.distance);
            }
        }
        else if(activeTarget)
            clearTarget();
    }

    void clearTarget()
    {
        RegisterButtonEvents buttonEvent = activeTarget.GetComponent<RegisterButtonEvents>();
        if (buttonEvent)
            buttonEvent.RemoveHover();
        activeTarget = null;
    }
}
