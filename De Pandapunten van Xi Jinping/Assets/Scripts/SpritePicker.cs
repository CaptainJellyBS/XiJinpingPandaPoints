using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePicker : MonoBehaviour
{
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Utility.Pick(sprites);    
    }
}
