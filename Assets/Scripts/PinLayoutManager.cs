using System;
using NUnit.Framework.Internal.Filters;
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
    }

    private void OnDestroy()
    {
        PinLayoutCard.PinLayoutSelected -= PinLayoutCardOnPinLayoutSelected;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChooseLayout(LayoutEnum.Triangle);
        }
    }

    private void ChooseLayout(LayoutEnum layoutType)
    {
        //switch
        //each case can call a function with some 2d array grid, just create it
        //case 1
        //call CreateLayout(2d array)
        int[][] grid;
        switch(layoutType)
        {
            case LayoutEnum.Triangle:
                grid = new int[][]
                {
                    new int[] {1, 0, 1, 0, 1, 0, 1}, 
                    new int[] {0, 1, 0, 1, 0, 1, 0},
                    new int[] {0, 0, 1, 0, 1, 0, 0},
                    new int[] {0, 0, 0, 1, 0, 0, 0}
                };
                CreateLayout(grid);
                break;
            case LayoutEnum.Block:
                grid = new int[][]
                {
                    new int[] { 1, 0, 1, 0, 1, 0, 1 },
                    new int[] { 1, 0, 1, 0, 1, 0, 1 },
                    new int[] { 0, 0, 1, 0, 1, 0, 0 },
                    new int[] { 0, 0, 0, 0, 0, 0, 0 }
                };
                CreateLayout(grid);
                break;
            case LayoutEnum.Diamond:
                grid = new int[][]
                {
                    new int[] { 0, 0, 0, 1, 0, 0, 0 },
                    new int[] { 0, 1, 0, 1, 0, 1, 0 },
                    new int[] { 1, 0, 0, 1, 0, 0, 1 },
                    new int[] { 0, 1, 0, 1, 0, 1, 0 }
                };
                CreateLayout(grid);
                break;
            // case LayoutEnum.ReverseTriangle:
            //     grid = new int[][]
            //     {
            //         new int[] {0, 0, 0, 1, 0, 0, 0},
            //         new int[] {0, 0, 1, 0, 1, 0, 0},
            //         new int[] {0, 1, 0, 1, 0, 1, 0},
            //         new int[] {1, 0, 1, 0, 1, 0, 1}
            //     };
            //     CreateLayout(grid);
            //     break;
        }
    }

    private void CreateLayout(int[][] grid)
    {
        float xOffset = .6f, zOffset = -.75f;
        foreach (int[] row in grid)
        {
            xOffset = .6f;
            foreach (int cell in row)
            {
                //place pin
                //check if pin is there
                if (cell == 1)
                {
                    // //Instantiate pin under pin parent, hi :)
                    // GameObject pin = Instantiate(pinPrefab, pinParent);
                    // //move to offset
                    // pin.transform.position = new Vector3(xOffset, 0, zOffset);
                    Instantiate(pinPrefab, pinParent);
                    pinPrefab.transform.position = new Vector3(xOffset, 0, zOffset);
                    Debug.Log("X: " + xOffset + " Z: " + zOffset);
                }
                //decrement
                xOffset -= .2f;
            }

            zOffset += .25f;
        }
    }
}
