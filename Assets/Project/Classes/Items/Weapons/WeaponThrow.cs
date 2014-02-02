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

    private float _throwForce = -1f;
    /// <summary>
    /// Сила броска (вычисляется исходя из поля Range, считаем что масса объекта = 1кг)
    /// </summary>
    public float ThrowForce
    {
        get
        {
            if (_throwForce < 0f)
            {
                var velocity = GetThrowVelocity(Vector2.zero, Vector2.zero + (Vector2.right)*Range);
                _throwForce = velocity.magnitude;
            }

            return _throwForce;
        }
    }

    /// <summary>
    /// Возвращает вектор-ускорение для броска
    /// </summary>
    /// <param name="startingPosition">Начальная точка броска</param>
    /// <param name="targetPosition">Целевая точка броска</param>
    public Vector2 GetThrowVelocity(Vector3 startingPosition, Vector3 targetPosition)
    {
        return GetTrajectoryVelocity(startingPosition, targetPosition, 1.25f,
                    Physics2D.gravity);
    }

    public static Vector3 GetTrajectoryVelocity(Vector3 startingPosition, Vector3 targetPosition, float lob, Vector3 gravity)
    {
        float physicsTimestep = Time.fixedDeltaTime;
        float timestepsPerSecond = Mathf.Ceil(1f / physicsTimestep);

        //By default we set n so our projectile will reach our target point in 1 second
        float n = lob * timestepsPerSecond;

        Vector3 a = physicsTimestep * physicsTimestep * gravity;
        Vector3 p = targetPosition;
        Vector3 s = startingPosition;

        Vector3 velocity = (s + (((n * n + n) * a) / 2f) - p) * -1 / n;

        //This will give us velocity per timestep. The physics engine expects velocity in terms of meters per second
        velocity /= physicsTimestep;
        return velocity;
    }

    public override WeaponInput GetInputController()
    {
        return new WeaponThrowInput(this, 35f, 15f, 80f);
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
