using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Button nextButton, optionButton0, optionButton1, optionButton2;
    public Text dialogueText, speakerName;
    public Speaker speakerOne, speakerTwo, speakerThree;
    bool nextPressed;
    public int currentPenalty = 0;

    public Coroutine PlayDialogue(Dialogue d)
    {
        return PlayDialogue(d.Content, d.Speakers, d.SpeakerAmount, d.SpeakerOneName, d.SpeakerTwoName, d.SpeakerThreeName);
    }

    public Coroutine PlayDialogue(string[] content, int[] whichSpeaker, int speakerAmount, string speakerOneName, string speakerTwoName = "", string speakerThreeName = "")
    {
        return StartCoroutine(PlayDialogueC(content, whichSpeaker, speakerAmount, speakerOneName, speakerTwoName, speakerThreeName));
    }

    IEnumerator PlayDialogueC(string[] content, int[] whichSpeaker, int speakerAmount, string speakerOneName, string speakerTwoName = "", string speakerThreeName = "")
    {
        GameManager.Instance.canPlace = false;
        dialoguePanel.SetActive(true); nextButton.gameObject.SetActive(true); 
        optionButton0.gameObject.SetActive(false); optionButton1.gameObject.SetActive(false); optionButton2.gameObject.SetActive(false);

        speakerOne.gameObject.SetActive(true);
        speakerTwo.gameObject.SetActive(speakerAmount > 1);
        speakerThree.gameObject.SetActive(speakerAmount > 2);

        for (int i = 0; i < content.Length; i++)
        {
            ToggleSpeaker(whichSpeaker[i], speakerOneName, speakerTwoName, speakerThreeName);
            dialogueText.text = content[i];

            while (!nextPressed) { yield return null; }
            nextPressed = false;
        }
        GameManager.Instance.canPlace = true;
        dialoguePanel.SetActive(false);
    }

    public Coroutine PlayCaughtDialogue(string atrocity)
    {
        return StartCoroutine(PlayCaughtDialogueC(atrocity));
    }

    IEnumerator PlayCaughtDialogueC(string atrocity)
    {

        yield return PlayDialogue(new string[] { "Wait... is that a " + atrocity + "?" }, new int[] { 1 },1,"The Whole World");
        
        GameManager.Instance.canPlace = false;

        dialoguePanel.SetActive(true); nextButton.gameObject.SetActive(false);
        dialogueText.text = "";
        speakerName.text = "Xi JinPing";
        speakerOne.gameObject.SetActive(true);
        speakerTwo.gameObject.SetActive(false);
        speakerThree.gameObject.SetActive(false);

        optionButton0.gameObject.SetActive(GameManager.Instance.AmountOfPosters > 0 && currentPenalty < 3);
        optionButton1.gameObject.SetActive(GameManager.Instance.AmountOfPlushies > 0 && currentPenalty < 3);
        optionButton2.gameObject.SetActive(true);

        nextPressed = false;
        while (!nextPressed) { yield return null; }
        nextPressed = false;

        optionButton0.gameObject.SetActive(false); optionButton1.gameObject.SetActive(false); optionButton2.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
        GameManager.Instance.canPlace = true;
    }

    public void PressNext()
    {
        nextPressed = true;
    }

    void ToggleSpeaker(int speaker, string speakerOneName, string speakerTwoName = "", string speakerThreeName = "")
    {
        switch(speaker)
        {
            case 1: speakerOne.Toggle(true); speakerTwo.Toggle(false); speakerThree.Toggle(false); speakerName.text = speakerOneName; break;
            case 2: speakerOne.Toggle(false); speakerTwo.Toggle(true); speakerThree.Toggle(false); speakerName.text = speakerTwoName; break;
            case 3: speakerOne.Toggle(false); speakerTwo.Toggle(false); speakerThree.Toggle(true); speakerName.text = speakerThreeName; break;
        }
    }

    public void DialogueOption(int opt)
    {
        nextPressed = true;
        switch (opt)
        {
            case 0:
                GameManager.Instance.AmountOfPosters--;
                GameManager.Instance.IncreasePosterPenalty();
                currentPenalty++;
                break;
            case 1:
                GameManager.Instance.AmountOfPlushies --;
                GameManager.Instance.IncreasePlushiePenalty();
                currentPenalty++;
                break;
            case 2:
                GameManager.Instance.GameOver();
                break;
        }
    }
}
