using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Collections;

[Serializable]
[RequireComponent(typeof(Collider2D))]
public class PickableItem : MonoBehaviour
{
    private static LayerMask? _itemsLayer;
    public static LayerMask ItemsLayer
    {
        get { return (LayerMask) (_itemsLayer ?? (_itemsLayer = LayerMask.NameToLayer("Item"))); }
    }

    [SerializeField]
    private UnityEngine.Object _itemSource;

    private IInspectable _inspectable;

    /// <summary>
    /// Предмет, как инспектируемый объект
    /// </summary>
    public IInspectable InspectableItem
    {
        get { return _inspectable ?? (_inspectable = (IInspectable) HoldedItem); }

        private set { _inspectable = value; }
    }

    /// <summary>
    /// Поднимаемый предмет, хранящийся в объекте сцены
    /// </summary>
    [SerializeField]
    public Item _item;

    public Item HoldedItem {
        get
        {
            return _item;
        } 

        set { _item = value; }
    }

    public void SetItemSource(UnityEngine.Object obj) 
    {
        if (obj == null) return;

        if (!obj.Equals(_itemSource))
        {
            _itemSource = obj;

            HoldedItem = CreateIsntanceFromSource(_itemSource);

            if (HoldedItem != null)
            {
                InspectableItem = (IInspectable) HoldedItem;
            }
            else
            {
                _itemSource = null;
                return;
            }
        }
    }
    public UnityEngine.Object GetItemSource()
    {
        return (UnityEngine.Object)_itemSource;
    }

    public void ClearItemSource()
    {
        _itemSource = null;
        HoldedItem = null;
        InspectableItem = null;
    }


    public Item CreateIsntanceFromSource(UnityEngine.Object obj)
    {
        var item = Item.CreateItem(obj);
        return item;
    }

    void Awake()
    {
        tag = "Item";

        if (collider2D)
        {
            collider2D.gameObject.layer = PickableItem.ItemsLayer;
        }
        else
            Debug.LogWarning("Отсутствует коллайдер на объекте [" + gameObject.name + "] : " + HoldedItem.LocalizedName);
    }
}
