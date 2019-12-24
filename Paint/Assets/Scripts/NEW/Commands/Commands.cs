using Paint.Grid;
using Paint.Objects.Interfaces;
using UnityEngine;

namespace Paint.Commands
{
    public enum CommandTypes { MoveCommand, AttackCommand }

    public interface iCommand
    {
        CommandTypes CommandType { get; }
    }

    public class MoveCommand : iCommand
    {
        public CommandTypes CommandType { get; private set; }
        public Vector3 MovePos { get; private set; }
        public Vector2Int TargetCellCoord { get; private set; }
        public System.Action OnMovementFinished;

        public MoveCommand(Vector3 movePos, int x, int y, System.Action onMovementFinishedFunc = null)
        {
            CommandType = CommandTypes.MoveCommand;

            MovePos = movePos;
            TargetCellCoord = new Vector2Int(x, y);
            OnMovementFinished = onMovementFinishedFunc;
        }
    }

    public class AttackCommand : iCommand
    {
        public CommandTypes CommandType { get; private set; }
        public iBattleObject Target { get; private set; }

        public AttackCommand(iBattleObject target)
        {
            CommandType = CommandTypes.AttackCommand;
            Target = target;
        }
    }
}
