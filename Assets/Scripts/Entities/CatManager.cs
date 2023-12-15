using System.Collections.Generic;
using Player;
using UnityEngine;

public class CatManager : MonoBehaviour
{
    public static CatManager Instance;
    
    [Header("DEBUGGING")]
    public List<Cat> cats;
    public int totalCatCount;
    public int deadCatAmount;

    public bool allCatsDead => deadCatAmount == totalCatCount;
    
    private Cat _spawnedCat;
    private GameObject _newCatGO;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        cats = new List<Cat>();
    }
    
    /// <summary>
    /// Link all cat type's specificities into the new instantiated cat.
    /// </summary>
    /// <param name="item">Type of the cat</param>
    public void SpawnCatGraphics(Item item)
    {
        Cat newCat = Instantiate(Registry.entitiesConfig.cats[0].basePrefab, transform).GetComponent<Cat>();
        
        // setup the cat
        newCat.Initialize(item.entityIndex, item.health);
        newCat.ability = Registry.entitiesConfig.cats[item.entityIndex].ability;
        newCat.autoAttacks = Registry.entitiesConfig.cats[item.entityIndex].autoAttack;
        
        // register the entity in lists 
        cats.Add(newCat);
        EntityManager.Instance.entities.Add(newCat);
        DeckManager.Instance.AddCat(newCat.id);
        
        // name the cat's game object
        newCat.gameObject.name = $"Cat_{totalCatCount}_{Registry.entitiesConfig.cats[item.entityIndex].entityName}";
        newCat.graphicsParent.SetActive(false);

        totalCatCount++;
    }
}