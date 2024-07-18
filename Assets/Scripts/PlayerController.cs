using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health.IHealthListener
{
    public GameObject[] weapons;

    public float walkSpeed = 7f;
    public float mouseSense = 10f;
    public float jumpSpeed = 10f;

    CharacterController characterController;

    public Transform cameraTransform;

    float verticalAngle;
    float horizotalAngle;
    float verticalSpeed;

    bool isGrounded;
    float groundedTimer;

    int currentWeapon;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        verticalAngle = 0;
        horizotalAngle = transform.localEulerAngles.y;

        characterController = GetComponent<CharacterController>();

        currentWeapon = 0;

        UpdateWeapon();
    }


    void Update()
    {
        //땅과의 접촉 여부 
        if (!characterController.isGrounded)
        {
            if (isGrounded)
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer > 0.5f)
                {
                    isGrounded = false;
                }
            }
        }
        else
        {
            isGrounded = true;
            groundedTimer = 0;
        }

        //점프
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
        }

        // 평행 이동
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        move = move * walkSpeed * Time.deltaTime;
        move = transform.TransformDirection(move);
        characterController.Move(move);

        //좌,우 마우스
        float turnPlayer = Input.GetAxis("Mouse X") * mouseSense;
        horizotalAngle += turnPlayer;

        if (horizotalAngle > 360) horizotalAngle -= 360;
        if (horizotalAngle < 0) horizotalAngle += 360;

        Vector3 currentAngle = transform.localEulerAngles;
        currentAngle.y = horizotalAngle;
        transform.localEulerAngles = currentAngle;

        //상,하 마우스
        float turnCam = Input.GetAxis("Mouse Y") * mouseSense;
        verticalAngle -= turnCam;
        verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f);

        currentAngle = cameraTransform.localEulerAngles;
        currentAngle.x = verticalAngle;
        cameraTransform.localEulerAngles = currentAngle;

        //낙하
        verticalSpeed -= 10 * Time.deltaTime;
        if (verticalSpeed < -10)
        {
            verticalSpeed = -10;
        }

        Vector3 verticalMove = new Vector3(0, verticalSpeed, 0);
        verticalMove = verticalMove * Time.deltaTime;
        CollisionFlags flag = characterController.Move(verticalMove);

        if ((flag & CollisionFlags.Below) != 0)
        {
            verticalSpeed = 0;
        }


        //무기 변경
        if (Input.GetButtonDown("WeaponChange"))
        {
            currentWeapon++;
            if (currentWeapon >= weapons.Length)
            {
                currentWeapon = 0;
            }

            UpdateWeapon();
        }

    }


    void UpdateWeapon()
    {
        foreach (GameObject w in weapons)
        {
            w.SetActive(false);
        }

        weapons[currentWeapon].SetActive(true);
    }


    public void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("TellMeDie", 1.0f);
    }


    void TellMeDie()
    {
        GameManager.instance.GameOverScene();
    }
}
