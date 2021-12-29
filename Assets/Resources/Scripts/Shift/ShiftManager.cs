using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(ShiftInputs))]
public class ShiftManager : MonoBehaviour
{
    [SerializeField] GameMode _gameMode;
    [SerializeField] BoardManager _board;
    [SerializeField] ShiftInputs _input;
    public bool EnableShifts {get; set; }

    private bool _isShifting;

    void Update()
    {
        if(!EnableShifts || _isShifting)
            return;

        if (_input.PressedLeft())
        {
            Shift(Vector2.left);
        }
        else if (_input.PressedRight())
        {
            Shift(Vector2.right);
        }
        else if (_input.PressedUp())
        {
            Shift(Vector2.up);
        }
        else if (_input.PressedDown())
        {
            Shift(Vector2.down);
        }
    }

    /// <summary> Get the last empty block space at the given direction.</summary>
    /// <param name="dir">Direction to iterate.</param>
    /// <param name="current">Position to use as base..</param>
    private Vector2 LastEmpty(Vector2 dir, Vector2 current)
    {
        Vector2 nextPosition = current + dir;

        if(_board.IsOutsideBoard(nextPosition) || _board.HasBlockAtSpace(nextPosition))
            return current;
        else
        {
            current = nextPosition;
            return LastEmpty(dir, current);
        }
    }

    /*
        Enumerate list using the shift direction as reference,
        that way, the elements that is close to the shift direction
        can be iterated first.

        Ex: 
            Case shift up this:
            
            |(C) ( ) (J)|     |(C) (D) (J)| 
            |(A) (D) (A)| ->  |(A) (M) (A)|
            |(Z) (M) (N)|     |(Z) ( ) (N)|

            The D element should be iterated before M, otherwise, that would happen:

            |(C) ( ) (J)|                                                        |(C) (D) (J)| 
            |(A) (D) (A)| ->  M does nothing, because there is a block above ->  |(A) ( ) (A)|
            |(Z) (M) (N)|                                                        |(Z) (M) (N)|
    */
    private void OrderListToDirection(Vector2 direction, out List<Block> list)
    {
        list = null;

        if(direction == Vector2.left)
            list = _board.GetBlocks().OrderBy(v => v.Position.x).ToList();
        else if (direction == Vector2.right)
            list = _board.GetBlocks().OrderByDescending(v => v.Position.x).ToList();
        else if (direction == Vector2.up)
            list = _board.GetBlocks().OrderByDescending(v => v.Position.y).ToList();
        else if (direction == Vector2.down)
            list = _board.GetBlocks().OrderBy(v => v.Position.y).ToList();
    }

    private void Shift(Vector2 dir)
    {
        _isShifting = true;

        List<Block> blocksToDestroy = new List<Block>();
        List<Block> blocksToMerge = new List<Block>();
        OrderListToDirection(dir, out var blocksNotChecked);

        Vector2 nextPosition;

        while(blocksNotChecked.Count > 0)
        {
            var current = blocksNotChecked[0];

            nextPosition = current.Position + dir;

            if(_board.IsOutsideBoard(nextPosition))
            {
                blocksNotChecked.Remove(current);
                continue;
            }

            Vector2 lastEmptyPosition = LastEmpty(dir, current.Position);
            Vector2 targetPosition = lastEmptyPosition;
            Vector2 nextPossiblePosition = targetPosition + dir;

            Block blockInTarget = null;
            bool shouldMerge = false;

            if(_board.HasBlockAtSpace(nextPossiblePosition, out blockInTarget))
            {
                bool canMerge = !blocksToMerge.Contains(current); // can only merge once in a movement
                canMerge &= !blocksToMerge.Contains(blockInTarget); // can only merge once in a movement
                canMerge &= current.CanMergeBlock(blockInTarget); // can only merge equals values

                if(canMerge)
                {
                    targetPosition = LastEmpty(dir, nextPossiblePosition);
                    shouldMerge = true;
                }
            }
            
            // Should Move
            if(targetPosition != current.Position)
            {
                if(shouldMerge)
                {
                    blocksToMerge.Add(current);
                    
                    blockInTarget.LeaveCurrentSpace();
                    blocksToDestroy.Add(blockInTarget);

                    blocksNotChecked.Remove(blockInTarget);
                }

                current.LeaveCurrentSpace();
                current.EnterSpace(_board.GetBlockSpaceAt(targetPosition));
            }

            blocksNotChecked.Remove(current);
        }

        _gameMode.OnEndShift(blocksToDestroy, blocksToMerge);
        _isShifting = false;
    }
}