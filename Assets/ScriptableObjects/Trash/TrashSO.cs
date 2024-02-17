using UnityEngine;
[CreateAssetMenu(fileName = "New Trash", menuName = "Scriptable Objects/Trash")]
public class TrashSO : ScriptableObject
{
    public enum TrashCollectionType { NONE = 0 };
    public TrashCollectionType Type;
    public int TrashGridSize;
    public Sprite Sprite;
    public int Cost;
}