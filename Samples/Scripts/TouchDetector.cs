using System;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    public const bool DEBUG = true;

    public const int ACTION_DOWN = 0;
    public const int ACTION_UP = 1;
    public const int ACTION_MOVE = 2;

    public Action<Vector2, int> onTouch;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                OnScreenTouch(touch.position, ACTION_DOWN);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                OnScreenTouch(touch.position, ACTION_MOVE);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnScreenTouch(touch.position, ACTION_UP);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnScreenTouch(Input.mousePosition, ACTION_DOWN);
            }
            else if (Input.GetMouseButton(0))
            {
                OnScreenTouch(Input.mousePosition, ACTION_MOVE);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnScreenTouch(Input.mousePosition, ACTION_UP);
            }
        }
    }

    void OnScreenTouch(Vector3 screenPoint, int action)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 localPoint = transform.InverseTransformPoint(hit.point);

            // Define a coordinate system as below.
            //
            // (0,0) o------------o
            //       |            |
            //       |            |
            //       |            |
            //       o------------o (1,1)
            //
            // Transform the hit position to that coordinate system.
            localPoint = new Vector3(localPoint.x + 0.5f, 0.5f - localPoint.y, 0);

            if (DEBUG)
            {
                Debug.LogFormat("Touched: {0}, {1}", localPoint.ToString("F4"), action);
            }

            if (onTouch != null)
            {
                onTouch.Invoke(localPoint, action);
            }
        }
    }
}
