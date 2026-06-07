using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    // Struktur data untuk satu baris dialog
    [System.Serializable]
    public struct DialogLine
    {
        public string namaKarakter;
        [TextArea(3, 5)] public string teksDialog;
        public AudioSource suaraKarakter;
        public GameObject objekKarakter; // Karakter yang harus aktif/muncul di baris ini
    }

    [Header("Pengaturan UI / Layar")]
    [SerializeField] private GameObject fadeScreenIn;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TMP_Text komponenTeksUtama;
    [SerializeField] private TMP_Text komponenNamaKarakter;
    [SerializeField] private GameObject tombolNext;

    [Header("Data Percakapan (Isi di Inspector)")]
    [SerializeField] private DialogLine[] daftarDialog;

    [Header("Status (Hanya Jeda/Tracker)")]
    [SerializeField] private float jedaAwalGame = 2f;
    [SerializeField] private float jedaMunculKarakter = 2f;

    [SerializeField] string targetScene;

    private int eventpos = 0;
    private int textLength;
    private int currentTextLength;

    void Update()
    {
        // Tetap mengambil data dari TextCreator milik Anda
        textLength = TextCreator.charCount;
    }

    void Start()
    {
        fadeScreenIn.SetActive(true);
        // Otomatis mencari komponen jika lupa di-drag di Inspector
        if (komponenTeksUtama == null && textBox != null)
            komponenTeksUtama = textBox.GetComponent<TMP_Text>();


        StartCoroutine(AlurPembukaGame());
    }

    // Coroutine khusus untuk awal game (Fade Screen)
    IEnumerator AlurPembukaGame()
    {
        tombolNext.SetActive(false);
        yield return new WaitForSeconds(jedaAwalGame);

        if (fadeScreenIn != null) fadeScreenIn.SetActive(false);

        // Jalankan dialog pertama (indeks 0)
        StartCoroutine(MulaiDialog(eventpos));
    }

    // Satu fungsi Coroutine untuk SEMUA dialog
    IEnumerator MulaiDialog(int index)
    {
        // Proteksi jika dialog sudah habis agar tidak error
        if (index >= daftarDialog.Length)
        {
            Debug.Log("Semua Dialog Selesai!");
            tombolNext.SetActive(false);
            yield break;
        }

        tombolNext.SetActive(false);
        DialogLine dataSekarang = daftarDialog[index];

        // 1. Aktifkan karakter jika dimasukkan di Inspector
        if (dataSekarang.objekKarakter != null)
        {
            dataSekarang.objekKarakter.SetActive(true);
            yield return new WaitForSeconds(jedaMunculKarakter);
        }

        textBox.SetActive(true);

        // 2. Set Teks Nama dan Isi Dialog
        if (komponenNamaKarakter != null) komponenNamaKarakter.text = dataSekarang.namaKarakter;
        if (komponenTeksUtama != null) komponenTeksUtama.text = dataSekarang.teksDialog;

        currentTextLength = dataSekarang.teksDialog.Length;

        // 3. Mainkan Suara / Text To Speech jika ada AudioSource
        if (dataSekarang.suaraKarakter != null)
        {
            dataSekarang.suaraKarakter.Play();
        }

        // 4. Trigger efek mengetik script TextCreator Anda
        TextCreator.runTextPrint = true;
        yield return new WaitForSeconds(1.05f);

        // 5. Tunggu sampai efek mengetik selesai
        yield return new WaitUntil(() => textLength == currentTextLength);
        yield return new WaitForSeconds(0.5f);

        // Bersiap untuk urutan berikutnya saat tombol Next diklik
        eventpos++;
        tombolNext.SetActive(true);
        if (textBox != null) textBox.SetActive(true);
    }

    // Fungsi yang dipasang di Button Next (On Click)
    public void NextButtons()
    {
        if (eventpos < daftarDialog.Length)
        {
            int indeksKarakterSebelumnya = eventpos - 1;

            if (indeksKarakterSebelumnya >= 0 && indeksKarakterSebelumnya < daftarDialog.Length)
            {
                GameObject karakterSelesai = daftarDialog[indeksKarakterSebelumnya].objekKarakter;
                if (karakterSelesai != null)
                {
                    karakterSelesai.SetActive(false); // Karakter langsung sembunyi/nonaktif
                    textBox.SetActive(false);
                }
            }
            StartCoroutine(MulaiDialog(eventpos));
        }
        else
        {
            if (targetScene != null) { SceneManager.LoadScene(targetScene); }
            tombolNext.SetActive(false);
        }
    }

}