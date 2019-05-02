using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of mappings that determine which UI items are mapped to which model fields.
/// Kinda like events.
/// Add a new one when you have a new model field that you want to map to a view.
/// 
/// *** NOTE - if you change the order of these in any way, make sure you update selection 
/// in any SMVView components you've already assigned in your project.
/// 
/// </summary>
public enum SMVmapping { undefined/*always include this*/, RotationSpeed, TimeLabel, TimeLabelUpdate, RotatingText, RotatingTextCount, TextColor };


