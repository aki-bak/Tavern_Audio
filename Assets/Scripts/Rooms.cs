//using UnityEngine;
//using FMOD.Studio;

///// <summary>
///// Zarządza stanem ambientu pokoju w zależności od pozycji gracza.
///// </summary>
//public class Rooms : MonoBehaviour
//{
//    /// <summary>
//    /// Wywoływane, gdy inny collider pozostaje wewnątrz triggera.
//    /// </summary>
//    private void OnTriggerStay(Collider other)
//    {
//        // Sprawdza, czy obiekt ma tag "Player".
//        if (other.CompareTag("Player"))
//        {
//            // Znajduje instancję RoomAmbient w scenie i ustawia flagę na true.
//            RoomAmbient roomAmbient = FindObjectOfType<RoomAmbient>();
//            if (roomAmbient != null)
//            {
//                roomAmbient.ambientActivated = true;
//            }
//        }
//    }

//    /// <summary>
//    /// Wywoływane, gdy inny collider opuszcza trigger.
//    /// </summary>
//    private void OnTriggerExit(Collider other)
//    {
//        // Sprawdza, czy obiekt ma tag "Player".
//        if (other.CompareTag("Player"))
//        {
//            // Znajduje instancję RoomAmbient w scenie i ustawia flagę na false.
//            RoomAmbient roomAmbient = FindObjectOfType<RoomAmbient>();
//            if (roomAmbient != null)
//            {
//                roomAmbient.ambientActivated = false;
//            }
//        }
//    }
//}
using UnityEngine;

/// <summary>
/// Zarządza stanem ambientu pokoju w zależności od pozycji gracza.
/// </summary>
public class Rooms : MonoBehaviour
{
    private RoomAmbient roomAmbient;

    void Start()
    {
        // Szukamy managera TYLKO RAZ na starcie gry, oszczędzając procesor
        roomAmbient = FindObjectOfType<RoomAmbient>();

        if (roomAmbient == null)
        {
            Debug.LogError("Nie znaleziono obiektu z komponentem RoomAmbient na scenie!");
        }
    }

    /// <summary>
    /// Wywoływane JEDEN RAZ, gdy gracz wchodzi w collider strefy.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && roomAmbient != null)
        {
            roomAmbient.ambientActivated = true;
        }
    }

    /// <summary>
    /// Wywoływane JEDEN RAZ, gdy gracz opuszcza collider strefy.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && roomAmbient != null)
        {
            roomAmbient.ambientActivated = false;
        }
    }
}