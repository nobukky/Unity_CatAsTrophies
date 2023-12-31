using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositionConfig", menuName = "Config/Entity/Composition", order = 3)]
public class CompositionConfig : ScriptableObject
{
    public string id;
    public bool isPlayerDeck;

    public CompositionTier tier;
    public string compositionName;
    public List<EntityConfig> entities = new List<EntityConfig>();

    public void Initialize()
    {
        for (int i = 0; i < 3; i++)
        {
            entities.Add(new EntityConfig());
        }
    }
}

public enum CompositionTier
{
    SIMPLE = 0,
    ELITE,
    BOSS
}