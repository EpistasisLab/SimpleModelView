using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleMain : MonoBehaviour {

    public GameObject rotatingTextPrefab;

    private GameObject[] rotatingTextObjects;


    //////////////////////////////////////////////////
    // Properties to get/set values using SMV
    //
    public float RotSpeed
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

    //////////////////////////////////////////////////
    // Event handler for SMV OnUpdate events. 
    //
    /// <summary> Event handler that gets called whenever an SMV-handled value is changed,
    /// whether via UI element or directly from code via SetValue().
    /// This handler must be assigned once to the event in the SMV object in editor.
    /// </summary>
    public void OnSVMUpdate(SMVmapping mapping)
    {
        if(mapping != SMVmapping.TimeLabel)
            Debug.Log("*** OnSVMUpdate called with mapping " + mapping.ToString());

        switch ((int)mapping)
        {
            case (int)SMVmapping.RotatingTextCount:
                SetupRotatingTextObjects();
           
                break;
            default:
                break;
        }
    }
    
    private float startTime;

	// Use this for initialization
	void Start () {
        startTime = UnityEngine.Time.time;
        rotatingTextObjects = new GameObject[0];
        Preset1();
	}
	
	// Update is called once per frame
	void Update () {
        //Use rotation speed mapping to set rotation position
        float phase = (UnityEngine.Time.time * SMV.Instance.GetValueFloat(SMVmapping.RotationSpeed)) % 1;
        foreach (GameObject txt in rotatingTextObjects)
        {
            txt.transform.rotation = Quaternion.Euler(0, 360 * (phase + 0.2f), 0);
        }

        //Update the time label mapping with current time
        TimeElapsed = Time.time - startTime;
    }

    //Instantiate the rotating text objects, using the current count value,
    // and have the SMV manager set itself up again.
    public void SetupRotatingTextObjects()
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

            rotatingTextObjects[i].transform.position = rotatingTextObjects[i].transform.position + new Vector3(i * 100, i * 100, i * 100);
        }

        //Tell the manager to query the scene and rebuild the list
        // of states and view mappings.
        SMV.Instance.SetupForScene();
    }

    public void Preset1()
    {
        LoadPreset(0.5f, "Hello World!", 1);
    }

    public void Preset2()
    {
        LoadPreset(1.5f, "Faster Faster Faster!", 5);
    }

    private void LoadPreset(float rotSpeed, string rotText, int rotTextCount)
    {
        RotSpeed = rotSpeed;
        RotatingText = rotText;
        RotatingTextCount = rotTextCount;
    }
}
