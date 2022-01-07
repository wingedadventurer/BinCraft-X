using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    const float ANGLE_Y_OFFSET_MAX = 89.0f;

    public float mouseSensitivity;

    [HideInInspector] public bool active;

    [Header("Ref")]
    public Transform transformHead;

    private float mouseOffsetY;

    void Update()
    {
        if (active)
        {
            // get look input
            Vector2 lookInput = Vector2.zero;
            lookInput.x = Input.GetAxis("Mouse X");
            lookInput.y = Input.GetAxis("Mouse Y");

            // apply look x input
            transform.Rotate(Vector3.up, lookInput.x * mouseSensitivity);

            // apply look y input
            mouseOffsetY = Mathf.Clamp(mouseOffsetY + lookInput.y * mouseSensitivity, -ANGLE_Y_OFFSET_MAX, ANGLE_Y_OFFSET_MAX);
            transformHead.localEulerAngles = new Vector3(-mouseOffsetY, 0, 0);
        }
    }
}
