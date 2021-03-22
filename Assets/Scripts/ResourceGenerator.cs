using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private float timer;
    private float timerMax;
    private BuildingTypeSO buildingType;
    //private Dictionary<ResourceTypeSO, float> resourcesDictionary;

    private void Awake()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        /*
           foreach(ResourceGeneratorData resourceGenerator in buildingType.resourceGeneratorList)
           {
               resourcesDictionary.Add(resourceGenerator.resourceType, resourceGenerator.timerMax);
           }*/
        timerMax = buildingType.resourceGeneratorData.timerMax;
    }

    private void Update()
       {/*
           timer -= Time.deltaTime;
           if(resourcesDictionary.Count >= 1)
           {

           }
           */

        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            timer += timerMax;
            //Debug.Log("Ding! " + buildingType.resourceGeneratorData.resourceType.nameString);
            ResourceManager.Instance.AddResource(buildingType.resourceGeneratorData.resourceType, 1);
        }
    }
}
