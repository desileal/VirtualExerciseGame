using TMPro;
using UnityEngine;
public class StatInfo : MonoBehaviour
{
    public LogEntry entryDetails;
    public string iconInfo;
    public TMP_Text infoText;
    public GameObject UIPanel;

    public void UpdateIconInfo(string info)
    {
        iconInfo = info;
    }

    public void UpdatePanelInfo()
    {
        System.TimeSpan timeSpan = System.TimeSpan.FromMinutes(entryDetails.activeTime);
        string formattedTime = $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        // Debug.Log($" Icon raw time: {entryDetails.activeTime}, Icon time: {formattedTime}");

        infoText.text = 
            $"{entryDetails.date}" +
            "\n\n" +
            $"{(entryDetails.streak ? "<color=#00FF00>You are on a streak!</color>" : "")}" +
            "\n\n" +
            $"You have burned <color=#FF0000>{entryDetails.caloriesBurnt}</color> calories!" +
            "\n\n" +
            $"That is equivalent to <color=#FFFF00>{iconInfo}</color>!" +
            "\n\n" +
            $"You have worked out for <color=#00FFFF>{formattedTime}</color> minutes!" +
            "\n\n" +
            $"You have acquired <color=#FFA500>{entryDetails.activePoints}</color> Points";
    }

    public void ShowUI()
    {
        UIPanel.SetActive(true);
    }

    public void HideUI()
    {
        UIPanel.SetActive(false);
    }
}
