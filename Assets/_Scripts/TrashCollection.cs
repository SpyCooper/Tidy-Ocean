using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrashCollection : MonoBehaviour
{
    private TrashSO.TrashCollectionType MaxTrashCollection = TrashSO.TrashCollectionType.Small;
    private bool CanCollectUnderWater = false;

    private void Awake() => GetComponent<BoxCollider2D>().isTrigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TrashHolder th;
        if (!(th = collision.gameObject.GetComponent<TrashHolder>()))
            return;
        if((CanCollectUnderWater || th.Trash.Underwater == false) && (int)MaxTrashCollection >= (int)th.Trash.Type)
        {
            //add money
            Destroy(th.gameObject); //could be animated
        }
    }
}
