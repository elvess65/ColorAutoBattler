using FrameworkPackage.UI.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameworkPackage.UI
{
    public class LevelLoader : MonoBehaviour
    {
        public UIAnimationController_Base AnimationController;

        protected int m_TargetLevelIndex;


        protected virtual void Start()
        {
            AnimationController.OnShowFinished += AnimationController_OnShowFinished;
            AnimationController.PlayAnimation(true);
        }

        public virtual void LoadLevel(int levelIndex)
        {
            m_TargetLevelIndex = levelIndex;

            AnimationController.OnHideFinished += AnimationController_OnHideFinished;
            AnimationController.gameObject.SetActive(true);
            AnimationController.PlayAnimation(false);
        }

        //Окончание анимации входа в сцену
        protected virtual void AnimationController_OnShowFinished()
        {
            AnimationController.OnShowFinished -= AnimationController_OnShowFinished;
            AnimationController.gameObject.SetActive(false);
        }

        //Окончание анимации выхода из сцены
        void AnimationController_OnHideFinished()
        {
            AnimationController.OnHideFinished -= AnimationController_OnHideFinished;
            LoadNextLevel();
        }

        protected virtual void LoadNextLevel() => SceneManager.LoadScene(m_TargetLevelIndex);
    }
}
