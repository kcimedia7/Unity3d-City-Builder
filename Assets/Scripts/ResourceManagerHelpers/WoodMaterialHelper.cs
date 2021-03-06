﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodMaterialHelper 
{
    private int _woodAmount;

    public WoodMaterialHelper(int woodAmount)
    {
        this._woodAmount = woodAmount;
    }

    public int WoodAmount
    {
        get => _woodAmount;
        set
        {
            if (value < 0)
            {
                _woodAmount = 0;
            }
            else
            {
                _woodAmount = value;
            }
        }
    }

    public void ReduceWoodAmount(int amount)
    {
        WoodAmount -= amount;
    }

    public void AddWoodAmount(int amount)
    {
        WoodAmount += amount;
    }

    public void CalculateWoodAmount(IEnumerable<StructureBaseSO> buildings)
    {
        CollectWoodAmount(buildings);
    }

    private void CollectWoodAmount(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            if(structure.GetType() == typeof(ManufacturerBaseSO) && ((ManufacturerBaseSO)structure).ManufactureType == ManufactureType.Wood)
            {
                WoodAmount += ((ManufacturerBaseSO)structure).GetMaterialAmount();
            }
        }
    }
}
