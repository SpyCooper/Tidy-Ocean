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
    [SerializeField] private Sprite rowBoat;
    [SerializeField] private Sprite fishingBoat;
    [SerializeField] private Sprite fishingNetBoat;
    [SerializeField] private Sprite trashBoat;

    private int playerCash;

    private void Awake()
    {
        Instance = this;

        ResetInventoryAmount();
        PlayerCashReset();
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
        TrashGenerator.Instance.ResetTrash();
    }

    public void GameEnded()
    {
        EndScreenManager.Instance.ShowEndScreen();
    }

    private void Respawn()
    {
        boat.transform.position = spawnPoint.transform.position;
    }

    public void EnteredDock()
    {
        Debug.Log("Entered Dock Area");

        List<TrashSO> TrashInInventory = TrashCollection.Instance.GetCollectedTrash();
        if(TrashInInventory.Count > 0 )
        {
            // Has trash in inventory
            foreach(TrashSO trash in TrashInInventory)
            {
                PlayerCashAddedUpdate(trash.TrashAmt);
            }

            TrashCollection.Instance.ClearCollectedTrash();
            UIManager.Instance.InventoryBarReset();
        }

        // check if cash can get new boat
        CheckAndSwitchBoats();


        if (TrashGenerator.Instance.CheckTrashGone())
        {
            GameEnded();
        }
    }

    private void PlayerCashAddedUpdate(int added)
    {
        playerCash += added;
        UIManager.Instance.cashAmountUpdate(playerCash);
    }

    private void PlayerCashSubtractedUpdate(int subtracted)
    {
        playerCash -= subtracted;
        UIManager.Instance.cashAmountUpdate(playerCash);
    }

    private void PlayerCashReset(int set=0)
    {
        playerCash = set;
        UIManager.Instance.cashAmountUpdate(playerCash);
    }

    private void CheckAndSwitchBoats()
    {
        switch (playerCash)
        {
            // row boat
            case 0:
                boat.transform.GetComponent<SpriteRenderer>().sprite = rowBoat;
                break;
            // fishing boat
            case 1000:
                boat.transform.GetComponent<SpriteRenderer>().sprite = fishingBoat;
                break;
            // fishing boat with net
            case 10000:
                boat.transform.GetComponent<SpriteRenderer>().sprite = fishingNetBoat;
                break;
            // trash boat
            case 100000:
                boat.transform.GetComponent<SpriteRenderer>().sprite = trashBoat;
                break;
            default:
                break;

        }
    }
}
