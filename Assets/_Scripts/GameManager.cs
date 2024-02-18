using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int inventoryCapacity = 100;
    public int currentlyUsedCapacity;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject boat;

    private void Awake()
    {
        Instance = this;

        ResetInventoryAmount();
    }
    private void Start()
    {
        Respawn();
    }

    private void Update()
    {
        
    }

    public void CollectedTrash(TrashSO trashSO)
    {
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
    }

    private void ResetInventoryAmount()
    {
        currentlyUsedCapacity = 0;
        UIManager.Instance.InventoryBarReset();
    }

    public void DayIsOver()
    {
        Debug.Log("Day has ended");
        Respawn();
        UIManager.Instance.ResetTimer();

        // add respawn trash
    }

    public void GameEnded()
    {
        EndScreenManager.Instance.ShowEndScreen();
    }

    private void Respawn()
    {
        boat.transform.position = spawnPoint.transform.position;
    }
}
