using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.UI;
using UnityEngine.Serialization;

public enum OrbDirection {Left, Right}
public class Orb: NetworkBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f;
    [SerializeField] public OrbDirection direction = OrbDirection.Right;
    //TODO - link this event with SpawnNewOrb in SpawnPointManager
    [FormerlySerializedAs("OnOrbHit")] public UnityEvent onOrbHit;
    public NetworkPlayer ownerPlayer;
    // player has 3s to hit the orb until it is destroyed and a new one is spawned
    // breaks their streak if they don't hit it in time
    [SerializeField] private float maxHitTime = 3f;
    private float spawnTime;
    private bool hasBeenHit = false;
    public static event Action<NetworkPlayer> OnOrbHitByPlayer;
    // handles sounds

    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
/*        if (!hasBeenHit && Time.time - spawnTime >= maxHitTime)
        {
            if (IsServer && ownerPlayer != null)
            {
                ownerPlayer.ResetStreak();
            }
            Destroy(gameObject);
        }*/
    }

    public void Initialize()
    {
        // sound?
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (direction == OrbDirection.Right && other.CompareTag("VRControllerRight") || direction == OrbDirection.Left && other.CompareTag("VRControllerLeft"))
        {
            Debug.Log("hit");
            hasBeenHit = true;

            NetworkPlayer player = other.GetComponentInParent<NetworkPlayer>();
            HandleHit(player);
            /*if (player == ownerPlayer)
            {
                Debug.Log("Owner");
                HandleHit(player);                
            }*/
        }
        
    }

    // add network player to arguments
    private void HandleHit(NetworkPlayer player)
    {
        OnOrbHitByPlayer?.Invoke(player); // add Player in brackets
        onOrbHit?.Invoke();
        onOrbHit.RemoveAllListeners();
        Destroy(gameObject, destroyDelay);
    }
}