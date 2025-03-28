using UnityEngine;

public class User : MonoBehaviour
{
    private string uname;
    private int score, heartRate, streak;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GenerateHearRate();
    }

    private void GenerateHearRate()
    {        
        heartRate = Random.Range(80, 110);
    }
}
