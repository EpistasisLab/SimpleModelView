using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleComplex : MonoBehaviour {

    //Used by our example code to show some text whose
    // properities we'll change using SMV views and mappings.
    public GameObject rotatingTextPrefab;

    //Array of instantiated rotatingTextPrefab objects
    private GameObject[] rotatingTextObjects;


    //////////////////////////////////////////////////
    // Properties to get/set values using SMV
    // When SMV.Instance.SetValue is called with a new value,
    //  a change-event is generated that can be received by user code
    //  by adding an event handler to the main SMV object in the editor.
    public float RotationSpeed
    {
        set { SMV.Instance.SetValue(SMVmapping.RotationSpeed, value);  }
        get { return SMV.Instance.GetValueFloat(SMVmapping.RotationSpeed); }
    }

    public string RotatingText
    {
        set { SMV.Instance.SetValue(SMVmapping.RotatingText, value); }
        get { return SMV.Instance.GetValueString(SMVmapping.RotatingText); }
    }

    public int RotatingTextCount
    {
        set { SMV.Instance.SetValue(SMVmapping.RotatingTextCount, value); }
        get { return SMV.Instance.GetValueInt(SMVmapping.RotatingTextCount); }
    }

    public float TimeElapsed
    {
        set { SMV.Instance.SetValue(SMVmapping.TimeLabel, value); }
        get { return SMV.Instance.GetValueFloat(SMVmapping.TimeLabel); }
    }

    public bool TimeElapsedDoUpdate
    {
        set { SMV.Instance.SetValue(SMVmapping.TimeLabelUpdate, value); }
        get { return SMV.Instance.GetValueBool(SMVmapping.TimeLabelUpdate);  }
    }

    public int TextColorIndex
    {
        set { SMV.Instance.SetValue(SMVmapping.TextColor, value); }
        get { return SMV.Instance.GetValueInt(SMVmapping.TextColor); }
    }

    //////////////////////////////////////////////////
    // Event handler for SMV OnUpdate events. 
    //
    /// <summary> Optional event handler that gets called whenever an SMV-handled value is changed,
    /// whether via UI element or directly from code via SetValue().
    /// This handler must be assigned once to the event in the SMV object in editor.
    /// </summary>
    public void OnSVMUpdate(SMVmapping mapping)
    {
        //Log when we get an event for demo purposes, but skip the TimeLabel update since they're each frame
        if(mapping != SMVmapping.TimeLabel)
            Debug.Log("*** OnSVMUpdate called with mapping " + mapping.ToString());

        switch ((int)mapping)
        {
            case (int)SMVmapping.RotatingTextCount:
                SetupRotatingTextObjects();
                break;
            case (int)SMVmapping.TextColor:
                UpdateRotatingTextColor();
                break;
            default:
                //ignore this mapping
                break;
        }
    }
    
    //For showing elapsed time
    private float startTime;

	void Start () {
        startTime = UnityEngine.Time.time;
        rotatingTextObjects = new GameObject[0];
        Preset1();
	}
	
	void Update () {
        //Use rotation speed mapping to set rotation position
        float phase = (UnityEngine.Time.time * RotationSpeed % 1);
        float phaseOffset = 0; //just for fun
        foreach (GameObject txt in rotatingTextObjects)
        {
            txt.transform.rotation = Quaternion.Euler(0, 360 * (phase + phaseOffset + 0.2f), 0);
            phaseOffset += 45;
        }

        //Update the time label mapping with current time
        if(TimeElapsedDoUpdate)   
            TimeElapsed = Time.time - startTime;
    }

    //Instantiate the rotating text objects, using the current count value,
    // and have the SMV manager set itself up again.
    private void SetupRotatingTextObjects()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        //destory old ones
        for (int i = 0; i < rotatingTextObjects.Length; i++)
        {
            if (rotatingTextObjects[i] != null)
            {
                //NOTE - Unity docs say not to use DestoryImmediate outside of editor scripts.
                //I'm using here for expediency, and it's working.
                GameObject.DestroyImmediate(rotatingTextObjects[i]);
                rotatingTextObjects[i] = null;
            }
        }

        //new ones
        rotatingTextObjects = new GameObject[RotatingTextCount];
        for(int i=0; i < rotatingTextObjects.Length; i++)
        {
            rotatingTextObjects[i] = Instantiate(rotatingTextPrefab, canvas.transform);
            rotatingTextObjects[i].transform.position = rotatingTextObjects[i].transform.position + new Vector3(i * 100, i * 50, 0);
        }

        UpdateRotatingTextColor();

        //Tell the manager to query the scene and rebuild the list
        // of controls and view mappings. We need to do this since we
        // destroyed the old rotatingTextObjects and made new ones, and
        // each contains an SMVViewText component.
        //Normally this only gets called once automatically from SMV.Start during
        // startup, so only need to call it if at runtime you add/remove UI elements 
        // that contain SMVView components.
        SMV.Instance.SetupForScene();
    }

    //Set the color of text objects using the mapped SMVView
    private void UpdateRotatingTextColor()
    {
        for (int i = 0; i < rotatingTextObjects.Length; i++)
        {
            Color newColor = TextColorIndex == 0 ? Color.red : TextColorIndex == 1 ? Color.magenta : Color.blue;
            rotatingTextObjects[i].GetComponent<Text>().color = newColor;
        }
    }

    public void Preset1()
    {
        LoadPreset(0.5f, "Hello World!", 1, true, 1);
    }

    public void Preset2()
    {
        LoadPreset(1.5f, "Faster!", 5, false, 2);
    }

    private void LoadPreset(float rotSpeed, string rotText, int rotTextCount, bool showTimeUpdate, int colorIndex)
    {
        RotationSpeed = rotSpeed;
        RotatingText = rotText;
        RotatingTextCount = rotTextCount;
        TimeElapsedDoUpdate = showTimeUpdate;
        TextColorIndex = colorIndex;
    }
}
