using UnityEngine;

public class SwipeHandler : MonoBehaviour
{
    public const float MaxSwipeTime = 1.2f; 
	public const float MinSwipeDistance = 0.18f;

    Vector2 _startPosition;
    float _startTime;

    Vector2 _swipeDirection;

    private Vector2 Normalize(Vector2 v) => v/Screen.width;

    public bool SwipedLeft() => _swipeDirection == Vector2.left;
    public bool SwipedRight() => _swipeDirection == Vector2.right;
    public bool SwipedUp() => _swipeDirection == Vector2.up;
    public bool SwipedDown() => _swipeDirection == Vector2.down;

    public void Update()
	{
        _swipeDirection = Vector2.zero;

        if(Input.touches.Length <= 0)
            return;

        Touch t = Input.GetTouch(0);

        if(t.phase == TouchPhase.Began)
        {
            _startPosition = Normalize(t.position);
            _startTime = Time.time;
        }

        if(t.phase == TouchPhase.Ended)
        {
            if (Time.time - _startTime > MaxSwipeTime) // Swipe has taken long
                return;

            Vector2 endPosition = Normalize(t.position);
            Vector2 swipe = new Vector2(endPosition.x - _startPosition.x, endPosition.y - _startPosition.y);

            if (swipe.magnitude < MinSwipeDistance) // Swipe was too short
                return;

            if (Mathf.Abs (swipe.x) > Mathf.Abs (swipe.y)) 
                _swipeDirection = swipe.x > 0 ?  Vector2.right : Vector2.left;
            else
                _swipeDirection = swipe.y > 0 ? Vector2.up : Vector2.down;
        }
	}
}