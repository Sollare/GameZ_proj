using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

[RequireComponent(typeof(PlayerAnimator))]
public sealed class PlayerController : CharacterController
{
    // Радиус взаимодействия с предметами
    public float InteractionRadius = 2f;

    public InventoryController InventoryController;
    public WeaponController WeaponController;


    public override void Move(float hor, float ver)
    {
        cachedRigidbody2D.AddForce(Vector2.right * hor * MovementSpeed);

        if(Mathf.Abs(cachedRigidbody2D.velocity.x) > MaxSpeed)
            cachedRigidbody2D.velocity = new Vector2(MaxSpeed * Mathf.Sign(cachedRigidbody2D.velocity.x), cachedRigidbody2D.velocity.y);

        // Сообщаем аниматору о осуществляемом движении
        animator.Move(cachedRigidbody2D.velocity.x, cachedRigidbody2D.velocity.y);
    }

    public override void Interract()
    {
        var objects = Physics2D.OverlapCircleAll(transform.position, InteractionRadius);

        foreach (var obj in objects)
        {
            if ("Item".Equals(obj.tag))
            {
                var item = obj.GetComponent<PickableItem>();
                InventoryController.PutItem(item.HoldedItem);
                Destroy(obj.gameObject);

                //print(item.HoldedItem.LocalizedName);
            }
        }
    }

    public override void Attack()
    {
        animator.Attack();
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void PerformDamage(uint damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Heal(uint amount)
    {
        throw new System.NotImplementedException();
    }
}