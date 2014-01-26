using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;


public abstract class CharacterAnimator : MonoBehaviour
{/// <summary>
    /// Делегат поворота 
    /// </summary>
    /// <param name="facingRight">Направлен ли теперь вправо персонаж</param>
    public delegate void FlipDelegate(bool facingRight);

    /// <summary>
    /// Событие поворота игрока
    /// </summary>
    public event FlipDelegate OnCharacterFlipEvent;

    protected virtual void InvokeFlipDelegate(bool facingright)
    {
        FlipDelegate handler = OnCharacterFlipEvent;
        if (handler != null) handler(facingright);
    }


    public Animator animator { get; protected set; }

    private bool facingRight = true;


    /// <summary>
    /// Метод возвращает AnimatorController, связанный с текущим аниматором персонажа.
    /// Необходимо реализовать его в классе наследнике, чтобы персонаж начал анимироваться именно этим контроллером
    /// </summary>
    public abstract RuntimeAnimatorController GetAnimatorController();

    public virtual void Move(float hor, float ver)
    {
        if (hor < 0 && facingRight)
           Flip();

        if(hor > 0 && !facingRight)
            Flip();

        animator.SetFloat("Speed", Mathf.Abs(hor));
    }

    /// <summary>
    /// Запуск анимации взамодействия
    /// </summary>
    /// <param name="interactedObject">Объект, с которым проводится взаимодействие</param>
    public abstract void Interract(GameObject interactedObject);

    public virtual void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public abstract void Die();

    public virtual void PerformDamage(float damage)
    {
        animator.SetTrigger("Hit");
    }

    void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        facingRight = !facingRight;

        InvokeFlipDelegate(facingRight);
    }
}
