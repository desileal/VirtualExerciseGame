
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private int repetitions;

    private int _currentIndex;
    private int _currentRep;

    private void Start()
    {
        SpawnNewOrb();
    }

    public Transform GetNextSpawnPoint()
    {
        if (spawnPoints.Count == 0) return null;

        Transform point = spawnPoints[_currentIndex];
        _currentIndex = (_currentIndex + 1) % spawnPoints.Count;
        if (_currentIndex == 0) _currentRep++;
        return point;
    }
    

    public void SpawnNewOrb()
    {
        if (_currentRep >= repetitions) return;
        
        Transform spawnPoint = GetNextSpawnPoint();
        if (spawnPoint == null) return;
        
        GameObject newOrb = Instantiate(orbPrefab, spawnPoint.position, Quaternion.identity);
        var orb = newOrb.GetComponent<Orb>();
        
        orb.onOrbHit.AddListener(SpawnNewOrb);
        
        orb.Initialize();
    }
}