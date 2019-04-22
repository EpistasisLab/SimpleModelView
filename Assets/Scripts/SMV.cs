using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

[Serializable]
public class OnUpdateEvent : UnityEvent<SMVmapping> { }

/// <summary>
/// Simple Model-View class.
/// </summary>
public class SMV : MonoBehaviorSingleton<SMV> {

    /// <summary> User should set this to a method in their own code to handle when
    ///  updates to a state happen, either from logic or UI/view side of things. </summary>
    public OnUpdateEvent onUpdateEvent;

    /// <summary> Flag to output some debug info, like the state and view-mapping info after initilization  </summary>
    public bool doDebugLogging = false;

    /// <summary>
    /// Array of SMVstates, each of which holds a value/state and the view(s) to which it's mapped
    /// There will always be one entry for each SMVmapping member.</summary>
    private SMVstate[] stateArray;

	// Use this for initialization instead of Awake in a MonoBehaviorSingleton object
	protected override void Initialize () {
        SetupForScene(true);
	}

    /// <summary> Initialize. Go through the scene and find all SMVviewBase components
    ///  and assign them to their respective SMVstate objects.
    ///  Can be called as needed to reload all the states and mappings if you're making 
    ///  runtime changes to the UI. 
    ///  Pass true to set up a new list and new states - typically this will only
    ///  be done once per scene. Otherwise, states and
    ///  their values are preserved, but scene is still searched to find UI changes.  
    ///  </summary>
    public void SetupForScene(bool initialize = false)
    {

        if( initialize)
            stateArray = new SMVstate[Enum.GetNames(typeof(SMVmapping)).Length];

        for(int i = 0; i < stateArray.Length; i++)
        {
            if (initialize)
            {
                stateArray[i] = new SMVstate();
                stateArray[i].Init((SMVmapping)i);
            }
            else
                stateArray[i].SetupMappings();
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
        stateArray[(int)mapping].SetValue(val);
    }

    /// <summary>
    /// Get the value for the passed mapping. If more than one view is mapped, then
    ///  value is returned from only the first.
    /// </summary>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public float GetValueFloat(SMVmapping mapping)
    {
        return stateArray[(int)mapping].GetValueFloat();
    }
    public int GetValueInt(SMVmapping mapping)
    {
        return stateArray[(int)mapping].GetValueInt();
    }
    public string GetValueString(SMVmapping mapping)
    {
        return stateArray[(int)mapping].GetValueString();
    }
    public bool GetValueBool(SMVmapping mapping)
    {
        return stateArray[(int)mapping].GetValueBool();
    }

    /// <summary> Called from states when their value has changed, either from UI or from call from app logic.
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
        Debug.Log("====== SMV states debug ======");
        foreach (SMVstate state in stateArray)
        {
            state.DebugDump();
        }
    }
}
