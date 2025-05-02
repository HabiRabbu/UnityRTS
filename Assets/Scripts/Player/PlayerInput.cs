using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float keyboardPanSpeed = 5;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoomDistance = 4f;
    [SerializeField] private float maxZoomDistance = 20f;

    private CinemachineFollow cinemachineFollow;
    private Vector3 startingFollowOffset;

    void Awake()
    {
        if (!cinemachineCamera.TryGetComponent(out cinemachineFollow))
        {
            Debug.LogError("Cinemachine Camera did not have CinemachineFollow! Zoom is broken...");
        }

        startingFollowOffset = cinemachineFollow.FollowOffset;
    }

    private void Update()
    {
        HandlePan();
        HandleZooming();
    }

    private void HandleZooming()
    {

        float zoomInput = 0f;

        zoomInput += Mouse.current.scroll.ReadValue().y * 15f;
        if (Keyboard.current.qKey.isPressed)
            zoomInput -= 1f;
        if (Keyboard.current.eKey.isPressed)
            zoomInput += 1f;


        if (Mathf.Abs(zoomInput) > 0.01f)
        {
            Vector3 followOffset = cinemachineFollow.FollowOffset;

            float originalY = followOffset.y;

            followOffset.y -= zoomInput * zoomSpeed * Time.deltaTime;
            followOffset.y = Mathf.Clamp(followOffset.y, minZoomDistance, maxZoomDistance);

            if (!Mathf.Approximately(followOffset.y, originalY))
            {
                followOffset.z += zoomInput * zoomSpeed * Time.deltaTime * 0.5f;
            }

            cinemachineFollow.FollowOffset = followOffset;
        }
    }

    private void HandlePan()
    {
        Vector2 moveAmount = Vector2.zero;

        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            moveAmount.y += keyboardPanSpeed;
        }
        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            moveAmount.y -= keyboardPanSpeed;
        }
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            moveAmount.x -= keyboardPanSpeed;
        }
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            moveAmount.x += keyboardPanSpeed;
        }

        moveAmount *= Time.deltaTime;
        cameraTarget.position += new Vector3(moveAmount.x, 0, moveAmount.y);
    }
}
