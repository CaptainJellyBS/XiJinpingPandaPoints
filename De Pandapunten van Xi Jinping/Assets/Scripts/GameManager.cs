using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text hoverText;
    public int amountOfPosters, amountOfPlushies, atrocitiesCovered;
    int amountOfAtrocities;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance.gameObject); Debug.LogWarning("This town is too small for two GameManagers"); }
        Instance = this;
    }

    private void Start()
    {
        SetHoverTextVisible(false);
        amountOfAtrocities = FindObjectsOfType<Atrocity>().Length;
    }

    public void SetHoverText(string content, bool vis = true)
    {
        hoverText.text = content;
        SetHoverTextVisible(vis);
    }

    public void SetHoverTextVisible(bool vis)
    {
        hoverText.transform.parent.gameObject.SetActive(vis);
    }

    public void ScoreAtrocity(Atrocity atrocity)
    {
        atrocitiesCovered++;
        atrocity.gameObject.SetActive(false);
        if(atrocitiesCovered >= amountOfAtrocities) { Debug.Log("WHOOP WE WON WE DID IT"); }
    }

    public void AddCoverup(CoverType c)
    {
        if(c == CoverType.plushie) { amountOfPlushies--; }
        if(c == CoverType.poster) { amountOfPosters--; }
    }

    public int GetCoverupAmount(CoverType c)
    {
        if (c == CoverType.plushie) { return amountOfPlushies; }
        if (c == CoverType.poster) { return amountOfPosters; }
        throw new System.ArgumentException("WTF BUDDY");
    }
}
