using UnityEngine;
using System;
using TMPro;

public class HayMachine : MonoBehaviour
{
    public float horizontalBoundary = 22;

    public float movementSpeed;
    public GameObject hayBalePrefab;
    public Transform haySpawnpoint;
    public float shootInterval;
    private float shootTimer;

    int points = 0;
    public TextMeshProUGUI pointsText;
    void addPoint()
    {
        points++;
        pointsText.text = points.ToString();
    }

    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }

    private void UpdateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // 1

        if (horizontalInput < 0 && transform.position.x > -horizontalBoundary) // 1
        {
            transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
        }
        else if (horizontalInput > 0 && transform.position.x < horizontalBoundary) // 2
        {
            transform.Translate(transform.right * movementSpeed * Time.deltaTime);
        }
    }

    private void ShootHay()
    {
        Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);
        audioSource.Play();
    }

    private void UpdateShooting() {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0 && Input.GetKey(KeyCode.Space)) {
            shootTimer = shootInterval;
            ShootHay();
        }
    }

    private void OnEnable()
    {
        Sheep.GotHit += addPoint;
    }

    // Es OBLIGATORIO desuscribirse cuando el objeto se desactiva para evitar errores de memoria
    private void OnDisable()
    {
        Sheep.GotHit -= addPoint;
    }
}
