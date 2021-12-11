using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text hoverText;
    public Text atrocityCounterText, timerText, postersLeftText, plushiesLeftText;


    int atrocitiesCovered;
    
    int amountOfPosters, amountOfPlushies, amountOfAtrocities, timer;

    public int AtrocitiesCovered
    {
        get { return atrocitiesCovered; }
        set { atrocitiesCovered = value; atrocityCounterText.text = (amountOfAtrocities - atrocitiesCovered).ToString(); }
    }
    public int AmountOfPosters
    {
        get { return amountOfPosters; }
        set { amountOfPosters = value; postersLeftText.text = amountOfPosters.ToString(); }
    }

    public int AmountOfPlushies
    {
        get { return amountOfPlushies; }
        set { amountOfPlushies = value; plushiesLeftText.text = amountOfPlushies.ToString(); }
    }

    public int AmountOfAtrocities
    {
        get { return amountOfAtrocities; }
        set { amountOfAtrocities = value; atrocityCounterText.text = (amountOfAtrocities - atrocitiesCovered).ToString(); }
    }

    public int Timer 
    {
        get { return timer; }
        set { timer = value; timerText.text = timer.ToString(); }
    }

    SpawnManager spawnManager; DialogueManager dialogueManager; Tutorial tutorial;

    [Header("Dialogue")]
    public Dialogue introDialogue;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance.gameObject); Debug.LogWarning("This town is too small for two GameManagers"); }
        Instance = this;
    }

    private void Start()
    {
        Timer = 20; AmountOfAtrocities = 0; AmountOfPlushies = 0; AmountOfPosters = 0;
        spawnManager = GetComponent<SpawnManager>();
        dialogueManager = GetComponent<DialogueManager>();
        tutorial = GetComponent<Tutorial>();
        SetHoverTextVisible(false);
        StartCoroutine(TutorialC());

    }

    IEnumerator TutorialC()
    {
        //Intro Dialogue
        yield return dialogueManager.PlayDialogue(introDialogue);
        yield return tutorial.PlayTutorial(0);
        StartCoroutine(ContinuousLevelSpawn());
    }

    IEnumerator ContinuousLevelSpawn()
    {
        int a = 1; int ro = 2; int t = 20; int c = 0;
        while(true)
        {
            SpawnNewLevel(a, ro);
            while (AtrocitiesCovered < AmountOfAtrocities && Timer>=0) { yield return new WaitForSeconds(1.0f); Timer--; }

            if (AtrocitiesCovered < AmountOfAtrocities) { Debug.Log("Ya fucked up!"); }
            else { a++; ro++; c++; if (c % 3 == 0) { t -= 2; } } //Fix this
            
            yield return new WaitForSeconds(2.0f);
            Timer = t;
        }
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

    public void ScoreAtrocity(CoverableObject atrocity)
    {
        AtrocitiesCovered++;
        atrocity.GetComponent<Collider2D>().enabled = false;
        //if(atrocitiesCovered >= amountOfAtrocities) { Debug.Log("WHOOP WE WON WE DID IT"); }
    }

    public void AddCoverup(CoverType c)
    {
        if(c == CoverType.Plushie) { AmountOfPlushies--; }
        if(c == CoverType.Poster) { AmountOfPosters--; }
    }

    public int GetCoverupAmount(CoverType c)
    {
        if (c == CoverType.Plushie) { return AmountOfPlushies; }
        if (c == CoverType.Poster) { return AmountOfPosters; }
        throw new System.ArgumentException("WTF BUDDY");
    }

    public void SpawnNewLevel(int _atrocities, int _randomObjects)
    {
        GameObject[] oldStuff = GameObject.FindGameObjectsWithTag("CoverObject");
        for (int i = 0; i < oldStuff.Length; i++)
        {
            Destroy(oldStuff[i]);
        }
        AmountOfAtrocities = _atrocities;
        AtrocitiesCovered = 0;
        spawnManager.SpawnNewLevel(_atrocities, _randomObjects);
    }
}
