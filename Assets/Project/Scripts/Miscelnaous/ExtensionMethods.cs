using UnityEngine;
using System.Collections;

public static class ExtensionMethods {

    public static T ForceComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        
        if(component == null)
            component = gameObject.AddComponent<T>();

        Debug.Log(component);

        return component;
    }
}
