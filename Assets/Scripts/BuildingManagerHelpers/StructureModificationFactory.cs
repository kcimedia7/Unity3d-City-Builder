﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureModificationFactory
{
    private static StructureModificationHelper _singleStructurePlacementHelper;
    private static StructureModificationHelper _structureRemovalHelper;
    private static StructureModificationHelper _roadStructurePlacementHelper;
    private static StructureModificationHelper _zonePlacementHelper;

    public static void PrepareFactory(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager)
    {
        _singleStructurePlacementHelper = new SingleStructurePlacementHelper(structureRepository, grid, placementManager);
        _structureRemovalHelper = new StructureRemovalHelper(structureRepository, grid, placementManager);
        _roadStructurePlacementHelper = new RoadPlacementModificationHelper(structureRepository, grid, placementManager);
        _zonePlacementHelper = new ZonePlacementHelper(structureRepository, grid, placementManager, Vector3.zero);
    }

    public static StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerRemoveBuildingState))
        {
            return _structureRemovalHelper;
        }
        else if(classType == typeof(PlayerBuildingZoneState))
        {
            return _zonePlacementHelper;
        }
        else if(classType == typeof(PlayerBuildingRoadState))
        {
            return _roadStructurePlacementHelper;
        }
        else
        {
            return _singleStructurePlacementHelper;
        }
    }
}
