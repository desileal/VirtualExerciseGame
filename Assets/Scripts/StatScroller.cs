using UnityEngine;
using UnityEngine.InputSystem;

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
        scroll = XRInputAction.XRILeft.Scroll;
        scroll.Enable();
    }

    private void OnDisable()
    {
        scroll.Disable();
    }

    void Update()
    {
        // Get input from VR controllers (e.g., thumbstick horizontal axis)
        float inputX = scroll.ReadValue<Vector2>().x;
        Debug.Log(inputX);
        
        
        // Update visible position
        transform.position += new Vector3(inputX * scrollSpeed * Time.deltaTime, 0f, 0f);
        
    }
}