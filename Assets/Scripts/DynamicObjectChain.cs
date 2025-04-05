using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using TMPro;

public class DynamicObjectChain : MonoBehaviour
{
    public GameObject tipObject;       // Reference to spawned tip
    public float boxScale = 0.25f;
    public Material horizontalBarMaterial;
    public Material verticalBarMaterial;
    public GameObject datePrefab;
    public GameObject streakIndicatorPreFab;
    
    void Start()
    {
        GenerateChain(5f, 3f, tipObject, true);
    }

    public void GenerateChain(float zLength, float yHeight, GameObject tipObject, bool streak)
    {
        
        GameObject tick = CreateBox(
            position: transform.position,
            scale: new Vector3( 0.7f, .05f, 0.1f),
            name: "tick"
            );
        
        //Create Date Object
        GameObject date = Instantiate(datePrefab, transform.position + new Vector3(0f, 0.05f, 0f), Quaternion.identity);
        // date.GetComponent<TextMeshProUGUI>().text = "04/05/25";

        date.GetComponent<TextMeshPro>().text = "04/05/25";
        
        
        // // Get all components attached to this GameObject
        // Component[] components = date.GetComponents<Component>();
        //
        // // Log each component's name and type
        // foreach (Component component in components)
        // {
        //     Debug.Log($"Component: {component.GetType().Name} | GameObject: {gameObject.name}");
        // }


        // Create first horizontal box
        GameObject horizontalBar = CreateBox(
            position: transform.position + new Vector3(0f, 0f, zLength/2),
            scale: new Vector3(boxScale, boxScale,zLength),
            name: "HorizontalBar"
        );
        
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

        // Calculate tip position
        Vector3 tipPosition = horizontalBarEnd + 
                            Vector3.up * yHeight;


        tipObject = Instantiate(
            tipObject, // Use first prefab in list
            tipPosition,
            Quaternion.identity
        );
        tipObject.name = "ChainTip";

        if (streak)
        {
            Instantiate(streakIndicatorPreFab, tipPosition, Quaternion.identity);
        }
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

    // void DestroyGeneratedObjects()
    // {
    //     if (firstBox) Destroy(firstBox);
    //     if (secondBox) Destroy(secondBox);
    //     if (tipObject) Destroy(tipObject);
    // }


}