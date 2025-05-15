using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    [SerializeField] private int maxHealth = 50;
    public static int health;
    public GameManager gameManager;
    private static HealthBar instance;
    private float dotTimer;
    private IEnumerator currentDamageCoroutine;
    public static bool isDead;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }
        if ( health <= 0 /*&& !isDead*/)
        {
            //isDead = true;
            gameManager.GameOver();
        }
    }


    public void TakeDamage(int damage)
    {
        {
            health -= damage;
        }
    }
    public void BeginDOTDamage(int damage, float duration)
    {
        if(currentDamageCoroutine != null)
        {
            StopCoroutine(currentDamageCoroutine);
        }

        currentDamageCoroutine = DotDamage(damage, duration);
        StartCoroutine(currentDamageCoroutine);
    }
    private IEnumerator DotDamage(int damage, float duration)
    {
        dotTimer += duration;

        while (dotTimer > 0)
        {

            yield return new WaitForSeconds(1.5f);
            dotTimer -= 1.5f;
            TakeDamage(damage);
        }
    }

}

