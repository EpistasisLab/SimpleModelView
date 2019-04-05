using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public Text textElem;
    /*public float RotSpeed
    {
        set;
        get;
    }*/
    private float scale;


	// Use this for initialization
	void Start () {
        //RotSpeed = 0.25f;
        SMV.Instance.SetValue(SMVmapping.RotationSpeed, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        float phase = (Time.time * SMV.Instance.GetValueFloat(SMVmapping.RotationSpeed)) % 1;
        textElem.transform.rotation = Quaternion.Euler(0, 360 * phase, 0);
	}

    void Refresh()
    {

    }

}
