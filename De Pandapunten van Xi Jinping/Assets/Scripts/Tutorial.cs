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
    public Spawner tutorialSpawner00, tutorialSpawner01, tutorialSpawner02;
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
        }

        throw new System.ArgumentException("That tutorial doesn't exist. Weirdo.");
    }

    IEnumerator TutorialZero()
    {
        hoveredOverTarget = false;
        tutorialPanel.SetActive(true);
        spawnManager.SpawnNewLevelTutorial(1, 2, new Spawner[] { tutorialSpawner00, tutorialSpawner01, tutorialSpawner02 });
        GameManager.Instance.AmountOfAtrocities = 1;

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


        tutorialPanel.SetActive(false);
        tutorialSpawner00.ResetSpawner(); tutorialSpawner01.ResetSpawner(); tutorialSpawner02.ResetSpawner();
        yield break;
    }

    public void HoveredOverTarget()
    {
        hoveredOverTarget = true;
    }
}
