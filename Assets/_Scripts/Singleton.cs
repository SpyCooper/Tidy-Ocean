using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //The instance of the class
    public static T Instance { get; private set; }
    //Destroy all others of the class
    public void Awake()
    {
        if (Instance == null)
            Instance = (T)this;
        else Destroy(this);
    }
}