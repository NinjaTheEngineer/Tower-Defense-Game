using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    //[SerializeField] private Transform mouseVisualTransform;
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public event EventHandler<OnBuildingPlacementTrialEventArgs> OnBuildingPlacementTrial;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }
    public class OnBuildingPlacementTrialEventArgs : EventArgs
    {
        public bool canBePlaced;
    }

    private Camera mainCamera;

    private bool canBuild;

    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //mouseVisualTransform.position = GetMouseWorldPosition();
        canBuild = CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition());
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null && canBuild)
            {
                if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                {
                    ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                    Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                }
            }
        }

    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;

        OnActiveBuildingTypeChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position)
    {
        if(buildingType == null)
        {
            return false;
        }
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            OnBuildingPlacementTrial?.Invoke(this,
                new OnBuildingPlacementTrialEventArgs { canBePlaced = false });
            return false;
        }


        collider2DArray = Physics2D.OverlapCircleAll(position, buildingType.minConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            //Colliders inside the construction raidus;
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            {
                if (buildingTypeHolder.buildingType == buildingType)
                {
                    OnBuildingPlacementTrial?.Invoke(this,
                        new OnBuildingPlacementTrialEventArgs { canBePlaced = false });
                    return false;
                }
            }
        }

        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            //Colliders inside the construction raidus;
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();

            if (buildingTypeHolder != null)
            {
                OnBuildingPlacementTrial?.Invoke(this,
                    new OnBuildingPlacementTrialEventArgs { canBePlaced = true });
                return true;
            }
        }

        OnBuildingPlacementTrial?.Invoke(this,
            new OnBuildingPlacementTrialEventArgs { canBePlaced = false });
        return false;
    }
}
