using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An SVMview for a Slider element
/// </summary>
public class SMVViewSlider : SMVviewBase
{

    public override void Init()
    {
        //Find the UI element within this components game object
        UIelement = transform.GetComponent<Slider>();
        ((Slider)UIelement).onValueChanged.AddListener(delegate { OnValueChangedTest(); });
        if (UIelement == null)
            Debug.LogError("uiElement == null");

        smvtype = SMVtypeEnum.slider;
        dataType = typeof(float);
    }

    public void OnValueChangedTest()
    {
        Debug.Log("OnValueChangedTest");
    }

    public override object GetValueAsObject()
    {
        return ((Slider)UIelement).value;
    }

    protected override void SetValueInternal(object val)
    {
        ((Slider)UIelement).value = (float) val;
    }
}