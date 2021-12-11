using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public GameObject on, off;

    public void Toggle(bool _on)
    {
        on.SetActive(_on);
        off.SetActive(!_on);
    }
}
