using UnityEngine;

[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string characterName;
    [Header("Time played")]
    public float secondsPlayed;

    [Header("Coordinates")]
    public float xPosition, yPosition, zPosition;
}
