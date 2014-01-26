using System;
using UnityEngine;
using System.Collections;

[Serializable]
public abstract class WeaponThrow : Weapon
{
    [SerializeField]
    private float _damageRadius;
    /// <summary>
    /// Радиус нанесения урона после столкновения / взрыва
    /// </summary>
    public float DamageRadius {
        get { return _damageRadius; }
        set 
        { 
            _damageRadius = Mathf.Clamp(value, 0f, 10f);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Радиус поражения:", GUILayout.MaxWidth(130f));
        DamageRadius = GUILayout.HorizontalSlider(DamageRadius, 0f, 10f);
        GUILayout.TextField(DamageRadius.ToString(), GUILayout.MaxWidth(50));
        GUILayout.EndHorizontal();
    }
}
