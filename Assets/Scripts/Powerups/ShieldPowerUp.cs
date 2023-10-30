using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    public float Duration;
    public GameObject Shield;
    public GameObject player;
    public int durationMax;

    private void Awake()
    {
        PlayerCollision playerCollision = player.gameObject.GetComponent<PlayerCollision>();

        Shield = GameObject.Find("Shield PowerUp");

        playerCollision.shield_ = Shield;

        Shield.SetActive(false);
    }

    private void OnEnable()
    {
        Duration = durationMax;
    }

    private void Update()
    {
        if (Duration > 0) Duration -= Time.deltaTime;
        else Shield.SetActive(false);



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemyWater") || other.CompareTag("enemyLightning") || other.CompareTag("enemyFire"))
        {
            BulletBase bullet_ = other.gameObject.GetComponent<BulletBase>();

            bullet_.DestroyBullet();
        }
    }
}
