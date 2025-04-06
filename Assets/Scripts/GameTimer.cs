using TMPro;
using UnityEngine;

// to do - coutndown to game start once the host starts game
// clients see from 00:10 to 0 then timer starts counting up
public class GameTimer : MonoBehaviour
{
    public TextMeshPro timerText;
    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer() => isRunning = false;
    public float GetElapsedTime() => elapsedTime;
}
