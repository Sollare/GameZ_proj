using Assets.Project.Scripts.Acrobatic;
using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

public class Ladder : IAcrobaticElement
{
    public CharacterBehaviour GetBehaviourInstance(CharacterController player, AcrobaticComponent acrobaticComponent)
    {
        return new PlayerLadderBehaviour((PlayerController)player, acrobaticComponent);
    }
}
