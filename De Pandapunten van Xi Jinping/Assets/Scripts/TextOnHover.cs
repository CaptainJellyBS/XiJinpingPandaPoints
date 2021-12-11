using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOnHover : MonoBehaviour
{
    public string text;

    private void OnMouseEnter()
    {
        GameManager.Instance.SetHoverText(text);
    }

    private void OnMouseExit()
    {
        GameManager.Instance.SetHoverTextVisible(false);
    }
}
