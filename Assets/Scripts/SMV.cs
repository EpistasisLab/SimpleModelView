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
    /// Dictionary of mappings and the view(s) they are mapped to, where
    /// 'view' is a componenet of a UI element
    /// </summary>
    private Dictionary<SMVmapping, List<SMVviewBase>> viewDict;

	// Use this for initialization instead of Awake in a MonoBehaviorSingleton object
	protected override void Initialize () {
        Init();
	}

    /// <summary> Initialize. Go through the scene and find all SMVviewBase components
    ///  and track them. </summary>
    void Init()
    {
        viewDict = new Dictionary<SMVmapping, List<SMVviewBase>>();
        SMVviewBase[] allViews = Resources.FindObjectsOfTypeAll<SMVviewBase>();
        foreach(SMVviewBase view in allViews)
        {
            view.Init();
            Add(view.mapping, view);
        }
        DebugDumpDict();
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
        foreach (SMVviewBase view in GetViewsForMapping(mapping))
        {
            view.SetValue(val);
        }
    }

    /// <summary>
    /// For mappings with 2 or more views, verify that each view holds the same value.
    /// Returns true if yes, otherwise print error message and return false.
    /// </summary>
    /// <param name="mapping"></param>
    /// <returns></returns>
    private bool VerifyEqualValues(SMVmapping mapping)
    {
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
        VerifyEqualValues(mapping);
        return GetViewsForMapping(mapping)[0].GetValueFloat();        
    }

    /// <summary>
    /// Add a mapping and SMVviewBase pair to the list
    /// </summary>
    /// <param name="mapping"></param>
    /// <param name="view"></param>
    void Add(SMVmapping mapping, SMVviewBase view)
    {
        List<SMVviewBase> viewsList;
        if( ! viewDict.TryGetValue(mapping, out viewsList))
        {
            List < SMVviewBase > views = new List<SMVviewBase>();
            views.Add(view);
            viewDict.Add(mapping, views);
        }
        else
        {
            //Mapping has already been added, so add new view to the list of views
            viewsList.Add(view);
        }
    }
	
    /// <summary>
    /// For a particular mapping, find all the views it's connected to
    /// </summary>
    /// <param name="mapping"></param>
    /// <returns></returns>
    List<SMVviewBase> GetViewsForMapping(SMVmapping mapping)
    {
        List<SMVviewBase> views = new List<SMVviewBase>();
        viewDict.TryGetValue(mapping, out views);
        return views;
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

    void DebugDumpDict()
    {
        string outStr = "SMV dict debug: \n";
        foreach (SMVmapping item in Enum.GetValues(typeof(SMVmapping)))
        {
            outStr += "item: " + item.ToString() + "\n";
            List<SMVviewBase> views = GetViewsForMapping(item);
            if (views == null)
            {
                outStr += "  no view entries found\n";
                continue;
            }
            foreach(SMVviewBase view in views)
            {
                outStr += view.mapping.ToString() + " mapped to a " + view.SMVType.ToString() + ", with behavior: " + view.UIelement.name + " and parent: " + view.gameObject.name + "\n";
            }
            outStr += "---\n";
        }
        Debug.Log(outStr);
    }
}
