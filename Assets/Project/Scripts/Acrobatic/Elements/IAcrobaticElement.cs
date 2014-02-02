using Assets.Project.Scripts.Acrobatic;
using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

/// <summary>
/// Интерфейс для объектов акробатического взаимодействия
/// </summary>
public interface IAcrobaticElement  
{
    /// <summary>
    /// Возвращает экземпляр поведения для указанного персонажа при пользовании данным объектом
    /// </summary>
    CharacterBehaviour GetBehaviourInstance(CharacterController character, AcrobaticComponent component);
}
