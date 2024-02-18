using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrashCollection : MonoBehaviour
{
    public static TrashCollection Instance { get; private set; }

    public TrashSO.TrashCollectionType MaxTrashCollection = TrashSO.TrashCollectionType.Small;
    public bool CanCollectUnderWater = false;

    private List<TrashSO> collectedTrash = new List<TrashSO>();

    private void Awake()
    {
        Instance = this;
        GetComponent<BoxCollider2D>().isTrigger = true;
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

    public List<TrashSO> GetCollectedTrash() => collectedTrash;

    public void ClearCollectedTrash() => collectedTrash.Clear();
}