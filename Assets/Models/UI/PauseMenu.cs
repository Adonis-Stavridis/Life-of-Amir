using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//script pour gérer l'écran de pause
public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenu;
    public GameObject main;
    public GameObject endMenu;
    public AudioSource sound;
    public PlayerController controller;
    public SpawnProjectile shoot;
    public TextMeshProUGUI musicValue;

    //vérifie si le joueur appuie sur la touche échapper
    //si oui, ça affiche l'écran de pause
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !endMenu.activeSelf)
        {
            if (!paused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
                main.SetActive(false);
                Time.timeScale = 0f;
                paused = true;
                AudioListener.pause = true;
                controller.enabled = false;
                shoot.enabled = false;
            }
        }
    }

    //bouton pour continuer de jouer
    public void resumeGame()
    {
        sound.Play();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        main.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
        AudioListener.pause = false;
        controller.enabled = true;
        shoot.enabled = true;
    }

    //bouton pour retourner au menu
    public void goMenu()
    {
        sound.Play();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
    }

    //slider pour modifier le volume
    public void musicChange(float value)
    {
        musicValue.text = Mathf.RoundToInt(value * 100) + "";
        AudioListener.volume = value;
    }
}
