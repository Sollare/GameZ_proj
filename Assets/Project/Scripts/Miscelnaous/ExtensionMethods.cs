using UnityEngine;
using System.Collections;

public static class ExtensionMethods {

    public static T ForceComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        
        if(component == null)
            component = gameObject.AddComponent<T>();
        
        return component;
    }
    public static Vector2 ApplyForceMode(this Rigidbody2D rigidbody2d, Vector2 force, ForceMode forceMode)
    {

        switch (forceMode)
        {
            case ForceMode.Force:
                return force;
            case ForceMode.Impulse:
                return force/Time.fixedDeltaTime;

            case ForceMode.Acceleration:
                return force*rigidbody2d.mass;

            case ForceMode.VelocityChange:
                return force*rigidbody2d.mass/Time.fixedDeltaTime;

            default:
                goto case ForceMode.Force;
        }

    }
}
