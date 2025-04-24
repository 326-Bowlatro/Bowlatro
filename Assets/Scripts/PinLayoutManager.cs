using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LayoutType
{
    Block,
    Triangle,
    Diamond,
    ReverseTriangle,
}

public class PinLayoutManager : MonoBehaviour
{
    public LayoutType LayoutType { get; private set; }
    public IEnumerable<Pin> Pins => pinParent.GetComponentsInChildren<Pin>();

    public const int NumPins = 10;
    public int NumPinsFallen => NumPins - Pins.Count(pin => !pin.IsKnockedOver);

    [SerializeField] private Transform pinParent;
    [SerializeField] private GameObject pinPrefab;

    public void OnEndTurn()
    {
        Pins.ToList().ForEach(pin => pin.OnEndTurn());
    }

    /// <summary>
    /// Spawns a pin layout based on the given type.
    /// </summary>
    public void SpawnLayout(LayoutType layoutType)
    {
        LayoutType = layoutType;

        // Map enum values to layout grids
        int[][] grid = layoutType switch
        {
            LayoutType.Triangle => new[]
            {
                new[] { 1, 0, 1, 0, 1, 0, 1 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
                new[] { 0, 0, 1, 0, 1, 0, 0 },
                new[] { 0, 0, 0, 1, 0, 0, 0 },
            },
            LayoutType.Block => new[]
            {
                new[] { 1, 0, 1, 0, 1, 0, 1 },
                new[] { 1, 0, 1, 0, 1, 0, 1 },
                new[] { 0, 0, 1, 0, 1, 0, 0 },
                new[] { 0, 0, 0, 0, 0, 0, 0 },
            },
            LayoutType.Diamond => new[]
            {
                new[] { 0, 0, 0, 1, 0, 0, 0 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
                new[] { 1, 0, 0, 1, 0, 0, 1 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
            },
            LayoutType.ReverseTriangle => new[]
            {
                new[] { 0, 0, 0, 1, 0, 0, 0 },
                new[] { 0, 0, 1, 0, 1, 0, 0 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
                new[] { 1, 0, 1, 0, 1, 0, 1 },
            },
            _ => throw new NotImplementedException(),
        };

        // Spawn pins in chosen layout
        const float baseX = -0.6f;
        const float baseZ = 0.75f;
        for (var z = 0; z < grid.Length; z++)
        {
            for (var x = 0; x < grid[z].Length; x++)
            {
                // Only spawn pin if 1
                if (grid[z][x] == 0)
                {
                    continue;
                }

                var pin = Instantiate(pinPrefab, pinParent);
                pin.transform.localPosition = new Vector3(baseX + x * 0.2f, 0, baseZ + z * 0.25f);
            }
        }
    }
}
