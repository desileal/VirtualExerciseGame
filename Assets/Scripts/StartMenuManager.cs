using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] GameObject connectionIndicator;
    [SerializeField] GameObject lobbyXRRig;
    public void StartHost()
    {
        
        var netMan = NetworkManager.Singleton;
        if (netMan != null && !netMan.IsServer && !netMan.IsClient)
        {
            NetworkManager.Singleton.StartHost();
            if (lobbyXRRig != null) lobbyXRRig.SetActive(false);
            //NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    public void StartClient()
    {
        var netMan = NetworkManager.Singleton;

        if (netMan != null && !netMan.IsServer && !netMan.IsHost)
        {
            
            NetworkManager.Singleton.StartClient();
            if (lobbyXRRig != null) lobbyXRRig.SetActive(false);
            connectionIndicator.SetActive(true);
        }
        
    }

    public void ViewStats()
    {
        SceneManager.LoadScene("StatsScene");
    }
}
