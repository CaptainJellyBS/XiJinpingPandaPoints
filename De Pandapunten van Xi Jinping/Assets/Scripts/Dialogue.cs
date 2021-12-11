using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea]
    public string[] Content;
    public int[] Speakers;
    public int SpeakerAmount;
    public string SpeakerOneName;
    public string SpeakerTwoName;
    public string SpeakerThreeName;
}
