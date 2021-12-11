using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverOverEvent : MonoBehaviour
{
    public UnityEvent enterEvent, exitEvent;

    private void OnMouseEnter()
    {
        enterEvent.Invoke();    
    }
    
    private void OnMouseExit()
    {
        exitEvent.Invoke();    
    }
}
