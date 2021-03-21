using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableTracker
{

    private Dictionary<Type, bool> UnlockedCharacters;


    public UnlockableTracker()
    {
        UnlockedCharacters = new Dictionary<Type, bool>()
        {
            { typeof(PlayerSpringState), true },
            { typeof(PlayerBulbState), false },
            { typeof(PlayerGlideState), false },
            { typeof(PlayerGoopState), false },
        };
    }

    public void UnlockCharacter(Type state)
    {
        if (UnlockedCharacters.ContainsKey(state))
        {
            UnlockedCharacters[state] = true;
        }
    }

    public void LockCharacter(Type state)
    {
        if (UnlockedCharacters.ContainsKey(state))
        {
            UnlockedCharacters[state] = false;
        }
    }

    public bool IsCharacterUnlocked(Type state)
    {
        bool isUnlocked = false;

        if (UnlockedCharacters.ContainsKey(state))
        {
            isUnlocked = UnlockedCharacters[state];
        }

        return isUnlocked;
    }

}
