using System.Threading.Tasks;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public Cat catDragged;

    private Vector3 dragStartPosition; //use to determine if the player click or start draging using the DISTANCE
    private float dragTimerStart; //use to determine if the player click or start draging using the TIME
    private bool allowDraging; //use to prevent instant draging when player only want to click

    private void Awake()
    {
        this.enabled = false; //disable Update at start
    }

    private void OnMouseDown()
    {
        //exceptions
        if (!CanDrag()) return;

        this.enabled = true; // enable Update
        dragTimerStart = 0;
        dragStartPosition = Input.mousePosition; // register pointer position

        HandManager.Instance.RemoveFromHand(catDragged.id);
        HandManager.Instance.HideHand();
    }

    private void OnMouseDrag()
    {
        //exceptions
        if (!CanDrag()) return;
        
        if (allowDraging)
        {
            catDragged.transform.position = new Vector3 (InputHandler.Instance.touchPos.x, InputHandler.Instance.touchPos.y + Registry.gameSettings.verticalOffsetDuringDrag, InputHandler.Instance.touchPos.z);
        }
    }
 
    private void OnMouseUp()
    {
        //exceptions
        if (!CanDrag()) return;

        this.enabled = false;

        if (!allowDraging)
        {
            Debug.Log($"Single click on cat that trigger explication layout.");
        }

        allowDraging = false;
        DragAndDropPlane.Instance.meshCollider.enabled = false;
        VerifyDistances();
        HandManager.Instance.ShowHand();
    }

    private void Update()
    {
        dragTimerStart += Time.deltaTime;
        //check if the drag is large enough or long enough to determine if it is a drag or just a click
        if ((Input.mousePosition - dragStartPosition).magnitude > Registry.gameSettings.dragingMinimumAmount || dragTimerStart > Registry.gameSettings.holdingTimeMaxSingleClick)
        {
            allowDraging = true;
            catDragged.OnDrag();
            DragAndDropPlane.Instance.meshCollider.enabled = true;
            this.enabled = false;
        }
    }

    /// <summary>
    /// Check if OnMouse() function can be used:
    /// (1) if the cat can move (is in the player's hand)
    /// (2) if the player can access inputs
    /// </summary>
    private bool CanDrag()
    {
        return catDragged.CanMove() && 
               InputHandler.Instance.CanAccessInput();
    }
    
    /// <summary>
    /// Snap the object on the nearest pawn position if close enough
    /// Otherwise, it's get back to the player's hand
    /// </summary>
    private void VerifyDistances()
    {
        BattlePawn closestPawn = BattlefieldManager.Instance.GetNearestPawnFromCursor(new Vector2(transform.position.x, transform.position.y));
        Vector2 transformPos = new Vector2(catDragged.transform.position.x, catDragged.transform.position.y);
        
        // snap to the closest battle pawn
        // else gets back into the player's hand
        if (BattlefieldManager.Instance.IsCloseEnough(transformPos, closestPawn.transform.position))
        {
            // if there is already a cat on that battle pawn,
            // put the former cat into the graveyard and place the new one
            if (closestPawn.entityIdLinked != "")
            {
                if (Misc.GetCatById(closestPawn.entityIdLinked) != null)
                {
                    Misc.GetCatById(closestPawn.entityIdLinked).Withdraw();
                }
            }
            closestPawn.Setup(catDragged.id);
            catDragged.UpdateBattlePosition(closestPawn.battlePosition);
            catDragged.PlaceOnBattlefield();
        }
        else
        {
            catDragged.PutInHand();
            HandManager.Instance.AddToHand(catDragged.id);
        }
    }

    private async void SingleClickChecker()
    {
        await Task.Delay((int)(Registry.gameSettings.holdingTimeMaxSingleClick * 1000));
    }
}