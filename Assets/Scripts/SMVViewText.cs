using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// An SVMview for a Text element. It's display-only, can't be edited, so simpler than others.
/// </summary>
public class SMVViewText : SMVviewBase
{

    public override void Init(SMVstate parent)
    {
        //Find the UI element within this components game object
        UIelement = transform.GetComponent<Text>();
        if (UIelement == null)
            Debug.LogError("uiElement == null");

        smvtype = SMVtypeEnum.text;
        dataType = typeof(string);
        this.parent = parent;
    }

    public override object GetValueAsObject()
    {
        return ((Text)UIelement).text;
    }

    protected override void SetValueInternal(object val)
    {
        ((Text)UIelement).text = val.ToString();
    }
}