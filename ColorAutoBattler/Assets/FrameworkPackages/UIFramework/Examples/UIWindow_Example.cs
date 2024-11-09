using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameworkPackage.UI.Windows.Example
{
    public class UIWindow_Example : UIWindow_CloseButton
    {
        [Space(10)]
        public Button Button_Example;

        protected override void Init()
        {
            Button_Example.onClick.AddListener(Button_Example_PressHandler);

            base.Init();
        }

        void Button_Example_PressHandler()
        {
            Debug.Log("UIWindow_Example: Press example button");
        }
    }
}
