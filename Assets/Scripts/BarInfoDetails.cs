using UnityEngine;

public class BarInfoDetails : MonoBehaviour
{
    
    [SerializeField] private TMPro.TMP_Text barDetailsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetBarDetails(string barDetails)
    {
        barDetailsText.text = barDetails;
    }
}
