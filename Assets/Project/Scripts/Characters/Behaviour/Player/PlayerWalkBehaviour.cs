using Assets.Project.Scripts.Acrobatic;
using UnityEngine;
using System.Collections;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

public class PlayerWalkBehaviour : CharacterBehaviour
{
    // Радиус взаимодействия с предметами
    public float InteractionRadius = 2f;

    private const float LadderInteractionDistance = 0.4f;

    private PlayerController _playerController;
    
    public PlayerWalkBehaviour(PlayerController controller) : base(controller, null)
    {
        _playerController = controller;
        this.controller = controller;
    }

    public override void Move(float hor, float ver)
    {
        if (!_playerController.grounded) return;

        controller.cachedRigidbody2D.AddForce(Vector2.right * hor * controller.MovementSpeed);

        if (Mathf.Abs(controller.cachedRigidbody2D.velocity.x) > controller.MaxSpeed)
            controller.cachedRigidbody2D.velocity = new Vector2(controller.MaxSpeed * Mathf.Sign(controller.cachedRigidbody2D.velocity.x), controller.cachedRigidbody2D.velocity.y);
    }

    public override void Interract(GameObject interactedObject)
    {
        var objects = Physics2D.OverlapCircleAll(controller.cachedTransform.position, InteractionRadius);

        foreach (var obj in objects)
        {
            if ("Item".Equals(obj.tag))
            {
                var item = obj.GetComponent<PickableItem>();
                (controller as PlayerController).InventoryController.PutItem(item.HoldedItem);
                Object.Destroy(obj.gameObject);
                break;
            }
        }

        objects = Physics2D.OverlapCircleAll(controller.cachedTransform.position, LadderInteractionDistance);
        foreach (var obj in objects)
        {
            Debug.LogWarning("name: " + obj.name + " tag:" + obj.gameObject.tag);
            if ("Acrobatic".Equals(obj.tag))
            {
                var acrobatic = obj.GetComponent<AcrobaticComponent>();

                // Получаем поведение для текущего персонажа (управляемого текущим поведением)
                var behaviour = acrobatic.GetBehaviourInstance(controller);
                //var animatorController = acrobatic.GetAnimatorController();

                controller.SetCharacterBehaviour(behaviour);
                //controller.SetAnimatorController(animatorController);
                
                break;
            }
        }
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

    public override void MakeTransitionFrom(CharacterBehaviour behaviour, BehaviourTransitionComplete callback)
    {
        controller.cachedRigidbody2D.isKinematic = false;
        callback(controller, this);
    }

    public override void MakeTransitionTo(CharacterBehaviour behaviour, BehaviourTransitionComplete callback)
    {
        callback(controller, this);
    }

    public override RuntimeAnimatorController GetAnimatorController()
    {
        return Resources.Load<RuntimeAnimatorController>("Characters/AnimatorControllers/Player/StandartController");
    }
}
