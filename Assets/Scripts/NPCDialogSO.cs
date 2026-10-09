using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogSO", menuName = "Scriptable Objects/NPCDialogSO")]
public class NPCDialogSO : ScriptableObject
{
    public DialogLine[] dialogLines;
}

[System.Serializable]
public class DialogLine
{
    public string text;
}
