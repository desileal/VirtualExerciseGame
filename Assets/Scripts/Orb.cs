using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

public enum OrbDirection {Left, Right}
public class Orb: MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f;
    [SerializeField] private OrbDirection direction = OrbDirection.Right;
    [FormerlySerializedAs("OnOrbHit")] public UnityEvent onOrbHit;
    
    public void Initialize()
    {
        // sound?
    }

    private void OnTriggerEnter(Collider other)
    {
        if (direction == OrbDirection.Right && other.CompareTag("VRControllerRight"))
        {
            HandleHit();
        }
        else if (direction == OrbDirection.Left && other.CompareTag("VRControllerLeft"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        onOrbHit?.Invoke();
        onOrbHit.RemoveAllListeners();
        Destroy(gameObject, destroyDelay);
    }
}