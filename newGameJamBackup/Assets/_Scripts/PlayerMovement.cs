using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float speed;


    [SerializeField] private Rigidbody playerRigidBody;
    private Vector3 movement;
    private Vector3 moveDirection;
    private Vector3 leftLane = new Vector3(-3, 0, 0);
    private Vector3 rightLane = new Vector3(3, 0, 0);
    private Vector3 middleLane = new Vector3(0, 0, 0);


    private float timeElapsed; //Vrijeme koje je proteklo od pocetka tranzicije
    [SerializeField] private float transitionTime = 0.5f; //Koliko dugo traje tranzicija izmedju traka 
    private bool isTransitioning = false; //Da li je igrac trenutno u tranziciji izmedju traka
    private bool isCentered;
    private bool isLeft;
    private bool isRight;
    private bool isGrounded;
    //private float jumpForce = 6;
    [SerializeField] private Vector3 jumpForce = new Vector3(0, 10, 0);


    private void Start()
    {
        timeElapsed = 0f;
        isCentered = true;
        isGrounded = true;
        if (isCentered)
        {
            playerRigidBody.position = middleLane;
        }
    }
    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isTransitioning)
        {
            //Jbg mora coroutine jer Lerp ne radi bez vremena
            if (isCentered)
            {
                StartCoroutine(MovementLeft());
            }
            else if (isLeft)
            {
                return;
            }
            else
            {
                StartCoroutine(BackToMiddleFromRight());
            }
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isTransitioning)
        {
            if (isCentered)
            {
                StartCoroutine(MovementRight());
            }
            else if (isRight)
            {
                return;
            }
            else
            {
                StartCoroutine(BackToMiddleFromLeft());
            }
        }

        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRigidBody.AddForce(jumpForce, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator MovementLeft()
    {
        //TimeElapsed je 0 na pocetku, transitioning je true da ne ulazi opet u ovu funkciju
        isTransitioning = true;
        while (timeElapsed < transitionTime && isCentered)
        {
            //while se vrti pola sekunde sto se moze i smanjiti i povecati
            Vector3 startPos = new Vector3(middleLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Vector3 endPos = new Vector3(leftLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            playerRigidBody.position = Vector3.Lerp(startPos, endPos, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //Na kraju resetira vrijeme i stavi transitioning na false
        timeElapsed = 0f;
        isTransitioning = false;
        isLeft = true;
        isCentered = false;
    }

    private IEnumerator MovementRight()
    {
        //TimeElapsed je 0 na pocetku, transitioning je true da ne ulazi opet u ovu funkciju

        isTransitioning = true;
        while (timeElapsed < transitionTime && isCentered)
        {
            //while se vrti pola sekunde sto se moze i smanjiti i povecati
            Vector3 startPos = new Vector3(middleLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Vector3 endPos = new Vector3(rightLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            playerRigidBody.position = Vector3.Lerp(startPos, endPos, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        //Na kraju resetira vrijeme i stavi transitioning na false
        timeElapsed = 0f;
        isTransitioning = false;
        isRight = true;
        isCentered = false;
    }

    private IEnumerator BackToMiddleFromLeft()
    {
        isTransitioning = true;
        while (timeElapsed < transitionTime && !isCentered)
        {
            Vector3 startPos = new Vector3(leftLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Vector3 endPos = new Vector3(middleLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            playerRigidBody.position = Vector3.Lerp(startPos, endPos, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0f;
        isTransitioning = false;
        isLeft = false;
        isCentered = true;
    }

    private IEnumerator BackToMiddleFromRight()
    {
        isTransitioning = true;
        while (timeElapsed < transitionTime && !isCentered)
        {
            Vector3 startPos = new Vector3(rightLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            Vector3 endPos = new Vector3(middleLane.x, gameObject.transform.position.y, gameObject.transform.position.z);
            playerRigidBody.position = Vector3.Lerp(startPos, endPos, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        timeElapsed = 0f;
        isTransitioning = false;
        isRight = false;
        isCentered = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out JumpSpace ground))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out JumpSpace ground))
        {
            isGrounded = false;
        }
    }
}