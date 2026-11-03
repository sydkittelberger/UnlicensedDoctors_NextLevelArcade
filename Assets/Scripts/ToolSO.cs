using UnityEngine;

[CreateAssetMenu(fileName = "ToolSO", menuName = "Scriptable Objects/ToolSO")]
public class ToolSO : ScriptableObject
{
    public Sprite toolSprite;
    public float rotationAmount;

    [Header("Germ Sprites")]
    public Sprite germUpSprite;
    public Sprite germDownSprite;
    public Sprite germLeftSprite;
    public Sprite germRightSprite;
    public Sprite germB1Sprite;
    public Sprite germB2Sprite;

    [Header("Germ Positions")]
    public Vector3 upPosition;
    public Vector3 downPosition;
    public Vector3 leftPosition;
    public Vector3 rightPosition;
    public Vector3 b1Position;
    public Vector3 b2Position;
}

