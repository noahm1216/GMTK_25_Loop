using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGearIndicator : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public Image arrowPointerUI;
    public Image childPointerUI;

    public void LateUpdate()
    {
        float x = 0f;
        float y = 0f;

        Vector2 A = Camera.main.WorldToScreenPoint(player.position);
        Vector2 B = Camera.main.WorldToScreenPoint(target.position);

        // slope
        var m = (B.y - A.y) / (B.x - A.x);
        // y-intercept
        var b = A.y + (-m * A.x);

        // x and y formulas reference
        // y = (m * x) + b;
        // x = (y - b) / m;

        Vector2 offset = arrowPointerUI.rectTransform.sizeDelta / 2;
        Rect scrRect = new Rect(offset, new Vector2(Screen.width - offset.x, Screen.height - offset.y));

        // clamp target x pos to screen to help find y
        x = Mathf.Clamp(B.x, scrRect.xMin, scrRect.xMax);
        y = (m * x) + b;

        // if y is off screen clamp it, then find a better x
        if (y < scrRect.yMin || y > scrRect.yMax)
        {
            y = Mathf.Clamp(y, scrRect.yMin, scrRect.yMax);
            x = (y - b) / m;
        }

        // arrow position
        Vector2 C = new Vector2(x, y);

        // this checks if target is on screen or not
        if (Vector2.Distance(B, C) > offset.x)
        {
            // target is far from arrow, so it is off screen
            // show arrow and position it
            arrowPointerUI.enabled = true;
            childPointerUI.enabled = true;
            arrowPointerUI.transform.position = C;

            // rotate arrow to point from player to target
            float angle = Mathf.Atan2(B.y - A.y, B.x - A.x) * Mathf.Rad2Deg;
            arrowPointerUI.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
        else
        {
            // target is close to arrow, so it is on screen
            // hide arrow
            arrowPointerUI.enabled = false;
            childPointerUI.enabled = false;
        }
    }
}

