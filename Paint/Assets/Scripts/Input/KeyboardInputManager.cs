using UnityEngine;

namespace Paint.InputSystem
{
    public class KeyboardInputManager : BaseInputManager
    {
        public override void UpdateInput()
        {
            Vector2 mDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            OnMove?.Invoke(mDir.normalized);
        }
    }
}
