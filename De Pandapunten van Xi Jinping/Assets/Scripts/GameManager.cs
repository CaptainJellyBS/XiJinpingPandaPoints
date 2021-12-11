using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text hoverText;
    public Text atrocityCounterText, timerText, postersLeftText, plushiesLeftText;
    public Image fade;

    [Header("Game Over UI Elements")]
    public Image gameOverPanel;
    public Text goText0, goText1, goText2, goText3;
    public GameObject goButton;
    
    [Header("Win UI Elements")]
    public Image winPanel;
    public Text winText0, winText1, winText2, winText3;
    public GameObject winButton;

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

    int penaltyPlushies, penaltyPosters;

    SpawnManager spawnManager; DialogueManager dialogueManager; Tutorial tutorial;

    [Header("Dialogue")]
    public Dialogue[] introDialogues;
    public Dialogue[] randomEndDialogues;

    [Header("Difficulties")]
    public int[] atrocities;
    public int[] randomObjects;
    public int[] timers;

    bool gameOver;
    public bool canPlace;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance.gameObject); Debug.LogWarning("This town is too small for two GameManagers"); }
        Instance = this;
    }

    private void Start()
    {
        Timer = 20; AmountOfAtrocities = 0; AmountOfPlushies = 0; AmountOfPosters = 0;
        penaltyPlushies = 0; penaltyPosters = 0;
        gameOver = false;
        spawnManager = GetComponent<SpawnManager>();
        dialogueManager = GetComponent<DialogueManager>();
        tutorial = GetComponent<Tutorial>();
        SetHoverTextVisible(false);
        StartCoroutine(GameLoop());

    }

    IEnumerator GameLoop()
    {
        //Intro Dialogue
        yield return dialogueManager.PlayDialogue(introDialogues[0]);
        yield return tutorial.PlayTutorial(0);
        yield return dialogueManager.PlayDialogue(Utility.Pick(randomEndDialogues));
        yield return QuickFade();

        yield return dialogueManager.PlayDialogue(introDialogues[1]);
        yield return tutorial.PlayTutorial(1);
        yield return dialogueManager.PlayDialogue(Utility.Pick(randomEndDialogues));
        yield return QuickFade();

        yield return dialogueManager.PlayDialogue(introDialogues[2]);
        yield return tutorial.PlayTutorial(2);
        yield return dialogueManager.PlayDialogue(Utility.Pick(randomEndDialogues));
        yield return QuickFade();

        yield return StartCoroutine(ContinuousLevelSpawn());

        if (gameOver) { yield return StartCoroutine(GameOverC()); }
        else { yield return StartCoroutine(WinC()); }
    }

    IEnumerator ContinuousLevelSpawn()
    {
        int prevPosters = AmountOfPosters, prevPlushies = AmountOfPlushies;
        for (int c = 0; c < timers.Length && c < atrocities.Length && c < randomObjects.Length ; c++)
        {
            Timer = timers[c];
            penaltyPlushies = 0; penaltyPosters = 0;
            AmountOfPosters = prevPosters; AmountOfPlushies = prevPlushies; //Weird shit
            SpawnNewLevel(atrocities[c], randomObjects[c]);

            while (AtrocitiesCovered < AmountOfAtrocities && Timer>=0 && (AmountOfPlushies > 0 || AmountOfPosters > 0)) { yield return new WaitForSeconds(1.0f); Timer--; }

            if (AtrocitiesCovered < AmountOfAtrocities) 
            {
                AmountOfPlushies = prevPlushies; AmountOfPosters = prevPosters;

                List<string> uncoveredAtrocities = new List<string>();
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Atrocity"))
                {
                    if (go.GetComponent<CoverableObject>().enabled) { uncoveredAtrocities.Add(go.GetComponent<TextOnHover>().text); }
                }

                foreach(string s in uncoveredAtrocities)
                {
                    yield return dialogueManager.PlayCaughtDialogue(s);
                    if (gameOver) { yield break; }
                }

                prevPosters -= penaltyPosters;
                prevPlushies -= penaltyPlushies;
                c--;
            }
            else { prevPosters = AmountOfPosters; prevPlushies = AmountOfPlushies; dialogueManager.currentPenalty = 0; }
            
            yield return dialogueManager.PlayDialogue(Utility.Pick(randomEndDialogues));
            yield return QuickFade();
        }
    }

    public void GameOver()
    {
        gameOver = true;
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

    public void IncreasePosterPenalty()
    {
        penaltyPosters++;
    }

    public void IncreasePlushiePenalty()
    {
        penaltyPlushies++;
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

    public Coroutine QuickFade()
    {
        return StartCoroutine(QuickFadeC());
    }

    public void QuickFadeEvent()
    {
        StartCoroutine(QuickFadeC());
    }

    public IEnumerator QuickFadeC()
    {
        fade.gameObject.SetActive(true);
        float t = 0;
        while (t < 1.0f)
        {
            fade.color = Color.Lerp(Color.clear, Color.black, t);
            t += Time.deltaTime * 2;
            yield return null;
        }

        t = 0;
        while (t < 1.0f)
        {
            fade.color = Color.Lerp(Color.black, Color.clear, t);
            t += Time.deltaTime * 2;
            yield return null;
        }



        fade.gameObject.SetActive(false);
    }

    IEnumerator GameOverC()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOverPanel.color = Color.clear;
        goText0.color = Color.clear;
        goText1.color = Color.clear;
        goText2.color = Color.clear;
        goText3.color = Color.clear;

        goButton.SetActive(false);

        float t = 0;
        while (t <= 0.8f)
        {
            gameOverPanel.color = Color.Lerp(Color.clear, Color.black, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }

        t = 0.0f;
        while (t <= 0.8f)
        {
            gameOverPanel.color = Color.Lerp(Color.clear, Color.black, t + 0.8f);
            goText0.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }
        
        t = 0.0f;
        while (t <= 0.8f)
        {
            goText0.color = Color.Lerp(Color.clear, Color.white, t+0.8f);
            goText1.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        } 
        
        t = 0.0f;
        while (t <= 0.8f)
        {
            goText1.color = Color.Lerp(Color.clear, Color.white, t+0.8f);
            goText2.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }
        
        t = 0.0f;
        while (t <= 1.0f)
        {
            goText2.color = Color.Lerp(Color.clear, Color.white, t+0.8f);
            goText3.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        goButton.SetActive(true);
    }

    IEnumerator WinC()
    {
        winPanel.gameObject.SetActive(true);
        winPanel.color = Color.clear;
        winText0.color = Color.clear;
        winText1.color = Color.clear;
        winText2.color = Color.clear;
        winText3.color = Color.clear;

        winButton.SetActive(false);

        float t = 0;
        while (t <= 0.8f)
        {
            winPanel.color = Color.Lerp(Color.clear, Color.black, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }

        t = 0.0f;
        while (t <= 0.8f)
        {
            winPanel.color = Color.Lerp(Color.clear, Color.black, t + 0.8f);
            winText0.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }

        t = 0.0f;
        while (t <= 0.8f)
        {
            winText0.color = Color.Lerp(Color.clear, Color.white, t + 0.8f);
            winText1.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }

        t = 0.0f;
        while (t <= 0.8f)
        {
            winText1.color = Color.Lerp(Color.clear, Color.white, t + 0.8f);
            winText2.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }

        t = 0.0f;
        while (t <= 1.0f)
        {
            winText2.color = Color.Lerp(Color.clear, Color.white, t + 0.8f);
            winText3.color = Color.Lerp(Color.clear, Color.white, t);
            t += Time.deltaTime / 2.5f; yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        winButton.SetActive(true);
    }
}
