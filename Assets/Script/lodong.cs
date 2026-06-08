using UnityEngine;

public class lodong : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("SENSOR MENDETEKSI OBJEK: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);
        // Memeriksa apakah objek yang menyentuh kotak adalah Player
        if (collision.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(20f);
        }
    }
}
