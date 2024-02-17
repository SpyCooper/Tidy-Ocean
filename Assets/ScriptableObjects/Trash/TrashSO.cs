using UnityEngine;
[CreateAssetMenu(fileName = "New Trash", menuName = "Scriptable Objects/Trash")]
public class TrashSO : ScriptableObject
{
    public enum TrashCollectionType { NONE = 0, Small = 1, Medium = 2, Large = 3, MAX = 1000 };
    public TrashCollectionType Type = TrashCollectionType.Small;
    public bool Underwater = false;
    public int TrashGridSize = 1;
    public Sprite Sprite;
    public int Cost = 0;
    public int TrashAmt = 0;
}