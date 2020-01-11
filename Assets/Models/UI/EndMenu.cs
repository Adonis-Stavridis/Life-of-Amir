using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//script pour gérer les conditions de victoire/défaite et le menu de fin
public class EndMenu : MonoBehaviour
{
    public bool win = true;
    public GameObject endMenu;
    public TextMeshProUGUI text;
    public AudioSource sound;
    public PlayerController controller;
    public SpawnProjectile shoot;

    //vérifie si le boss a été tué
    void Update()
    {
        GameObject[] listBoss = GameObject.FindGameObjectsWithTag("Boss");
        if (listBoss.Length == 0)
        {
            StartCoroutine("waitEnd");
        }
    }

    //fait arrêter le jeu et affiche l'écran de fin
    public void endGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (win) text.text = "Victory";
        Time.timeScale = 0f;
        endMenu.SetActive(true);
        controller.enabled = false;
        shoot.enabled = false;
        AudioListener.pause = true;
    }

    //timer pour afficher l'écran de fin
    IEnumerator waitEnd()
    {
        yield return new WaitForSeconds(0.5f);
        win = true;
        endGame();
    }

    //bouton pour rejouer
    public void replay()
    {
        sound.Play();
        endMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(1);
    }

    //bouton pour aller au menu
    public void goMenu()
    {
        sound.Play();
        endMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
    }
}
