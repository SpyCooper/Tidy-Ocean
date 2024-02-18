using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrashCollection : MonoBehaviour
{
    public static TrashCollection Instance { get; private set; }

    private TrashSO.TrashCollectionType MaxTrashCollection = TrashSO.TrashCollectionType.Small;
    private bool CanCollectUnderWater = false;

    private List<TrashSO> collectedTrash;

    private void Awake()
    {
        Instance = this;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        collectedTrash = new List<TrashSO>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TrashHolder th;

        if (GameManager.Instance.currentlyUsedCapacity < GameManager.Instance.inventoryCapacity)
        {
            if (!(th = collision.gameObject.GetComponent<TrashHolder>()) || GameManager.Instance.currentlyUsedCapacity >= GameManager.Instance.inventoryCapacity)
                return;
            if ((CanCollectUnderWater || th.Trash.Underwater == false) && (int)MaxTrashCollection >= (int)th.Trash.Type)
            {
                collectedTrash.Add(th.Trash);

                GameManager.Instance.CollectedTrash(th.Trash);

                Destroy(th.gameObject); //could be animated
            }
        }
    }

    public List<TrashSO> GetCollectedTrash() 
    {
        return collectedTrash;
    }

    public void ClearCollectedTrash()
    {
        if(collectedTrash != null)
        {
            collectedTrash.Clear();
        }
    }

    public int TrashMoney()
    {
        int amt = collectedTrash.Sum(c => c.Cost);
        collectedTrash.Clear();
        return amt;
    }
}
