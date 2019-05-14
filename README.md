# Simple Model View

Simple Model View (SMV) is a Unity package meant to be a very simple implementation of a Model View-type system for simpliflying UI plumbing. 

It creates a simple paradigm for:
- synchronizing a state value and multiple UI elements
- getting update events when a state value is changed either via UI element or via code.

### Getting Started

#### Unity Version
Developed under Unity 2018.2

#### Installation

Clone this project and copy the Assets/SimpleModelView folder into your Unity project.

##### Unity 2018 and TextMeshPro
When you load the ExampleComplex scene, you'll be prompted to load TextMeshPro into the project - that's fine. However the transform for the example `RotationSpeed_TextMeshProWorldSpace` gets messed up, but don't worry about that for demo purposes.

##### Unity 2017 and earlier
You'll need to either:

- remove the two source files that use TextMeshPro (_SMVViewTextMeshProUGUI_ and _SMVViewTextMeshProWV_) and ignore the warnings in the example scene
- or, get TextMeshPro and add it to your project

# Concepts

### Mappings
SMV uses a user-defined _mapping_ to link a value with a UI element. The user specifies one or more _mappings_ to use in a project

Each _mapping_ is represented by a single value stored in an `SMVControl` (somewhat analogous to a controller in MVC paradigm), and by one or more `SMVView` components that each create a link to a UI element. This way, several UI elements (via `SMVView`) can be mapped to a single value (`SMVControl`) - for example if you have a `Slider` UI element and want a `Text` element to show its value, or if you have both desktop and VR UI's for your program with different UI elements. If the value in the `SMVControl` changes, either from code or UI interaction, all mapped UI elements will also be updated, and the new value is accessible via code.

### Validation

The `SMVView` types `SMVViewInputFieldFloat` and `SMVViewInputFieldInt` will only accept float and integer values, respectively, and will discard invalid input strings when input by the user, and then restore the prior valid state.

# Usage

### And the SMView namespace to your scripts
`using SMView;` 

### Add the `SMV` singleton component to your project.
It doesn't matter where you add it. In the examples, it's in a game object called SMV.

Access the members of the `SMV` singleton by accessing its instance: `SMV.Instance`

### Define one or more mappings
Edit the members of the `SVMmapping` enum in the script _SMVmappings\_USER\_DEFINED_. Give a descriptive name to each mapping you want.

### Add SMVView components
For each UI element that you want to map, add the appropriate SMVView componenent, and assign the mapping it should use in the dropdown. For example, if you want to map a `Slider`, add an `SMVViewSlider` component to your slider, and choose the mapping it should use.

### `SMVControl` state/value can be changed in two ways
1) Automatically when a mapped UI element is changed.

2) Manually when your code calls `SMV.Instance.SetValue(<mapping enum>)` with the enum corresponding to the mapping you want to change.

### Retrieving the state of an `SMVControl`

From your code, simply call `SMV.Instance.GetValueXYZ(<mapping enum>)`, where `XYZ` is the data type for the mapping.

### Value Changes

Each `SMVView` type automatically handles value-change events for the UI element it belongs to. When an event is received, it updates the `SMVControl` that it is mapped to, which in turn updates any other `SMVView` objects that are a part of the mapping. The `SMVControl` then generates its own update that can be optionally handled by the user's code by supplying a listener to the `SMV.OnUpdateEvent` event in the editor (see below).

# Basic Example

Load the sample scene `ExampleBasic`.

The `SMV` GameObject in the top of the scene holds the `SMV` singleton.

This example uses a single mapping (`RotationSpeed`) from those defined in _SMVmappings_USER_DEFINED.cs_. The other mappings are used in the `ExampleComplex` scene. Any mappings that aren't used by your UI elements are ignored.

In the scene under `Canvas | Panel | SMV-mapped-items` there are 3 UI elements. Look at each of them and you'll see each has an `SMVView*` component attached. They're all set to the `RotationSpeed` mapping.

When any of the UI elements are changed, the `SMV` system handles updating each of the other mapped elements (after validation in the case of `SMVViewInputFieldFloat`), and updates the state for the mapped `SMVControl`.

From the code in _ExampleMainBasic_ component, you can see how the value of the mapping is accessed. A property is used to simplify access:

	public float RotationSpeed
	{
	    set { SMV.Instance.SetValue(SMVmapping.RotationSpeed, value);  }
	    get { return SMV.Instance.GetValueFloat(SMVmapping.RotationSpeed); }
	}

Whenever RotationSpeed is assigned a value, the mapped UI elements are updated.

# Complex Example

Load the sample scene `ExampleComplex`.

Besides showing examples of all the possible `SMVView*` types, this example shows how to receive change events for all mapped UI elements in one place.

#### Change Events

When a mapping's value is changed (either from code or UI), an event is generated that can be handled by providing a listener for the `SMV.OnUpdateEvent` (set in the editor like for other UI event listeners).

In the listener, you can filter the event by mapping, and handle it (or ignore it) as you please. This provides an easy, single spot for handling value changes both from code and from UI. 

For example, see the _ExampleComplex_ script in the `ExampleComplex` scene:

    public void OnSVMUpdate(SMVmapping mapping)
    {
        //Log when we get an event for demo purposes, but skip the TimeLabel update since they're each frame
        if(mapping != SMVmapping.TimeLabel)
            Debug.Log("*** OnSVMUpdate called with mapping " + mapping.ToString());

        switch ((int)mapping)
        {
            case (int)SMVmapping.RotatingTextCount:
                SetupRotatingTextObjects();
                break;
            case (int)SMVmapping.TextColor:
                UpdateRotatingTextColor();
                break;
            default:
				//ignore this mapping
                break;
        }
    }   

## Contributing

If you make some fixes or additions, submit a pull request! And thanks!

# Authors

Michael Stauffer, Univ. of Pennsylvania, Philadelphia PA.
stauffer@upenn.edu

# License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

