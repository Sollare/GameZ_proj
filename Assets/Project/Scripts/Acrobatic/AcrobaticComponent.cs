using System;
using UnityEngine;

namespace Assets.Project.Scripts.Acrobatic
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class AcrobaticComponent: MonoBehaviour, IAcrobaticElement
    {
        public enum AcrobaticElements
        {
            Ladder,
            Border,
            Climb
        }

        private IAcrobaticElement acrobaticElement;
        protected CharacterBehaviour originalBehaviour;

        public AcrobaticElements Type = AcrobaticElements.Ladder;

        void Awake()
        {
            acrobaticElement = GetAcrobaticElement(Type);
            collider2D.isTrigger = true;
            tag = "Acrobatic";
        }

        /// <summary>
        /// Установить первоначальное поведение (до того, как персонаж использовал данный объект)
        /// </summary>
        /// <param name="behaviour">Изначальное поведение персонажа</param>
        public void SetOriginalBehaviour(CharacterBehaviour behaviour)
        {
            originalBehaviour = behaviour;
        }

        /// <summary>
        /// Возвращает экземпляр поведения персонажа для взаимодействия с данным акробатическим компонентом
        /// </summary>
        /// <param name="player">Персонаж, осуществляющий взаимодействие</param>
        public CharacterBehaviour GetBehaviourInstance(Characters.Controllers.CharacterController player)
        {
            return GetBehaviourInstance(player, this);
        }

        //public RuntimeAnimatorController GetAnimatorController()
        //{
        //    RuntimeAnimatorController animatorController = null;

        //    if (acrobaticElement != null)
        //        animatorController = acrobaticElement.GetAnimatorController();

        //    return animatorController;
        //}

        public static IAcrobaticElement GetAcrobaticElement(AcrobaticElements type)
        {
            switch (type)
            {
                case AcrobaticElements.Ladder:
                    return new Ladder();

                case AcrobaticElements.Border:
                    return null;

                case AcrobaticElements.Climb:
                    return new Climb();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public CharacterBehaviour GetBehaviourInstance(Characters.Controllers.CharacterController character, AcrobaticComponent component)
        {
            CharacterBehaviour behaviour = null;

            if (acrobaticElement != null)
                behaviour = acrobaticElement.GetBehaviourInstance(character, component);

            return behaviour;
        }
    }
}
