using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleBasic : MonoBehaviour {

    /// <summary>
    /// The text we're showing and rotating
    /// </summary>
    public Text rotatingText;

    //////////////////////////////////////////////////
    // Properties to easily get/set values using SMV
    public float RotationSpeed
    {
        set { SMV.Instance.SetValue(SMVmapping.RotationSpeed, value);  }
        get { return SMV.Instance.GetValueFloat(SMVmapping.RotationSpeed); }
    }

    //For showing elapsed time
    private float startTime;

	void Start () {
        startTime = UnityEngine.Time.time;

        //This will set both the SMVControl (ie value or state) and the SMVView UI element associated with rotation speed
        RotationSpeed = 0.5f;
	}
	
	void Update () {
        //Use rotation speed mapping to get the speed and set rotation position
        float phase = (UnityEngine.Time.time * RotationSpeed % 1);
        rotatingText.transform.rotation = Quaternion.Euler(0, 360 * phase, 0);
    }


}
