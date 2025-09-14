using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public PowerupData data;
    public float bobAmplitude = 0.25f;
    public float bobSpeed = 2f;
    public abstract void Accept(IPowerUpVisitor visitor);

    void Update()
    {
        transform.Rotate(Vector3.up * 40f * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController pc = other.GetComponent<PlayerController>();
        AudioManager.Instance.Play("Collect");
        Accept(pc.powerUp);
        Destroy(gameObject);
    }
}
