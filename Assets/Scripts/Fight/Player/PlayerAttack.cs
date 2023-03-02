using UnityEngine;

namespace Fight.Player
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
