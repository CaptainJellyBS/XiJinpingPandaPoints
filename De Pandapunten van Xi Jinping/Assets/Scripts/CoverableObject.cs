using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverableObject : MonoBehaviour
{
    public bool isAtrocity;
    public LayerMask mask;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            gameObject.layer = 2;
        }

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(OeiOeiWatEenGeknoei());
        }

    }

    IEnumerator OeiOeiWatEenGeknoei()
    {
        yield return null;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.one, 0.1f, mask, -100, 100);        
        gameObject.layer = 7;
        if (hit) { if (isAtrocity) { GameManager.Instance.ScoreAtrocity(this); } enabled = false; }
    }
}
