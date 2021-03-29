 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;
    public float constructionTimerMax;

    public string GetConstructionResourceCostString()
    {
        string str = "";
        foreach(ResourceAmount resource in constructionResourceCostArray)
        {
            str += "<color=#" + resource.resourceType.colorHex +">" +
                resource.resourceType.nameShort + ":" + resource.amount + 
                " </color>";
        }
        return str;
    }
}
