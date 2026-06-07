using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocationPoint : MonoBehaviour
{
    [Header("Data")]
    public LocationData data;
    public InfoPanel infoPanel;

    [Header("Visual Terkunci")]
    [Tooltip("Objek ikon gembok yang tampil saat lokasi terkunci (bisa Image/GameObject).")]
    public GameObject gembokIcon;

    [Tooltip("Teks harga yang muncul di atas gembok, misal '50 Poin'.")]
    public TextMeshProUGUI hargaText;

    [Header("Audio")]
    [SerializeField] AudioSource klik;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        RefreshVisual();
    }

    /// <summary>
    /// Perbarui tampilan titik (gembok/terbuka) sesuai status unlock saat ini.
    /// Panggil juga setelah pemain berhasil membuka lokasi.
    /// </summary>
    public void RefreshVisual()
    {
        bool unlocked = GameDataManager.instance.IsUnlocked(data);

        if (gembokIcon != null)
            gembokIcon.SetActive(!unlocked);

        if (hargaText != null)
            hargaText.gameObject.SetActive(!unlocked);

        if (hargaText != null && !unlocked)
            hargaText.text = data.hargaBuka + " Poin";
    }

    void OnClick()
    {
        klik.Play();
        // Kirim data ke InfoPanel; InfoPanel yang menangani
        // logika terkunci/buka berdasarkan status unlock
        infoPanel.Show(data, this);
    }
}
