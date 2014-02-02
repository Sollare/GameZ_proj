using System.Security.Cryptography.X509Certificates;

using UnityEngine;
using System.Collections;

/// <summary>
/// Интерфейс для предметов, которые могут быть экипированы на персонажа
/// </summary>
public interface IEquipable
{
    /// <summary>
    /// В момент, когда мы экипируем предмет на себя, мы начинаем использовать данный контроллер для анимации персонажа
    /// </summary>
    RuntimeAnimatorController GetAnimatorController();

    /// <summary>
    /// Метод, вызывается снаружи (например компонентом PlayerInput) и передает нам информацию о вводе для управления работой предмета. Например вкл/откл фонарика.
    /// </summary>
    /// <param name="axis">Название оси, контроллирующей работу данного предмета</param>
    /// <param name="value">Введенное значение</param>
    void SetAxis(string axis, float value);

    /// <summary>
    /// Возвращает экипируемый префаб
    /// </summary>
    GameObject GetEquipablePrefab();

    /// <summary>
    /// Возврощает контроллер ввода для текущего оружия (принципы управления оружием могут отличается у разных типов)
    /// </summary>
    WeaponInput GetInputController();
}

//TODO: как использовать предмет по нажатию клавиши клавиатуры, когда он экипирован?
// Вариант: 
// Предмет содержит список Axis, которые ему нужно слушать. 
// В момент экипировки мы в общий класс управления – PlayerInput передаем нужную нам ось (например Shoot2), 
// он сохраняет ее в Dictionary <string,IEquipable> и каждый апдейт пробегает в цикле по слушаемым осям, и вызывает метод SetAxis у экипированных предметов. 
// Когда снимаем предмет – отписываемся.

