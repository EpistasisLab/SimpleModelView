using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SMView;

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

	void Start () {
        //This will set both the SMVControl (ie state value) and the SMVView UI element associated with RotationSpeed mapping.
        RotationSpeed = 0.5f;
	}
	
	void Update () {
        //Use rotation speed mapping to get the speed and set rotation position
        float phase = (UnityEngine.Time.time * RotationSpeed % 1);
        rotatingText.transform.rotation = Quaternion.Euler(0, 360 * phase, 0);
    }


}
