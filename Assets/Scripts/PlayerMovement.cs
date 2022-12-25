//PlayerMovement - handles player movement
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float sprintSpeed = 24f;
    public float sprintStaminaCost = 10f;
    public float nonSprintStaminaRegenRate = 10f;
    public float nonMovementStaminaRegenRate = 20f;

    public float maxStamina = 100f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Slider staminaSlider;

    Vector3 velocity;

    bool isGrounded;
    float stamina;

    void Start()
    {
        stamina = maxStamina;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isSprinting =
            Input.GetKey(KeyCode.LeftShift)
            && stamina > 0
            && stamina > sprintStaminaCost * Time.deltaTime;
        float sprintMultiplier = isSprinting ? 2 : 1;

        // Set the stamina regeneration rate based on whether the player is sprinting or not
        float staminaRegenRate = isSprinting
            ? nonSprintStaminaRegenRate
            : nonSprintStaminaRegenRate * 2;

        // Decrease stamina when sprinting
        if (x == 0 && z == 0)
        {
            staminaRegenRate = nonMovementStaminaRegenRate;
        }

        // Decrease stamina when sprinting
        if (isSprinting)
        {
            stamina -= sprintStaminaCost * Time.deltaTime;
        }
        // Increase stamina when not sprinting
        else
        {
            stamina += staminaRegenRate * Time.deltaTime;
        }
        // Clamp stamina between 0 and maxStamina
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        float currentSpeed = speed * sprintMultiplier;

        Vector3 moveDirection = Camera.main.transform.forward * z + Camera.main.transform.right * x;
        moveDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up).normalized;

        controller.Move(moveDirection * speed * sprintMultiplier * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        // Update stamina slider value
        staminaSlider.value = stamina;
    }
}
