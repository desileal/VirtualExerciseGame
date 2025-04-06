using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DynamicObjectChain : MonoBehaviour
{
    public float boxScale = 0.25f;
    public Material horizontalBarMaterial;
    public Material verticalBarMaterial;
    public GameObject datePrefab;
    public GameObject streakIndicatorPreFab;
    public GameObject barDetailsPrefab;
    

    public GameObject GenerateChain(Vector3 position, float length, float height, GameObject tipObject, bool streak, string dateText)
    {
        float zLength = length * 0.25f;
        float yHeight = height * 0.25f;
        
        GameObject tick = CreateBox(
            position: position,
            scale: new Vector3( 0.7f, .05f, 0.1f),
            name: "tick"
            );
        
        //Create Date Object
        GameObject date = Instantiate(datePrefab, position + new Vector3(0f, 0.05f, 0f), Quaternion.identity);
        // date.GetComponent<TextMeshProUGUI>().text = "04/05/25";
        date.transform.SetParent(tick.transform);

        date.GetComponent<TextMeshPro>().text = dateText;
        
        // Create first horizontal box
        GameObject horizontalBar = CreateBox(
            position: position + new Vector3(0f, 0f, zLength/2),
            scale: new Vector3(boxScale, boxScale,zLength),
            name: "HorizontalBar"
        );
        horizontalBar.transform.SetParent(tick.transform);
        
        horizontalBar.GetComponent<Renderer>().material = horizontalBarMaterial;

        HoverInfoBox horizontalIfoScript = horizontalBar.AddComponent<HoverInfoBox>();
        horizontalIfoScript.boxInfo = $"Action Points: {length * 10}";
        horizontalIfoScript.tooltipPrefab = barDetailsPrefab;
        


        // Calculate end position of first box
        Vector3 horizontalBarEnd = horizontalBar.transform.position + 
                            new Vector3(0f, 0f, zLength/2);
        


        // Create second vertical box
        GameObject VerticalBar = CreateBox(
            position: horizontalBarEnd + new Vector3(0f, yHeight/2, 0f),
            scale: new Vector3(boxScale, yHeight, boxScale),
            name: "VerticalBar"
        );
        VerticalBar.GetComponent<Renderer>().material = verticalBarMaterial;
        VerticalBar.transform.SetParent(tick.transform);
        
        System.TimeSpan timeSpan = System.TimeSpan.FromMinutes(height * 10f);
        string formattedTime = $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
        // Debug.Log($" Icon bar time: {length}, bar time: {formattedTime}");
        
        HoverInfoBox verticalInfoScript = VerticalBar.AddComponent<HoverInfoBox>();
        verticalInfoScript.boxInfo = $"Active Time: {formattedTime}";
        verticalInfoScript.tooltipPrefab = barDetailsPrefab;
        
        // Calculate tip position
        Vector3 tipPosition = horizontalBarEnd + 
                            Vector3.up * yHeight;


        tipObject = Instantiate(
            tipObject, // Use first prefab in list
            tipPosition + new Vector3(0f, 0.4f, 0f),
            Quaternion.identity
        );
        tipObject.name = "ChainTip";
        
        tipObject.transform.SetParent(tick.transform);
        if (streak)
        {
            GameObject srteakIndicator = Instantiate(streakIndicatorPreFab, tipPosition, Quaternion.identity);
            srteakIndicator.transform.SetParent(tick.transform);
        }
        
        return tick;
    }

    
    GameObject CreateBox(Vector3 position, Vector3 scale, string name)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.position = position;
        box.transform.localScale = scale;
        box.name = name;
        box.transform.SetParent(this.transform);
        return box;
    }



}