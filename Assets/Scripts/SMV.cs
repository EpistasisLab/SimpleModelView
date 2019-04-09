using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// Simple Model-View class.
/// </summary>
public class SMV : MonoBehaviorSingleton<SMV> {

    /// <summary>
    /// Array of SMVstates, each of which holds a value/state and the view(s) to which it's mapped
    /// There will always be one entry for each SMVmapping member.</summary>
    private SMVstate[] stateArray;

	// Use this for initialization instead of Awake in a MonoBehaviorSingleton object
	protected override void Initialize () {
        Init();
	}

    /// <summary> Initialize. Go through the scene and find all SMVviewBase components
    ///  and assign them to their respective SMVstate objects </summary>
    void Init()
    {
        stateArray = new SMVstate[Enum.GetNames(typeof(SMVmapping)).Length];
        for(int i = 0; i < stateArray.Length; i++)
        {
            stateArray[i] = new SMVstate();
            stateArray[i].Init((SMVmapping)i);
        }
            
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
    /// For mappings with 2 or more views, verify that each view holds the same value.
    /// Returns true if yes, otherwise print error message and return false.
    /// </summary>
    /// <param name="mapping"></param>
    /// <returns></returns>
    private bool VerifyEqualValues(SMVmapping mapping)
    {
        /* todo
        object prevVal = null;
        foreach (SMVviewBase view in GetViewsForMapping(mapping))
        {
            object obj = view.GetValueAsObject();
            if( prevVal != null)
                if( prevVal != obj)
                {
                    Debug.LogError(System.Reflection.MethodBase.GetCurrentMethod().Name + "Views don't all have equal value");
                    return false;
                }
            prevVal = obj;
        }
        */
        return true;
    }

    /// <summary>
    /// Get the value for the passed mapping. If more than one view is mapped, then
    ///  value is returned from only the first.
    /// </summary>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public float GetValueFloat(SMVmapping mapping)
    {
        //???
        VerifyEqualValues(mapping); 
        return stateArray[(int)mapping].GetValueFloat();
    }

    /// <summary>
    /// Handle for UI events. All UI elements that include a SMVviewBase component should call this
    /// </summary>
    /// <param name="view"></param>
    public void OnHandleViewUpdate(SMVviewBase view)
    {

    }

    private void Update()
    {
    }

    //public void UpdateMapping

    void DebugDump()
    {
        Debug.Log("SMV states debug:");
        foreach (SMVstate state in stateArray)
        {
            state.DebugDump();
        }
    }
}
