using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Events", menuName = "Middle-Men/Events", order = 1)]
public class Events : ScriptableObject
{
    public Action OnNewPlayerTurn;
    public Action OnEndPlayerTurn;
    public Action OnNewEnemyTurn;
    public Action OnEndEnemyTurn;

    public Action OnCatsUseAutoAttack;
    public Action OnEnemiesUseAutoAttack;

    // Communication between InputHandler.cs and HandManager.cs
    public Action OnClickNotCat;
    public Action<string, string> OnCatStacked;
    public Action OnCatDestacked;

    // Scene management
    public Action OnSceneLoaded;

    //Sound Management
    public Action AttackSound;
    public Action DebuffSound;
    public Action BuffSound;
    public Action HealSound;
    public Action onRestClick;
}