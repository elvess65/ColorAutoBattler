using Paint.Objects.Interfaces;

namespace Paint.Processors
{
    public interface iObjectBattleProcessor
    {
        event System.Action<iBattleObject> OnAttack;

        bool HasTarget { get; }

        void SetTarget(iBattleObject target);
        void ClearTarget();
        void ProcessAttack();
    }
}


