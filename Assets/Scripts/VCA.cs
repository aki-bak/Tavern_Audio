using UnityEngine;
using UnityEngine.UI;

public class FMODVolumeController : MonoBehaviour
{
    [Header("UI Sliders")]
    [SerializeField] private Slider globalMuteSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // FMOD VCA Instances
    private FMOD.Studio.VCA globalMuteVca;
    private FMOD.Studio.VCA musicVca;
    private FMOD.Studio.VCA sfxVca;
    private FMOD.Studio.VCA insideVca;   // Dla Tawerny
    private FMOD.Studio.VCA outsideVca;  // Dla Outside

    // Zmienne pomocnicze do przełączania wyciszenia (Toggle)
    private bool isInsideMuted = false;
    private bool isOutsideMuted = false;

    void Start()
    {
        // Pobieranie VCA na podstawie nazw z Twojego projektu (image_94aef4.png)
        globalMuteVca = FMODUnity.RuntimeManager.GetVCA("vca:/Global_Mute");
        musicVca = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        sfxVca = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        insideVca = FMODUnity.RuntimeManager.GetVCA("vca:/Inside_Amb");   // Ścieżka do VCA tawerny
        outsideVca = FMODUnity.RuntimeManager.GetVCA("vca:/Outside_Amb"); // Ścieżka do VCA na zewnątrz

        // Inicjalizacja sliderów UI
        InitializeSlider(globalMuteVca, globalMuteSlider);
        InitializeSlider(musicVca, musicSlider);
        InitializeSlider(sfxVca, sfxSlider);

        // Rejestracja zdarzeń dla sliderów
        if (globalMuteSlider != null) globalMuteSlider.onValueChanged.AddListener(SetGlobalMuteVolume);
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    void Update()
    {
        // Klawisz 1: Całkowite wyciszenie / odciszenie Tawerny (Inside_Amb)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (insideVca.isValid())
            {
                isInsideMuted = !isInsideMuted;
                // Jeśli true -> wycisz do 0, jeśli false -> daj na maksa (1)
                insideVca.setVolume(isInsideMuted ? 0f : 1f);
                Debug.Log($"Tawerna (Inside_Amb) wyciszona: {isInsideMuted}");
            }
        }

        // Klawisz 2: Całkowite wyciszenie / odciszenie Świata (Outside_Amb)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (outsideVca.isValid())
            {
                isOutsideMuted = !isOutsideMuted;
                // Jeśli true -> wycisz do 0, jeśli false -> daj na maksa (1)
                outsideVca.setVolume(isOutsideMuted ? 0f : 1f);
                Debug.Log($"Na zewnątrz (Outside_Amb) wyciszone: {isOutsideMuted}");
            }
        }
    }

    private void InitializeSlider(FMOD.Studio.VCA vca, Slider slider)
    {
        if (slider == null) return;

        if (vca.isValid())
        {
            vca.getVolume(out float currentVolume);
            slider.value = currentVolume;
        }
    }

    // --- Metody dla Sliderów ---

    public void SetGlobalMuteVolume(float volume)
    {
        if (globalMuteVca.isValid()) globalMuteVca.setVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        if (musicVca.isValid()) musicVca.setVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxVca.isValid()) sfxVca.setVolume(volume);
    }

    private void OnDestroy()
    {
        if (globalMuteSlider != null) globalMuteSlider.onValueChanged.RemoveListener(SetGlobalMuteVolume);
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }
}