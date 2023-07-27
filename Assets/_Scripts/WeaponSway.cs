using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] public float swaySensitivity = 0.02f;
    [SerializeField] private float swayClamp = 20f;
    [SerializeField] private float swaySmoothness = 20f;
    private Vector3 startPosition;
    private Vector3 nextPosition;
    private Vector3 currentVelocity = Vector3.zero;
    private PlayerInput _playerInput;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = _playerInput.actions["Look"].ReadValue<Vector2>().x * swaySensitivity * Time.deltaTime;
        float mouseY = _playerInput.actions["Look"].ReadValue<Vector2>().y * swaySensitivity * Time.deltaTime;

        mouseX = Math.Clamp(mouseX, -swayClamp, swayClamp);
        mouseY = Math.Clamp(mouseY, -swayClamp, swayClamp);

        nextPosition = new Vector3(mouseX, mouseY, 0);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, nextPosition + startPosition,
            ref currentVelocity, swaySmoothness * Time.deltaTime);
    }
}
