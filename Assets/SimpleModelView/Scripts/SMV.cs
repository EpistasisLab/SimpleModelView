using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

namespace SMView
{

[Serializable]
public class OnUpdateEvent : UnityEvent<SMVmapping> { }

/// <summary>
/// Main class, a singleton, for simple Model-View-* type behavior.
/// The main purpose is to make interaction with UI elements simpler and centralized.
/// The user defines 'mappings' that are used to connect model state variables with UI elements
/// and event callbacks.
/// Add an instance of this component to your scene and it will initialize on start in the Initialize() method.
/// </summary>
public class SMV : MonoBehaviorSingleton<SMV> {

    /// <summary> User should set this to a method in their own code to handle when
    ///  updates to a control happen, either from logic or UI/view side of things. </summary>
    public OnUpdateEvent onUpdateEvent;

    /// <summary> Flag to output some debug info, like the control and view-mapping info after initilization  </summary>
    public bool doDebugLogging = false;

    /// <summary>
    /// Array of SMVcontrol, each of which holds a value/state and the view(s) to which it's mapped
    /// There will always be one entry for each SMVmapping member.</summary>
    private SMVcontrol[] controlArray;

	// Use this for initialization instead of Awake in a MonoBehaviorSingleton object
	protected override void Initialize () {
        SetupForScene(true);
	}

    /// <summary> Initialize. Go through the scene and find all SMVviewBase components
    ///  and assign them to their respective SMVcontrol objects.
    ///  Can be called as needed to reload all the controlss and mappings if you're making 
    ///  runtime changes to the UI. 
    ///  Pass true to set up a new list and new controls - typically this will only
    ///  be done once per scene. Otherwise, controls and
    ///  their values are preserved, but scene is still searched to find UI changes.  
    ///  </summary>
    public void SetupForScene(bool initialize = false)
    {

        if( initialize)
            controlArray = new SMVcontrol[Enum.GetNames(typeof(SMVmapping)).Length];

        for(int i = 0; i < controlArray.Length; i++)
        {
            if (initialize)
            {
                controlArray[i] = new SMVcontrol();
                controlArray[i].Init((SMVmapping)i);
            }
            else
                controlArray[i].SetupMappings();
        }
        
        if(doDebugLogging)
            DebugDump();
    }

    /// <summary>
    /// Assign a value for the mapping. 
    /// User must pass value of appropriate type for the mapping. 
    /// If value is the wrong type, and error gets printed and view is not changed.
    /// </summary>
    /// <param name="mapping"></param>
    /// <param name="val"></param>
    public void SetValue(SMVmapping mapping, object val)
    {
        controlArray[(int)mapping].SetValue(val);
    }

    /// <summary>
    /// Get the value for the passed mapping. If more than one view is mapped, then
    ///  value is returned from only the first.
    /// </summary>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public float GetValueFloat(SMVmapping mapping)
    {
        return controlArray[(int)mapping].GetValueFloat();
    }
    public int GetValueInt(SMVmapping mapping)
    {
        return controlArray[(int)mapping].GetValueInt();
    }
    public string GetValueString(SMVmapping mapping)
    {
        return controlArray[(int)mapping].GetValueString();
    }
    public bool GetValueBool(SMVmapping mapping)
    {
        return controlArray[(int)mapping].GetValueBool();
    }

    /// <summary> Called from controls when their value has changed, either from UI or from call from app logic.
    /// Generate an event that's picked up by user's code. </summary>
    /// <param name="mapping"></param>
    public void MappingValueUpdated(SMVmapping mapping)
    {
        onUpdateEvent.Invoke(mapping);
    }

    /// <summary>
    /// Get an object of type with default value.
    /// Returns null for non-value-types </summary>
    public object GetDefault(Type type)
    {
        if (type == null)
            return null;

        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }

    public void DebugDump()
    {
        Debug.Log("====== SMVcontrols dump ======");
        foreach (SMVcontrol control in controlArray)
        {
            control.DebugDump();
        }
    }
}

}//namespace
