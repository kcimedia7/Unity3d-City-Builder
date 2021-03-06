﻿using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class StructureUpgradeHelperTests
    {
        private GameObject _gridPosition1GameObject = null;
        private GameObject _gridPosition2GameObject = null;
        private GridStructure _grid;
        private GameObject _structureObject = new GameObject();
  
        private Vector3 _gridPosition1 = new Vector3(3, 0, 3);
        private Vector3 _gridPosition2 = new Vector3(6, 0, 6);
        private Vector3Int _gridPosition1Int;
        private StructureModificationHelper _structureModificationHelper;

        [SetUp]
        public void Init()
        {
            StructureRepository structureRepository = TestHelpers.CreateStructureRepositoryContainingZoneStructure();
            IPlacementManager placementManager = Substitute.For<IPlacementManager>();
            _gridPosition1Int = Vector3Int.FloorToInt(_gridPosition1);
            placementManager.CreateGhostStructure(default, default).ReturnsForAnyArgs(_gridPosition1GameObject);
            _grid = new GridStructure(3, 10, 10);
            IResourceManager resourceManager = Substitute.For<IResourceManager>();
            resourceManager.CanIBuyIt(default, default, default).Returns(true);
            _structureModificationHelper = new StructureUpgradeHelper(structureRepository, _grid, placementManager, resourceManager);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneHasAlreadyUpgraded()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(0, 0, 0));
            residentialZone.fullyUpgraded = true;
            Assert.True(residentialZone.HasFullyUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneHasNotUpgraded()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasFullyUpgraded());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeAddStructureToBeModifiedPasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }         
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeAddStructureToBeModifiedFails()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition2);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreNotEqual(_gridPosition1GameObject, objectInDictionary);
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeAddNewStructureDataForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.AreEqual(residentialZone, objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeAddNewStructureDataForUpgradeFails()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            ZoneStructureSO residentialZone2 = CreateResidentialZoneAtPosition(new Vector3Int(6, 0, 6));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.AreNotEqual(residentialZone2, objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeAddOldStructureForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInOldStructureDictionary);
        }          
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeAddOldStructureForUpgradeFails()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition2);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.AreNotEqual(_gridPosition1GameObject, objectInOldStructureDictionary);
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeSetOldStructureGameObjectToInActiveForUpgradeUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsTrue(objectInOldStructureDictionary.activeSelf == false);
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradeSetOldStructureGameObjectToInActiveForUpgradeUpgradeFails()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsFalse(objectInOldStructureDictionary.activeSelf == true);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtRemoveStructureToBeModifiedPasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }               

        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtRemoveOldStructureForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsNull(objectInOldStructureDictionary);
        }

        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtSetOldStructureGameObectToActivePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            var structureGameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            Assert.IsTrue(structureGameObject.activeSelf == true);
        }

        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtSetOldStructureGameObectToActiveFails()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            var structureGameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            Assert.IsFalse(structureGameObject.activeSelf == false);
        }

        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtRemoveNewStuctureDataForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneCancelUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
            Assert.IsNull(objectInOldStructureDictionary);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneConfirmUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(residentialZone.HasFullyUpgraded());
        }           
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneConfirmDestroyOldStructuresForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsNull(objectInOldStructureDictionary);
        }         
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneConfirmPlaceUpgradedStructuresOnTheMapUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInNewStructureDictionary);
        }             

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeAddStructureToBeModifiedPasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeAddStructureToBeModifiedFails()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition2);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreNotEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeAddNewStructureDataForUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.AreEqual(commercialZone, objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeAddNewStructureDataForUpgradeFails()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            ZoneStructureSO commercialZone2 = CreateCommercialZoneAtPosition(new Vector3Int(6, 0, 6));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.AreNotEqual(commercialZone2, objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeAddOldStructureForUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInOldStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeAddOldStructureForUpgradeFails()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition2);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.AreNotEqual(_gridPosition1GameObject, objectInOldStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeSetOldStructureGameObjectToInActiveForUpgradeUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsTrue(objectInOldStructureDictionary.activeSelf == false);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradeSetOldStructureGameObjectToInActiveForUpgradeUpgradeFails()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsFalse(objectInOldStructureDictionary.activeSelf == true);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneRevokeStructureUpgradePlacementAtRemoveStructureToBeModifiedPasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        [Test]
        public void CommercialZoneRevokeStructureUpgradePlacementAtRemoveOldStructureForUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsNull(objectInOldStructureDictionary);
        }

        [Test]
        public void CommercialZoneRevokeStructureUpgradePlacementAtSetOldStructureGameObectToActivePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            var structureGameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            Assert.IsTrue(structureGameObject.activeSelf == true);
        }

        [Test]
        public void CommercialZoneRevokeStructureUpgradePlacementAtSetOldStructureGameObectToActiveFails()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            var structureGameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            Assert.IsFalse(structureGameObject.activeSelf == false);
        }

        [Test]
        public void CommercialZoneRevokeStructureUpgradePlacementAtRemoveNewStuctureDataForUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneCancelUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
            Assert.IsNull(objectInOldStructureDictionary);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneConfirmUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(commercialZone.HasFullyUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneConfirmDestroyOldStructuresForUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsNull(objectInOldStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneConfirmPlaceUpgradedStructuresOnTheMapUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInNewStructureDictionary);
        }       
        
        // A Test behaves as an ordinary method
        [Test]
        public void AgricultureZoneSelectForUpgradePasses()
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void AgricultureZoneCancelUpgradePasses()
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void AgricultureZoneConfirmUpgradePasses()
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(agricultureZone.HasFullyUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeAddStructureToBeModifiedPasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeAddStructureToBeModifiedFails()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition2);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreNotEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeAddNewStructureDataForUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.AreEqual(powerPlant, objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeAddNewStructureDataForUpgradeFails()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            ZoneStructureSO commercialZone2 = CreateCommercialZoneAtPosition(new Vector3Int(6, 0, 6));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.AreNotEqual(commercialZone2, objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeAddOldStructureForUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInOldStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeAddOldStructureForUpgradeFails()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _gridPosition1GameObject = _grid.GetStructureFromTheGrid(_gridPosition2);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.AreNotEqual(_gridPosition1GameObject, objectInOldStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeSetOldStructureGameObjectToInActiveForUpgradeUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsTrue(objectInOldStructureDictionary.activeSelf == false);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradeSetOldStructureGameObjectToInActiveForUpgradeUpgradeFails()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsFalse(objectInOldStructureDictionary.activeSelf == true);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilityRevokeStructureUpgradePlacementAtRemoveStructureToBeModifiedPasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        [Test]
        public void PowerPlantSingleFacilityRevokeStructureUpgradePlacementAtRemoveOldStructureForUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsNull(objectInOldStructureDictionary);
        }

        [Test]
        public void PowerPlantSingleFacilityRevokeStructureUpgradePlacementAtSetOldStructureGameObectToActivePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            var structureGameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            Assert.IsTrue(structureGameObject.activeSelf == true);
        }

        [Test]
        public void PowerPlantSingleFacilityRevokeStructureUpgradePlacementAtSetOldStructureGameObectToActiveFails()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            var structureGameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            Assert.IsFalse(structureGameObject.activeSelf == false);
        }

        [Test]
        public void PowerPlantSingleFacilityRevokeStructureUpgradePlacementAtRemoveNewStuctureDataForUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilityCancelUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
            Assert.IsNull(objectInOldStructureDictionary);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilityConfirmUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(powerPlant.HasFullyUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilityConfirmDestroyOldStructuresForUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsNull(objectInOldStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilityConfirmPlaceUpgradedStructuresOnTheMapUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void WaterTowerSingleFacilitySelectForUpgradePasses()
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void WaterTowerSingleFacilityCancelUpgradePasses()
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void WaterTowerSingleFacilityConfirmUpgradePasses()
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(waterTower.HasFullyUpgraded());
        }


        // A Test behaves as an ordinary method
        [Test]
        public void SiloSingleFacilitySelectForUpgradePasses()
        {
            SingleFacilitySO silo = CreateSiloSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_gridPosition1GameObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void SiloSingleFacilityCancelUpgradePasses()
        {
            SingleFacilitySO silo = CreateSiloSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void SiloSingleFacilityConfirmUpgradePasses()
        {
            SingleFacilitySO silo = CreateSiloSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(silo.HasFullyUpgraded());
        }

        private static ZoneStructureSO CreateResidentialZone()
        {
            ZoneStructureSO residentialZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            residentialZone.buildingName = "ResidentialZone";
            residentialZone.zoneType = ZoneType.Residential;
            residentialZone.requireRoadAccess = true;
            residentialZone.requirePower = true;
            residentialZone.requireWater = true;
            residentialZone.upgradable = true;
            residentialZone.fullyUpgraded = false;
            residentialZone.upkeepCost = 0;
            residentialZone.prefab = TestPrefab;
            residentialZone.maxFacilitySearchRange = 2;
            residentialZone.upgradeIncome = new int[1] { 1 };
            residentialZone.upgradePlacementCost = new int[1] { 0 };
            residentialZone.upgradeLevelPrefabs = new GameObject[1] { TestPrefab2 };
            residentialZone.upgradedResidentsAmount = new int[1] { 0 };
            residentialZone.upgradeRequiredSteelAmount = new int[1] { 0 };
            residentialZone.upgradeRequiredWoodAmount = new int[1] { 0 };
            return residentialZone;
        }        
        
        private static ZoneStructureSO CreateCommercialZone()
        {
            ZoneStructureSO commercialZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            commercialZone.buildingName = "Commercial";
            commercialZone.zoneType = ZoneType.Commercial;
            commercialZone.requireRoadAccess = true;
            commercialZone.requirePower = true;
            commercialZone.requireWater = true;
            commercialZone.upgradable = true;
            commercialZone.fullyUpgraded = false;
            commercialZone.upkeepCost = 0;
            commercialZone.prefab = TestPrefab;
            commercialZone.maxFacilitySearchRange = 2;
            commercialZone.upgradeIncome = new int[1] { 1 };
            commercialZone.upgradePlacementCost = new int[1] { 1 };
            commercialZone.upgradeLevelPrefabs = new GameObject[1] { TestPrefab2 };
            commercialZone.upgradeRequiredSteelAmount = new int[1] { 0 };
            commercialZone.upgradeRequiredWoodAmount = new int[1] { 0 };
            return commercialZone;
        }

        private static ZoneStructureSO CreateAgricultureZone()
        {
            ZoneStructureSO agricultureZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            agricultureZone.buildingName = "Agriculture";
            agricultureZone.zoneType = ZoneType.Agridcultural;
            agricultureZone.requireRoadAccess = true;
            agricultureZone.requirePower = true;
            agricultureZone.requireWater = true;
            agricultureZone.upgradable = true;
            agricultureZone.fullyUpgraded = false;
            agricultureZone.upkeepCost = 0;
            agricultureZone.prefab = TestPrefab;
            agricultureZone.maxFacilitySearchRange = 2;
            agricultureZone.upgradeIncome = new int[1] { 1 };
            agricultureZone.upgradePlacementCost = new int[1] { 1 };
            agricultureZone.upgradeLevelPrefabs = new GameObject[1] { TestPrefab2 };
            agricultureZone.upgradeRequiredSteelAmount = new int[1] { 0 };
            agricultureZone.upgradeRequiredWoodAmount = new int[1] { 0 };
            return agricultureZone;
        }

        private static SingleFacilitySO CreatePowerPlantSingleFacility()
        {
            SingleFacilitySO powerPlant = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            powerPlant.buildingName = "Power plant";
            powerPlant.facilityType = FacilityType.Power;
            powerPlant.requireRoadAccess = true;
            powerPlant.requirePower = false;
            powerPlant.requireWater = false;
            powerPlant.upgradable = true;
            powerPlant.fullyUpgraded = false;
            powerPlant.upkeepCost = 0;
            powerPlant.prefab = TestPrefab;
            powerPlant.upgradePlacementCost = new int[1] { 1 };
            powerPlant.maxCustomersUpgraded = new int[1] { 1 };
            powerPlant.upgradeLevelPrefabs = new GameObject[1] { TestPrefab2 };
            powerPlant.upgradeRequiredSteelAmount = new int[1] { 0 };
            powerPlant.upgradeRequiredWoodAmount = new int[1] { 0 };
            return powerPlant;
        }

        private static SingleFacilitySO CreateWaterTowerSingleFacility()
        {
            SingleFacilitySO waterTower = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            waterTower.buildingName = "Water Tower";
            waterTower.facilityType = FacilityType.Water;
            waterTower.requireRoadAccess = true;
            waterTower.requirePower = false;
            waterTower.requireWater = false;
            waterTower.upgradable = true;
            waterTower.fullyUpgraded = false;
            waterTower.upkeepCost = 0;
            waterTower.prefab = TestPrefab;
            waterTower.upgradePlacementCost = new int[1] { 1 };
            waterTower.maxCustomersUpgraded = new int[1] { 1 };
            waterTower.upgradeLevelPrefabs = new GameObject[1] { TestPrefab2 };
            waterTower.upgradeRequiredSteelAmount = new int[1] { 0 };
            waterTower.upgradeRequiredWoodAmount = new int[1] { 0 };
            return waterTower;
        }

        private static SingleFacilitySO CreateSiloSingleFacility()
        {
            SingleFacilitySO silo = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            silo.buildingName = "Silo";
            silo.facilityType = FacilityType.Silo;
            silo.requireRoadAccess = true;
            silo.requirePower = false;
            silo.requireWater = false;
            silo.upgradable = true;
            silo.fullyUpgraded = false;
            silo.upkeepCost = 0;
            silo.prefab = TestPrefab;
            silo.upgradePlacementCost = new int[1] { 1 };
            silo.maxCustomersUpgraded = new int[1] { 1 };
            silo.upgradeLevelPrefabs = new GameObject[1] { TestPrefab2 };
            silo.upgradeRequiredSteelAmount = new int[1] { 0 };
            silo.upgradeRequiredWoodAmount = new int[1] { 0 };
            return silo;
        }

        private ZoneStructureSO CreateResidentialZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO residentialZone = CreateResidentialZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, residentialZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return residentialZone;
        }        
        
        private ZoneStructureSO CreateCommercialZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO commercialZone = CreateCommercialZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, commercialZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return commercialZone;
        }

        private ZoneStructureSO CreateAgricultureZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, agricultureZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return agricultureZone;
        }

        private SingleFacilitySO CreatePowerPlantSingleFacilityAtPosition(Vector3Int position)
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacility();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, powerPlant);
            StructureEconomyManager.PrepareFacilityStructure(position, _grid);
            return powerPlant;
        }

        private SingleFacilitySO CreateWaterTowerSingleFacilityAtPosition(Vector3Int position)
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacility();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, waterTower);
            StructureEconomyManager.PrepareFacilityStructure(position, _grid);
            return waterTower;
        }

        private SingleFacilitySO CreateSiloSingleFacilityAtPosition(Vector3Int position)
        {
            SingleFacilitySO silo = CreateSiloSingleFacility();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, silo);
            StructureEconomyManager.PrepareFacilityStructure(position, _grid);
            return silo;
        }
    }
}
