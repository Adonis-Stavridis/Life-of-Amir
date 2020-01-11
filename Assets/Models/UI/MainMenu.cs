using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//script pour gérer le menu principal
public class MainMenu : MonoBehaviour
{
    public AudioSource sound;

    //met le volume principal du jeu à 50%
    void Start()
    {
        AudioListener.volume = 0.5f;
    }

    //bouton lancer le jeu
    public void playGame()
    {
        sound.Play();
        SceneManager.LoadScene(1);
    }

    //bouton quitter le jeu
    public void quitGame()
    {
        sound.Play();
        Application.Quit();
    }
}
