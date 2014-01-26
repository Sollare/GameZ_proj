using UnityEngine;

namespace Assets.Project.Scripts.Characters.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class CharacterController : MonoBehaviour
    {
        /// <summary>
        /// Компонент аниминирования персонажа
        /// </summary>
        public CharacterAnimator animator { get; protected set; }

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
        protected Transform cachedTransform
        {
            get
            {
                if (_transform) return transform;
                else return (_transform = transform);
            }
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

        /// <summary>
        /// Метод перемещения персонажа
        /// </summary>
        /// <param name="hor">Ввод по горизонтали</param>
        /// <param name="ver">Ввод по вертикали</param>
        public abstract void Move(float hor, float ver);

        /// <summary>
        /// Взаимодействие с окружающими предметами
        /// </summary>
        public abstract void Interract();

        /// <summary>
        /// Атака 
        /// </summary>
        public abstract void Attack();

        /// <summary>
        /// Смерть персонажа
        /// </summary>
        public abstract void Die();

        /// <summary>
        /// Нанесение урона по персонажу
        /// </summary>
        /// <param name="damage">Количество нанесенного урона</param>
        public virtual void PerformDamage(uint damage)
        {
            Health -= damage;
        }

        /// <summary>
        /// Лечение персонажа
        /// </summary>
        /// <param name="amount">Количество прибавляемых хитпоинтов</param>
        public virtual void Heal(uint amount)
        {
            Health += amount;
        }
        protected virtual void Awake()
        {
            animator = GetComponent<CharacterAnimator>();
        }
    }
}