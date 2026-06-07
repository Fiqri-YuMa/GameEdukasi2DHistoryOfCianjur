using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeContros : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    private const string BGM_KEY = "BGMVolumeSaved";
    private const string MIXER_PARAMETER = "BGM";

    void Start()
    {
        // Mengambil nilai simpanan, default ke 0.75f jika belum ada
        float savedVolume = PlayerPrefs.GetFloat(BGM_KEY, 0.75f);

        // Atur posisi slider fisik sesuai nilai simpanan
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Terapkan volume saat game dimulai
        SetVolume(savedVolume);
    }

    public void SetVolume(float sliderValue)
    {
        // Konversi nilai slider linear (0 sampai 1) ke Logaritmik desibel (-80 sampai 20)
        float mixerVolume = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;

        audioMixer.SetFloat(MIXER_PARAMETER, mixerVolume);

        // Simpan nilai asli slider ke PlayerPrefs
        PlayerPrefs.SetFloat(BGM_KEY, sliderValue);
        PlayerPrefs.Save();
    }
}
