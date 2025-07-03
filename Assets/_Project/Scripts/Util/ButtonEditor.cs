#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonEditor : Editor
{
    private Dictionary<string, object> paramCache = new Dictionary<string, object>();
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Vẽ inspector mặc định
        
        GUILayout.Space(10);
        // Lấy tất cả phương thức có ButtonAttribute
        MonoBehaviour mono = (MonoBehaviour)target;
        MethodInfo[] methods = mono.GetType().GetMethods(
            BindingFlags.Instance | 
            BindingFlags.Static | 
            BindingFlags.Public | 
            BindingFlags.NonPublic
        );

        foreach (MethodInfo method in methods)
        {
            ButtonAttribute buttonAttr = (ButtonAttribute)Attribute.GetCustomAttribute(
                method, 
                typeof(ButtonAttribute)
            );

            if (buttonAttr != null)
            {
                string buttonText = string.IsNullOrEmpty(buttonAttr.Text) 
                    ? method.Name 
                    : buttonAttr.Text;

                // Kiểm tra phương thức không có tham số
                if (method.GetParameters().Length == 0)
                {
                    if (GUILayout.Button(buttonText))
                    {
                        method.Invoke(mono, null); // Gọi phương thức
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.BeginHorizontal();
                    ParameterInfo[] parameters = method.GetParameters();
                    object[] args = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        string key = method.Name + "_" + param.Name;

                        // Hiển thị UI tùy theo type
                        if (param.ParameterType == typeof(int))
                        {
                            if (!paramCache.ContainsKey(key)) paramCache[key] = 0;
                            paramCache[key] = EditorGUILayout.IntField(param.Name, (int)paramCache[key]);
                        }
                        else if (param.ParameterType == typeof(float))
                        {
                            if (!paramCache.ContainsKey(key)) paramCache[key] = 0f;
                            paramCache[key] = EditorGUILayout.FloatField(param.Name, (float)paramCache[key]);
                        }
                        else if (param.ParameterType == typeof(string))
                        {
                            if (!paramCache.ContainsKey(key)) paramCache[key] = "";
                            paramCache[key] = EditorGUILayout.TextField(param.Name, (string)paramCache[key]);
                        }
                        else if (param.ParameterType == typeof(bool))
                        {
                            if (!paramCache.ContainsKey(key)) paramCache[key] = false;
                            paramCache[key] = EditorGUILayout.Toggle(param.Name, (bool)paramCache[key]);
                        }
                        else
                        {
                            EditorGUILayout.LabelField($"Type {param.ParameterType.Name} not supported");
                        }

                        // Gán vào mảng args
                        args[i] = paramCache[key];
                    }
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button(buttonText))
                    {
                        method.Invoke(mono, args);
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}
#endif