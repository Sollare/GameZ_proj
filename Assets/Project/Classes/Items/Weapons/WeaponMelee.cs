using System;
using UnityEngine;
using System.Collections;

[Serializable]
public abstract class WeaponMelee : Weapon
{
    [SerializeField]
    private float _attackTime;
    /// <summary>
    /// Скорость нанесения удара
    /// </summary>
    public float AttackTime {
        get { return _attackTime; }
        set 
        {
            _attackTime = Mathf.Clamp(value, 0.1f, float.MaxValue);
        }
    }

    [SerializeField]
    private float _durability;
    /// <summary>
    /// Прочность оружия
    /// </summary>
    public float Durability {
        get { return _durability; }
        set 
        {
            _durability = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public override RuntimeAnimatorController GetAnimatorController()
    {
        return Resources.Load<RuntimeAnimatorController>("Characters/AnimatorControllers/Player/WeaponAnimatorControllers/CommonMeleeAnimatorController");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(5f);

        GUILayout.Label("Влияет на скорость воспроизведения анимации");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Скорость удара:", GUILayout.MaxWidth(130f));
        AttackTime = GUILayout.HorizontalSlider(AttackTime, 0.1f, 2f);
        GUILayout.TextField(AttackTime.ToString(), GUILayout.MaxWidth(50));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Прочность оружия:", GUILayout.MaxWidth(130f));
        Durability = GUILayout.HorizontalSlider(Durability, 0f, 1f);
        GUILayout.TextField(Durability.ToString(), GUILayout.MaxWidth(50));
        GUILayout.EndHorizontal();
    }
}
