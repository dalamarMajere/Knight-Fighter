using System;

namespace GameLogic
{
    public static class Events
    {
        public static event Action OnPlayerDied;

        public static void RaisePlayerDied()
        {
            OnPlayerDied?.Invoke();
        }
    }
}