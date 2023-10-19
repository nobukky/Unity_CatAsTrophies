using System;
using System.Threading.Tasks;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    
    [Header("DEBUGGING")]
    public TurnState state;
    public int turnCounter;
    public int actionCounter;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        turnCounter = 0;
    }
    
    public async void HandleTurnState()
    {
        switch (state)
        {
            case TurnState.PlayerTurn:
                
                turnCounter++;
                
                // discard the hand and fulfill it back
                // if there is no more cats in the deck, shuffle the graveyard into the deck
                // handle the case where no cats can be draw
                FulfillHand(5);
                Debug.Log("TURNMANAGER: the player's is filled");
                
                // await for the player to perform three actions:
                // 1. place/replace cat on the battlefield to trigger their ability
                // 2. activate the ability of a cat that is already on the battlefield
                await HandlePlayerActions();
                Debug.Log("TURNMANAGER: the player did its three actions");
                
                // cats use their auto attacks
                BattlefieldManager.Instance.AutoAttackEnemies();
                Debug.Log("TURNMANAGER: cats attacked enemies");
                
                // give the turn to the enemies
                state = TurnState.EnemyTurn;
                HandleTurnState();
                
                break;
            
            case TurnState.EnemyTurn:
                
                // enemies uses their auto attacks
                Debug.Log("TURNMANAGER: enemies attacked cats");
                
                // re-launched a new turn
                state = TurnState.PlayerTurn;
                HandleTurnState();
                
                break;
        }
    }

    private void FulfillHand(int _catCount)
    {
        HandManager.Instance.DiscardHand();
        for (int i = 0; i < _catCount; i++)
        {
            HandManager.Instance.DrawCat(Misc.GetCatById(CatGenerator.Instance.cats, DeckManager.Instance.RemoveCat()).typeIndex);
        }
    }

    private async Task HandlePlayerActions()
    {
        actionCounter = 0;
        while (actionCounter < 3)
        {
            await Task.Delay(250);
        }
    }
}

public enum TurnState
{
    PlayerTurn = 0,
    EnemyTurn
}