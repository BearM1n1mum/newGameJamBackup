using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IncomingObjectMovement : MonoBehaviour
{
    private Rigidbody incomingObjectRB;
    [SerializeField] private float speed = 5f;

    private void Start()
    {
        incomingObjectRB = gameObject.GetComponent<Rigidbody>();
        incomingObjectRB.linearVelocity = transform.forward * -speed;
        StartCoroutine(DestroyObjectAfterTime(12f));
    }

    private IEnumerator DestroyObjectAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement player))
        {
            Destroy(gameObject);
        }
    }
}
