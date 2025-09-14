using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    SpeedBoost,
    DoubleDash,
}

public class PlayerPowerUp : MonoBehaviour, IPowerUpVisitor
{
    [SerializeField] private ParticleSystem _speedBoostEffect;
    [SerializeField] private ParticleSystem _doubleDashEffect;

    private List<PowerUpInstance> activePowerUps = new();

    private class PowerUpInstance
    {
        public PowerUpType type;
        public float remainingTime;
        public float value;
        public Coroutine coroutine;
    }

    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    public void ApplyPowerUp(PowerUp powerUp)
    {
        PowerUpInstance existing = activePowerUps.Find(p => p.type == powerUp.type);

        if (existing != null)
        {
            existing.remainingTime += powerUp.data.duration;
        }
        else
        {
            PowerUpInstance newInstance = new PowerUpInstance
            {
                type = powerUp.type,
                remainingTime = powerUp.data.duration,
                value = powerUp.data.value
            };

            newInstance.coroutine = StartCoroutine(RunPowerUp(newInstance));

            activePowerUps.Add(newInstance);
        }
    }

    private IEnumerator RunPowerUp(PowerUpInstance instance)
    {
        ActivateEffect(instance.type, true, instance.value);

        while (instance.remainingTime > 0)
        {
            instance.remainingTime -= Time.deltaTime;
            yield return null;
        }

        ActivateEffect(instance.type, false, instance.value);
        activePowerUps.Remove(instance);
    }

    private void ActivateEffect(PowerUpType type, bool activate, float value)
    {
        switch (type)
        {
            case PowerUpType.SpeedBoost:
                if (activate)
                {
                    _speedBoostEffect.Play();
                    controller.moveSpeed *= value;
                }
                else
                {
                    controller.moveSpeed /= value;
                    _speedBoostEffect.Stop();
                }
                break;

            case PowerUpType.DoubleDash:
                if (activate)
                {
                    controller.canDoubleDash = value > 0;
                    _doubleDashEffect.Play();
                }
                else
                {
                    controller.canDoubleDash = false;
                    _doubleDashEffect.Stop();
                }
                break;
        }
    }

    public void Visit(SpeedBoostPowerUp powerUp)
    {
        ApplyPowerUp(powerUp);
    }

    public void Visit(DoubleDashPowerUp powerUp)
    {
        ApplyPowerUp(powerUp);
    }
}
