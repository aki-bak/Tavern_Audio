using UnityEngine;

/// <summary>
/// Klasa przechowująca informację o stanie ambientu pokoju.
/// </summary>
public class RoomAmbient : MonoBehaviour
{
    // Flaga publiczna, która jest ustawiana przez inne skrypty.
    public bool ambientActivated;
}
//using UnityEngine;
//using FMODUnity;
//using FMOD.Studio;

///// <summary>
///// Klasa zarządzająca instancją snapshotu FMOD w zależności od stanu aktywacji.
///// </summary>
//public class RoomAmbient : MonoBehaviour
//{
//    [Header("FMOD Settings")]
//    // Tutaj w Inspektorze wskażesz swój snapshot (np. snapshot:/InsideRoomSnap)
//    [EventRef] public string snapshotPath;

//    private EventInstance snapshotInstance;

//    // Logika ukryta: wykrywa moment, w którym zmienia się True/False
//    private bool _ambientActivated;
//    public bool ambientActivated
//    {
//        get { return _ambientActivated; }
//        set
//        {
//            if (_ambientActivated != value)
//            {
//                _ambientActivated = value;
//                ToggleSnapshot(_ambientActivated); // Reagujemy na zmianę!
//            }
//        }
//    }

//    private void ToggleSnapshot(bool activate)
//    {
//        if (string.IsNullOrEmpty(snapshotPath)) return;

//        if (activate)
//        {
//            // Jeśli instancja nie istnieje lub jest nieprawidłowa, twórz nową
//            if (!snapshotInstance.isValid())
//            {
//                snapshotInstance = RuntimeManager.CreateInstance(snapshotPath);
//            }

//            snapshotInstance.start();
//            Debug.Log("🚪 Gracz wszedł do strefy: Snapshot FMOD URUCHOMIONY");
//        }
//        else
//        {
//            if (snapshotInstance.isValid())
//            {
//                // Zatrzymanie z zachowaniem czasu schodzenia (Release w AHDSR)
//                snapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
//                Debug.Log("🔓 Gracz opuścił strefę: Snapshot FMOD ZATRZYMANY (Zwalnianie...)");
//            }
//        }
//    }

//    private void OnDestroy()
//    {
//        // Czyszczenie pamięci ram po wyłączeniu gry / zmianie sceny
//        if (snapshotInstance.isValid())
//        {
//            snapshotInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
//            snapshotInstance.release();
//        }
//    }
//}