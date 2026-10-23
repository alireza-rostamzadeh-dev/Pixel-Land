
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Follow")]
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime = 0.2f;
    public float offsetx;
    public float offsety;

    [Header("Low Health Shake")]
    [SerializeField, Range(0f, 1f)] private float lowHealthThreshold = 0.3f;
    [SerializeField] private float maxShakeIntensity = 0.18f;
    [SerializeField] private float minimumShakeIntensity = 0.025f;
    [SerializeField] private float shakeSpeed = 25f;

    private PlayerStats playerStats;
    private Vector3 followPosition;
    private Vector3 velocity;
    private float shakeTimer;

    private void Awake()
    {
        followPosition = transform.position;
        FindPlayer();
    }

    private void Start()
    {
        FindPlayer();

        if (playerStats == null)
            Debug.LogError("CameraFollow: PlayerStats not found.", this);
    }

    private void LateUpdate()
    {
        if (player == null || playerStats == null)
            FindPlayer();

        if (player == null)
            return;

        Vector3 targetPosition = new Vector3(
            player.position.x + offsetx,
            player.position.y + offsety,
            -10f
        );

        followPosition = Vector3.SmoothDamp(
            followPosition,
            targetPosition,
            ref velocity,
            smoothTime
        );

        Vector3 shakeOffset = Vector3.zero;

        if (playerStats != null && playerStats.maxHealth > 0f)
        {
            float healthPercent = Mathf.Clamp01(
                playerStats.currentHealth / playerStats.maxHealth
            );

            if (healthPercent > 0f && healthPercent <= lowHealthThreshold)
            {
                float intensity = Mathf.Lerp(
                    minimumShakeIntensity,
                    maxShakeIntensity,
                    1f - healthPercent / lowHealthThreshold
                );

                shakeTimer += Time.deltaTime * shakeSpeed;

                float x = (Mathf.PerlinNoise(shakeTimer, 0f) - 0.5f) * 2f;
                float y = (Mathf.PerlinNoise(0f, shakeTimer) - 0.5f) * 2f;

                shakeOffset = new Vector3(x, y, 0f) * intensity;
            }
            else
            {
                shakeTimer = 0f;
            }
        }

        transform.position = followPosition + shakeOffset;
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

            if (playerMovement != null)
                player = playerMovement.transform;
        }

        if (player != null)
            playerStats = player.GetComponent<PlayerStats>();

        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();
    }
}
