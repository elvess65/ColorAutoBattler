using Paint.Grid;

namespace Paint.Inputs
{
    public interface iInputManager
    {
        event System.Action<GridCell> OnInputResult;

        void ProcessUpdate(float deltaTime);
    }
}
