using System.Linq;
using UnityEngine;
using Data;

public class MapManager : MonoBehaviour
{
    [Header("REFERENCE")]
    public MapView mapView;
    public GameObject mapCanvasParent;
    public RSE_DebugLog rseDebugLog;
    
    [Header("DEBUGGING")]
    public Map currentMap;

    public void ShowCanvasNormal()
    {
        mapView.showEscapeButton = true;
        mapView.isInteractible = true;
        ShowCanvas();
    }

    public void ShowCanvasUninteractible()
    {
        mapView.showEscapeButton = true;
        mapView.isInteractible = false;
        ShowCanvas();
    }

    public void ShowCanvasLocked()
    {
        mapView.showEscapeButton = false;
        mapView.isInteractible = true;
        ShowCanvas();
    }

    private void ShowCanvas()
    {
        mapCanvasParent.SetActive(true);
        Initialize();
    }
    public void HideCanvas()
    {
        mapCanvasParent.SetActive(false);
        mapView.ClearMap();
    }

    /// <summary>
    /// Generate a new map if the old one, doesn't exists or is completed.
    /// Otherwise, show the current map
    /// </summary>
    private void Initialize()
    {
        if (DataManager.data.map != null && DataManager.data.map.IsNotEmpty())
        {
            Map map = DataManager.data.map;
            
            // generate a new map, if the payer has already reached the boss 
            if (map.playerPath.Any(point => point.Equals(map.GetBossNode().point)) || 
                DataManager.data.endBattleStatus == EndBattleStatus.DEFEATED)
            {
                GenerateNewMap();
            }
            // load the current map, if player has not reached the boss yet
            else
            {
                map.UndoPlayerPath();
                currentMap = map;
                mapView.ShowMap(map);
            }
        }
        else
        {
            GenerateNewMap();
        }
    }
    
    /// <summary>
    /// Generates a new map based on the map config
    /// </summary>
    public void GenerateNewMap()
    {
        DataManager.data.playerStorage.ResetAllData();
        DataManager.data.playerStorage.SwitchToInGameDeck();
        
        if (DataManager.data.playerStorage.GetLenght(DataManager.data.playerStorage.inGameDeck) <= 0)
        {
            HideCanvas();
            rseDebugLog.Call("Your deck of cats is empty. Fill it before starting a new run.", Color.red);
            return;
        }
        
        currentMap = MapGenerator.GetMap(Registry.mapConfig);
        mapView.ShowMap(currentMap);
    }

    /// <summary>
    /// Save the current map into the persistant data
    /// </summary>
    public void SaveMap()
    {
        if (currentMap == null) return;

        DataManager.data.map = currentMap;
        DataManager.Save();
    }
    
    private void OnApplicationQuit()
    {
        SaveMap();
    }
}