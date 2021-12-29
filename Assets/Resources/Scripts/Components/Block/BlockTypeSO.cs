using UnityEngine;

[CreateAssetMenu(menuName = "Data/BlockTypeSO", fileName = "New BlockTypeSO")]
public class BlockTypeSO : ScriptableObject
{
    [SerializeField] BlockType[] _types;

    private void Awake() 
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public BlockType RandomBetween(BlockType.PossibleValues minValue, BlockType.PossibleValues maxValue)
    {
        int rand = Random.Range((int) minValue, (int) maxValue + 1);
        return GetWithValue((BlockType.PossibleValues) rand);
    }

    public BlockType GetWithValue(BlockType.PossibleValues value)
    {
        for(int i = 0; i < _types.Length; i++)
        {
            if(_types[i].Value == value)
                return _types[i];
        }

        throw new System.Exception($"Block type with value {value} not found");
    }
}
