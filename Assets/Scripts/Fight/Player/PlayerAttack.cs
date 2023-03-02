using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fight
{
    public class PlayerAttack : CharacterAttack
    {
        private void Update()
        {
            if (HasInput())
            {
                Attack();
            }
        }

        private bool HasInput()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}
