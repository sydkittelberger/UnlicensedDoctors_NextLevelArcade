using UnityEngine;

[CreateAssetMenu(fileName = "ToolSO", menuName = "Scriptable Objects/ToolSO")]
public class ToolSO : ScriptableObject
{
    public Sprite toolSprite;

    [Header("Germ Positions")]
    public Vector3 upPosition;
    public Vector3 downPosition;
    public Vector3 leftPosition;
    public Vector3 rightPosition;
    public Vector3 b1Position;
    public Vector3 b2Position;
}

