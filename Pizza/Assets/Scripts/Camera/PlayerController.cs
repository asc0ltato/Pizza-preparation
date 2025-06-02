using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    private Camera playerCamera;
    private Vector2 rotation;

    private float rotateSpeed = 600f;
    private float walkSpeed = 2f;
    private float runSpeed = 4f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        rotation = new Vector2(0f, 180f);
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * rotateSpeed * Time.deltaTime;
            rotation.y += mouseDelta.x;
            rotation.x = Mathf.Clamp(rotation.x - mouseDelta.y, -90f, 90f);

            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0f, 0f);
        }

        float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        move *= moveSpeed;

        characterController.Move(move * Time.deltaTime);
    }
}