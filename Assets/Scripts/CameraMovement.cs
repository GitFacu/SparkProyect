using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] Transform _target;
    [SerializeField] float _baseDistance = 6f;
    [SerializeField] float _maxDistance = 7f;
    [SerializeField] float _mouseSensitivity = 2f;
    [SerializeField] float _pitchMin = -13f;
    [SerializeField] float _pitchMax = 60f;

    [Header("Run Offset Settings")]
    [SerializeField] float _distanceLerpSpeed = 2f;
    [SerializeField] float _distanceLerpSpeedStop = 1f;
    [SerializeField] float _shiftReleaseDelay = 1f;

    [Header("Pitch Influence Settings")]
    [SerializeField] float _yOffsetMin = -1f;
    [SerializeField] float _yOffsetMax = 1f;
    [SerializeField] float _zOffsetMin = -1f;
    [SerializeField] float _zOffsetMax = 1f;

    [Header("Aiming Settings")]
    [SerializeField] float _aimXOffset = 2f;
    [SerializeField] float _aimYOffset = 0.5f;
    [SerializeField] float _aimZOffset = 1f;
    [SerializeField] float _offsetLerpSpeed = 5f;
    [SerializeField] float _aimFOV = 40f;
    [SerializeField] float _normalFOV = 60f;
    [SerializeField] float _fovLerpSpeed = 5f;

    private float _yaw = 0f;
    private float _pitch = 20f;
    private float _currentDistance;
    private bool _isShiftReleased = false;
    private float _releaseTime = 0f;

    private float _currentXVisualOffset = 0f;
    private float _currentXCamOffset = 0f;
    private float _currentYCamOffset = 0f;
    private float _currentZCamOffset = 0f;
    private Camera _cam;

    private void OnEnable()
    {
        PlayerHealth.OnDead += OnDead;
    }

    void Start()
    {
        _currentDistance = _baseDistance;
        _cam = GetComponent<Camera>();
        if (_cam == null) _cam = Camera.main;
        _cam.fieldOfView = _normalFOV;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDead -= OnDead;
    }

    void LateUpdate()
    {
        // Rotación con mouse
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _yaw += mouseX;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, _pitchMin, _pitchMax);

        // Movimiento y correr
        bool isMoving = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).sqrMagnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;
        if (!isRunning && Input.GetKeyUp(KeyCode.LeftShift))
        {
            _isShiftReleased = true;
            _releaseTime = Time.time;
        }

        if (isRunning)
        {
            _currentDistance = Mathf.Lerp(_currentDistance, _maxDistance, Time.deltaTime * _distanceLerpSpeed);
            _isShiftReleased = false;
        }
        else if (_isShiftReleased && Time.time > _releaseTime + _shiftReleaseDelay)
        {
            _currentDistance = Mathf.Lerp(_currentDistance, _baseDistance, Time.deltaTime * _distanceLerpSpeedStop);
        }

        // Pitch offsets
        float verticalFactor = Mathf.InverseLerp(_pitchMin, _pitchMax, _pitch);
        float yOffset = Mathf.Lerp(_yOffsetMin, _yOffsetMax, verticalFactor);
        float zOffset = Mathf.Lerp(_zOffsetMin, _zOffsetMax, verticalFactor);

        // Aiming (click derecho)
        bool isAiming = Input.GetMouseButton(1);

        float targetXVisualOffset = isAiming ? _aimXOffset : 0f;
        float targetXCamOffset = isAiming ? _aimXOffset : 0f;
        float targetYCamOffset = isAiming ? _aimYOffset : 0f;
        float targetZCamOffset = isAiming ? _aimZOffset : 0f;

        // Lerp para desplazamientos de cámara
        _currentXVisualOffset = Mathf.Lerp(_currentXVisualOffset, targetXVisualOffset, Time.deltaTime * _offsetLerpSpeed);
        _currentXCamOffset = Mathf.Lerp(_currentXCamOffset, targetXCamOffset, Time.deltaTime * _offsetLerpSpeed);
        _currentYCamOffset = Mathf.Lerp(_currentYCamOffset, targetYCamOffset, Time.deltaTime * _offsetLerpSpeed);
        _currentZCamOffset = Mathf.Lerp(_currentZCamOffset, targetZCamOffset, Time.deltaTime * _offsetLerpSpeed);

        // FOV
        float targetFOV = isAiming ? _aimFOV : _normalFOV;
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, targetFOV, Time.deltaTime * _fovLerpSpeed);

        // Calcular la posición de la cámara
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 offset = new Vector3(_currentXCamOffset, yOffset + _currentYCamOffset, -(_currentDistance + zOffset - _currentZCamOffset));
        Vector3 cameraOffset = rotation * offset;
        transform.position = _target.position + cameraOffset;

        // Punto de enfoque desplazado en X
        Vector3 visualTargetPos = _target.position + _target.right * _currentXVisualOffset;
        transform.LookAt(visualTargetPos);
    }

    private void OnDead()
    {
        this.enabled = false;
    }
}