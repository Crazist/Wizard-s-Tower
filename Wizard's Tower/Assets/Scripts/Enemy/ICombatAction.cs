using System;

namespace Enemy
{
    public interface ICombatAction
    {
        void Execute(Action onComplete);
        void Interrupt();
    }
}