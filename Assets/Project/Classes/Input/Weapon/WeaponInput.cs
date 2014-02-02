using UnityEngine;
using System.Collections;

public abstract class WeaponInput
{
    protected Weapon weapon;

    public WeaponInput(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public abstract Vector2 GetInput();
}
