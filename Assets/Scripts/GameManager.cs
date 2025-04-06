using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

// inherit network behaviour for RPC calls
public class GameManager : MonoBehaviour
{
    private int currentPlayerScore, currentPlayerHeartRate, currentPlayerCalories;
    private int totalTargetsSpawned, totalHits;
    private int currentStreak = 0;
    private int highestStreak = 0;
    private float comboMultiplier = 1f;
    private bool isGameRunning = false;
    [SerializeField] private TextMeshPro streakText;
    [SerializeField] private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro heartRateText;
    [SerializeField] private TextMeshPro caloriesText;
    [SerializeField] GameObject resultsPanel;

    public void Start()
    {
        StartGame();
    }

    public void Update()
    {
        if (!isGameRunning)
        {
            EndGame();
        }
    }

    public void OnEnable()
    {
        //Orb.OnOrbHitByPlayer += HandleOrbHit;
    }

    // add NetworkPlayer arguments to functions later
    private void HandleOrbHit()
    {
        IncreasePlayerScore();
        IncreaseStreak();
        GenerateHeartRate();
    }

    public void StartGame()
    {
        // reset player score
        currentPlayerScore = 0;
        currentPlayerHeartRate = 0;
        currentPlayerCalories = 0;
        totalTargetsSpawned = 0;
        totalHits = 0;
        isGameRunning = true;
        // TODO
        // start spawning new orbs
    }

    // todo - toggle button to end game once users are done
    public void EndGame()
    {
        isGameRunning = false;
        Debug.Log("Game Over");
        // submit score server rpc
        // update leaderboard
    }

    public void GenerateHeartRate()
    {
        currentPlayerHeartRate = Random.Range(80, 120);
        heartRateText.text = currentPlayerHeartRate.ToString();
    }

    public void IncreaseCalories()
    {
        currentPlayerCalories += Random.Range(1, 5);
        caloriesText.text = currentPlayerCalories.ToString();
    }

    public void IncreasePlayerScore()
    {
        currentPlayerScore += 1;
        totalHits += 1;
        if (scoreText != null)
        {
            scoreText.text = currentPlayerScore.ToString();
        }
    }

    public void OnTargetSpawned()
    {
        totalTargetsSpawned++;
    }

    public void IncreaseStreak()
    {
        currentStreak++;
        comboMultiplier = 1f + currentStreak * 0.1f;
        if (currentStreak > highestStreak) highestStreak = currentStreak;
        UpdateStreakUI();
    }

    public void ResetStreak()
    {
        currentStreak = 0;
        comboMultiplier = 1f;
        UpdateStreakUI();
    }

    public void UpdateStreakUI()
    {
        streakText.text = currentStreak.ToString();
    }

    public float GetAccuracy()
    {
        return totalTargetsSpawned > 0 ? (float)totalHits / totalTargetsSpawned * 100f : 0f;
    }


    // create leaderboard for end of game
    public void ShowResults()
    {
        // call get accuracy 
        // highest streak

    }
}
    /*public override void FinishGame(bool submitScore = true)
    {
        base.FinishGame(submitScore);

        // functionality for end of game
        // show congratulations
    }*/

    // TODO - create method to track player score
    /*public void LocalPlayerIncreaseScore(int score)
    {
        if (m_MiniGameManager.currentNetworkedGameState == MiniGameManager.GameState.InGame)
        {
            currentPlayerScore += score;
            // any function to fire across all connected clients has to be stamped with RPC
            // any clause with net code has to inherit network behaviour 
            m_MiniGameManager.SubmitScoreServerRpc(currentPlayerScore, XRINetworkPlayer.LocalPlayer.OwnerClientId); 
        }
    }*/

//[ServerRpc]
// server and client is called host
// otherwise just client (client RPC)
// self check if calls are client or server - reference network manager (singleton)
//private void FixedUpdate()
//    {
//        
//    }

//[ClientRpc]
//void increaseScore()
//{
//
//}