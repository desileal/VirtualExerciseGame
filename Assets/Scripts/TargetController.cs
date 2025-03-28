using UnityEngine;
using XRMultiplayer.MiniGames;

public class TargetController : MonoBehaviour
{
    // TODO - drag in game manager from unity
    public GameManager gameManager;
    private Vector3 originPosition;
    private Rigidbody rb;

    private void Awake()
    {
        originPosition = this.transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // TODO - call this method in unity once the target has been hit
    public void DelayResetTarget()
    {
        // delays resetting the target bubble for three seconds
        // TODO - make the number a variable that updates as the time goes on and makes the level more challenging
        Invoke(nameof(ResetTarget), 3f);
    }


    public void ResetTarget()
    {
        this.transform.position = originPosition;
    }

    // increase score
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target")) {
            // increase player score
            // TODO - set tag for the target bubbles
        }
        gameManager.LocalPlayerIncreaseScore(1);
    }
}
