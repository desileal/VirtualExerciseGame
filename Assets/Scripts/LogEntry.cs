using UnityEngine;

[System.Serializable]
public struct LogEntry
{
    [SerializeField] public float activePoints;
    [SerializeField] public float activeTime;
    [SerializeField] public bool streak;
    [SerializeField] public float caloriesBurnt;
    [SerializeField] public string date;
}