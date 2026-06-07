using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class InfoPanel : MonoBehaviour
{
    [Header("UI References")]
    public Image photoImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Tombol Masuk (saat terbuka)")]
    public Button masukButton;
    public GameObject masukButtonObj;

    [Header("Panel Terkunci")]
    [Tooltip("Panel yang muncul saat lokasi masih terkunci.")]
    public GameObject panelTerkunci;

    [Tooltip("Teks info poin pemain & harga buka, misal: 'Poin kamu: 30 / Butuh: 50 Poin'")]
    public TextMeshProUGUI infoTerkunciText;

    [Tooltip("Tombol 'Buka Lokasi' yang memotong poin pemain.")]
    public Button bukaButton;

    [Tooltip("Teks pada tombol buka, untuk update label harga secara dinamis.")]
    public TextMeshProUGUI bukaTombolText;

    [Header("Default")]
    public LocationData defaultData;

    [Header("Fade")]
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header("Audio")]
    [SerializeField] AudioSource klik;

    // -------------------------------------------------------
    // State
    // -------------------------------------------------------
    private string targetScene;
    private LocationData currentData;
    private LocationPoint currentPoint; // Titik yang memanggil Show()

    // -------------------------------------------------------
    // Unity
    // -------------------------------------------------------
    void Start()
    {
        fadeIn.SetActive(true);

        masukButton.onClick.AddListener(OnMasukClick);
        bukaButton.onClick.AddListener(OnBukaClick);

        Show(defaultData, null);
    }

    // -------------------------------------------------------
    // API publik
    // -------------------------------------------------------

    /// <summary>
    /// Tampilkan info lokasi. Jika terkunci, tampilkan panel unlock.
    /// </summary>
    public void Show(LocationData data, LocationPoint point)
    {
        currentData = data;
        currentPoint = point;
        targetScene = data.sceneName;

        photoImage.sprite = data.photo;
        nameText.text = data.locationName;
        descriptionText.text = data.description;

        bool unlocked = GameDataManager.instance.IsUnlocked(data);
        TampilkanState(unlocked);
    }

    // -------------------------------------------------------
    // Internal
    // -------------------------------------------------------

    void TampilkanState(bool unlocked)
    {
        // Tombol masuk hanya tampil kalau terbuka
        if (masukButtonObj != null)
            masukButtonObj.SetActive(unlocked);

        // Panel terkunci hanya tampil kalau belum terbuka
        if (panelTerkunci != null)
            panelTerkunci.SetActive(!unlocked);

        if (!unlocked && currentData != null)
        {
            int poinPemain = GameDataManager.instance.totalPoin;
            int harga = currentData.hargaBuka;

            // Info poin
            if (infoTerkunciText != null)
                infoTerkunciText.text =
                    $"Artefak kamu : <color=#F5D76E>{poinPemain}</color>\n" +
                    $"Butuh     : <color=#FF6B6B>{harga}</color>";

            // Label tombol buka
            if (bukaTombolText != null)
                bukaTombolText.text = $"Buka ({harga} Artefak)";

            // Nonaktifkan tombol jika poin kurang
            if (bukaButton != null)
                bukaButton.interactable = poinPemain >= harga;
        }
    }

    void OnBukaClick()
    {
        if (currentData == null) return;

        bool berhasil = GameDataManager.instance.CobaUnlock(currentData);

        if (berhasil)
        {
            klik.Play();

            // Refresh visual titik di peta
            if (currentPoint != null)
                currentPoint.RefreshVisual();

            // Ganti tampilan panel ke mode terbuka
            TampilkanState(true);
        }
        else
        {
            // Poin tidak cukup — bisa tambahkan animasi getar / feedback di sini
            Debug.Log("Artefak tidak cukup untuk membuka " + currentData.locationName);
        }
    }

    void OnMasukClick()
    {
        StartCoroutine(Waiting());
    }

    IEnumerator Waiting()
    {
        klik.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(2);
        if (!string.IsNullOrEmpty(targetScene))
            SceneManager.LoadScene(targetScene);
    }
}
