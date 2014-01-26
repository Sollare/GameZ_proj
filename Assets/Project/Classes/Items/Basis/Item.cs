using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Collections;

/// <summary>
/// Базовый класс для предметов игрового мира
/// </summary>
public abstract class Item : ScriptableObject
{
    /// <summary>
    /// Локализованное имя
    /// </summary>
    public string LocalizedName;

    public static ItemType CreateItem<ItemType>() where ItemType : Item
    {
        Debug.Log("CreateItem 2");
        var item = (ItemType)CreateInstance(typeof(ItemType));

        return item;
        
    }

    public static Item CreateItem(UnityEngine.Object itemSource)
    {
        Debug.Log("CreateItem 1");
        try
        {
            Type itemType = Assembly.GetExecutingAssembly().GetType(itemSource.name);
            var item = (Item) CreateInstance(itemType);
            Debug.Log("Created item: " + item); 

            return item;
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка при создании инстанса предмета: " + e.Message); 
            return null;
        }



        //foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
        //{
        //    // Находим объекты того же типа что ItemType, у которого есть конструктор
        //    if (t.BaseType == typeof(Item) && t.Name.Equals(itemSource.name))
        //    {
        //        Debug.Log("Type: " + t);
        //        var item = (Item) Assembly.GetExecutingAssembly().CreateInstance(t.FullName);

        //        return item;
        //    }
        //}

    }


    /// <summary>
    /// Максимальное количество предметов в одной ячейке инвентаря
    /// </summary>
    public abstract int stackCapacity { get; }

    /// <summary>
    /// Изображение предмета в инвентаре
    /// </summary>
    public abstract Sprite inventorySprite { get; }

    /// <summary>
    /// Возвращает префаб объекта сцены для данного поднимаемого объекта
    /// </summary>
    public abstract GameObject GetPickablePrefab();
}
