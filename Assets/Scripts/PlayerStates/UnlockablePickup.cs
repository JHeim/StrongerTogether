using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnlockablePickup : MonoBehaviour
{

    [SerializeField]
    private float _floatSpeed = 1.5f;
    
    [SerializeField]
    private float _floatSpan = 0.5f;

    private float _startYPosition;

    public UnityEvent PickupEvent = new UnityEvent();

    enum UnlockableEnum
    {
        Goop,
        Glide,
        Bulb
    }

    private Dictionary<UnlockableEnum, Type> _playerStates = new Dictionary<UnlockableEnum, Type>()
    {
        { UnlockableEnum.Goop, typeof(PlayerGoopState) },
        { UnlockableEnum.Glide, typeof(PlayerGlideState) },
        { UnlockableEnum.Bulb, typeof(PlayerBulbState) },
    };

    [SerializeField]
    private UnlockableEnum _unlockable;

    public Type PickupUnlockable()
    {
        var unlockable = GetUnlockable();

        PickupEvent?.Invoke();

        return unlockable;
    }

    public Type GetUnlockable()
    {
        return _playerStates[_unlockable];
    }

    private void Start()
    {
        _startYPosition = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            _startYPosition + Mathf.Sin(Time.time * _floatSpeed) * _floatSpan / 2.0f,
            transform.position.z);
    }

}
