using UnityEngine;
using System.Collections;

public static class Layers
{
    /// <summary>
    /// Слой физических препятствий
    /// </summary>
    public static readonly int Obstacle = 1 << LayerMask.NameToLayer("Obstacle");
}
