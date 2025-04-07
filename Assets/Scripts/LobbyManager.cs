using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
{
    public GameObject lobbyCanvas;
    public Button startButton;
    public Button endButton;
    public TextMeshPro gameStatusText;
    public TextMeshPro joinStatusText;

    public GameObject hostSpawnPointManager;
    public GameObject ClientSpawnPointManager;


    private bool gameStarted = false;

    public void InitializeLobby()
    {
        // Show lobby UI only at start
        lobbyCanvas.SetActive(true);

        // show join status
        joinStatusText.text = NetworkManager.Singleton.IsHost ? "You are the Host" : "You are a Client";

        // Show start button only if host
        startButton.gameObject.SetActive(NetworkManager.Singleton.IsHost);
        

        // Optional quit logic
        endButton.onClick.AddListener(QuitGame);

        gameStatusText.text = "Waiting for Host to Start...";

        StartGame();
    }

    // TODO 
    // Call EndGameClientRpc() to show LobbyCanvas with final message
    // Show the QuitButton
    // When clicked, it runs QuitGame() as above
    [ClientRpc]
    public void EndGameClientRpc()
    {
        lobbyCanvas.SetActive(true);
        gameStatusText.text = "Game Over � Great work!";
        startButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
    }

    private void QuitGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            // Host loads scene for everyone
            NetworkManager.Singleton.SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        }
        else
        {
            // Client disconnects and loads locally
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("StartScene");
        }
    }

    public void StartGame()
    {
        Debug.Log("start button pressed");
        if (NetworkManager.Singleton.IsHost && !gameStarted)
        {
            gameStarted = true;
            gameStatusText.text = "Starting game...";
            StartGameClientRpc();
        }
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        lobbyCanvas.SetActive(false); // hide lobby UI for players
        gameStarted = true;

        foreach (var player in FindObjectsByType<NetworkPlayer>(FindObjectsSortMode.None))
        {
            if (player.IsOwner)
            {
                player.StartGameTimer();
                if (player.IsHost)
                {
                    AssignSpawnPointManager(player, hostSpawnPointManager);

                }
                else
                {

                    AssignSpawnPointManager(player, ClientSpawnPointManager);
                }
                
            }
        }
    }

    private void AssignSpawnPointManager(NetworkPlayer player, GameObject spawnManager)
    {
        spawnManager.transform.SetParent(player.transform);
        spawnManager.transform.position = player.transform.position;
        player.GetComponentInChildren<SpawnPointManager>().StartSpawning(player);
    }
}
