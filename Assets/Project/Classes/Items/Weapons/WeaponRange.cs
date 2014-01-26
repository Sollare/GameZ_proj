using UnityEngine;
using System.Collections;

public abstract class WeaponRange : Weapon
{
    [SerializeField]
    private float _reloadRate;
    /// <summary>
    /// Скорость перезарядки
    /// </summary>
    public float ReloadRate {
        get { return _reloadRate; }
        set 
        {
            _reloadRate = Mathf.Clamp(value, 0f, float.MaxValue); ;
        }
    }

    [SerializeField]
    private uint _holderCapacity;
    /// <summary>
    /// Вместительность магазина
    /// </summary>
    public uint HolderCapacity {
        get { return _holderCapacity; }
        set 
        { 
            _holderCapacity = value;
        }
    }

    [SerializeField]
    private uint _holderCount;
    /// <summary>
    /// Текущее количество патронов в обойме
    /// </summary>
    public uint HolderCount {
        get { return _holderCount; }
        set 
        {
            _holderCount = (uint) Mathf.Clamp(value, 0, HolderCount); 
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.FlexibleSpace();

        GUILayout.Label("Параметры огневного оружия");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Скорострельность:");
        Rate = GUILayout.HorizontalSlider(Rate, 0.02f, 5f);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Скорость перезарядки:");
        ReloadRate = GUILayout.HorizontalSlider(ReloadRate, 0f, 10f);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Вместимость обоймы:");
        HolderCapacity = (uint) GUILayout.HorizontalSlider(HolderCapacity, 0, 100);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Патрон в обойме:");
        HolderCount = (uint)GUILayout.HorizontalSlider(HolderCount, 0, 100);
        GUILayout.EndHorizontal();
    }
}