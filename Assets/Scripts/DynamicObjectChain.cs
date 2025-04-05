using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using TMPro;

public class DynamicObjectChain : MonoBehaviour
{
    public float boxScale = 0.25f;
    public Material horizontalBarMaterial;
    public Material verticalBarMaterial;
    public GameObject datePrefab;
    public GameObject streakIndicatorPreFab;
    
    void Start()
    {
        // GenerateChain(new Vector3(1f, 0f, 5f),5f, 3f, tipObject, true);
    }

    public GameObject GenerateChain(Vector3 position, float zLength, float yHeight, GameObject tipObject, bool streak, string dateText)
    {
        
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

        // Calculate end position of first box
        Vector3 horizontalBarEnd = horizontalBar.transform.position + 
                            new Vector3(0f, 0f, zLength/2);
        
        Debug.Log(horizontalBarEnd);


        // Create second vertical box
        GameObject VerticalBar = CreateBox(
            position: horizontalBarEnd + new Vector3(0f, yHeight/2, 0f),
            scale: new Vector3(boxScale, yHeight, boxScale),
            name: "VerticalBar"
        );
        VerticalBar.GetComponent<Renderer>().material = verticalBarMaterial;
        VerticalBar.transform.SetParent(tick.transform);
        
        // Calculate tip position
        Vector3 tipPosition = horizontalBarEnd + 
                            Vector3.up * yHeight;


        tipObject = Instantiate(
            tipObject, // Use first prefab in list
            tipPosition,
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