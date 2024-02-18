using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int inventoryCapacity;
    public int currentlyUsedCapacity;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject boat;
    [SerializeField] private GameObject trashCollectionHitbox;
    [SerializeField] private Sprite rowBoat;
    [SerializeField] private Sprite fishingBoat;
    [SerializeField] private Sprite fishingNetBoat;
    [SerializeField] private Sprite trashBoat;

    private int trashBoatCost = 10;
    private int fishingNetBoatCost = 10;
    private int fishingBoatCost = 5;

    private int playerCash;

    private float smallBoatScaleX = 0.85f;
    private float smallBoatScaleY = 1.29f;
    private float smallBoatOffsetY = 0.14f;

    private float largeBoatScaleX = 1.77f;
    private float largeBoatScaleY = 2.06f;
    private float largeBoatOffsetY = 0.4f;

    private float smallBoatCollectionScaleX = 1.09f;
    private float smallBoatCollectionScaleY = 1.36f;
    private float smallBoatCollectionOffsetY = 0.12f;

    private float largeBoatCollectionScaleX = 1.81f;
    private float largeBoatCollectionScaleY = 2.1f;
    private float largeBoatCollectionOffsetY = 0.4f;

    private void Awake()
    {
        Instance = this;

        ResetInventoryAmount();
        PlayerCashReset();
        CheckAndSwitchBoats();
    }
    private void Start()
    {
        Respawn();
    }

    public void CollectedTrash(TrashSO trashSO, bool subtract = false)
    {
        switch(trashSO.Type)
        {
            case TrashSO.TrashCollectionType.Small:
                currentlyUsedCapacity += 5 * (subtract ? -1 : 1);
                break;
            case TrashSO.TrashCollectionType.Medium: 
                currentlyUsedCapacity += 15 * (subtract ? -1 : 1);
                break;
            case TrashSO.TrashCollectionType.Large:
                currentlyUsedCapacity += 25 * (subtract ? -1 : 1);
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
        if(TrashInInventory.Count > 0)
        {
            // Has trash in inventory
            foreach(TrashSO trash in TrashInInventory)
            {
                PlayerCashAddedUpdate(trash.Cost);
            }

            TrashCollection.Instance.ClearCollectedTrash();
            UIManager.Instance.InventoryBarReset();
            currentlyUsedCapacity = 0;
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
        if (playerCash >= trashBoatCost)
        {
            boat.transform.GetComponent<SpriteRenderer>().sprite = trashBoat;
            inventoryCapacity = 200;
            PlayerCashSubtractedUpdate(trashBoatCost);
            TrashCollection.Instance.CanCollectUnderWater = true;
            TrashCollection.Instance.MaxTrashCollection = TrashSO.TrashCollectionType.MAX;
            BoatIsSmall(false);
        }
        else if (playerCash >= fishingNetBoatCost)
        {
            boat.transform.GetComponent<SpriteRenderer>().sprite = fishingNetBoat;
            inventoryCapacity = 150;
            PlayerCashSubtractedUpdate(fishingNetBoatCost);
            TrashCollection.Instance.CanCollectUnderWater = true;
            TrashCollection.Instance.MaxTrashCollection = TrashSO.TrashCollectionType.Medium;
            BoatIsSmall(true);
        }
        else if (playerCash >= fishingBoatCost)
        {
            boat.transform.GetComponent<SpriteRenderer>().sprite = fishingBoat;
            inventoryCapacity = 100;
            PlayerCashSubtractedUpdate(fishingBoatCost);
            TrashCollection.Instance.MaxTrashCollection = TrashSO.TrashCollectionType.Medium;
            BoatIsSmall(true);
        }
        else if(playerCash == 0)
        {
            boat.transform.GetComponent<SpriteRenderer>().sprite = rowBoat;
            inventoryCapacity = 50;
            TrashCollection.Instance.MaxTrashCollection = TrashSO.TrashCollectionType.Small;
            BoatIsSmall(true);
        }
    }

    private void BoatIsSmall(bool isSmall)
    {
        if (isSmall)
        {
            boat.GetComponent<BoxCollider2D>().offset = new Vector2(0f, smallBoatOffsetY);
            boat.GetComponent<BoxCollider2D>().size = new Vector2(smallBoatScaleX, smallBoatScaleY);
            trashCollectionHitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0f, smallBoatCollectionOffsetY);
            trashCollectionHitbox.GetComponent<BoxCollider2D>().size = new Vector2(smallBoatCollectionScaleX, smallBoatCollectionScaleY);
        }
        else
        {
            boat.GetComponent<BoxCollider2D>().offset = new Vector2(0f, smallBoatOffsetY);
            boat.GetComponent<BoxCollider2D>().size = new Vector2(largeBoatScaleX, largeBoatScaleY);
            trashCollectionHitbox.GetComponent<BoxCollider2D>().offset = new Vector2(0f, largeBoatCollectionOffsetY);
            trashCollectionHitbox.GetComponent<BoxCollider2D>().size = new Vector2(largeBoatCollectionScaleX, largeBoatCollectionScaleY);
        }
    }
}
