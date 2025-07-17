using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangFunctions : MonoBehaviour
{
    private enum BoomerangState { Rest, Launched, Returning, Melee }
    private BoomerangState state = BoomerangState.Rest;

    [Header("References")]
    [SerializeField] GameObject _restBoomerang;
    [SerializeField] GameObject _launchBoomerang;
    [SerializeField] GameObject _meleeBoomerang;
    [SerializeField] Collider _catchZoneCollider;
    //[SerializeField] HUDController _hud;

    

    [Header("Movement Settings")]
    [SerializeField] float _launchSpeed = 10f;
    [SerializeField] float _returnSpeed = 8f;
    [SerializeField] float _maxDistance = 15f;
    [SerializeField] float _collisionDetectRadius = 0.2f;
    [SerializeField] LayerMask _collisionLayers;
    [SerializeField] float _catchDistance = 0.5f;

    [Header("Rest Position Offset")]
    [SerializeField] Vector3 _restOffset = new Vector3(0, 0, -1);

    [Header("Launch Offset")]
    [SerializeField] Vector3 _launchOffset = new Vector3(0, 0, 1);

    [Header("Melee Settings")]
    [Tooltip("Cooldown (seconds) después de un ataque melee/cargado")]
    [SerializeField] float _meleeCooldown = 2f;
    [Tooltip("Segundos necesarios para carga de ataque cargado")]
    [SerializeField] float _chargeThreshold = 1f;
    private float _meleeCooldownTimer = 0f;

    private bool _rightClicked = false;
    private Vector3 _launchDirection;
    private Vector3 _launchStartPosition;

    // Melee state vars
    private bool _isHolding = false;
    private bool _isCharging = false;
    private float _holdTime = 0f;
    [SerializeField] float _multi = 0f;
    public float Multi => _multi;
    private Camera mainCamera;
    private Animator meleeAnimator;

    void Start()
    {
        state = BoomerangState.Rest;
        _restBoomerang.SetActive(true);
        _launchBoomerang.SetActive(false);
        _meleeBoomerang.SetActive(false);
        _restBoomerang.transform.localPosition = _restOffset;
        _restBoomerang.transform.localRotation = Quaternion.identity;
        if (_catchZoneCollider != null)
            _catchZoneCollider.enabled = true;

        mainCamera = Camera.main;
        meleeAnimator = _meleeBoomerang.GetComponent<Animator>();
        _multi = 1f;
    }

    void Update()
    {
        if (_meleeCooldownTimer > 0f)
            _meleeCooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1)) _rightClicked = true;
        if (Input.GetMouseButtonUp(1)) _rightClicked = false;

        switch (state)
        {
            case BoomerangState.Rest:
                HandleRestState();
                break;

            case BoomerangState.Melee:
                HandleMeleeState();
                break;

            case BoomerangState.Launched:
            case BoomerangState.Returning:
                MoveLaunchBoomerang();
                break;
        }
    }

    private void HandleRestState()
    {
        _restBoomerang.transform.localPosition = _restOffset;
        _restBoomerang.transform.localRotation = Quaternion.identity;

        // Launch
        if (_rightClicked && Input.GetMouseButtonDown(0) && _meleeCooldownTimer <= 0f)
        {
            Launch();
            return;
        }

        // Begin Melee
        if (Input.GetMouseButtonDown(0) && !_rightClicked && _meleeCooldownTimer <= 0f)
        {
            state = BoomerangState.Melee;
            //hud._barraCarga.gameObject.SetActive(true);
            _isHolding = true;
            _isCharging = false;
            _holdTime = 0f;
            Debug.Log("Melee");
            // Play normal attack
            _meleeBoomerang.SetActive(true);
            if (meleeAnimator != null)
                meleeAnimator.Play("Boomerang Attack");
        }
    }

    private void HandleMeleeState()
    {
        if (!_isHolding) return;
        _restBoomerang.SetActive(false);
        // 1) Acumular tiempo de hold
        _holdTime += Time.deltaTime;
        // Limitar a threshold para no crecer más
        _holdTime = Mathf.Min(_holdTime, _chargeThreshold);

        // 2) Actualizar barra de carga de 0 a 1 de forma totalmente lineal
        float progress = _holdTime / _chargeThreshold;
        //hud?.UpdateChargeBar(progress);

        // Obtener estado de animación actual
        AnimatorStateInfo info = meleeAnimator.GetCurrentAnimatorStateInfo(0);

        // 3) Al completar animación normal y si mantiene presionado, entrar en carga
        if (info.IsName("Boomerang Attack") && info.normalizedTime >= 1f && Input.GetMouseButton(0) && !_isCharging)
        {
            _isCharging = true;
            meleeAnimator?.Play("Boomerang Attack Charge");
            //hud?.UpdateChargeBar(1f); // lleno
            return;
        }

        // 4) Si suelta después de cargar o después de iniciar carga
        if (Input.GetMouseButtonUp(0) && (info.IsName("Boomerang Attack Charge") || _isCharging))
        {
            _isHolding = false;
            _isCharging = false;
            // Release
            _multi = Mathf.Lerp(1f, 2f, Mathf.Clamp01(_holdTime / _chargeThreshold));
            meleeAnimator?.Play("Boomerang Attack Release");
            //hud?.HideChargeBar();
            StartCoroutine(HideMeleeAfterRelease());
            return;
        }

        // Release
        if (Input.GetMouseButtonUp(0))
        {
            if (_isCharging)
            {
                if (meleeAnimator != null)
                    _multi = Mathf.Lerp(1f, 2f, Mathf.Clamp01(_holdTime / _chargeThreshold));
                meleeAnimator.Play("Boomerang Attack Release");
            }
            // Reset
            _isHolding = false;
            state = BoomerangState.Rest;
            _meleeCooldownTimer = _meleeCooldown;
            // Hide after animation
            StartCoroutine(HideMeleeAfterRelease());
        }
    }

    private IEnumerator HideMeleeAfterRelease()
    {
        // Wait for release clip length
        AnimatorStateInfo info = meleeAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(info.length);
        _meleeBoomerang.SetActive(false);
        //hud._barraCarga.gameObject.SetActive(false);//
        state = BoomerangState.Rest;
        _multi = 1f;
        _restBoomerang.SetActive(true);
    }

    private void Launch()
    {
        state = BoomerangState.Launched;
        _restBoomerang.SetActive(false);
        _launchBoomerang.SetActive(true);
        _launchBoomerang.transform.SetParent(null);
        _launchBoomerang.transform.rotation = Quaternion.identity;
        Vector3 startPos = transform.position + transform.TransformVector(_launchOffset);
        _launchBoomerang.transform.position = startPos;
        _launchStartPosition = startPos;
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 target = ray.origin + ray.direction * _maxDistance;
        _launchDirection = (target - startPos).normalized;
        if (_catchZoneCollider != null)
            _catchZoneCollider.enabled = false;
    }

    private void MoveLaunchBoomerang()
    {
        Vector3 currentPos = _launchBoomerang.transform.position;
        if (state == BoomerangState.Launched)
        {
            _launchBoomerang.transform.position = currentPos + _launchDirection * _launchSpeed * Time.deltaTime;
            bool hitEnv = Physics.CheckSphere(currentPos, _collisionDetectRadius, _collisionLayers);
            float traveled = Vector3.Distance(_launchStartPosition, currentPos);
            if (hitEnv || traveled >= _maxDistance)
            {
                state = BoomerangState.Returning;
                if (_catchZoneCollider != null)
                    _catchZoneCollider.enabled = true;
            }
        }
        else // Returning
        {
            Vector3 returnDir = (transform.position - currentPos).normalized;
            _launchBoomerang.transform.position = currentPos + returnDir * _returnSpeed * Time.deltaTime;
            if (Vector3.Distance(currentPos, transform.position) <= _catchDistance)
                CatchBoomerang();
        }
    }

    private void CatchBoomerang()
    {
        state = BoomerangState.Rest;
        _launchBoomerang.SetActive(false);
        _restBoomerang.SetActive(true);
        _restBoomerang.transform.localPosition = _restOffset;
        _restBoomerang.transform.localRotation = Quaternion.identity;
        _launchBoomerang.transform.SetParent(transform);
        _launchBoomerang.transform.localPosition = _launchOffset;
        if (_catchZoneCollider != null)
            _catchZoneCollider.enabled = true;
    }
}
