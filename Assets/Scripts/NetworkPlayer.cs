using UnityEngine;
using Unity.Netcode;
using TMPro;


public class NetworkPlayer : NetworkBehaviour
{
    public NetworkVariable<int> Score = new NetworkVariable<int>();
    public NetworkVariable<int> HeartRate = new NetworkVariable<int>();
    public NetworkVariable<int> Calories = new NetworkVariable<int>();
    public NetworkVariable<int> Streak = new NetworkVariable<int>();

    public GameObject statsCanvasPrefab;
    private GameObject canvasInstance;
    public Transform canvasAnchorPoint; // position above mat for the canvas to stay

    [Header("UI")]
    public TextMeshPro scoreText;
    public TextMeshPro heartRateText;
    public TextMeshPro caloriesText;
    public TextMeshPro streakText;
    public TextMeshPro timerText;

    [HideInInspector] public SpawnPointManager spawnPointManager;

    private float elapsedTime = 0f;
    private bool isTimerRunning = true;

    private void OnEnable()
    {
        Orb.OnOrbHitByPlayer += HandleOrbHit;
    }

    private void OnDisable()
    {
        Orb.OnOrbHitByPlayer -= HandleOrbHit;
    }

    public void Update()
    {
        if (!IsOwner || !isTimerRunning) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StartGameTimer()
    {
        isTimerRunning = true;
        elapsedTime = 0f;
    }

    public void StopGameTimer()
    {
        isTimerRunning = false;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log("On Network Spawn");

        if (!IsOwner)
        {
            // Only disable for remote players
            var cam = GetComponentInChildren<Camera>();
            if (cam != null) cam.enabled = false;

            var audio = GetComponentInChildren<AudioListener>();
            if (audio != null) audio.enabled = false;

            return;
        }

        if (IsOwner)
        {
            var lobbyCam = GameObject.Find("LobbyCamera");
            if (lobbyCam != null)
            {
                lobbyCam.SetActive(false);
            }

            Score.OnValueChanged += UpdateScoreUI;
            HeartRate.OnValueChanged += UpdateHeartRateUI;
            Calories.OnValueChanged += UpdateCaloriesUI;
            Streak.OnValueChanged += UpdateStreakUI;

            string spawnerName = IsHost ? "SpawnPointManager_Host" : "SpawnPointManager_Client";
            spawnPointManager = GameObject.Find(spawnerName)?.GetComponent<SpawnPointManager>();

            if (spawnPointManager != null)
            {
                spawnPointManager.StartSpawning(this); // Kick off orb spawning
            }
            else
            {
                Debug.LogError("SpawnPointManager not found!");
            }

            string anchorName = IsHost ? "CanvasAnchor_Host" : "CanvasAnchor_Client";
            canvasAnchorPoint = GameObject.Find(anchorName)?.transform;

            if (canvasAnchorPoint == null)
            {
                Debug.LogError("Canvas Anchor Point not found!");
            }
            if (statsCanvasPrefab == null)
            {
                Debug.LogError("StatsCanvasPrefab is NULL");
            }

            canvasInstance = Instantiate(statsCanvasPrefab, canvasAnchorPoint.position, Quaternion.identity);
            Debug.Log($"[NetworkPlayer] OnNetworkSpawn called for client ID: {OwnerClientId}, IsOwner: {IsOwner}, IsHost: {IsHost}");

            if (IsOwner)
            {
                Debug.Log($"[NetworkPlayer] Spawning stats canvas for owner: {OwnerClientId}");
                
            }

            scoreText = canvasInstance.transform.Find("Canvas/StatsPanel/Score_Value")?.GetComponent<TextMeshPro>();
            caloriesText = canvasInstance.transform.Find("Canvas/StatsPanel/Calories_Value")?.GetComponent<TextMeshPro>();
            streakText = canvasInstance.transform.Find("Canvas/StatsPanel/Streak_Value")?.GetComponent<TextMeshPro>(); 
            heartRateText = canvasInstance.transform.Find("Canvas/StatsPanel/HR_Value")?.GetComponent<TextMeshPro>();
            timerText = canvasInstance.transform.Find("Canvas/StatsPanel/Timer")?.GetComponent<TextMeshPro>();

            var nameText = canvasInstance.transform.Find("Canvas/StatsPanel/PlayerName")?.GetComponent<TextMeshPro>();
            nameText.text = IsHost ? "Host" : "Client"; // Or pull from a custom player name

            UpdateAllUI();
            // TODO - link to a button later
            StartGameTimer();
        }
    }

    private void UpdateAllUI()
    {
        scoreText.text = "0";
        caloriesText.text = "0"; 
        streakText.text = "0";
        heartRateText.text = "0";
    }

    private void HandleOrbHit(NetworkPlayer player)
    {
        if (player != this) return;

        AddScore(1);
        IncreaseStreak();
        SimulateHeartRate();
        SimulateCalories();
    }

    public void AddScore(int points)
    {
        if (IsServer) Score.Value += points;
        else SubmitScoreRequestServerRpc(points);
    }

    [ServerRpc]
    void SubmitScoreRequestServerRpc(int points)
    {
        Score.Value += points;
    }

    private void UpdateScoreUI(int oldVal, int newVal)
    {
        if (IsOwner && scoreText != null)
        {
            scoreText.text = newVal.ToString();
        }
    }

    public void SimulateHeartRate()
    {  
        int hr = Random.Range(80, 120);
        if (IsServer)
        {
            HeartRate.Value = hr;
        }

        else SubmitHeartRateRequestServerRPC(hr);
    }

    [ServerRpc]
    void SubmitHeartRateRequestServerRPC(int hr)
    {
        HeartRate.Value = hr;
    }

    private void UpdateHeartRateUI(int oldVal, int newVal)
    {
        if(IsOwner && heartRateText != null)
        {
            heartRateText.text = newVal.ToString();
        }
    }

    public void SimulateCalories()
    {
        int calories = Random.Range(1, 5);
        if (IsServer)
        {
            Calories.Value += calories;
        }
        else SubmitCaloriesRequestServerRPC(calories);
    }

    [ServerRpc]
    void SubmitCaloriesRequestServerRPC(int cal)
    {
        
        Calories.Value += cal;
    }

    private void UpdateCaloriesUI(int oldVal, int newVal)
    {
        if(IsOwner && caloriesText != null)
        {
            caloriesText.text = newVal.ToString();
        }
    }

    public void IncreaseStreak()
    {
        if (IsServer)
        {
            Streak.Value += 1;
        }
        else SubmitStreakRequestServerRpc(Streak.Value+1);
    }

    public void ResetStreak()
    {
        if (IsServer) 
        {
            Streak.Value = 0;
        }
        else SubmitStreakRequestServerRpc(0);
    }

    [ServerRpc]
    void SubmitStreakRequestServerRpc(int streak)
    {
        Streak.Value = streak;
    }

    private void UpdateStreakUI(int oldVal, int newVal)
    {
        if(IsOwner &&  streakText != null)
        {
            streakText.text = newVal.ToString();
        }
    }

}
