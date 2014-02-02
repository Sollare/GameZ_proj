using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour
{
    private Inventory _inventory;
    public IEnumerator<Item> ItemsEnumerator; 

    /// <summary>
    /// Состояние хранимого предмета
    /// </summary>
    public enum HoldedItemState
    {
        Picked, // Поднят
        Dropped, // Сброшен
        Equipped, // Экипирован
        Brocked // Сломан
    }

    public delegate void HoldedItemStateChanged(Item item, HoldedItemState newState);

    /// <summary>
    /// Событие изменения состояния предмета в инвентаре
    /// </summary>
    public event HoldedItemStateChanged OnHoldedItemStateChanged;

    private void InvokeOnHoldedItemStateChanged(Item item, HoldedItemState newstate)
    {
        print(string.Format("Предмет {0} был {1}", item.LocalizedName, newstate));

        HoldedItemStateChanged handler = OnHoldedItemStateChanged;
        if (handler != null) handler(item, newstate);
    }

    void Awake()
    {
        _inventory = Inventory.InitWithCapacity(10);
        ItemsEnumerator = _inventory.Items.GetEnumerator();
    }

    public int ItemsCount
    {
        get
        {
            if (_inventory != null)
                return _inventory.Items.Count;
            else return 0;
        }
    }

    /// <summary>
    /// Попытка положить предмет. 
    /// </summary>
    /// <param name="item">Предмет, помещаемый в инвентарь</param>
    /// <returns>Возвращает сообщение класса Message</returns>
    public Message PutItem(Item item)
    {
        for (int i = 0; i < _inventory.Items.Capacity; i++)
            {
                if (_inventory.Items[i] == null) // Если пустой слот
                {
                    _inventory.Items[i] = item;
                    InvokeOnHoldedItemStateChanged(item, HoldedItemState.Picked);
                    return new Message(string.Concat("Предмет ", item.LocalizedName, " подобран"), Message.MessageType.Info, null);
                }    
            }
            
        return new Message("Инвентарь переполнен", Message.MessageType.Error, null);
    }

    /// <summary>
    /// Изъятие предмета из инвентаря
    /// </summary>
    /// <param name="item">Извлекаемый предмет</param>
    public Message RemoveItem(Item item)
    {
        if (_inventory.Items.Contains(item))
        {
            // Проверка а можем ли мы в текущем состоянии выбросить предмет
            _inventory.Items.Remove(item);
            InvokeOnHoldedItemStateChanged(item, HoldedItemState.Dropped);
            return new Message(string.Concat("Предмет ", item.LocalizedName, " выброшен"), Message.MessageType.Info,
                null);
        }
        else
        {
            Debug.LogError("Попытка выбросить предмет, не содержащийся в инвентаре");
            return null;
        }

    }

    public Item GetItemAt(int position)
    {
        if (position < 0 || position >= _inventory.Items.Capacity) return null;

        return _inventory.Items[position];
    }
}
