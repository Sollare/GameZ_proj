using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof (LineRenderer))]
public class WeaponThrowView : WeaponView
{
    public LayerMask collisionLayer;

    protected Material trajectoryMaterial;

    private LineRenderer predictionLine;
    private List<Vector3> points;
    private WeaponThrow throwWeapon;
    
    private void Awake()
    {
        predictionLine = gameObject.ForceComponent<LineRenderer>();
        predictionLine.castShadows = false;
        predictionLine.receiveShadows = false;
        predictionLine.SetWidth(0.1f, 0.1f);
        predictionLine.enabled = false;

        trajectoryMaterial = Resources.Load<Material>("Materials/TrajectoryPredictionMaterial");
        predictionLine.material = trajectoryMaterial;

        //collisionLayerMask = LayerMask.NameToLayer("Obstacle");// | LayerMask.NameToLayer("Character");
        points = new List<Vector3>(150);

        enabled = false;
    }

    public override void InitializeWith(PlayerController player, WeaponController controller, Weapon weapon, Transform weaponInstance)
    {
        base.InitializeWith(player, controller, weapon, weaponInstance);

        throwWeapon = (Weapon as WeaponThrow);
    }

    public override void Attack(Transform weaponInstance)
    {

    }

    public override void StartDraw()
    {
        enabled = true;
    }

    public override void StopDraw()
    {
        predictionLine.enabled = false;
        enabled = false;
    }

    private void Update()
    {
        UpdatePredictionLine(points.Capacity);
    }

    private void UpdatePredictionLine(int pointsCount)
    {
        //var playerPos = Camera.main.WorldToScreenPoint(WeaponInstance.position);
        var throwVector = Controller.input.normalized * throwWeapon.ThrowForce;

        points.Clear();

        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 posN = GetTrajectoryPoint(transform.position, throwVector, i, Physics.gravity);

            if (i > 0)
            {
                var previous = points[i - 1];
                var hit = Physics2D.Linecast(previous, posN, collisionLayer);
                if (hit)
                {
                    posN = hit.point;
                    posN.z = -1f;
                    points.Add(posN);
                    break;
                }
            }

            posN.z = -1f;
            points.Add(posN);
        }

        predictionLine.SetVertexCount(points.Count);

        for (int i = 0; i < points.Count; i++)
        {
            predictionLine.SetPosition(i, points[i]);
        }

        predictionLine.enabled = true;
    }


    /// <summary>
    /// Возвращает точку траектории броска
    /// </summary>
    /// <param name="startingPosition">Позиция начала броска</param>
    /// <param name="initialVelocity">Изначальный вектор ускорения</param>
    /// <param name="timestep">Через сколько секунд вычисляем траекторию</param>
    /// <param name="gravity">Сила гравитации</param>
    private Vector3 GetTrajectoryPoint(Vector3 startingPosition, Vector3 initialVelocity, float timestep,
        Vector3 gravity)
    {
        float physicsTimestep = Time.fixedDeltaTime;
        Vector3 stepVelocity = physicsTimestep*initialVelocity;

        //Gravity is already in meters per second, so we need meters per second per second
        Vector3 stepGravity = physicsTimestep*physicsTimestep*gravity;

        var pos = startingPosition + (timestep*stepVelocity) + (((timestep*timestep + timestep)*stepGravity) / 2.0f);


        return pos;
    }

    /// <summary>
    /// Возвращает вектор броска в целевую точку
    /// </summary>
    /// <param name="startingPosition">Стартовая позиция</param>
    /// <param name="targetPosition">Направление броска</param>
    /// <param name="lob"></param>
    /// <param name="gravity">Сила гравитации</param>
    /// <returns></returns>
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
}
