using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static bool IsTouchingScreen(out Touch touch)
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // Check if touch is over UI elements
            if (EventSystem.current != null)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return false;
                }
            }

            return true;
        }

        touch = new Touch();
        return false;
    }

    public static bool IsTouchingScreen()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if touch is over UI elements
            if (EventSystem.current != null)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }
}
