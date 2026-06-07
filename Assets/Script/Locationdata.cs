using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "Map/Location Data")]
public class LocationData : ScriptableObject
{
    public string locationName;

    [TextArea(2, 4)]
    public string description;

    public Sprite photo;
    public string sceneName;

    [Header("Unlock Settings")]
    [Tooltip("Harga poin untuk membuka lokasi ini. 0 = gratis/selalu terbuka.")]
    public int hargaBuka = 0;

    [Tooltip("Centang jika lokasi ini terbuka sejak awal (misal: lokasi pertama).")]
    public bool unlockedByDefault = false;
}
