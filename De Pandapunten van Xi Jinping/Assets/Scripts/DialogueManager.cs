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

        dialoguePanel.SetActive(false);
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
}
