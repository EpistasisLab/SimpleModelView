using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Class the holds the value/state for a model-view mapping, and manages
///  the one or more views that are assigned to it. </summary>
public class SMVstate {

    /// <summary> The mapping that this value is tied to </summary>
    private SMVmapping mapping;
    public SMVmapping Mapping { get { return mapping; } }

    /// <summary> The views that are associated with the mapping </summary>
    List<SMVviewBase> views;

    /// <summary> Get the number of views mapped to this state </summary>
    public int Count
    {
        get { return views.Count; }
    }

    /// <summary> The data type this link uses. i.e. the data type used for setting and returned by getting value.
    /// All views that this link contains, i.e. all view that use the same mapping, must use same data type as well.</summary>
    private System.Type dataType;
    public System.Type DataType { get { return dataType; } }

    /// <summary> The current value </summary>
    private object value;

	// ctor
	public SMVstate()
    {
        views = new List<SMVviewBase>();
        this.mapping = SMVmapping.undefined;
        value = null;
    }

    /// <summary> Initialize a new instance. This will reset value to default. </summary>
    /// <param name="mapping"></param>
    public void Init(SMVmapping mapping)
    {
        this.mapping = mapping;
        SetupMappings();
        value = SMV.Instance.GetDefault(DataType);
    }

    /// <summary>
    /// Search for and setup mappings to views. Does not change the
    /// current value, so can be called when UI changes but you want
    /// to preserve value.
    /// </summary>
    public void SetupMappings()
    {
        views = new List<SMVviewBase>();
        bool typeIsSet = false;
        bool foundText = false;

        SMVviewBase[] allViews = Resources.FindObjectsOfTypeAll<SMVviewBase>();
        foreach (SMVviewBase view in allViews)
        {
            if(view.mapping == mapping)
            {
                view.Init(this);
                //Special handling for Text elements, which are always string.
                //Keep track and if no non-Text elements are found, then state type will be string.
                //Ugly.
                if(view.SMVType == SMVviewBase.SMVtypeEnum.text)
                {
                    foundText = true;
                }
                else
                {
                    if (!typeIsSet)
                    {
                        dataType = view.DataType;
                        typeIsSet = true;
                    }
                    //If a 2nd or later mapping is of a different type, show an error and skip it,
                    // EXCEPT for Text type (noted and skipped above) which is just for display so works with any type
                    if (view.DataType != dataType)
                    {
                        Debug.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name + ": tried adding view with type of " + view.DataType.Name + ", but doesn't match this link's type of " + dataType.Name + ", for view " + view.SMVType.ToString() + " from game object " + view.UIelement.transform.parent.name + ". Skipping.");
                        continue;
                    }
                }
                //Add this view to the list
                views.Add(view);
            }
        }
        //special handling when there's only Text elements
        if (!typeIsSet && foundText)
            dataType = typeof(string);
    }

    public void SetValue(object val)
    {
        if( Count == 0)
        {
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + ": no views mapped for this state.");
            return;
        }

        if (val.GetType() != this.DataType)
        {
            Debug.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name + ": input with value " + val.ToString() + ", and type of " + val.GetType().Name + ", doesn't match this state's type of " + DataType.Name + ", for mapping " + mapping.ToString() +". Skipping setting of value.");
            return;
        }

        //Store the actual value
        value = val;
        
        //Update each UI element
        foreach( SMVviewBase view in views)
        {
            view.SetValue(val);
        }

        //Invoke event for the optional update handler
        SMV.Instance.MappingUpdated(mapping);
    }

    /// <summary> Return the current value as generic object type </summary>
    public object GetValueAsObject()
    {
        return value;
    }

    public float GetValueFloat()
    {
        if (Count == 0)
        {
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + ": no views mapped for this state.");
            return 0f;
        }

        float test = 0f;
        if (ValidateDataType(test))
        {
            return (float)value;
        }
        else
            return (float)SMV.Instance.GetDefault(DataType);
    }

    public int GetValueInt()
    {
        if (Count == 0)
        {
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + ": no views mapped for this state.");
            return 0;
        }

        int test = 0;
        if (ValidateDataType(test))
        {
            return (int)value;
        }
        else
            return (int)SMV.Instance.GetDefault(DataType);
    }

    public string GetValueString()
    {
        if (Count == 0)
        {
            Debug.LogWarning(System.Reflection.MethodBase.GetCurrentMethod().Name + ": no views mapped for this state.");
            return "";
        }

        string test = "";
        if (ValidateDataType(test))
        {
            return (string)value;
        }
        else
            return (string)SMV.Instance.GetDefault(DataType);
    }

    private bool ValidateDataType(object val)
    {
        if (val.GetType() != this.DataType)
        {
            Debug.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name + ": request for value of type " + val.GetType().Name + " doesn't match this link's type of " + DataType.Name + ". Returning default value.");
            return false;
        }
        return true;
    }

    public void DebugDump()
    {
        Debug.Log("--------- State dump: mapping: " + this.mapping.ToString() + " dataType: " + this.DataType + " and views: ");
        if (Count == 0)
        {
            Debug.Log("No views mapped");
            return;
        }

        foreach ( SMVviewBase view in views)
        {
            Debug.Log("  " + view.mapping.ToString() + " mapped to SMVtype " + view.SMVType.ToString() + ", with behavior: " + view.UIelement.GetType() + " in gameobject: " + view.gameObject.name + "\n");
        }
    }
}
