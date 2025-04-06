using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HoverInfoBox : MonoBehaviour
{
    public string boxInfo; // info you want to display
    public GameObject tooltipPrefab;

    private GameObject currentTooltip;
    private XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = gameObject.AddComponent<XRSimpleInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (tooltipPrefab != null && currentTooltip == null)
        {
            currentTooltip = Instantiate(tooltipPrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity);
            currentTooltip.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = boxInfo;
            currentTooltip.transform.SetParent(transform); // optional, keep it stuck to the box
            currentTooltip.SetActive(true);
        }
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
        }
    }

    void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
    }
}