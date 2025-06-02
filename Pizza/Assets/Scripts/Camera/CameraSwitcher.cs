using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;
    public Camera overheadCamera;

    private bool isOverheadCameraActive = false;

    public bool IsOverheadCameraActive()
    {
        return isOverheadCameraActive;
    }

    void Start()
    {
        playerCamera.gameObject.SetActive(true);
        overheadCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        isOverheadCameraActive = !isOverheadCameraActive;

        if (isOverheadCameraActive)
        {
            playerCamera.gameObject.SetActive(false);
            overheadCamera.gameObject.SetActive(true);
        }
        else
        {
            playerCamera.gameObject.SetActive(true);
            overheadCamera.gameObject.SetActive(false);
        }
    }
}