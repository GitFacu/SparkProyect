using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Movimiento")] //Rotacion 1)
    [SerializeField] private float _rotationSpeed = 180;
    [SerializeField] private float _floatHeight = 0.4f;
    [SerializeField] private float _floatSpeed = 1.8f;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _ramdomOffSet;
    [SerializeField] private Vector3 _rotationAxis = Vector3.up;
    [SerializeField] private Space _rotationSpace = Space.World;

    private void Start() //Rotacion 2)
    {
        _startPosition = transform.position;
        _ramdomOffSet = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update()
    {
        RotateObjecto();
        //FloatObject
    }

    #region MOVIMIENTO DE OBJETO
    private void RotateObjecto()
    {
        transform.Rotate(_rotationAxis.normalized, _rotationSpeed * Time.deltaTime, _rotationSpace);
    }

    private void FloatObject()
    {
        float targetY = _startPosition.y + Mathf.Sin(Time.deltaTime * _floatSpeed + _ramdomOffSet) * _floatHeight;
        transform.position = new Vector3(_startPosition.x, targetY, _startPosition.z);
    }
    #endregion

}
