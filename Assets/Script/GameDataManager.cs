using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // Singleton agar bisa diakses dari mana saja: GameDataManager.instance
    public static GameDataManager instance;

    public int totalPoin = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TambahPoin(int jumlah)
    {
        totalPoin += jumlah;
        SaveData();
    }

    // -------------------------------------------------------
    // SISTEM BUKA LOKASI
    // -------------------------------------------------------

    /// <summary>
    /// Cek apakah lokasi sudah terbuka.
    /// Key yang disimpan: "Unlocked_[locationName]"
    /// </summary>
    public bool IsUnlocked(LocationData data)
    {
        if (data.unlockedByDefault) return true;
        return PlayerPrefs.GetInt("Unlocked_" + data.locationName, 0) == 1;
    }

    /// <summary>
    /// Coba buka lokasi dengan memotong poin.
    /// Kembalikan true jika berhasil, false jika poin tidak cukup.
    /// </summary>
    public bool CobaUnlock(LocationData data)
    {
        if (IsUnlocked(data)) return true;

        if (totalPoin >= data.hargaBuka)
        {
            totalPoin -= data.hargaBuka;
            PlayerPrefs.SetInt("Unlocked_" + data.locationName, 1);
            SaveData();
            return true;
        }

        return false; // Poin tidak cukup
    }

    /// <summary>
    /// Reset semua status unlock (untuk keperluan testing / new game).
    /// </summary>
    public void ResetSemuaUnlock()
    {
        // Hapus semua key PlayerPrefs dan mulai ulang
        PlayerPrefs.DeleteAll();
        totalPoin = 0;
    }

    // -------------------------------------------------------
    // SIMPAN & MUAT
    // -------------------------------------------------------

    public void SaveData()
    {
        PlayerPrefs.SetInt("PoinTersimpan", totalPoin);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        totalPoin = PlayerPrefs.GetInt("PoinTersimpan", 0);
    }
}
