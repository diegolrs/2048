using UnityEngine;

public class BlockSpace : MonoBehaviour
{
    private Block _blockToHandle;
    private BoardManager _manager;

    public Vector2 Position => transform.localPosition;

    public Block GetBlock() => _blockToHandle;
    public bool IsHoldingBlock() => _blockToHandle != null;
    public void HoldBlock(Block  block) => _blockToHandle = block;                                                       
    public void ReleaseBlock(Block block) 
    {
        if(block == _blockToHandle)
            _blockToHandle = null;
        else
            Debug.Log("[Block Space]: This block doesn't belong to this space");
    } 
}