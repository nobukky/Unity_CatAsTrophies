using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;
    
    [Header("REFERENCES")]
    public Transform[] handPoints;
    
    [Header("DEBUGGING")]
    public string[] catsInHand;
    public string newCatId;
    public Vector3[] handPointsDefaultPosition;

    private void Awake() => Instance = this;
    
    public void Initialize()
    {
        // hand limit is 5
        Instance.catsInHand = new string[] {null, null, null, null, null};
        //Save the position of hand point to reset hand later
        Instance.handPointsDefaultPosition = new Vector3[5];
        for (int i = 0; i < handPointsDefaultPosition.Length; i++)
        {
            handPointsDefaultPosition[i] = handPoints[i].position;
        }

        Registry.events.OnClickNotCat += ArrangeHand; //Called from InputHandler.cs
    }
    
    /// <summary>
    /// Set each cat in the hand inactive in the hierarchy
    /// </summary>
    public void HideHand()
    {
        foreach (string catId in catsInHand)
        {
            if (Misc.GetCatById(catId) != null)
            {
                Misc.GetCatById(catId).graphicsParent.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set each cat in the hand active in the hierarchy
    /// </summary>
    public void ShowHand()
    {
        foreach (string catId in catsInHand)
        {
            if (Misc.GetCatById(catId) != null)
            {
                Misc.GetCatById(catId).graphicsParent.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Spawn a cat and add it to the player's hand
    /// </summary>
    /// <param name="_catId">Type index of the cat</param>
    public void DrawCat(string _catId)
    {
        newCatId = Misc.GetCatById(_catId).PutInHand();
    }
    
    /// <summary>
    /// Add a cat to the player's hand and return the position it should be placed
    /// </summary>
    public Vector3 AddToHand(string _catId)
    {
        Vector3 output = Vector3.one;

        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == null)
            {
                catsInHand[i] = _catId;
                output = handPoints[i].position;
                break;
            }
        }

        if (output == Vector3.one)
        {
            Debug.LogError("HAND MANAGER: cats in hand limit reach. can't add cat.", this);
            Debug.Break();
        }

        return output;
    }

    /// <summary>
    /// Arrange hand and shift it number of cat is even
    /// </summary>
    public void ArrangeHand()
    {
        List<string> newCatsInHand = new List<string>();

        // Get remaining cats in hand
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] != null)
            {
                newCatsInHand.Add(catsInHand[i]);
            }
        }
        // Remove every cats of hand
        for (int i = 0; i < catsInHand.Length; i++)
        {
            catsInHand[i] = null;
        }

        // check the number of remaining cat in hand to center the hand properly
        if (newCatsInHand.Count % 2 == 0) //number of cats in hand is even
        {
            for (int i = 0; i < handPoints.Length; i++)
            {
                handPoints[i].position = new Vector3(handPointsDefaultPosition[i].x - 0.5f, handPointsDefaultPosition[i].y, handPointsDefaultPosition[i].z);
            }
        }
        else //number of cats in hand is odd
        {
            for (int i = 0; i < handPointsDefaultPosition.Length; i++)
            {
                handPoints[i].position = new Vector3(handPointsDefaultPosition[i].x, handPointsDefaultPosition[i].y, handPointsDefaultPosition[i].z);
            }
        }

        // Add remaining cats in hand
        foreach (string catId in newCatsInHand)
        {
            Misc.GetCatById(catId).PutInHand();
        }
    }

    /// <summary>
    /// highlight one cat and display information about it
    /// </summary>
    public void HighlightCat(Cat highlightedCat)
    {
        Debug.Log($"HighLightCat : {highlightedCat}");

        //Create a list of all cat that are non highlighted
        int indexStackedHandPointPosition = 0;
        for (int i = 0;i < catsInHand.Length;i++)
        {
            if (catsInHand[i] != highlightedCat.id && catsInHand[i] != null)
            {
                handPoints[i].localPosition = Registry.gameSettings.stackedHandPointPosition[indexStackedHandPointPosition];
                Misc.GetCatById(catsInHand[i]).transform.position = handPoints[i].position;
                indexStackedHandPointPosition++;
            }
            else if (catsInHand[i] != null)
            {
                handPoints[i].localPosition = Registry.gameSettings.highlightedHandPointPosition;
                Misc.GetCatById(catsInHand[i]).transform.position = handPoints[i].position;
            }
        }
    }

    /// <summary>
    /// Remove a cat from the player's hand
    /// </summary>
    public void RemoveFromHand(string _catId)
    {
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == _catId)
            {
                catsInHand[i] = null;
            }
        }
    }

    /// <summary>
    /// Discard all cats in the player's hand
    /// </summary>
    public void DiscardHand()
    {
        foreach (string catId in catsInHand)
        {
            if (catId != null)
            {
                Misc.GetCatById(catId).Withdraw();
                RemoveFromHand(catId);
            }
        }
    }

    /// <summary>
    /// Seek available position in the player's hand
    /// </summary>
    public Vector3 GetAvailablePosition()
    {
        Vector3 output = Vector3.one;
        
        for (int i = 0; i < catsInHand.Length; i++)
        {
            if (catsInHand[i] == null)
            {
                output = handPoints[i].position;
                break;
            }
        }

        if (output == Vector3.one)
        {
            Debug.LogError("HAND MANAGER: cats in hand limit reach. can't add cat.", this);
            Debug.Break();
        }
        
        return output;
    }
}