using System;
using UnityEngine;
using System.Collections;

public class UserInterfaceController : MonoBehaviour
{
    private Transform _throwTarget;

    // Спрайт экипированного предмета
    public SpriteRenderer EquippedItemSprite;

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.InventoryController.OnHoldedItemStateChanged += InventoryItemStateChanged;

        _throwTarget = GameObject.Find("ThrowTarget").transform;
        _throwTarget.gameObject.SetActive(false);

        EquippedItemSprite.sprite = null;
    }


    private void InventoryItemStateChanged(Item item, InventoryController.HoldedItemState newState)
    {
        // Выводим сообщение (поднят такой то предмет)
        if (newState == InventoryController.HoldedItemState.Picked)
        {
            EquippedItemSprite.sprite = item.inventorySprite;
        }
    }

    public void ItemEquipped(Item item)
    {
        EquippedItemSprite.sprite = item.inventorySprite;
    }
}
