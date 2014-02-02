using UnityEngine;
using System.Collections;

/// <summary>
/// Ипределяет обобщенный метод управления игровым персонажем
/// </summary>
public interface IControllable
{
    /// <summary>
    /// Обобщенный метод перемещения персонажем посредством указания направления перемещения
    /// </summary>
    /// <param name="hor">Движение по горизонтали</param>
    /// <param name="ver">Движение по вертикали</param>
    void Move(float hor, float ver);

    /// <summary>
    /// Обобщенный метод взаимодействия с объектом окружения
    /// </summary>
    /// <param name="interactedObject">Объект, с которым проводится взаимодействие</param>
    void Interract(GameObject interactedObject);

    /// <summary>
    /// Обобщенный метод атаки
    /// </summary>
    /// <param name="target">Атакуемая цель</param>
    void Attack(GameObject target);

    /// <summary>
    /// Обобщенный метод для смерти персонажа
    /// </summary>
    void Die();

    /// <summary>
    /// Обобщенный метод получения урона персонажем
    /// </summary>
    /// <param name="damage">Полученный урон</param>
    void Hit(float damage);
}
