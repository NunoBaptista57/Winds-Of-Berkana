using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class BoatSailController : PlayerBoatBehaviour
{
    [System.Serializable]
    public class ModeParams
    {
        [ReadOnlyInspector] public string name;
        [ReadOnlyInspector] public SailMode mode;
        public float GravityTorqueMultiplier = 1;
        public float GravityForceMultiplier = 1;
    }

    [Header("Modes")]
    [SerializeField] ModeParams[] _modeParams;
    Dictionary<SailMode, ModeParams> modeParams = new Dictionary<SailMode, ModeParams>();
    List<SailMode> modeList = (Enum.GetValues(typeof(SailMode)) as SailMode[]).ToList();

    [Header("Gravity")]
    [Min(0), SerializeField] float GravityTorque = 1.0f;
    [Min(0), SerializeField] float GravityForce = 1.0f;

    new Rigidbody rigidbody;

    private void OnValidate()
    {
        Array.Resize(ref _modeParams, modeList.Count);

        for (int i = 0; i < modeList.Count; i++)
        {
            _modeParams[i].name = modeList[i].ToString();
            _modeParams[i].mode = modeList[i];
        }
    }

    protected override void Awake()
    {
        base.Awake();
        rigidbody = GetComponent<Rigidbody>();
        foreach (var p in _modeParams)
        {
            modeParams[p.mode] = p;
        }
    }

    private void FixedUpdate()
    {
        var currentParams = modeParams[player.sailMode];

        rigidbody.AddTorque(GravityTorque * currentParams.GravityTorqueMultiplier * Vector3.Cross(Vector3.up, transform.forward).normalized);
    }
}
