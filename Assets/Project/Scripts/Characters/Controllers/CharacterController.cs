using System;
using UnityEngine;
using CharacterController = Assets.Project.Scripts.Characters.Controllers.CharacterController;

namespace Assets.Project.Scripts.Characters.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharacterController : MonoBehaviour, IControllable
    {
        /// <summary>
        /// Компонент аниминирования персонажа
        /// </summary>
        public CharacterAnimator characterAnimator { get; protected set; }

        /// <summary>
        /// Текущее поведение персонажа
        /// </summary>
        public CharacterBehaviour currentBehaviour { get; protected set; }

        /// <summary>
        /// Скорость с которой персонаж должен удариться о землю, чтобы получить урон
        /// </summary>
        protected float FallSpeedToHit = 7f;

        /// <summary>
        /// Минимальный урон, наносимый персонажу при столкновении с землей на скорости FallSpeedToHit
        /// </summary>
        protected float FallMinHit = 15f;

        private float _health = 100;
        /// <summary>
        /// Здоровье персонажа
        /// </summary>
        public float Health
        { 
            get {
                return _health;
            }

            private set {
                _health = Mathf.Clamp(value, 0f, InitialHealth);

                if (value <= 0)
                    Die ();
            }
        }

        private Rigidbody2D _rigidbody2D;

        /// <summary>
        /// Кешированный физический компонент
        /// </summary>
        public Rigidbody2D cachedRigidbody2D
        {
            get
            {
                if (_rigidbody2D == null) _rigidbody2D = rigidbody2D;
                return _rigidbody2D;
            }
        }

        private Transform _transform;

        /// <summary>
        /// Кешированный физический компонент
        /// </summary>
        public Transform cachedTransform
        {
            get
            {
                if (_transform) return transform;
                else return (_transform = transform);
            }
        }

        private CircleCollider2D _legsCollider;
        public CircleCollider2D LegsCollider
        {
            get { return _legsCollider; }
        }

        #region Поля инспектора

        /// <summary>
        /// Сила, прилагающаяся к персонажу во время его перемещения
        /// </summary>
        public float MovementSpeed = 10f;

        /// <summary>
        /// Максимальная скорость перемещения по горизонтальной оси
        /// </summary>
        public float MaxSpeed = 10f;

        /// <summary>
        /// Изначальное количество здоровья
        /// </summary>
        public float InitialHealth = 100f;

        #endregion
        protected virtual void Awake()
        {
            characterAnimator = GetComponent<CharacterAnimator>();
            _legsCollider = GetComponent<CircleCollider2D>();
        }
        
        public void SetCharacterBehaviour(CharacterBehaviour newBehaviour)
        {
            if (newBehaviour == null || newBehaviour is UncontrollableBehaviour)
            {
                currentBehaviour = newBehaviour;
                Debug.Log(string.Format(">> Переходим в неуправляемое состояние"));
                return;
            }

            var previousBehaviour = currentBehaviour;
            //currentBehaviour = behaviour;
            SetAnimatorController(newBehaviour.GetAnimatorController());
            
            if (previousBehaviour != null && !(previousBehaviour is UncontrollableBehaviour))
            {
                Debug.Log(string.Format("Начинаем переход из состояния [{0}] в состояние [{1}] >> ", previousBehaviour, newBehaviour));
                // По идее они должны идти один за другим
                previousBehaviour.MakeTransitionTo(newBehaviour,
                    delegate
                    {
                        currentBehaviour = newBehaviour;
                        currentBehaviour.MakeTransitionFrom(previousBehaviour, BehaviourTransitionComplete);
                    });
            }
            else
            {
                currentBehaviour = newBehaviour;
                currentBehaviour.MakeTransitionFrom(null, BehaviourTransitionComplete);
            }
        }

        private void BehaviourTransitionComplete(CharacterController character, CharacterBehaviour transitionBehaviour)
        {
            Debug.Log(string.Format(">> Переход в состояние [{0}] завершен", transitionBehaviour));
        }

        public void SetAnimatorController(RuntimeAnimatorController controller)
        {
            if(controller != null)
                characterAnimator.animator.runtimeAnimatorController = controller;
        }

        /// <summary>
        /// Метод перемещения персонажа
        /// </summary>
        /// <param name="hor">Ввод по горизонтали</param>
        /// <param name="ver">Ввод по вертикали</param>
        public abstract void Move(float hor, float ver);

        /// <summary>
        /// Взаимодействие с окружающими предметами
        /// </summary>
        public abstract void Interract(GameObject interactedObject);

        /// <summary>
        /// Атака 
        /// </summary>
        public abstract void Attack(GameObject target);

        /// <summary>
        /// Смерть персонажа
        /// </summary>
        public abstract void Die();

        /// <summary>
        /// Нанесение урона по персонажу
        /// </summary>
        /// <param name="damage">Количество нанесенного урона</param>
        public virtual void Hit(float damage)
        {
            Health -= damage;
            characterAnimator.PerformDamage(damage);
        }

        /// <summary>
        /// Лечение персонажа
        /// </summary>
        /// <param name="amount">Количество прибавляемых хитпоинтов</param>
        public virtual void Heal(uint amount)
        {
            Health += amount;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.relativeVelocity.y < -FallSpeedToHit)
            {
                // TODO : грубый расчет, нужно расчитывать по другой формуле
                var damage = FallMinHit * Mathf.Pow(Mathf.Abs(coll.relativeVelocity.y) / FallSpeedToHit, 3);
                Debug.Log(damage);
                Hit(damage);
            }
        }
    }
}