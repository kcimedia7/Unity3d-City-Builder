﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureRepository : MonoBehaviour
{
    public CollectionSO modelDataCollection;

    public List<string> GetZoneNames()
    {
        return modelDataCollection.zoneStructures.Select(zone => zone.buildingName).ToList();
    }

    public List<string> GetSingleStructureNames()
    {
        return modelDataCollection.singleStructures.Select(facility => facility.buildingName).ToList();
    }

    public string GetRoadStructureName()
    {
        return modelDataCollection.roadStructure.buildingName;
    }

    public GameObject GetBuildingPrefabByName(string structureName, StructureType structureType)
    {
        GameObject structurePrefabToReturn = null;
        switch(structureType)
        {
            case StructureType.Zone:
                structurePrefabToReturn = GetZoneBuildingPrefabByName(structureName);
                break;
            case StructureType.SingleStructure:
                structurePrefabToReturn = GetSingleStructureBuildingPrefabByName(structureName);
                break;
            case StructureType.Road:
                structurePrefabToReturn = GetRoadBuildingPrefab();
                break;
            default:
                throw new System.Exception("No such type" + structureType);
        }

        if(structurePrefabToReturn == null)
        {
            throw new Exception("No prefab for that name " + structureName);
        }

        return structurePrefabToReturn;
    }

    public GameObject GetUpgradeBuildingPrefab(StructureBaseSO structureData)
    {
        GameObject upgradeStructurePrefabToReturn = null;
        Type structureDataType = structureData.GetType();
        if(structureDataType == typeof(ZoneStructureSO))
        {
            foreach (var structure in modelDataCollection.zoneStructures)
            {
                if (((ZoneStructureSO)structureData).zoneType == structure.zoneType)
                {
                    upgradeStructurePrefabToReturn = structure.upgradePrefabVariants[0];
                }
            }
        }
        else if(structureDataType == typeof(SingleFacilitySO))
        {
            upgradeStructurePrefabToReturn = ((SingleFacilitySO)structureData).upgradePrefab;
        }
        return upgradeStructurePrefabToReturn;
    }

    public int GetStructureUpgradeIncome(StructureBaseSO structureData)
    {
        int upgradeAmountToReturn = 0;
        Type structureDataType = structureData.GetType();
        if (structureDataType == typeof(ZoneStructureSO))
        {
            upgradeAmountToReturn = ((ZoneStructureSO)structureData).upgradedIncome;
        }

        return upgradeAmountToReturn;
    }

    private GameObject GetZoneBuildingPrefabByName(string structureName)
    {
        var foundStructure = modelDataCollection.zoneStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
        if(foundStructure != null)
        {
            return foundStructure.prefab;
        }
        return null;
    }

    public StructureBaseSO GetStructureData(string structureName, StructureType structureType)
    {
        switch (structureType)
        {
            case StructureType.Zone:
                return modelDataCollection.zoneStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
            case StructureType.SingleStructure:
                return modelDataCollection.singleStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
            case StructureType.Road:
                return modelDataCollection.roadStructure;
            case StructureType.None:
                return null;
        }

        return null;
    }

    private GameObject GetSingleStructureBuildingPrefabByName(string structureName)
    {
        var foundStructure = modelDataCollection.singleStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
        if(foundStructure != null)
        {
            return foundStructure.prefab;
        }
        return null;
    }

    private GameObject GetRoadBuildingPrefab()
    {
        return modelDataCollection.roadStructure.prefab;
    }

}

public enum StructureType
{
    Zone,
    SingleStructure,
    Road,
    None
}
