using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button startButton;

    void Start()
    {
        exitButton.onClick.AddListener(() => {
            Debug.Log("Exited");
            Application.Quit();
        });
        startButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.KyleTest);
        });

        // start music
        // start animations
    }
}
