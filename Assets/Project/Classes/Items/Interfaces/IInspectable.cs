using UnityEngine;
using System.Collections;

/// <summary>
/// Интерфейс для предметов, параметры которых могут быть просмотрены и изменены в Inspector'e Unity3D
/// </summary>
public interface IInspectable
{
    /// <summary>
    /// Данный метод будет вызван внутри PickableItemInspector для отображения параметров объекта
    /// </summary>
    void OnInspectorGUI();
}
