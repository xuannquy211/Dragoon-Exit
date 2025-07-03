using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
public class ButtonAttribute : PropertyAttribute
{
    public string Text { get; }

    public ButtonAttribute(string text = null)
    {
        Text = text;
    }
}