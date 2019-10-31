using UnityEngine;

namespace Paint.InputSystem
{
    public class KeyboardInputManager : BaseInputManager
    {
        public override void UpdateInput()
        {
            Vector2 mDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            OnMove?.Invoke(mDir.normalized);

            if (Input.GetMouseButtonDown(0))
                OnShoot?.Invoke(GetDirFromScreenCenterToMouse().normalized);
        }

        Vector2 GetDirFromScreenCenterToMouse()
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 dirFromCenterToMouse = mousePos - screenCenter;

            return dirFromCenterToMouse;
        }
    }
}
