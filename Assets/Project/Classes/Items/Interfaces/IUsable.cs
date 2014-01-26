using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public delegate void UseDelegate(object sender, Hashtable args);

public interface IUsable 
{
    /// <summary>
    /// Варианты использования объекта. Ключ - локализованная строка с вариантом использования, значение - делегат метода использования
    /// </summary>
    Dictionary<string, UseDelegate> useCases { get; set; } 
}
