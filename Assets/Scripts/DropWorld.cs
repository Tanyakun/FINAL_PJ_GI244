using UnityEngine;

public class DropWorld : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
