using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float fallForce = 0.5f;

    [Space(10)]
    [SerializeField] Transform groundRaycaster;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask groundSolidLayer;

    [Header("Absorb")]
    [SerializeField] float cooldownAbsorb;
    private bool isAbsorbing;

    [Header("Release")]
    [SerializeField] float cooldownRelease;
    [SerializeField] GameObject releaseLight;
    [SerializeField] float timerShutdownLight = 0.5f;
    private bool isReleasing;

    public bool CanMove { get; set; }
    public bool CanGoNextLevel { get; set; }


    private float horizontalDir;

    private bool isFacingRight = true;

    private Rigidbody2D rb;
    private Animator animator;

    public event Action OnAbsorb;
    public event Action OnRelease;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!CanMove)
        {
            horizontalDir = 0f;
            animator.SetFloat("Horizontal", horizontalDir);
            return;
        }

        horizontalDir = Input.GetAxisRaw("Horizontal");

        if (!isAbsorbing && !isReleasing)
        {
            float absHorizontal = Mathf.Abs(horizontalDir);
            animator.SetFloat("Horizontal", absHorizontal);
        }

        GatherAction();

        if (Input.GetButtonDown("Jump") && IsGroundedSolid())
        {
            AudioManager.Instance.PlayEffect("RedJump");
            rb.AddForce(Vector2.up * jumpForce * 1.5f, ForceMode2D.Impulse);
        }
        else if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            AudioManager.Instance.PlayEffect("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        

        if(CanGoNextLevel && Input.GetKeyDown(KeyCode.W))
        {
            FindFirstObjectByType<LevelLoadingManager>().LoadNextLevel();
        }

        Flip();
    }

    private void GatherAction()
    {
        if (isAbsorbing)
            return;

        if (isReleasing)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(CoResetAbsorb());
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CoResetRelease());
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalDir * speed, rb.velocity.y);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundRaycaster.position, 0.5f, groundLayer);
    }

    private bool IsGroundedSolid()
    {
        return Physics2D.OverlapCircle(groundRaycaster.position, 0.5f, groundSolidLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontalDir < 0f || !isFacingRight && horizontalDir > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator CoResetAbsorb()
    {
        // absorb

        OnAbsorb?.Invoke();
        isAbsorbing = true;

        AudioManager.Instance.PlayEffect("Absorb");

        animator.SetTrigger("Absorb");

        yield return new WaitForSeconds(cooldownAbsorb);

        animator.ResetTrigger("Absorb");

        isAbsorbing = false;
    }

    private IEnumerator CoResetRelease()
    {
        // release

        OnRelease?.Invoke();
        isReleasing = true;

        releaseLight.SetActive(true);
        StartCoroutine(CoShutdownReleaseLight());

        AudioManager.Instance.PlayEffect("Release");

        animator.SetTrigger("Release");

        yield return new WaitForSeconds(cooldownRelease);

        animator.ResetTrigger("Release");

        isReleasing = false;
    }

    private IEnumerator CoShutdownReleaseLight()
    {
        yield return new WaitForSeconds(timerShutdownLight);

        releaseLight.SetActive(false);
    }
}
