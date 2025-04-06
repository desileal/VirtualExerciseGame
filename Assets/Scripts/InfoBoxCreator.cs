using UnityEngine;


public class InfoBoxCreator : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI infoText;
    [SerializeField] GameObject infoCanvas;
    [SerializeField] StatInfo info;

    public void UpdateInfoText()
    {
        infoText.text = info.entryDetails.date;
        infoText.text += $"You have burned {info.entryDetails.caloriesBurnt} calories!";
        infoText.text += $"That is equivelant to {info.iconInfo}!";
        infoText.text += $"\n";
        infoText.text += $"You have workedout for {info.entryDetails.activeTime / 60f} minutes! ";
        infoText.text += $"You have aquired {info.entryDetails.activePoints} Points";
    }
}
