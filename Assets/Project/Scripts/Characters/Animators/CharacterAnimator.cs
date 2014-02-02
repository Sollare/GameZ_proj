using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
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

    private Animator _animator;

    public Animator animator
    {
        get
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();

            return _animator;
        }
    }

    private bool _facingRight = true;

    /// <summary>
    /// Смотрит ли персонаж вправо
    /// </summary>
    public bool facingRight
    {
        get
        {
            return _facingRight;
        }

        protected set { _facingRight = value; }
    }

    public virtual void Move(float hor, float ver)
    {
        if (hor < 0 && facingRight)
           Flip();

        if (hor > 0 && !facingRight)
            Flip();

        animator.SetFloat("Horizontal", Mathf.Abs(hor));
    }

    /// <summary>
    /// Запуск анимации взамодействия
    /// </summary>
    /// <param name="interactedObject">Объект, с которым проводится взаимодействие</param>
    public abstract void Interract(GameObject interactedObject);

    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>();
    }

    public virtual void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public abstract void Die();

    public virtual void PerformDamage(float damage)
    {
        animator.SetTrigger("Hit");
    }

    public void StopAnimations()
    {
        if(animator.animation)
            animator.animation.Stop();
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
