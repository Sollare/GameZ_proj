using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

/// <summary>
/// Поведение-заглушка
/// </summary>
public class UncontrollableBehaviour : CharacterBehaviour 
{
    public UncontrollableBehaviour(CharacterController controller, GameObject initiator) : base(controller, initiator)
    {
    }

    public override void MakeTransitionFrom(CharacterBehaviour behaviour, CharacterBehaviour.BehaviourTransitionComplete callback)
    {
    }

    public override void MakeTransitionTo(CharacterBehaviour behaviour, CharacterBehaviour.BehaviourTransitionComplete callback)
    {
    }

    public override void Move(float hor, float ver)
    {
    }

    public override void Interract(GameObject interactedObject)
    {
    }

    public override void Attack(GameObject target)
    {
    }

    public override void Die()
    {
    }

    public override void Hit(float damage)
    {
    }

    public override RuntimeAnimatorController GetAnimatorController()
    {
        return null;
    }
}
