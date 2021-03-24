using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObject;
    private ResourceNearbyOverlay resourceNearbyOverlay;

    private void Awake()
    {
        spriteGameObject = transform.Find("Sprite").gameObject;
        resourceNearbyOverlay = transform.Find("pfResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();

        Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        BuildingManager.Instance.OnBuildingPlacementTrial += BuildingManager_OnBuildingPlacementTrial;
    }

    private void BuildingManager_OnBuildingPlacementTrial(object sender, BuildingManager.OnBuildingPlacementTrialEventArgs e)
    {
        ShowAvailability(e.canBePlaced);
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if(e.activeBuildingType == null)
        {
            Hide();
            resourceNearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void ShowAvailability(bool canBePlaced)
    {
        if (canBePlaced)
        {
            spriteGameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.6f);
        }
        else
        {
            spriteGameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.4f);
        }
    }

    private void Hide()
    {
        spriteGameObject.SetActive(false);
    }
}
