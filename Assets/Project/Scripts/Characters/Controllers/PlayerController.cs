using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

public class PlayerController : CharacterController
{
    /// <summary>
    /// Инвентарь персонажа
    /// </summary>
    public InventoryController InventoryController;

    /// <summary>
    /// Контроллер вооружения персонажа
    /// </summary>
    public WeaponController WeaponController;

    /// <summary>
    /// Контроллер управления персонажем
    /// </summary>
    public PlayerInput InputController;

    /// <summary>
    /// Находится ли персонаж на земле
    /// </summary>
    public bool grounded { get; private set; }

    private Transform _groundTarget;
    
    protected override void Awake()
    {
        base.Awake();
        SetCharacterBehaviour(new PlayerWalkBehaviour(this));

        _groundTarget = transform.FindChild("GroundTarget");
    }

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, _groundTarget.position, Layers.Obstacle);
    }

    public override void Move(float hor, float ver)
    {
        if(Math.Abs(hor) > 0.05f || Math.Abs(ver) > 0.05f)
            currentBehaviour.Move(hor, ver);

        // Сообщаем аниматору о осуществляемом движении
        characterAnimator.Move(cachedRigidbody2D.velocity.x, cachedRigidbody2D.velocity.y);
    }

    public override void Interract(GameObject interactedObject)
    {
        currentBehaviour.Interract(null);
    }

    public override void Attack(GameObject target)
    {
        characterAnimator.Attack();
        currentBehaviour.Attack(null);
    }

    public override void Die()
    {
        currentBehaviour.Die();
    }
}