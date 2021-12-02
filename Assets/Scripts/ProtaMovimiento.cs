using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ProtaMovimiento : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public GameObject BulletPrefab;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;
    private float LastShoot;
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource walkSFX;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource dieSFX;

    public Text Score;
    int ScoreNum;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        currentHealth = startingHealth;
        ScoreNum = 0;
        Score.text = "Puntos : " + ScoreNum;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Animator.SetBool("Shooting", true);
        }
        else Animator.SetBool("Shooting", false);
        // Movimiento
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 p = transform.position;
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-10.0f, 10.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
        transform.position = p;
        Animator.SetBool("Speed", Horizontal != 0.0f);

        // Detectar Suelo
        if (Physics2D.Raycast(transform.position, Vector3.down, 5.0f))
        {
            Grounded = true;
        }
        else Grounded = false;
        Animator.SetBool("Grounded", Grounded);

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Disparar
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
            
        }

        // Agacharse
        if (Input.GetKey(KeyCode.S) && Grounded)
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.S)) {
            Animator.SetBool("crouch", false);
        }
    }

    private void FixedUpdate()
    {

        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    private void Crouch()
    {
        walkSFX.Play();
        Animator.SetBool("crouch", true);
    }

    private void Jump()
    {
        jumpSFX.Play();
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void Shoot()
    {
        shootSFX.Play();
        Vector3 direction;
        if (transform.localScale.x == 10.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }

    public void Hit()
    {
        hitSFX.Play();
        currentHealth = Mathf.Clamp(currentHealth - 1, 0, startingHealth);

        if (currentHealth == 0)
        {
            dieSFX.Play();
            Destroy(gameObject);
            SceneManager.LoadScene("Portada");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            SceneManager.LoadScene("Portada");
        }

        if (collision.tag == "Coin1")
        {
            ScoreNum += 1;
            Score.text = "Puntos : " + ScoreNum;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Coin2")
        {
            ScoreNum += 5;
            Score.text = "Puntos : " + ScoreNum;
            Destroy(collision.gameObject);
        }
    }
}