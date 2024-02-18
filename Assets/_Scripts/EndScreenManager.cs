using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenManager : MonoBehaviour
{
    public static EndScreenManager Instance { get; private set; }

    [SerializeField] private GameObject EndScreen;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        HideEndScreen();
    }

    public void HideEndScreen()
    {
        EndScreen.SetActive(false);
    }

    public void ShowEndScreen()
    {
        EndScreen.SetActive(true);
    }
}
