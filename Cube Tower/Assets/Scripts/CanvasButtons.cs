using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasButtons : MonoBehaviour
{
    public Sprite musicOn, musicOff;
    private void Start()
    {
        if (PlayerPrefs.GetString("music") == "No" && gameObject.name == "Sound")
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
    public void RestartGame()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(RestartGameIE());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        IEnumerator RestartGameIE()
        {
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void LoadInstagram()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
        }

        Application.OpenURL("https://www.instagram.com/aladka03/");
    }

    public void LoadShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(RestartGameIE());
        }
        else
        {
            SceneManager.LoadScene("Shop");

        }
        IEnumerator RestartGameIE()
        {
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("Shop");
        }
    }

    public void CloseShop()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            GetComponent<AudioSource>().Play();
            StartCoroutine(RestartGameIE());
        }
        else
        {
            SceneManager.LoadScene("Main");

        }
        IEnumerator RestartGameIE()
        {
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("Main");
        }
    }

    public void MusicWork()
    {
        if (PlayerPrefs.GetString("music") == "No")
        {
            PlayerPrefs.SetString("music", "Yes");
            GetComponent<AudioSource>().Play();
            GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            PlayerPrefs.SetString("music", "No");
            GetComponent<Image>().sprite = musicOff;
        }
    }
}
