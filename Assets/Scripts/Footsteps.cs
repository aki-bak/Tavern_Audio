using UnityEngine;
using FMODUnity;

/// <summary>
/// Zarządza odtwarzaniem dźwięków kroków, skoków i lądowania w zależności od powierzchni.
/// </summary>
public class Footsteps : MonoBehaviour
{
    // Publiczne referencje do zdarzeń FMOD.
    public EventReference footstepsEvent;
    public EventReference jumpEvent;
    public EventReference landEvent;

    private float lastFootstepTime = 0f;
    private float distToGround;

    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool isJumping = false;

    void Start()
    {
        // Pobieramy wysokość collidera
        if (GetComponent<Collider>() != null)
        {
            distToGround = GetComponent<Collider>().bounds.extents.y;
        }
        else
        {
            distToGround = 1.0f; // Domyślna wartość, jeśli brak collidera
        }
    }

    void Update()
    {
        // Sprawdza, czy gracz skacze, używając spacji.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayJump();
        }
    }

    void FixedUpdate()
    {
        HandleFootsteps();
    }

    /// <summary>
    /// Obsługuje logikę odtwarzania dźwięków kroków.
    /// </summary>
    private void HandleFootsteps()
    {
        // Sprawdza, czy gracz się porusza.
        bool isMoving = (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        // Sprawdza, czy gracz biegnie.
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isMoving && IsGrounded())
        {
            // Ustawia interwał na podstawie tego, czy gracz biegnie.
            float footstepInterval = isRunning ? 0.25f : 0.45f;

            if (Time.time - lastFootstepTime > footstepInterval)
            {
                lastFootstepTime = Time.time;
                PlayFootsteps();
            }
        }
    }

    /// <summary>
    /// Odtwarza dźwięk kroków w zależności od powierzchni.
    /// </summary>
    private void PlayFootsteps()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f))
        {
            string surfaceTag = hit.collider.tag;
            PlaySurfaceSound(footstepsEvent, surfaceTag);
        }
    }

    /// <summary>
    /// Odtwarza dźwięk skoku.
    /// </summary>
    private void PlayJump()
    {
        if (IsGrounded())
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f))
            {
                string surfaceTag = hit.collider.tag;
                PlaySurfaceSound(jumpEvent, surfaceTag);
            }
            isGrounded = false;
            isJumping = true;
        }
    }

    /// <summary>
    /// Obsługuje dźwięk lądowania po skoku.
    /// </summary>
    private void OnCollisionEnter(Collision col)
    {
        if (!isGrounded && isJumping)
        {
            PlayLanding();
        }
    }

    /// <summary>
    /// Odtwarza dźwięk lądowania.
    /// </summary>
    private void PlayLanding()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distToGround + 0.5f))
        {
            string surfaceTag = hit.collider.tag;
            PlaySurfaceSound(landEvent, surfaceTag);
        }
        isGrounded = true;
        isJumping = false;
    }

    /// <summary>
    /// Ogólna metoda do odtwarzania dźwięku na podstawie tagu powierzchni jako One-Shot.
    /// </summary>
    private void PlaySurfaceSound(EventReference eventRef, string surfaceTag)
    {
        // Sprawdzenie, czy referencja do eventu w ogóle istnieje w inspektorze
        if (eventRef.IsNull) return;

        string surfaceParameter = null;

        // Instrukcja SWITCH do mapowania Tagu na Parametr FMOD.
        switch (surfaceTag)
        {
            case "Stone":
            case "Inside_stone":
            case "Outside":
                surfaceParameter = "Stone";
                break;

            case "Wood":
            case "Inside_wood":
                surfaceParameter = "Wood";
                break;

            case "Stairs":
                surfaceParameter = "Stairs";
                break;

            case "Chandelier":
                surfaceParameter = "Chandelier";
                break;
        }

        // Jeśli znaleziono pasujący parametr, odtwórz dźwięk w bezpieczny sposób
        if (surfaceParameter != null)
        {
            // Tworzymy lokalną, tymczasową instancję dźwięku
            FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(eventRef);

            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
            instance.setParameterByNameWithLabel("Footsteps_Switcher", surfaceParameter);

            instance.start();
            instance.release(); // Nakazuje FMOD zniszczenie instancji OD RAZU po zakończeniu odtwarzania pliku wave
        }
    }

    /// <summary>
    /// Sprawdza, czy gracz znajduje się na podłożu.
    /// </summary>
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.5f);
    }
}