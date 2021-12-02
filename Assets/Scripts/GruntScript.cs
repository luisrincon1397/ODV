using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public Transform Prota;
    public GameObject BulletPrefab;

    private int Health = 3;
    private float LastShoot;

    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource dieSFX;

    private Animator Animator;

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Prota == null) return;

        Vector3 direction = Prota.position - transform.position;
        if (direction.x >= 0.0f) transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
        else transform.localScale = new Vector3(-10.0f, 10.0f, 10.0f);

        float distance = Mathf.Abs(Prota.position.x - transform.position.x);

        if (distance < 10.0f && Time.time > LastShoot + 0.75f)
        {
            Shoot();
            Animator.SetBool("Shooting", true);
            LastShoot = Time.time;
        }
        else Animator.SetBool("Shooting", false);
    }

    private void Shoot()
    {
        shootSFX.Play();

        Vector3 direction = new Vector3(transform.localScale.x, 0.0f, 0.0f);
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }

    public void Hit()
    {
        hitSFX.Play();

        Health -= 1;
        if (Health == 0) 
        {
            dieSFX.Play();
            Destroy(gameObject); 
        }
    }
}