using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject focalPoint;

    private Rigidbody rb;

    private InputAction moveAction;
    private InputAction smashAction;
    private InputAction breakAction;

    private bool hasPowerUp = false;

    private Coroutine smashRoutine;

    public Image[] hearts; // Image HP
    private int lives = 2; // HP Player
    private Vector3 spawnPoint; // Spawn player

    public GameObject gameOverPanel; // Panel Game Over


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        smashAction = InputSystem.actions.FindAction("Smash");
        breakAction = InputSystem.actions.FindAction("Break");

        spawnPoint = transform.position; // save position spawnpoint
        UpdateHeartsUI(); // call Update HP
    }

    // Update is called once per frame
    void Update()
    {
        float v = moveAction.ReadValue<Vector2>().y;
        rb.AddForce(v * speed * focalPoint.transform.forward);
        if (breakAction.IsPressed())
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (smashAction.triggered && hasPowerUp)
        {
            smashRoutine = StartCoroutine(SmashRoutine());
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void RespawnOrGameOver()
    {
        if (lives > 0)
        {
            hearts[lives].enabled = false; // hide HP
            lives--;
            transform.position = spawnPoint;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            Debug.Log("Game Over");
            hearts[0].enabled = false; // Last hide HP
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true); // Game Over Panel On
            }
            enabled = false;
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            Destroy(gameObject);
        }// ถ้าตายครบแล้วจะหยุดการทำงานและจะทำลายทิ้ง
    }// Respawn 2 lives

    private void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = (i <= lives);
        }
    }// Update HP

    /*IEnumerator UpdeteRoutine()
    {
        float v = moveAction.ReadValue<Vector2>().y;
        rb.AddForce(v * speed * focalPoint.transform.forward);
        if (breakAction.IsPressed())
        {
            rb.linearVelocity = Vector3.zero;
        }
        yield return null;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            hasPowerUp = true;
            StartCoroutine(PowerUpCooldown(5f));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            var d = other.transform.position - transform.position;
            var dir = d.normalized;
            var enemyRb = other.gameObject.GetComponent<Rigidbody>();
            enemyRb.AddForce(dir * 10f, ForceMode.Impulse);
        }
    }

    IEnumerator PowerUpCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        hasPowerUp = false;
        if (smashRoutine != null)
        {
            StopCoroutine(smashRoutine);
        }
    }

    IEnumerator SmashRoutine()
    {
        float chargingTime = 0f;

        while (smashAction.IsPressed())
        {
            chargingTime += Time.deltaTime;
            if (chargingTime > 2.0f)
            {
                break;
            }
            yield return null;
        }

        if (chargingTime < 2.0f)
        {
            yield break;
        }

        var enemies =  FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        for (int i=0; i < enemies.Length; i++)
        {
            var enemyRb = enemies[i].GetComponent<Rigidbody>();
            enemyRb.AddForce(Vector3.up * 15f, ForceMode.Impulse);
        }
    }
}
