using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial")]
    public Text tutorialText;
    public GameObject tutorialPanel;
    [TextArea]
    public string[] tutorial0Text; 
    [TextArea]
    public string[] tutorial1Text;

    public Spawner tutorialSpawner00, tutorialSpawner01, tutorialSpawner02;
    public Spawner tutorialSpawner10, tutorialSpawner11, tutorialSpawner12;
    public Spawner tutorialSpawner20, tutorialSpawner21, tutorialSpawner22;

    public GameObject tutorialSpawnerParent;
    bool hoveredOverTarget;

    SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
    }

    public Coroutine PlayTutorial(int index)
    {
        switch(index)
        {
            case 0: return StartCoroutine(TutorialZero());
            case 1: return StartCoroutine(TutorialOne());
            case 2: return StartCoroutine(TutorialTwo());
        }

        throw new System.ArgumentException("That tutorial doesn't exist. Weirdo.");
    }

    IEnumerator TutorialZero()
    {
        hoveredOverTarget = false;
        tutorialPanel.SetActive(true);
        spawnManager.SpawnNewLevelTutorial(1, 2, new Spawner[] { tutorialSpawner00, tutorialSpawner01, tutorialSpawner02 });
        GameManager.Instance.AmountOfAtrocities = 1;
        GameManager.Instance.AtrocitiesCovered = 0;

        tutorialText.text = tutorial0Text[0];

        while (!hoveredOverTarget) { yield return null; }


        while(GameManager.Instance.AmountOfAtrocities - GameManager.Instance.AtrocitiesCovered > 0)
        {
            tutorialText.text = tutorial0Text[1];

            while (GameManager.Instance.AmountOfPosters > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);

            //This hurts
            if (GameManager.Instance.AmountOfAtrocities - GameManager.Instance.AtrocitiesCovered > 0)
            {
                tutorialText.text = tutorial0Text[3];
                yield return new WaitForSeconds(3.0f);
                GameObject[] pain = GameObject.FindGameObjectsWithTag("CoverObject");
                for (int i = 0; i < pain.Length; i++)
                {
                    Destroy(pain[i]);
                }

                GameManager.Instance.AmountOfPosters++;
            }
        }

        tutorialText.text = tutorial0Text[2];
        yield return new WaitForSeconds(2.0f);
        GameObject[] suffering = GameObject.FindGameObjectsWithTag("CoverObject");
        for (int i = 0; i < suffering.Length; i++)
        {
            Destroy(suffering[i]);
        }

        tutorialPanel.SetActive(false);
        tutorialSpawner00.ResetSpawner(); tutorialSpawner01.ResetSpawner(); tutorialSpawner02.ResetSpawner();
        yield break;
    }

    IEnumerator TutorialOne()
    {
        hoveredOverTarget = false; tutorialPanel.SetActive(true);
        spawnManager.SpawnNewLevelTutorial(1, 2, new Spawner[] { tutorialSpawner10, tutorialSpawner11, tutorialSpawner12 });
        
        GameManager.Instance.AmountOfAtrocities = 1;
        GameManager.Instance.AtrocitiesCovered = 0;

        tutorialText.text = tutorial1Text[0];

        while (!hoveredOverTarget) { yield return null; }

        tutorialText.text = tutorial1Text[1];

        while (!Input.GetMouseButton(0)) { yield return null; }
        yield return new WaitForSeconds(2.0f);


        while (GameManager.Instance.AmountOfAtrocities - GameManager.Instance.AtrocitiesCovered > 0)
        {
            tutorialText.text = tutorial1Text[2];

            while (GameManager.Instance.AmountOfPlushies > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);

            //This hurts
            if (GameManager.Instance.AmountOfAtrocities - GameManager.Instance.AtrocitiesCovered > 0)
            {
                tutorialText.text = tutorial1Text[4];
                yield return new WaitForSeconds(3.0f);
                GameObject[] suffering = GameObject.FindGameObjectsWithTag("CoverObject");
                for (int i = 0; i < suffering.Length; i++)
                {
                    Destroy(suffering[i]);
                }

                GameManager.Instance.AmountOfPlushies++;
            }
        }

        tutorialText.text = tutorial1Text[3];
        yield return new WaitForSeconds(2.0f);

        GameObject[] pain = GameObject.FindGameObjectsWithTag("CoverObject");
        for (int i = 0; i < pain.Length; i++)
        {
            Destroy(pain[i]);
        }

        tutorialPanel.SetActive(false);
        tutorialSpawner10.ResetSpawner(); tutorialSpawner11.ResetSpawner(); tutorialSpawner12.ResetSpawner();
        yield break;
    }   
    
    IEnumerator TutorialTwo()
    {
        spawnManager.SpawnNewLevelTutorial(2, 1, new Spawner[] { tutorialSpawner20, tutorialSpawner21, tutorialSpawner22 });
        
        GameManager.Instance.AmountOfAtrocities = 2;
        GameManager.Instance.AtrocitiesCovered = 0;
        GameManager.Instance.Timer = 30;

        while (GameManager.Instance.AtrocitiesCovered < GameManager.Instance.AmountOfAtrocities && GameManager.Instance.Timer >= 0)
        { yield return new WaitForSeconds(1.0f); GameManager.Instance.Timer--; }

        
        GameObject[] pain = GameObject.FindGameObjectsWithTag("CoverObject");
        for (int i = 0; i < pain.Length; i++)
        {
            Destroy(pain[i]);
        }

        tutorialPanel.SetActive(false);
        tutorialSpawner20.ResetSpawner(); tutorialSpawner21.ResetSpawner(); tutorialSpawner22.ResetSpawner();
        yield break;
    }

    public void HoveredOverTarget()
    {
        hoveredOverTarget = true;
    }
}
