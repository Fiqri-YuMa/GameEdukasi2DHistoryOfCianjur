using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class MainMenu : MonoBehaviour
{
    public GameObject FadeOut;
    public GameObject MulaiMain;
    public GameObject Tutorial;
    public GameObject settings;
    public GameObject exit;
    public GameObject lanjuts;
    public GameObject setings;


    [SerializeField] AudioSource klik;
    private int data;

    private void Start()
    {
        data = GameDataManager.instance.totalPoin;
        if(PlayerPrefs.HasKey("PoinTersimpan"))
        {
            lanjuts.SetActive(true);
        }
        else
        {
            lanjuts.SetActive(false);
        }
    }
    IEnumerator NextSxene()
    {
        klik.Play();
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

    public void mulai()
    {
        GameDataManager.instance.ResetSemuaUnlock();
        StartCoroutine(NextSxene());
    }

    public void lanjut()
    {
        StartCoroutine(NextSxene());
    }

    public void tutor()
    {
        StartCoroutine(Tutorials());
    }

    IEnumerator Tutorials()
    {
        klik.Play();
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Tutorial");
    }

    public void pengaturanb() { setings.SetActive(true); }
    public void pengaturant() { setings.SetActive(false); }

    public void KeluarDariGame()
    {
        // Berfungsi saat game sudah di-build (PC, Android, dll)
        Application.Quit();
    }

}
