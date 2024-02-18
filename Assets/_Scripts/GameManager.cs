using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int inventoryCapacity = 100;
    public int currentlyUsedCapacity;

    private void Awake()
    {
        Instance = this;

        ResetInventoryAmount();
    }
    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void CollectedTrash(TrashSO trashSO)
    {
        Debug.Log(trashSO.ToString());
        switch(trashSO.Type)
        {
            case TrashSO.TrashCollectionType.Small:
                currentlyUsedCapacity += 5;
                break;
            case TrashSO.TrashCollectionType.Medium: 
                currentlyUsedCapacity += 15;
                break;
            case TrashSO.TrashCollectionType.Large:
                currentlyUsedCapacity = 25;
                break;
            default:
                currentlyUsedCapacity += 0;
                break;
        }
        float convertedUsedCapacity = ((float)currentlyUsedCapacity) / inventoryCapacity;
        UIManager.Instance.InventoryBarChange(convertedUsedCapacity);

        Debug.Log(currentlyUsedCapacity);
        Debug.Log("converted " + convertedUsedCapacity);
    }

    private void ResetInventoryAmount()
    {
        currentlyUsedCapacity = 0;
        UIManager.Instance.InventoryBarReset();
    }

    public void DayIsOver()
    {
        Debug.Log("Day has ended");
    }
}
