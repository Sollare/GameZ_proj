using Assets.Project.Scripts.Acrobatic;
using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

public class PlayerClimbBehaviour : CharacterBehaviour
{
    private AcrobaticComponent _acrobaticComponent;

    public PlayerClimbBehaviour(PlayerController controller, AcrobaticComponent acrobaticComponent)
        : base(controller, acrobaticComponent.gameObject)
    {
        _acrobaticComponent = acrobaticComponent;
    }

    public override void Move(float hor, float ver)
    {
        throw new System.NotImplementedException();
    }

    public override void Interract(GameObject interactedObject)
    {
        throw new System.NotImplementedException();
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

        }
    }

    public override void MakeTransitionTo(CharacterBehaviour behaviour, BehaviourTransitionComplete callback)
    {
        throw new System.NotImplementedException();
    }

    public override RuntimeAnimatorController GetAnimatorController()
    {
        throw new System.NotImplementedException();
    }
}
