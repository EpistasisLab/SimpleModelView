using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SMView
{

    /// <summary>
    /// An SVMview for a dropdown element, stores and returns current selected dropdown item index number as int.
    /// </summary>
    public class SMVViewDropdown : SMVviewBase
    {
        protected override void InitDerived()
        {
            //Find the UI element within this components game object
            UIelement = transform.GetComponent<Dropdown>();
            if (UIelement == null)
                Debug.LogError("uiElement == null");

            //Add the change-listener from base class
            //Always use the EndEdit event so we don't end up with validation errors while typing
            (((Dropdown)UIelement).onValueChanged).AddListener(delegate { OnValueChangedListener(); });

            smvtype = SMVtypeEnum.dropdown;
            dataType = typeof(int);
        }

        public override object GetValueAsObject()
        {
            //Just return the string object, and validation method will handle the parsing
            return ((Dropdown)UIelement).value;
        }

        protected override void SetValueInternal(object val)
        {
            ((Dropdown)UIelement).value = (int)val;
            ((Dropdown)UIelement).RefreshShownValue();
        }
    }
}