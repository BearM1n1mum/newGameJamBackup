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

    private float jumpforce = 5;
    [SerializeField] private Vector3 jumpForce = new Vector3(0, 10, 0);

    private Animator animator;                          // Animator komponenta, obavezna za animacije
    private CharacterController characterController;    // takoder, character controller
    private bool HasJumped;                             // bool i prijelazu u animatoru
    private bool isFalling;                             //bool i prijelazu u animatoru
    private bool isRunning;                             //bool i prijelazu u animatoru
    private bool isGroundedAnim;                        //bool i prijelazu u animatoru



    private void Start()
    {

        animator = GetComponent<Animator>();
        timeElapsed = 0f;
        isCentered = true;
        isGrounded = true;
        animator.SetBool("isRunning", true);             //postavljam kao poèetnu animaciju kako bi odmah od starta krenula "run" animacija
        isRunning = true;                               //uz svaki "SetBool" treba bit i varijabla bool
        isGroundedAnim = true;                         // s obzirom da isGrounded koristimo kao bitan dio mehanika skakanja, nazvao sam ga slicno kako bi razlikovali ovaj bool da je vezan za samu animaciju
        if (isCentered)
        {
            playerRigidBody.position = middleLane;
        }
    }
    private void Update()
    {



        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isTransitioning/* && isGrounded*/)
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

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isTransitioning /*&& isGrounded*/)
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


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded /*&& isTransitioning*/)
        {
            Jump();
            
        }


    }

    private IEnumerator MovementLeft()
    {
        //TimeElapsed je 0 na pocetku, transitioning je true da ne ulazi opet u ovu funkciju
        isTransitioning = true;
        while (timeElapsed < transitionTime && isCentered /*&& isGrounded*/)
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
        while (timeElapsed < transitionTime && isCentered /*&& isGrounded*/)
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
        while (timeElapsed < transitionTime && !isCentered /*&& isGrounded*/)
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
        while (timeElapsed < transitionTime && !isCentered /*&& isGrounded*/)
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

    private void Jump()
    {
        //playerRigidBody.linearVelocity = new Vector3(playerRigidBody.linearVelocity.x, 0f, playerRigidBody.linearVelocity.z);
        playerRigidBody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        animator.SetBool("isRunning", false);                            //kad skoci, u animatoru se is running postavlja na false
        animator.SetBool("isFalling", true);                            // kad skoci pokrece se bool isFalling u smislu "u zraku je"
        animator.SetBool("HasJumped", true);                           // kontra isGrounded-u pa mora bit provjera je li skocio
        animator.SetBool("isGroundedAnim", false);                    // isGrounded animacija false, jer je u zraku
        isRunning = false;                                           // popratni bool
        isFalling = true;                                           // popratni bool
        HasJumped = true;                                          // popratni bool
        isGroundedAnim = false;                                   // popratni bool
        isGrounded = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BoxCollider ground))
        {

            // ovdje se dogada trigger kontakt tako da sve resetiramo na kontra od onoga u "Jump()"
            isGrounded = true;
            animator.SetBool("isRunning", true);
            animator.SetBool("isGroundedAnim",true);
            animator.SetBool("HasJumped",false);
            isGroundedAnim = true;
            isRunning = true;
            HasJumped = false;
            Debug.Log("isRunning" + isRunning);
            Debug.Log("isGroundedAnim" + isGroundedAnim);
            Debug.Log("hasjumped" + HasJumped);
            Debug.Log("Grounded");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out BoxCollider ground))
        {
            isGrounded = false;
           
            Debug.Log("Grounded");
        }
    }

}