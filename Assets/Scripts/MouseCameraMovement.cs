using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraMovement : MonoBehaviour
{
    [SerializeField] float mousePositionX;
    float mousePositionY;

    float halfWidth;
    float halfHeight;

    //Percent relative smallest screen coordinate
    [SerializeField][Range(0f, 0.2f)] float movementBorder;
    int movementBorderInPixels;

    float movementX;
    float movementY;
    [SerializeField][Range(0f, 1f)] float movementStrenght;

    Vector3 cameraStep;

    void FixedUpdate()
    {
        halfWidth = Screen.width / 2;
        halfHeight = Screen.height / 2;

        mousePositionX = Input.mousePosition.x - halfWidth;
        mousePositionY = Input.mousePosition.y - halfHeight;

        movementBorderInPixels = (int)(Mathf.Min(Screen.width, Screen.height) * movementBorder);

        // Check if X inside movement zone relative to screen ratio
        if (mousePositionX < (-halfWidth) + movementBorderInPixels
        || mousePositionY < (-halfHeight) + movementBorderInPixels
        || mousePositionX > halfWidth - movementBorderInPixels
        || mousePositionY > halfHeight - movementBorderInPixels)
        {
            if (mousePositionX < 1) {
                movementX = Mathf.InverseLerp(0f, halfWidth, - mousePositionX);
                cameraStep.x = - Mathf.Lerp(0f, movementStrenght, movementX);
            } else if (mousePositionX > 1){
                movementX = Mathf.InverseLerp(0f, halfWidth, mousePositionX);
                cameraStep.x = Mathf.Lerp(0f, movementStrenght, movementX);
            }

            if (mousePositionY < 1) {
                movementY = Mathf.InverseLerp(0f, halfHeight, - mousePositionY);
                cameraStep.y = - Mathf.Lerp(0f, movementStrenght, movementY);
            } else if (mousePositionY > 1){
                movementY = Mathf.InverseLerp(0f, halfHeight, mousePositionY);
                cameraStep.y = Mathf.Lerp(0f, movementStrenght, movementY);
            }
        }
        else {
            cameraStep.x = 0;
            cameraStep.y = 0;
        }

        transform.position += cameraStep;
    }
}
