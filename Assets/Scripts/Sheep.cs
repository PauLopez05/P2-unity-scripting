using System;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float runSpeed;
    public float gotHayDestroyDelay;
    private bool hitByHay;

    public float dropDestroyDelay ; // 1
    private Collider myCollider; // 2
    private Rigidbody myRigidbody;
    private SheepSpawner sheepSpawner;

    public static event Action GotHit;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }

    private void HitByHay()
    {
        sheepSpawner.RemoveSheepFromList (gameObject);
        hitByHay = true; // 1
        runSpeed = 0; // 2

        //make sheep fly
        myRigidbody .isKinematic = false;
        myRigidbody.AddForce(new Vector3(0,50,30), ForceMode.Impulse);
        gameObject.AddComponent<Rotate>().rotationSpeed = new Vector3(-1000,0,0);
        audioSource.Play();

        GotHit?.Invoke();

        Destroy(gameObject, gotHayDestroyDelay); // 3
    }

    private void OnTriggerEnter (Collider other) // 1
    {
        if (other.CompareTag("Hay") && !hitByHay) // 2
        {
            Destroy(other.gameObject); // 3
            HitByHay(); // 4
        }
        else if (other.CompareTag("DropSheep"))
        {
            Drop();
        }

    }

    private void Drop()
    {
        sheepSpawner.RemoveSheepFromList (gameObject);
        myRigidbody .isKinematic = false; // 1
        myCollider .isTrigger = false; // 2
        Destroy(gameObject, dropDestroyDelay ); // 3
    }

    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }

}
