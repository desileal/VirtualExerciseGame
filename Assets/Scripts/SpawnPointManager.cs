
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// network behavior - singleton?
public class SpawnPointManager : NetworkBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private int repetitions;

    private int _currentIndex;
    private int _currentRep;

    private NetworkPlayer owner;

    public void StartSpawning(NetworkPlayer player)
    {
        owner = player;
        SpawnNewOrb();
    }

    public Transform GetNextSpawnPoint()
    {
        if (spawnPoints.Count == 0) return null;

        Transform point = spawnPoints[_currentIndex];
        Debug.Log(point.transform.position);
        _currentIndex = (_currentIndex + 1) % spawnPoints.Count;
        if (_currentIndex == 0) _currentRep++;
        return point;
    }
    
    // only spawn new orbs from the server
    // use network object pool instead of reinstantiating
    public void SpawnNewOrb()
    {
        Debug.Log("spawning orbs");
        if (_currentRep >= repetitions) return;
        
        Transform spawnPoint = GetNextSpawnPoint();
        if (spawnPoint == null) return;
        
        GameObject newOrb = Instantiate(orbPrefab, spawnPoint.position, Quaternion.identity);
        Orb orb = newOrb.GetComponent<Orb>();
        orb.ownerPlayer = owner;
        orb.onOrbHit.AddListener(SpawnNewOrb);
        if (spawnPoint.name.Contains("R"))
        {
            orb.direction = OrbDirection.Right;
        }
        else if (spawnPoint.name.Contains("L"))
        {
            orb.direction = OrbDirection.Left;
        }
        orb.Initialize();

        // Make it a networked object with owner
        NetworkObject orbNetObj = newOrb.GetComponent<NetworkObject>();
        orbNetObj.SpawnWithOwnership(owner.OwnerClientId); 
    }
}