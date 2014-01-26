using UnityEngine;
using System.Collections;

public sealed class WeaponStick : WeaponMelee
{
    private Sprite _equippedSprite;
    private Sprite _inventorySprite;

    public WeaponStick()
    {
        LocalizedName = "Палка";
    }

    public override int stackCapacity
    {
        get { return 1; }
    }

    public override Sprite inventorySprite
    {
        get 
        {
            if (_inventorySprite == null)
                _inventorySprite = Resources.Load<Sprite>("Sprites/Prototype/stick");

            return _inventorySprite;
        }
    }
    public override Sprite equippedSprite
    {
        get
        {
            if (_equippedSprite == null)
                _equippedSprite = Resources.Load<Sprite>("Sprites/Prototype/stick");

            return _equippedSprite;
        }
    }


    public override void SetAxis(string axis, float value)
    {
        throw new System.NotImplementedException();
    }


    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Throw(Vector2 direction)
    {
        throw new System.NotImplementedException();
    }
}
