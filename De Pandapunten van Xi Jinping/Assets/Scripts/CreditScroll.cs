using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroll : MonoBehaviour
{
    Vector3 startPos;
    public float scrollSpeed;
    float resetPoint;
    bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        resetPoint = transform.position.y + 1220;
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * scrollSpeed;   
        
        if(transform.position.y > resetPoint) { ResetScrolling(); }
    }

    public void ResetScrolling()
    {
        if (!initialized) { return; }
        transform.position = startPos;
    }
}
