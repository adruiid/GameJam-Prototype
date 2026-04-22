using UnityEngine;
using System.Collections.Generic;

public class CogwheelSpinner : MonoBehaviour
{
    [SerializeField] GameObject cogwheelPrefab;
    [SerializeField] float orbitRadius = 2.5f;
    [SerializeField] float rotationSpeed = 360f; // degrees per second
    [SerializeField] float heightOffset = 1.5f; // waist height
    [SerializeField] int skillLevel = 1; // 1, 3, or 5 cogwheels
    [SerializeField] float damageAmount = 10f;
    [SerializeField] LayerMask enemyLayer;
    
    private List<Cogwheel> cogwheels = new List<Cogwheel>();
    private float currentRotation = 0f;

    void Start()
    {
        SpawnCogwheels();
    }

    void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        UpdateCogwheelPositions();
    }

    private void SpawnCogwheels()
    {
        int cogwheelCount = skillLevel switch
        {
            1 => 1,
            2 => 3,
            3 => 5,
            _ => 1
        };

        for (int i = 0; i < cogwheelCount; i++)
        {
            float angle = (360f / cogwheelCount) * i;
            GameObject cogwheelObj = Instantiate(cogwheelPrefab, transform.position, Quaternion.identity);
            
            Cogwheel cogwheel = cogwheelObj.GetComponent<Cogwheel>();
            if (cogwheel == null)
            {
                cogwheel = cogwheelObj.AddComponent<Cogwheel>();
            }
            
            cogwheel.Initialize(this, angle, damageAmount, enemyLayer);
            cogwheels.Add(cogwheel);
        }
    }

    private void UpdateCogwheelPositions()
    {
        int cogwheelCount = cogwheels.Count;
        for (int i = 0; i < cogwheelCount; i++)
        {
            float angle = (360f / cogwheelCount) * i + currentRotation;
            float rad = angle * Mathf.Deg2Rad;
            
            Vector3 offset = new Vector3(Mathf.Cos(rad) * orbitRadius, heightOffset, Mathf.Sin(rad) * orbitRadius);
            cogwheels[i].transform.position = transform.position + offset;
        }
    }

    public void SetSkillLevel(int level)
    {
        if (level != skillLevel && (level == 1 || level == 2 || level == 3))
        {
            skillLevel = level;
            // Clear old cogwheels
            foreach (Cogwheel cogwheel in cogwheels)
            {
                Destroy(cogwheel.gameObject);
            }
            cogwheels.Clear();
            SpawnCogwheels();
        }
    }
}

public class Cogwheel : MonoBehaviour
{
    private CogwheelSpinner spinner;
    private float baseAngle;
    private float damage;
    private LayerMask enemyLayer;
    private HashSet<Collider> hitThisFrame = new HashSet<Collider>();

    public void Initialize(CogwheelSpinner spinnerRef, float angle, float damageAmount, LayerMask layer)
    {
        spinner = spinnerRef;
        baseAngle = angle;
        damage = damageAmount;
        enemyLayer = layer;
    }

    void Update()
    {
        hitThisFrame.Clear();
        CheckForEnemies();
    }

    private void CheckForEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, 0.5f, enemyLayer);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (!hitThisFrame.Contains(enemyCollider))
            {
                EnemyStats enemyStats = enemyCollider.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.recieveDamage(damage);
                    hitThisFrame.Add(enemyCollider);
                }
            }
        }
    }
}
