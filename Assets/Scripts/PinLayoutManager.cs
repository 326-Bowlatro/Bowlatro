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

    [SerializeField]
    private Transform pinParent;

    [SerializeField]
    private GameObject pinPrefab;

    [Header("Special Pins Settings")]
    public bool enableSpecialPins = false;
    public List<GameObject> specialPinPrefabs;
    [Tooltip("Number of special pins to spawn (when enabled)")]
    public int specialPinsCount = 1;

    // Persistent settings
    private static bool _specialPinsEnabled = false;
    private static List<GameObject> _activeSpecialPinPrefabs = new List<GameObject>();
    private static int _activeSpecialPinsCount = 1;

    private void Awake()
    {
        // Apply persistent settings if they exist
        if (_activeSpecialPinPrefabs.Count <= 0) return;
        enableSpecialPins = _specialPinsEnabled;
        specialPinPrefabs = new List<GameObject>(_activeSpecialPinPrefabs);
        specialPinsCount = _activeSpecialPinsCount;
    }

    public static void EnableSpecialPins(bool enable, List<GameObject> prefabs = null, int count = 1)
    {
        _specialPinsEnabled = enable;
        if (prefabs != null)
        {
            _activeSpecialPinPrefabs = new List<GameObject>(prefabs);
        }
        _activeSpecialPinsCount = count;
    }

    public void OnEndTurn()
    {
        foreach (var pin in Pins)
        {
            pin.OnEndTurn();
        }
    }

    public void ClearPins()
    {
        foreach (var pin in Pins.ToList())
        {
            Destroy(pin.gameObject);
        }
    }

    public void SpawnPins(LayoutType layoutType)
    {
        LayoutType = layoutType;

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

        List<Vector3> pinPositions = new List<Vector3>();
        const float baseX = -0.6f;
        const float baseZ = 0;
        
        // First spawn all regular pins
        for (var z = 0; z < grid.Length; z++)
        {
            for (var x = 0; x < grid[z].Length; x++)
            {
                if (grid[z][x] == 0) continue;

                Vector3 position = new Vector3(baseX + x * 0.2f, 0, baseZ + z * 0.25f);
                var pin = Instantiate(pinPrefab, pinParent);
                pin.transform.localPosition = position;
                pinPositions.Add(position);
            }
        }

        // Replace with special pins if enabled
        if (enableSpecialPins && specialPinPrefabs != null && specialPinPrefabs.Count > 0)
        {
            // Filter out null prefabs to prevent missing script errors
            specialPinPrefabs = specialPinPrefabs.Where(p => p != null).ToList();
            
            if (specialPinPrefabs.Count > 0)
            {
                SpawnSpecialPins(pinPositions);
            }
            else
            {
                Debug.LogWarning("No valid special pin prefabs available");
            }
        }
    }

    private void SpawnSpecialPins(List<Vector3> allPinPositions)
    {
        int pinsToReplace = Mathf.Min(specialPinsCount, allPinPositions.Count);
        List<Vector3> shuffledPositions = new List<Vector3>(allPinPositions);
        Shuffle(shuffledPositions);

        for (int i = 0; i < pinsToReplace; i++)
        {
            GameObject specialPrefab = specialPinPrefabs[UnityEngine.Random.Range(0, specialPinPrefabs.Count)];
            if (specialPrefab == null)
            {
                Debug.LogError("Attempted to instantiate null special pin prefab");
                continue;
            }

            Pin pinToReplace = FindPinAtPosition(shuffledPositions[i]);
            if (pinToReplace != null)
            {
                Destroy(pinToReplace.gameObject);
            }

            var specialPin = Instantiate(specialPrefab, pinParent);
            if (specialPin == null)
            {
                Debug.LogError("Failed to instantiate special pin prefab");
                continue;
            }
            specialPin.transform.localPosition = shuffledPositions[i];
        }
    }

    private Pin FindPinAtPosition(Vector3 position)
    {
        return Pins.FirstOrDefault(pin => 
            Vector3.Distance(pin.transform.localPosition, position) < 0.01f);
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}