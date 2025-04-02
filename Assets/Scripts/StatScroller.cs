using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class StatScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private float currentPosition = 0f;
    public XRIDefaultInputActions XRInputAction;
    private InputAction scroll;

    private void Awake()
    {
        XRInputAction = new XRIDefaultInputActions();
    }

    private void OnEnable()
    {
        scroll = XRInputAction.XRILeftInteraction.SelectValue;
        scroll.Enable();
    }

    private void OnDisable()
    {
        scroll.Disable();
    }

    void Start()
    {

    }

    void Update()
    {
        // Get input from VR controllers (e.g., thumbstick horizontal axis)
        float inputZ = scroll.ReadValue<Vector2>().y;
        
        
        // Update visible position
        transform.position += new Vector3(transform.position.x, transform.position.y, inputZ * scrollSpeed * Time.deltaTime);
        
    }
}