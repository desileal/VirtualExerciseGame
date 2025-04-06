using UnityEngine;
using UnityEngine.Serialization;


public class StatGenerator : MonoBehaviour
{
    [SerializeField] private Vector3 initialLocation;
    [SerializeField] private LogEntry[] entries;
    [SerializeField] private GameObject[] caloriesIcons;
    [SerializeField] private DynamicObjectChain entryBuilder;
    [SerializeField] private float distanceBetweenEntries;
    [SerializeField] private string[] iconDescription;

    private Vector3 _currentSpawnPosition;
    
    private void Start()
    {
        _currentSpawnPosition = initialLocation;

        foreach (var entry in entries)
        {
            GameObject calorieIcon = GetCaloriesIcon(entry);
            calorieIcon.GetComponent<StatInfo>().entryDetails = entry;
            calorieIcon.GetComponent<StatInfo>().UpdateIconInfo(GetCaloriesInfo(entry));
            calorieIcon.GetComponent<StatInfo>().UpdatePanelInfo();
            GameObject chain =  entryBuilder.GenerateChain(_currentSpawnPosition, entry.activePoints/10f,
                entry.activeTime/10f, calorieIcon, entry.streak, entry.date);
            chain.transform.parent = transform;
            _currentSpawnPosition += new Vector3(distanceBetweenEntries, 0f, 0f);
        }
    }

    private GameObject GetCaloriesIcon(LogEntry entry)
    {
        int index = Mathf.FloorToInt(entry.caloriesBurnt / 50f);
        index = Mathf.Clamp(index, 0, caloriesIcons.Length - 1);

        return caloriesIcons[index];
    }
    
    private string GetCaloriesInfo(LogEntry entry)
    {
        int index = Mathf.FloorToInt(entry.caloriesBurnt / 50f);
        index = Mathf.Clamp(index, 0, iconDescription.Length - 1);

        return iconDescription[index];
    }
    
}
