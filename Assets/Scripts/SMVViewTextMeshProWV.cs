using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// An SVMview for a world-view TextMeshPro (for world-space 3D text, different than TextMeshProUGUI) element. It's display-only, can't be edited, so simpler than others.
/// </summary>
public class SMVViewTextMeshProWV : SMVviewBase
{

    /// <summary> Number of decimal places to show when showing a float value </summary>
    public int decimalPlaces = 2;

    /// <summary> A string prefix added to begin of whatever string is assigned to this </summary>
    public string prefix = "";

    /// <summary> A string postfix added to end of whatever string is assigned to this </summary>
    public string postfix = "";


    protected override void InitDerived()
    {
        //Find the UI element within this components game object
        UIelement = transform.GetComponent<TextMeshPro>();
        if (UIelement == null)
            Debug.LogError(this.GetType().Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name + ": uiElement == null");

        smvtype = SMVtypeEnum.textMeshProWorldSpace;
        dataType = typeof(string);
    }

    public override object GetValueAsObject()
    {
        return ((TextMeshPro)UIelement).text;
    }

    protected override void SetValueInternal(object val)
    {
        string txt;
        if (val.GetType() == typeof(float))
            txt = ((float)val).ToString("F"+decimalPlaces.ToString());
        else
            txt = val.ToString();
        ((TextMeshPro)UIelement).text = prefix + txt + postfix;
    }
}