using UnityEngine;

public class MultiTouchBehaviour : MonoBehaviour
{
    void Update()
    {
        HandleMultiTouchInput();
    }

    private void HandleMultiTouchInput()
    {
        // Not multi-touch, skip...
        if (Input.touchCount < 2)
            return;

        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
        {
            // initialize tracking... (currently, just do nothing)
            return;
        }

        if (firstTouch.phase == TouchPhase.Stationary || secondTouch.phase == TouchPhase.Stationary)
        {
            // continue; user might only be moving 1 finger
            // it is only important that there are >=2 touches
        }

        if (firstTouch.phase == TouchPhase.Moved && secondTouch.phase == TouchPhase.Moved)
        {
            // Get vectors for previous frame
            Vector2 oldFirstPosition = firstTouch.position - firstTouch.deltaPosition;
            Vector2 oldsecondPosition = secondTouch.position - secondTouch.deltaPosition;

            // Scale according to multi-touch gesture

            float oldDistance = Vector2.Distance(oldFirstPosition, oldsecondPosition);
            float newDistance = Vector2.Distance(firstTouch.position, secondTouch.position);
            float zoomMagnitude = newDistance / oldDistance;

            transform.localScale *= zoomMagnitude;

            // Rotate according to multi-touch gesture

            Vector2 oldLine = oldsecondPosition - oldFirstPosition;
            Vector2 newLine = secondTouch.position - firstTouch.position;
            float rotationAngle = Vector2.SignedAngle(newLine, oldLine);

            Vector3 rotationAxes = new Vector3(0.0f, 1.0f, 0.0f);

            transform.Rotate(rotationAxes, rotationAngle);
        }

        if (firstTouch.phase == TouchPhase.Canceled || secondTouch.phase == TouchPhase.Canceled)
        {
            // do nothing... maybe exit tracking...
            return;
        }

        if (firstTouch.phase == TouchPhase.Ended || secondTouch.phase == TouchPhase.Moved)
        {
            // do nothing... maybe exit tracking...
            return;
        }
    }

}
