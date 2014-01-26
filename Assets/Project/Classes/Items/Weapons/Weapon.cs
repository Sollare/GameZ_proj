using System;
using UnityEngine;
using System.Collections;

[Serializable]
public abstract class Weapon : Item, IInspectable, IEquipable
{
    [SerializeField]
    private float _damage = 0f;
    /// <summary>
    /// Урон от оружия
    /// </summary>
    public float Damage
    {
        get { return _damage; }
        set { _damage = Mathf.Clamp(value, 0f, float.MaxValue); }
    }

    [SerializeField]
    private float _range = 0f;
    /// <summary>
    /// Расстояние воздействия оружия
    /// </summary>
    public float Range
    {
        get { return _range; }
        set
        {
            _range = Mathf.Clamp(value, 1f, float.MaxValue);
        }
    }

    [SerializeField]
    private float _rate;
    /// <summary>
    /// Скорострельность / Скорость нанесения ударов. Для ближнего оружия изменяет скорость анимации нанесения удара
    /// </summary>
    public float Rate
    {
        get { return _rate; }
        set
        {
            _rate = Mathf.Clamp(value, 0.1f, 3f);
        }
    }

    /// <summary>
    /// Использование оружия
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// Бросок оружия
    /// </summary>
    /// <param name="direction">Направление броска</param>
    public virtual void Throw(Vector2 direction)
    {
        
    }

    public virtual void OnInspectorGUI()
    {
        GUILayout.Label("Урон:");
        GUILayout.BeginHorizontal();
        Damage = GUILayout.HorizontalSlider(Damage, 0f, 1000f);
        GUILayout.TextField(Damage.ToString(), GUILayout.MaxWidth(50));
        GUILayout.EndHorizontal();

        GUILayout.Label("Дальность:");
        GUILayout.BeginHorizontal();
        Range = GUILayout.HorizontalSlider(Range, 0f, 1000f);
        GUILayout.TextField(Range.ToString(), GUILayout.MaxWidth(50));
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();

        GUILayout.Label("Частота:", GUILayout.MaxWidth(130f));
        Rate = GUILayout.HorizontalSlider(Rate, 0.1f, 5f);
        GUILayout.TextField(Rate.ToString(), GUILayout.MaxWidth(50));
        GUILayout.EndHorizontal();
    }

    public override GameObject GetPickablePrefab()
    {
        return Resources.Load<GameObject>("Prefabs/ItemsPrefab/Weapons/" + GetType().Name);
    }
    public GameObject GetEquipablePrefab()
    {
        return Resources.Load<GameObject>("Prefabs/EquipmentPrefab/Weapons/" + GetType().Name);
    }

    public abstract RuntimeAnimatorController GetAnimatorController();

    public abstract void SetAxis(string axis, float value);

    public abstract Sprite equippedSprite { get; }


}
