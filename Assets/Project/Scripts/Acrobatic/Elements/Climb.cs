using Assets.Project.Scripts.Acrobatic;
using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

public class Climb : IAcrobaticElement 
{
    public CharacterBehaviour GetBehaviourInstance(CharacterController player, AcrobaticComponent acrobaticComponent)
    {
        return new PlayerClimbBehaviour((PlayerController)player, acrobaticComponent);
    }

    public RuntimeAnimatorController GetAnimatorController()
    {
        throw new System.NotImplementedException();
    }
}
