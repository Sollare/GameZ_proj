
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : CharacterAnimator
{
    private List<StateMachineController> _stateMachineControllers;
    private StateMachineController _lastStateMachineController;

    public PlayerController PlayerController { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        PlayerController = GetComponent<PlayerController>();
        _stateMachineControllers = new List<StateMachineController>();

        //animator.runtimeAnimatorController = GetAnimatorController();
        PlayerController.WeaponController.OnWeaponStateChanged += WeaponStateChanged;
    }

    private void WeaponStateChanged(Weapon weapon, WeaponController.WeaponState newState)
    {
        print(string.Format("Оружие {0} изменило свое состояние {1}", weapon.LocalizedName, newState));

        if (newState == WeaponController.WeaponState.Equipped)
        {
            // Берем другой контроллер для анимации персонажа
            var animatorController = weapon.GetAnimatorController();

            if (animatorController != null)
                animator.runtimeAnimatorController = animatorController;
            
#if UNITY_EDITOR
            if (weapon is WeaponMelee) // Изменяем скорость анимации в соответствие со скоростью атаки
            {
                _lastStateMachineController = new AttackStateController();
                _lastStateMachineController.Do(animator, (weapon as WeaponMelee).AttackTime);
                _stateMachineControllers.Add(_lastStateMachineController);
            }
#endif
        }

        if (newState == WeaponController.WeaponState.Unequipped)
        {
#if UNITY_EDITOR
            if (_lastStateMachineController != null) // Откатываем контроллер к предыдущему состоянию
            {
                _lastStateMachineController.Undo();
                _stateMachineControllers.Remove(_lastStateMachineController);
                _lastStateMachineController = null;
            }
#endif
            // Берем стандартный
            animator.runtimeAnimatorController = PlayerController.currentBehaviour.GetAnimatorController();
            //GetAnimatorController();
        }
    }
    
    public override void Interract(GameObject interactedObject)
    {
        throw new System.NotImplementedException();
    }

    public override void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void PerformDamage(float damage)
    {
        base.PerformDamage(damage);
    }

    void OnDestroy()
    {
        foreach (var stateMachineController in _stateMachineControllers)
        {
            stateMachineController.Undo();
        }
    }
}
