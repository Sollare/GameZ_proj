using System;
using Assets.Project.Scripts.Acrobatic;
using Holoville.HOTween;
using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

public class PlayerLadderBehaviour : CharacterBehaviour
{
    private float ClimbingSpeed = 1f;

    private AcrobaticComponent _acrobaticComponent;
    private Transform[] exits;

    protected bool enabled = false;

    public PlayerLadderBehaviour(PlayerController controller, AcrobaticComponent acrobaticComponent)
        : base(controller, acrobaticComponent.gameObject)
    {
        _acrobaticComponent = acrobaticComponent;

        exits = new Transform[_acrobaticComponent.transform.childCount];
        for (int i = 0; i < _acrobaticComponent.transform.childCount; i++)
        {
            exits[i] = _acrobaticComponent.transform.GetChild(i);
        }
    }

    public override void Move(float hor, float ver)
    {
        if (!enabled) return;

        controller.cachedTransform.Translate(Vector2.up * ver * ClimbingSpeed * Time.fixedDeltaTime);

        var closest = ClosestExit(_acrobaticComponent);
        // Угол между ВВЕРХ игрока и точкой выхода с лестницы
        var angle = Vector2.Angle(closest.up, controller.cachedTransform.up);

        if (Mathf.Abs(ver) > 0f) // Если мы движемся по вертикали...
        {
            if (Math.Abs(angle) < 0.1f && ver > 0.1f && controller.cachedTransform.position.y >= closest.transform.position.y)
                // ... точка выхода тоже ведет наверх и мы уже поднялись выше этой точки
            {
                controller.SetCharacterBehaviour(new UncontrollableBehaviour(controller, null)); // Отключаем управление

                var outputPoint = closest.position +
                                  Vector3.up*(controller.LegsCollider.radius - controller.LegsCollider.center.y);
                var tween = controller.cachedTransform.positionTo(0.5f, outputPoint);
                tween.setOnCompleteHandler(delegate(AbstractGoTween goTween)
                {
                    controller.SetCharacterBehaviour(new PlayerWalkBehaviour(controller as PlayerController));
                });

                return;
            }

            if (Math.Abs(Math.Abs(angle) - 180f) < 1f && ver < 0.1f &&
                controller.cachedTransform.position.y <= closest.transform.position.y)
            {
                // TODO: запускаем анимацию слазания с лестницы, ждем пока завершится
                controller.SetCharacterBehaviour(new PlayerWalkBehaviour(controller as PlayerController));
            }
        }

        controller.characterAnimator.animator.SetFloat("Vertical", Mathf.Abs(ver));
    }

    private Transform ClosestExit(AcrobaticComponent component)
    {
        float minDistance = float.MaxValue;
        Transform closest = null;

        foreach (var exit in exits)
        {
            float distance = Vector2.Distance(controller.cachedTransform.position, exit.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = exit;
            }
        }

        return closest;
    }

    public override void Interract(GameObject interactedObject)
    {
        //controller.SetCharacterBehaviour(new PlayerWalkBehaviour(controller as PlayerController));
        //throw new System.NotImplementedException();
    }

    public override void Attack(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(float damage)
    {
        throw new System.NotImplementedException();
    }

    public override void MakeTransitionFrom(CharacterBehaviour behaviour, BehaviourTransitionComplete callback)
    {
        if (behaviour is PlayerWalkBehaviour) // Если до этого шли
        {
            controller.cachedRigidbody2D.isKinematic = true;

            var behaviourObject = Initiator;

            float minDistance = float.MaxValue;
            Transform point = null;

            // Т.к. это лестница, надо найти ближайшую точку входа от игрока
            for (int i = 0; i < behaviourObject.transform.childCount; i++)
            {
                var child = behaviourObject.transform.GetChild(i);

                if (child.name.Contains("InputPoint"))
                {
                    if (Vector3.Distance(controller.transform.position, child.position) < minDistance)
                    {
                        minDistance = Vector3.Distance(controller.transform.position, child.position);
                        point = child;
                    }
                }
            }

            if (point == null)
            {
                Debug.LogError("Не найдено точки входа на лестницу. Точка входа должна быть дочерним игровым объектом лестницы с именем 'InputPoint'");
                return;
            }

            var horPoint = new Vector3(point.position.x, controller.cachedTransform.position.y,
                controller.cachedTransform.position.z);
            // Определяем как долго нужно перемещаться к точке входа на лестницу, чтобы не было слишком быстро
            var speedHor = 2f * Vector2.Distance(controller.cachedTransform.position, horPoint) / controller.MaxSpeed;
            var speedVer = 2f * Vector2.Distance(horPoint, point.position) / controller.MaxSpeed;

            speedHor = Mathf.Clamp(speedHor, 0.01f, 1f);

            var tweenHor = controller.cachedTransform.positionTo(speedHor, horPoint);
            tweenHor.setOnCompleteHandler(delegate
            {
                var tweenVer = controller.cachedTransform.positionTo(speedVer, point.position);
                tweenVer.setOnCompleteHandler(delegate
                {
                    enabled = true;
                    callback(controller, this);
                });
            });
        }
    }

    public override void MakeTransitionTo(CharacterBehaviour behaviour, BehaviourTransitionComplete callback)
    {
        callback(controller, this);
    }
    
    public override RuntimeAnimatorController GetAnimatorController()
    {
        return Resources.Load<RuntimeAnimatorController>("Characters/AnimatorControllers/Player/LadderController");
    }
}
