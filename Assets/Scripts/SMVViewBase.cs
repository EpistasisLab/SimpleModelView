using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Part of Simple Model-View system (SMV).
/// Attach as component to UI elements that are to be mapped using SMV.
/// </summary>
public abstract class SMVviewBase : MonoBehaviour {

    public enum SMVtypeEnum { undefined, toggle, slider, text, inputfield };

    /// <summary> Internal tracking of type of view/UI element.
    /// Not sure what we'll need this for, other than debugging. </summary>
    protected SMVtypeEnum smvtype;
    public SMVtypeEnum SMVType { get { return smvtype; } }

    /// <summary> The data type this view uses. i.e. the data type used for setting and returned by getting value</summary>
    protected System.Type dataType;
    public System.Type DataType { get { return dataType; } }

    /// <summary>
    /// True for views that are textual in nature (Text, InputField), and thus can be used to hold
    /// string, float or int values. </summary>
    public bool IsTextual { get { return SMVType == SMVtypeEnum.inputfield; } }

    /// <summary> The name of the mapping we're handling with this view </summary>
    public SMVmapping mapping;

    /// <summary> The UI element we're controlling/reading for this view element. Gets assigned automatically. </summary>
    private UIBehaviour uiElement;
    public UIBehaviour UIelement { protected set { uiElement = value; } get { return uiElement; } }

    /// <summary> The SMVstate that this view belongs too </summary>
    protected SMVstate parent;

    // Use this for initialization
    void Start() {

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

    /// <summary> Set up everything that's specific to this SMVview type.
    /// This is called during init of the main SMV object. </summary>
    public abstract void Init(SMVstate parent);

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

    /// <summary>
    /// Single setvalue method takes an object and does type-checking.
    /// </summary>
    /// <param name="val">param of type that matches the view you're mapped to</param>
    public void SetValue(object val)
    {
        if (!ValidateAndParse(ref val))
            return;

        //If it's a new value, then set update the view. We do this check
        // to avoid updating when not needed, in case the UI event triggers some event
        // in response that leads to a loop (really should know from the logic if this is possible, but leave it for now).
        if( val != GetValueAsObject())
            SetValueInternal(val);
    }

    protected abstract void SetValueInternal(object val);

    /// <summary>
    /// Checked the passed value to see that its data type matches that of the view's data type.
    /// For textual-class views, if we get a string for a non-string textual-class view, attempt to parse it into the appropriate
    /// type. On success, change val to the parsed value.
    /// </summary>
    /// <param name="val">The object to validate for type. May hold new object on return for textual-class views</param>
    /// <returns>True on success. False if non-matching types or string could not be parsed to appropriate type for textual-class views.</returns>
    private bool ValidateAndParse(ref object val)
    {
        //If it's a simple Text element, no need to validate since it's display-only, i.e. non-editable.
        //Any input will just be turned to string
        if (SMVType == SMVtypeEnum.text)
            return true;

        if ( IsTextual )
        {
            //Textual views (InputField) can be set up to have different
            // data types. Make sure the type matches here, and if not and it's 
            // a string, try to parse it into proper type.
            if (val.GetType() == this.DataType)
                return true;
            //If we have a string, see if it can be parsed into the right type
            if (val.GetType() == typeof(string))
            {
                if(val.GetType() == typeof(float))
                {
                    float newVal;
                    if( float.TryParse(val.ToString(), out newVal ))
                    {
                        val = newVal;
                        return true;
                    }
                }
                else
                if (val.GetType() == typeof(int))
                {
                    int newVal;
                    if (int.TryParse(val.ToString(), out newVal))
                    {
                        val = newVal;
                        return true;
                    }
                }
                //else...
                //Parsing/conversion failed
                //Fall through to below and report the error
            }
        }

        if (val.GetType() != this.DataType)
        {
            Debug.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name + ": input with value " + val.ToString() + ", and type of " + val.GetType().Name + ", doesn't match or can't be converted to the view's type of " + DataType.Name + " for view " + SMVType.ToString() + " in game object " + UIelement.transform.parent.name);
            return false;
        }

        return true;
    }

    //TODO - better error handling. And return null instead of 0 or "" ?
    //
    /*
    public virtual float GetValueFloat()
    {
        object val = GetValueAsObject();
        if( ValidateAndParse(ref val))
        {
            return (float)val;
        }
        return 0;
    }
    public virtual int GetValueInt()
    {
        object val = GetValueAsObject();
        if (ValidateAndParse(ref val))
        {
            return (int)val;
        }
        return 0;
    }
    public virtual string GetValueString()
    {
        object val = GetValueAsObject();
        if (ValidateAndParse(ref val))
        {
            return (string)val;
        }
        return "";
    }
    */
    //Derived classes must define
    public abstract object GetValueAsObject();

    /// <summary> Listener for all derived classes when value changes via UI.
    /// It validates the value and updates the parent SMVstate </summary>
    public void OnValueChangedListener()
    {
        //Debug.Log("OnValueChangedListener called");
        object val = GetValueAsObject();
        if( ValidateAndParse(ref val))
        {
            //Value is valid, and if it's a string it has been parsed into numeric type if appropriate
            //Update the parent and it will update any other views with same mapping
            parent.SetValue(val);
        }
        else
        {
            //It failed, likely because it's a string type and didn't parse into the proper
            // numeric type, so revert the value in UI and ignore
            SetValue(parent.GetValueAsObject());
        }
    }

}


