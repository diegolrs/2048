using UnityEngine;

public class ShiftInputs : MonoBehaviour
{
    [SerializeField] SwipeHandler _swipeHandler;

    public bool PressedLeft()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) || _swipeHandler.SwipedLeft();
    }

    public bool PressedRight()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) || _swipeHandler.SwipedRight();
    }

    public bool PressedUp()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || _swipeHandler.SwipedUp();
    }

    public bool PressedDown()
    {
        return Input.GetKeyDown(KeyCode.DownArrow) || _swipeHandler.SwipedDown();
    }
}