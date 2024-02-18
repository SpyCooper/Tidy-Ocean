using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button closeCreditsButton;
    [SerializeField] private Button startButton;
    [SerializeField] private GameObject creditsScreen;

    void Start()
    {

        exitButton.onClick.AddListener(() => {
            Debug.Log("Exited");
            Application.Quit();
        });
        creditsButton.onClick.AddListener(() => {
            creditsScreen.SetActive(true);
        });
        closeCreditsButton.onClick.AddListener(() => {
            creditsScreen.SetActive(false);
        });
        startButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.KyleTest);
        });

        // start music
        // start animations


        creditsScreen.SetActive(false);
    }
}
