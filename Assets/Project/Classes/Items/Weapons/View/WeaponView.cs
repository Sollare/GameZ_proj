using UnityEngine;
using System.Collections;

public abstract class WeaponView : MonoBehaviour
{
    /// <summary>
    /// Оружие, с которым связано текущее отображение
    /// </summary>
    protected Weapon Weapon;

    /// <summary>
    /// Трансформ объекта-оружия
    /// </summary>
    protected Transform WeaponInstance;

    /// <summary>
    /// Игрок, связанный с данным отображением оружия
    /// </summary>
    protected PlayerController Player;

    /// <summary>
    /// Игрок, связанный с данным отображением оружия
    /// </summary>
    protected WeaponController Controller;

    public virtual void InitializeWith(PlayerController player, WeaponController controller, Weapon weapon, Transform weaponInstance)
    {
        Weapon = weapon;
        WeaponInstance = weaponInstance;
        Controller = controller;
        Player = player;
        
        Player.characterAnimator.OnCharacterFlipEvent += OnFlipEvent;
    }

    private void OnFlipEvent(bool facingRight)
    {
        if (WeaponInstance)
        {
            WeaponInstance.localEulerAngles = new Vector3(0, WeaponInstance.localEulerAngles.y + 180, 0);
        }
    }

    public abstract void Attack(Transform weaponInstance);

    /// <summary>
    /// Отрисовка (линия броска, прицела)
    /// </summary>
    public abstract void StartDraw();

    /// <summary>
    /// Остановить отрисовку (линия броска, прицела)
    /// </summary>
    public abstract void StopDraw();

    void OnDestroy()
    {
        if (Player)
            Player.characterAnimator.OnCharacterFlipEvent -= OnFlipEvent;
    }
}
