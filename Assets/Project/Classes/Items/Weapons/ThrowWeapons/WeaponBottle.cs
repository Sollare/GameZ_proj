using System;
using UnityEngine;


[Serializable]
public sealed class WeaponBottle : WeaponThrow
{
    private Sprite _inventoryPicture;
    private Sprite _equippedSprite;

    public WeaponBottle()
    {
        LocalizedName = "Бутылка";
    }

    public override int stackCapacity
    {
        get { return 1; }
    }

    public override Sprite inventorySprite
    {
        get
        {
            if (_inventoryPicture == null)
                _inventoryPicture = Resources.Load<Sprite>("Sprites/Prototype/bottle");

            return _inventoryPicture;
        }
    }
    public override Sprite equippedSprite
    {
        get
        {
            if (_equippedSprite == null)
                _equippedSprite = Resources.Load<Sprite>("Sprites/Prototype/bottle");

            return _equippedSprite;
        }
    }

    public override RuntimeAnimatorController GetAnimatorController()
    {
        return null;
        //throw new System.NotImplementedException();
    }

    public override void SetAxis(string axis, float value)
    {
        throw new NotImplementedException();
    }


    public override void Attack()
    {
        throw new NotImplementedException();
    }
}

