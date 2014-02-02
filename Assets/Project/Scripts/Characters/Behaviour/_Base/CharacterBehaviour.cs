using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

/// <summary>
/// Обобщенный класс для определения поведения персонажа в ответ на ввод пользователя. 
/// Для каждого своего состояния у персонажа должна быть разная реакция на ввод (идет, ползет, карабкается)
/// </summary>
public abstract class CharacterBehaviour : IControllable
{
    /// <summary>
    /// Делегат перехода из одного поведения в другое.
    /// </summary>
    /// <param name="character">Персонаж, совершивий переход состояния</param>
    /// <param name="transitionBehaviour">Состояние, активировавшее переход</param>
    public delegate void BehaviourTransitionComplete(CharacterController character, CharacterBehaviour transitionBehaviour);

    /// <summary>
    /// Объект-инициатор поведения (лестница, препятствие, персонаж и т.п.)
    /// </summary>
    public GameObject Initiator { get; protected set; }

    /// <summary>
    /// Управляемый персонаж
    /// </summary>
    protected CharacterController controller { get; set; }

    /// <summary>
    /// Открытый конструктор для создания экземпляра поведения персонажа
    /// </summary>
    /// <param name="controller">Персонаж, для которого предназначается поведение</param>
    /// <param name="initiator">Объект-инициатор поведения</param>
    public CharacterBehaviour(CharacterController controller, GameObject initiator)
    {
        this.Initiator = initiator;
        this.controller = controller;
    }

    /// <summary>
    /// Осуществить переход из предыдущего поведения (нужно для проигрывния анимации перехода, перемещения персонажа к лестнице и т.п.)
    /// </summary>
    /// <param name="behaviour">Предыдущее поведение персонажа</param>
    /// <param name="callback">По завершению перехода будет вызван данный метод</param>
    public abstract void MakeTransitionFrom(CharacterBehaviour behaviour, BehaviourTransitionComplete callback);

    /// <summary>
    /// Осуществить переход из текущего поведения в следующее (нужно для проигрывния анимации перехода, перемещения персонажа к лестнице и т.п.)
    /// </summary>
    /// <param name="behaviour">Предыдущее поведение персонажа</param>
    /// <param name="callback">По завершению перехода будет вызван данный метод</param>
    public abstract void MakeTransitionTo(CharacterBehaviour behaviour, BehaviourTransitionComplete callback);

    public abstract void Move(float hor, float ver);

    public abstract void Interract(GameObject interactedObject);

    public abstract void Attack(GameObject target);

    public abstract void Die();

    public abstract void Hit(float damage);
    


    /// <summary>
    /// Метод возвращает AnimatorController, связанный с текущим аниматором персонажа.
    /// Необходимо реализовать его в классе наследнике, чтобы персонаж начал анимироваться именно этим контроллером
    /// </summary>
    public abstract RuntimeAnimatorController GetAnimatorController();
}
