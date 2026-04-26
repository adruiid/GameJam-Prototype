using Unity.VisualScripting;
using UnityEngine;

public class UniversalStatMultiplier : MonoBehaviour
{
    [SerializeField]private float damageMultiplier;
    [SerializeField]private float speedMultiplier;
    [SerializeField] private float healthMultiplier;

    public float getDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void setDamageMultiplier(float value)
    {
        damageMultiplier = value;
    }

    public float getSpeedMultiplier() { return speedMultiplier; }
    public float getHealthMultiplier() { return healthMultiplier; } 

    public void setHealthMultiplier(float value) { healthMultiplier = value; }

    public void setSpeedMultiplier(float value) { speedMultiplier = value; }

    public void setUniversalMultiplier(float value)
    {
        healthMultiplier = value;
        damageMultiplier = value;
        speedMultiplier = value;
    }
}
