using UnityEngine;

[System.Serializable]
public struct BlockType
{
    public enum PossibleValues { _2, _4, _8, _16, _32, _64, _128, _256, _512, _1024, _2048, Size }
    public PossibleValues Value;
    public Color Color;
    public Color TextColor;

    public int ToInt()
    {
        int index = (int) Value;
        return (int) Mathf.Pow(2, index + 1);
    }
}