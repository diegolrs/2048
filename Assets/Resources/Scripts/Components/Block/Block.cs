using TMPro;
using UnityEngine;

[RequireComponent(typeof(BlockAnimationController))]
public class Block : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] TextMeshPro _text;
    [SerializeField] BlockTypeSO _blockTypeSO;
    [SerializeField] BlockAnimationController _anim;
    
    private BlockType _type;

    private void OnDestroy() => LeaveCurrentSpace();

    public Vector2 Position => transform.localPosition;
    public bool CompareValue(BlockType.PossibleValues value) => _type.Value == value;

    private BlockSpace _blockSpace;

    public void SetType(BlockType type)
    {
        _type = type;
        ApplyOnUI(_type);
    }

    private void ApplyOnUI(BlockType type)
    {
        _renderer.color = type.Color;
        _text.text = type.ToInt().ToString();
        _text.color = type.TextColor;
    }

    public void IncreaseValue()
    {
        int index = (int) _type.Value + 1;
        int size = (int) BlockType.PossibleValues.Size;

        if(index >= size)
            index = size - 1;

        var value = (BlockType.PossibleValues) index;

        _type = _blockTypeSO.GetWithValue(value);
        ApplyOnUI(_type);
    }

    public void EnterSpace(BlockSpace space)
    {
        _blockSpace = space;
        _blockSpace.HoldBlock(this);
    }

    public void GoToBlockSpacePosition()
    {
        if(_blockSpace != null)
        {
            _anim.MakeSlideAnimation(_blockSpace.Position);
        }
    }

    public void LeaveCurrentSpace()
    {
        if(_blockSpace != null)
        {
            _blockSpace.ReleaseBlock(this);
            _blockSpace = null;
        }
    }

    public bool CanMergeBlock(Block otherBlock) => otherBlock.CompareValue(_type.Value);
    public void MergeBlock()
    {
        this.IncreaseValue();
        _anim.PlayMergeAnimation();  
    }
}