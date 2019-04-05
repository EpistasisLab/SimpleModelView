using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Part of Simple Model-View system (SMV).
/// Attach as component to UI elements that are to be mapped using SMV.
/// </summary>
public class SMVViewORIG : MonoBehaviour {

    public enum SMVtype { undefined, toggle, slider, text, inputfield };

    /// <summary> The name of the mapping we're handling with this view </summary>
    public SMVmapping mapping;

    /// <summary> The UI element we're controlling/reading for this view element. Gets assigned automatically. </summary>
    private UIBehaviour uiElement;
    public UIBehaviour UIelement { get { return uiElement; } }

    /// <summary> Internal tracking of type of view/UI element </summary>
    private SMVtype type;
    public SMVtype Type {  get { return type; } }

    // Use this for initialization
    void Awake() {
        FindUIelement();
        if (uiElement == null)
            Debug.LogError("uiElement == null");
        type = GetSMVtype();

        //testing
        /*
        Debug.Log("----");
        float f = 344.5f;
        PrintVal(123);
        PrintVal(f);
        PrintVal("a string");
        Debug.Log("PrintVal from GetValue ");
        PrintVal(GetValue());
        Debug.Log("cast GetValue() to float: ");
        float ret = (float)GetValueUntyped();
        */
    }

    //testing
    void PrintVal(object o)
    {
        Debug.Log("PrintVal type: " + o.GetType().Name + " value: " + o);
    }

    /// <summary>
    /// Generic SetValue function for this view element.
    /// If the passed value is the same as the current value of the view element,
    ///  nothing happens - this is done to avoid possible re-trigger of change-events (if that even happens with setting value directly, I don't know).
    /// </summary>
    /// <param name="obj"></param>

    public void SetValue(object obj)
    {
        switch (type)
        {
            case SMVtype.slider:
                if (obj.GetType() == typeof(float))
                {
                    if (GetValueSlider() == (float)obj)
                        return;
                    ((Slider)uiElement).value = (float)obj;
                    return;
                }
                else
                    Debug.LogError("SetValue: expected type float, got type " + obj.GetType().Name);
                break;
            case SMVtype.text:
                if (GetValueText() == obj.ToString())
                    return;
                ((Text)uiElement).text = obj.ToString();
                break;
            case SMVtype.inputfield:
                if (GetValueInputField() == obj.ToString())
                    return;
                ((InputField)uiElement).text = obj.ToString();
                break;
            case SMVtype.toggle:
                if (obj.GetType() == typeof(bool))
                {
                    if (GetValueToggle() == (bool)obj)
                        return;
                    ((Toggle)uiElement).isOn = (bool)obj;
                    return;
                }
                else
                    Debug.LogError("SetValue: expected type bool, got type " + obj.GetType().Name);
                break;
            default:
                Debug.LogError("SetValue: mismatched type " + obj.GetType().Name + " for SMVtype " + type.ToString());
                break;
        }

    }
    
    /// <summary> Public GetValue, returns an object. Callers responsibility to cast result to desired type. </summary>
    /// <returns></returns>
    public object GetValueAsObject()
    {
        switch (type)
        {
            case SMVtype.slider:
                return GetValueSlider();
            case SMVtype.text:
                return GetValueText();
            case SMVtype.inputfield:
                return GetValueInputField();
            case SMVtype.toggle:
                return GetValueToggle();
            default:
                Debug.LogError("GetValue: unrecognized SMVtype: " + type.ToString());
                return null;
        }

    }

    private float GetValueSlider()
    {
        return ((Slider)uiElement).value;
    }

    private string GetValueText()
    {
        return ((Text)uiElement).text;
    }

    private string GetValueInputField()
    {
        return ((InputField)uiElement).text;
    }

    private bool GetValueToggle()
    {
        return ((Toggle)uiElement).isOn;
    }



    /*
    public void SetValue(float val)
    {
        switch (type)
        {
            case SMVtype.slider:
                ((Slider)uiElement).value = val; break;
            case SMVtype.text:
                ((Text)uiElement).text = val.ToString(); break;
            case SMVtype.inputfield:
                ((InputField)uiElement).text = val.ToString(); break;
            default:
                Debug.LogError("mismatched type " + val.GetType().Name + " for SMVtype " + type.ToString()); break;
        }
    }

    public void SetValue(int val)
    {
        switch (type)
        {
            case SMVtype.text:
                ((Text)uiElement).text = val.ToString(); break;
            case SMVtype.inputfield:
                ((InputField)uiElement).text = val.ToString(); break;
            default:
                Debug.LogError("mismatched type " + val.GetType().Name + " for SMVtype " + type.ToString()); break;
        }
    }

    public void SetValue(string val)
    {
        switch (type)
        {
            case SMVtype.text:
                ((Text)uiElement).text = val; break;
            case SMVtype.inputfield:
                ((InputField)uiElement).text = val; break;
            default:
                Debug.LogError("mismatched type " + val.GetType().Name + " for SMVtype " + type.ToString()); break;
        }
    }
    */

    /// <summary> Automatically find an appropriate UI element in the game object this component is a part of </summary>
    void FindUIelement()
    {
        string[] types = { "Toggle", "Slider", "Text" };
        foreach(string t in types)
        {
            if (transform.GetComponent(t) != null)
            {
                uiElement = transform.GetComponent(t) as UIBehaviour;
                break;
            }
        }
    }

    SMVtype GetSMVtype()
    {
        if( uiElement.GetType() == typeof(Slider))
        {
            return SMVtype.slider;
        }
        if (uiElement.GetType() == typeof(Toggle))
        {
            return SMVtype.toggle;
        }
        if (uiElement.GetType() == typeof(Text))
        {
            return SMVtype.text;
        }
        if (uiElement.GetType() == typeof(InputField))
        {
            return SMVtype.inputfield;
        }
        Debug.LogError("returning undefined SMVtype");
        return SMVtype.undefined;
    }
}
