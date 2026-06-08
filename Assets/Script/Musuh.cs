using UnityEngine;

public class Musuh : MonoBehaviour
{
    enum StatusMusuh { Patroli, Mengejar }

    [Header("Status")]
    [SerializeField] private StatusMusuh statusSaatIni = StatusMusuh.Patroli;

    [Header("Pengaturan Gerakan")]
    public float kecepatanPatroli = 2f;
    public float kecepatanMengejar = 4f;
    public float kekuatanLompat = 6f; // Menambahkan variabel kontrol lompat di Inspector
    public float jarakDeteksi = 5f;

    [Header("Titik Patroli")]
    public Transform titikA;
    public Transform titikB;

    private Transform targetPatroliSaatIni;
    private Transform targetPemain;
    private Rigidbody2D rb;
    private float arahJalanX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPatroliSaatIni = titikB;

        GameObject pemain = GameObject.FindWithTag("Player");
        if (pemain != null) targetPemain = pemain.transform;
    }

    void FixedUpdate()
    {
        if (targetPemain == null) return;

        float jarakKePemain = Vector2.Distance(transform.position, targetPemain.position);

        if (jarakKePemain <= jarakDeteksi)
        {
            statusSaatIni = StatusMusuh.Mengejar;
        }
        else
        {
            statusSaatIni = StatusMusuh.Patroli;
        }

        if (statusSaatIni == StatusMusuh.Mengejar)
        {
            AksiMengejar();
        }
        else
        {
            AksiPatroli();
        }
    }

    void AksiPatroli()
    {
        if (targetPatroliSaatIni == null) return;

        // Hitung arah X (kanan/kiri) menuju titik patroli
        Vector2 arah = (targetPatroliSaatIni.position - transform.position).normalized;
        arahJalanX = arah.x;

        // PERBAIKAN: Menggunakan linearVelocity agar tidak mengunci pergerakan vertikal (Y) saat lompat
        rb.linearVelocity = new Vector2(arahJalanX * kecepatanPatroli, rb.linearVelocity.y);

        AturArahWajah(arahJalanX);

        if (Vector2.Distance(transform.position, targetPatroliSaatIni.position) < 1f)
        {
            targetPatroliSaatIni = (targetPatroliSaatIni == titikA) ? titikB : titikA;
        }
    }

    void AksiMengejar()
    {
        if (targetPemain == null) return;

        // Hitung arah X menuju pemain
        Vector2 arah = (targetPemain.position - transform.position).normalized;
        arahJalanX = arah.x;

        // PERBAIKAN: Menggunakan linearVelocity agar kompatibel dengan gaya lompat
        rb.linearVelocity = new Vector2(arahJalanX * kecepatanMengejar, rb.linearVelocity.y);

        AturArahWajah(arahJalanX);
    }

    void AturArahWajah(float arahX)
    {
        if (arahX > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (arahX < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jarakDeteksi);
    }

    // Fungsi ini sekarang akan berfungsi 100% karena gerakan horizontal tidak lagi mengunci sumbu Y
    public void lompat()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, kekuatanLompat);
        Debug.Log("Skrip Musuh: BERHASIL LOMPAT!");
    }
}
