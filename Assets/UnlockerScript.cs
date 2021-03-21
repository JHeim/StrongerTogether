using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class UnlockerScript : MonoBehaviour
{
    private BoxCollider2D _collider;

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

    public Type GetUnlockable()
    {
        return _playerStates[_unlockable];
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider2D>();
        }
        Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
    }


}
