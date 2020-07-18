﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStructureInfoPanelHelper : MonoBehaviour
{
    public TextMeshProUGUI nameText, incomeText, upkeepText, clientText;
    public Toggle powerToggle, waterToggle, roadToggle;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DisplayBasicStructureInfo(StructureBaseSO data)
    {
        Show();
        HideElement(clientText.gameObject);
        HideElement(powerToggle.gameObject);
        HideElement(waterToggle.gameObject);
        HideElement(roadToggle.gameObject);
        SetText(nameText, data.buildingName);
        SetText(incomeText, data.GetIncome() + "");
        SetText(upkeepText, data.upkeepCost + "");
    }

    public void DisplayZoneStructureInfo(ZoneStructureSO data)
    {
        Show();
        HideElement(clientText.gameObject);
        SetText(nameText, data.buildingName);
        SetText(incomeText, data.GetIncome() + "");
        SetText(upkeepText, data.upkeepCost + "");
        if(data.requirePower)
        {
            SetToggle(powerToggle, data.HasPower());
        }
        else
        {
            HideElement(powerToggle.gameObject);
        }

        if (data.requireRoadAccess)
        {
            SetToggle(roadToggle, data.HasRoadAccess());
        }
        else
        {
            HideElement(roadToggle.gameObject);
        }

        if (data.requireWater)
        {
            SetToggle(waterToggle, data.HasWater());
        }
        else
        {
            HideElement(waterToggle.gameObject);
        }
    }

    public void DisplayFacilityStructureInfo(SingleFacilitySO data)
    {
        Show();
        SetText(nameText, data.buildingName);
        SetText(incomeText, data.GetIncome() + "");
        SetText(upkeepText, data.upkeepCost + "");
        if(data.requirePower)
        {
            SetToggle(powerToggle, data.HasPower());
        }
        else
        {
            HideElement(powerToggle.gameObject);
        }

        if (data.requireRoadAccess)
        {
            SetToggle(roadToggle, data.HasRoadAccess());
        }
        else
        {
            HideElement(roadToggle.gameObject);
        }

        if (data.requireWater)
        {
            SetToggle(waterToggle, data.HasWater());
        }
        else
        {
            HideElement(waterToggle.gameObject);
        }
        SetText(clientText, data.GetNumberOfCustomers() + "/" + data.maxCustomers);
    }

    private void HideElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(false);
    }

    private void ShowElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(true);
    }

    private void SetText(TextMeshProUGUI element, string value)
    {
        ShowElement(element.gameObject);
        element.text = value;
    }

    private void SetToggle(Toggle element, bool value)
    {
        ShowElement(element.gameObject);
        element.isOn = value;
    }
}
