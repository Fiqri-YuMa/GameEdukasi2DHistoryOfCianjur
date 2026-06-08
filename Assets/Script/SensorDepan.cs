using UnityEngine;

public class SensorDepan : MonoBehaviour
{
    private Musuh skripMusuh;

    void Start()
    {
        // Mengambil komponen AIKarakterMusuh2D dari objek induknya
        skripMusuh = GetComponentInParent<Musuh>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SENSOR MENDETEKSI OBJEK: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);
        // Memeriksa apakah objek yang menyentuh kotak adalah Player
        if (collision.CompareTag("batu"))
        {
            if (skripMusuh != null)
            {
                skripMusuh.lompat(); // Memerintahkan musuh untuk lompat
            }
            Debug.Log("Musuh melompat!");
            //GetComponent<Musuh>().lompat();
        }
    }
}
