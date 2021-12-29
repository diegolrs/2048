using UnityEngine;

public class GameEndValidator : MonoBehaviour
{
    [SerializeField] BoardManager _board;

    public bool PlayerWonGame()
    {
        foreach(Block block in _board.GetBlocks())
        {
            if(block.CompareValue(BlockType.PossibleValues._2048))
                return true;
        }

        return false;
    }

    public bool PlayerLosedGame()
    {
        foreach(Block block in _board.GetBlocks())
        {
            if(CanMakeAMovement(block))
                return false;
        }

        return true;
    }

    // Valid moves: 
    // 1 - Target is a empty block space
    // 2 - Block in target block space can be merged
    private bool IsAValideMove(Block block, Vector2 dir)
    {
        if(_board.IsOutsideBoard(block.Position + dir))
            return false;

        BlockSpace space = _board.GetBlockSpaceAt(block.Position + dir);

        return space.IsHoldingBlock() ? space.GetBlock().CanMergeBlock(block) : true;
    }

    private bool CanMakeAMovement(Block block)
    {
        return (  IsAValideMove(block, Vector2.up)
               || IsAValideMove(block, Vector2.down)
               || IsAValideMove(block, Vector2.left)
               || IsAValideMove(block, Vector2.right));
    }
}
