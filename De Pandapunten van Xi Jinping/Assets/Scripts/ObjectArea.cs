using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoverType { Poster, Plushie}
public enum CanCoverType { Poster, Plushie, Both, Neither}
public class ObjectArea : MonoBehaviour
{
    public GameObject toPlace, preview;
    public CoverType coverType;

    private void Start()
    {
        transform.position = Vector3.Scale(transform.position, new Vector3(1, 1, 0)) + new Vector3(0, 0, 0.05f);
    }

    private void OnMouseEnter()
    {
        if(Input.GetMouseButton(0))
        {
            if (GameManager.Instance.GetCoverupAmount(coverType) <= 0 || !GameManager.Instance.canPlace) { return; }

            preview.SetActive(true);
            transform.position = Vector3.Scale(transform.position, new Vector3(1, 1, 0)) + new Vector3(0, 0, -0.05f);
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.GetCoverupAmount(coverType) <= 0 || !GameManager.Instance.canPlace) { return; }

        preview.SetActive(true);
        transform.position = Vector3.Scale(transform.position, new Vector3(1, 1, 0)) + new Vector3(0, 0, -0.05f);
    }

    private void OnMouseExit()
    {
        preview.SetActive(false);
        transform.position = Vector3.Scale(transform.position, new Vector3(1, 1, 0)) + new Vector3(0, 0, 0.05f);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if(GameManager.Instance.GetCoverupAmount(coverType) <= 0 || !GameManager.Instance.canPlace) { return; }
            Instantiate(toPlace, Utility.MousePosTwoD() + new Vector3(0, 0, -0.025f), Quaternion.identity);
            GameManager.Instance.AddCoverup(coverType);
            preview.SetActive(false);
            transform.position = Vector3.Scale(transform.position, new Vector3(1, 1, 0)) + new Vector3(0, 0, 0.05f);
        }
    }

}
