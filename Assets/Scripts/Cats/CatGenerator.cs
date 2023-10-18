using System.Collections.Generic;
using UnityEngine;

public class CatGenerator : MonoBehaviour
{
    public static CatGenerator Instance;
    
    [Header("DEBUGGING")]
    public List<Cat> cats;
    
    private int totalCatCount;
    private Cat spawnedCat;
    private GameObject spawnedCatGO;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        InstantiateCats();
    }

    private void InstantiateCats()
    {
        for (int i = 0; i < Registry.playerConfig.deckLenght; i++)
        {
            var newCat = Instantiate(Registry.catConfig.cats[0].catBasePrefab, transform).GetComponent<Cat>();
            newCat.gameObject.SetActive(false);
            cats.Add(newCat);
            
            // add this new cat into the deck
            DeckManager.Instance.AddCat(newCat.id);
        }
    }
    
    /// <summary>
    /// Depool cat from the pool the instantiate them 
    /// </summary>
    /// <param name="_typeIndex">Type of the cat</param>
    /// <param name="_pos">Position of the cat</param>
    public Cat SpawnCatGraphics(int _typeIndex, Vector3 _pos)
    {
        // get cat game object and place it
        spawnedCatGO = cats[totalCatCount].gameObject;
        spawnedCatGO.transform.position = _pos;
        spawnedCatGO.name = $"Cat_{totalCatCount}_{Registry.catConfig.cats[_typeIndex].catName}";

        // setup the cat
        spawnedCat = spawnedCatGO.GetComponent<Cat>();
        spawnedCat.Initialize(_typeIndex);
        spawnedCatGO.SetActive(true);
        totalCatCount++;

        return spawnedCat;
    }
}