using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class MouseLook : MonoBehaviour
{
    [Header("Settings")] 
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor = true;
    public float mouseSensitivity = 2f;

    [Header("Player")] public GameObject character;

    Vector2 targetDirection;
    Vector2 targetCharacterDirection;
    Vector2 mouseAbsolute;
    Vector2 mouseDelta;
    float lastSentRotation;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        targetDirection = transform.localRotation.eulerAngles;
        targetCharacterDirection = character.transform.localRotation.eulerAngles;
    }

    void Update()
    {
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseAbsolute += mouseDelta * mouseSensitivity;
        mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
        transform.localRotation = Quaternion.AngleAxis(-mouseAbsolute.y, targetOrientation * Vector3.right) *
                                  targetOrientation;

        var yRotation = Quaternion.AngleAxis(mouseAbsolute.x, Vector3.up);
        character.transform.localRotation = yRotation * targetCharacterOrientation;
    }
}
