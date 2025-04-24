using UnityEngine;

public class PinLayoutManager : MonoBehaviour
{
    [SerializeField] private Transform pinParent;
    [SerializeField] private GameObject pinPrefab;
    [SerializeField] private int laneNumber;
    private void Start()
    {
        PinLayoutCard.PinLayoutSelected += PinLayoutCardOnPinLayoutSelected;
    }

    private void PinLayoutCardOnPinLayoutSelected(LayoutEnum layoutType)
    {
        //create layout to be what the enum states
        ChooseLayout(layoutType);
        GameManager.Instance.layoutType = layoutType;
    }

    private void OnDestroy()
    {
        PinLayoutCard.PinLayoutSelected -= PinLayoutCardOnPinLayoutSelected;
    }

    private void Update()
    {
        
    }

    public void ChooseLayout(LayoutEnum layoutType)
    {
        //switch
        //each case can call a function with some 2d array grid, just create it
        //case 1
        //call CreateLayout(2d array)
        int[][] grid = layoutType switch
        {
            LayoutEnum.Triangle => new[]
            {
                new[] { 1, 0, 1, 0, 1, 0, 1 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
                new[] { 0, 0, 1, 0, 1, 0, 0 },
                new[] { 0, 0, 0, 1, 0, 0, 0 },
            },
            LayoutEnum.Block => new[]
            {
                new[] { 1, 0, 1, 0, 1, 0, 1 },
                new[] { 1, 0, 1, 0, 1, 0, 1 },
                new[] { 0, 0, 1, 0, 1, 0, 0 },
                new[] { 0, 0, 0, 0, 0, 0, 0 },
            },
            LayoutEnum.Diamond => new[]
            {
                new[] { 0, 0, 0, 1, 0, 0, 0 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
                new[] { 1, 0, 0, 1, 0, 0, 1 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
            },
            LayoutEnum.ReverseTriangle => new[]
            {
                new[] { 0, 0, 0, 1, 0, 0, 0 },
                new[] { 0, 0, 1, 0, 1, 0, 0 },
                new[] { 0, 1, 0, 1, 0, 1, 0 },
                new[] { 1, 0, 1, 0, 1, 0, 1 },
            },
            _ => null
            // _ => throw new NotImplementedException()
            
        };

        CreateLayout(grid);
    }

    private void CreateLayout(int[][] grid)
    {
        float baseX = -0.6f;
        float baseZ = 0.75f;

        for (int z = 0; z < grid.Length; z++)
        {
            for (int x = 0; x < grid[z].Length; x++)
            {
                // Only spawn pin if 1
                if (grid[z][x] == 0)
                {
                    continue;
                }

                var pin = Instantiate(pinPrefab, pinParent);
                pin.transform.localPosition = new Vector3(baseX + x * 0.2f, 0, baseZ + z * 0.25f);

                // Debug.Log("X: " + z + " Y: " + x + " Value: " + grid[z][x]);
            }
        }
    }
}
