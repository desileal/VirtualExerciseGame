using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void ViewStats()
    {
        SceneManager.LoadScene("StatsScene");
    }
}
