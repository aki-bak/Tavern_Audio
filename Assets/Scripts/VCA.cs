using UnityEngine;
using FMODUnity;

public class AudioMuteSystem : MonoBehaviour
{
    private FMOD.Studio.VCA vcaGlobal;
    private FMOD.Studio.VCA vcaMusic;
    private FMOD.Studio.VCA vcaInside;
    private FMOD.Studio.VCA vcaOutside;

    private bool globalMute = false;
    private bool musicMute = false;
    private bool insideMute = false;
    private bool outsideMute = false;

    void Start()
    {
        vcaGlobal = RuntimeManager.GetVCA("vca:/Global_Mute");
        vcaMusic = RuntimeManager.GetVCA("vca:/Music mute");
        vcaInside = RuntimeManager.GetVCA("vca:/Inside_Amb");
        vcaOutside = RuntimeManager.GetVCA("vca:/Outside_Amb");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) ToggleMute(vcaGlobal, ref globalMute);
        if (Input.GetKeyDown(KeyCode.I)) ToggleMute(vcaMusic, ref musicMute);
        if (Input.GetKeyDown(KeyCode.O)) ToggleMute(vcaInside, ref insideMute);
        if (Input.GetKeyDown(KeyCode.P)) ToggleMute(vcaOutside, ref outsideMute);
    }

    private void ToggleMute(FMOD.Studio.VCA vca, ref bool isMuted)
    {
        if (!vca.isValid())
        {
            Debug.LogWarning("Nie znaleziono VCA! Upewnij się, że wcisnąłeś F7 w FMOD.");
            return;
        }

        isMuted = !isMuted;

        float targetVolume = isMuted ? 0f : 1f;
        vca.setVolume(targetVolume);
    }
}