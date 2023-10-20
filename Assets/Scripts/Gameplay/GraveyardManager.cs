using System.Collections.Generic;
using UnityEngine;

public class GraveyardManager : MonoBehaviour
{
    public static GraveyardManager Instance;

    [Header("DEBUGGING")] 
    public List<string> catsInGraveyard;

    private void Awake() => Instance = this;

    public void Initialize()
    {
        Instance.catsInGraveyard = new List<string>();
    }
    
    public void AddCat(string _cat)
    {
        Instance.catsInGraveyard.Add(_cat); 
    }
}