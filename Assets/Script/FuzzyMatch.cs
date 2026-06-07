using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class FuzzyMatch : MonoBehaviour
{
    // Struktur data untuk Pertanyaan
    [Serializable]
    public struct DataPertanyaan
    {
        public string teksPertanyaan;
        public string jawabanBenar;
        public int hadiahPoin; // Poin jika benar
        public AudioSource suaraPertanyaan;
    }

    [Header("Daftar Pertanyaan")]
    public List<DataPertanyaan> listPertanyaan;
    private int indexSekarang = 0;

    [Header("Komponen UI")]
    public TextMeshProUGUI UI_TeksPertanyaan;
    public TMP_InputField UI_InputJawaban;
    public TextMeshProUGUI UI_TeksHasilStatus;
    public TextMeshProUGUI UI_TeksTotalPoin;
    public Button tombolSubmit;
    public GameObject artefak;
    public GameObject fadeout;

    [SerializeField] AudioSource klik;

    [Header("Pengaturan Fuzzy")]
    public float batasMinimalBenar = 85f;

    public string targetScene;

    private int textLength;
    private int currentTextLength;

    void Start()
    {
        UpdateTampilanPoin();
        StartCoroutine(TampilkanPertanyaan());
        tombolSubmit.onClick.AddListener(CekJawaban);
    }

    void Update()
    {
        // Tetap mengambil data dari TextCreator milik Anda
        textLength = TextCreator.charCount;
    }

    // Gunakan IEnumerator untuk proses sekuensial teks ketik
    System.Collections.IEnumerator TampilkanPertanyaan()
    {
        if (indexSekarang < listPertanyaan.Count)
        {
            if (indexSekarang == 0) { yield return new WaitForSeconds(2f); }

            UI_TeksPertanyaan.text = listPertanyaan[indexSekarang].teksPertanyaan;
            currentTextLength = listPertanyaan[indexSekarang].teksPertanyaan.Length;
            TextCreator.runTextPrint = true;


            listPertanyaan[indexSekarang].suaraPertanyaan.Play();
            // Menunggu sampai teks selesai mengetik
            yield return new WaitUntil(() => textLength == currentTextLength);

            yield return new WaitForSeconds(1f);
            artefak.SetActive(false);

            // Munculkan kembali input setelah teks selesai mengetik
            UI_InputJawaban.gameObject.SetActive(true);
            tombolSubmit.gameObject.SetActive(true);

            UI_InputJawaban.text = "";
            UI_TeksHasilStatus.text = "Ketik jawabanmu...";
        }
        else
        {
            fadeout.SetActive(true);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(targetScene);
        }
    }

    void CekJawaban()
    {
        klik.Play();

        // Validasi proteksi ekstra jika list kosong atau index out of bounds
        if (listPertanyaan == null || indexSekarang >= listPertanyaan.Count) return;

        string input = UI_InputJawaban.text;
        string jawabanAsli = listPertanyaan[indexSekarang].jawabanBenar;

        float skorFuzzy = GetSimilarityScore(jawabanAsli, input);

        if (skorFuzzy >= batasMinimalBenar)
        {
            int poinDidapat = listPertanyaan[indexSekarang].hadiahPoin;

            // Pastikan GameDataManager ada sebelum menambah poin (Null-safety)
            if (GameDataManager.instance != null)
            {
                GameDataManager.instance.TambahPoin(poinDidapat);
            }

            artefak.SetActive(true);

            UI_TeksHasilStatus.text = $"BENAR! Dapat Artefak";
            UI_TeksHasilStatus.color = Color.green;



            // Sembunyikan UI agar rapi selama masa transisi delay
            UI_InputJawaban.gameObject.SetActive(false);
            tombolSubmit.gameObject.SetActive(false);

            // FIX: Panggil Coroutine delay di sini agar pindah ke soal berikutnya!
            StartCoroutine(DelayPertanyaanBerikutnya(3f));
        }
        else
        {
            UI_TeksHasilStatus.text = $"SALAH! (Mirip: {Mathf.RoundToInt(skorFuzzy)}%)";
            UI_TeksHasilStatus.color = Color.red;

            // OPSI A (Pindah soal meskipun salah): Un-comment baris di bawah jika salah tetap lanjut
            UI_InputJawaban.gameObject.SetActive(false);
            tombolSubmit.gameObject.SetActive(false);
            StartCoroutine(DelayPertanyaanBerikutnya(3f));

            // OPSI B (Beri kesempatan mengulang): Biarkan kosong seperti ini agar pemain bisa mengetik ulang
        }

        UpdateTampilanPoin();
    }

    // Coroutine pembantu untuk menggantikan Invoke
    System.Collections.IEnumerator DelayPertanyaanBerikutnya(float delay)
    {
        // Matikan tombol submit sementara agar pemain tidak spam klik saat transisi
        tombolSubmit.interactable = false;

        yield return new WaitForSeconds(delay);

        tombolSubmit.interactable = true;
        indexSekarang++;
        StartCoroutine(TampilkanPertanyaan());
    }

    void UpdateTampilanPoin()
    {
        // Proteksi NullReferenceException jika text UI lupa di-drag di Inspector
        if (UI_TeksTotalPoin == null) return;

        if (GameDataManager.instance != null)
        {
            UI_TeksTotalPoin.text = "Artefak : " + GameDataManager.instance.totalPoin;
        }
        else
        {
            UI_TeksTotalPoin.text = "Artefak : 0";
        }
    }

    // ================== ALGORITMA FUZZY (LEVENSHTEIN) ==================
    private float GetSimilarityScore(string source, string target)
    {
        string s = source.ToLower().Trim();
        string t = target.ToLower().Trim();

        if (s == t) return 100f;

        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0) return 0;
        if (m == 0) return 0;

        for (int i = 0; i <= n; d[i, 0] = i++) ;
        for (int j = 0; j <= m; d[0, j] = j++) ;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }

        return (1.0f - ((float)d[n, m] / Math.Max(n, m))) * 100f;
    }
}