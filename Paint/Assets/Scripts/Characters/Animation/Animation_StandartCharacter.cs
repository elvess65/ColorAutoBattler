using UnityEngine;

namespace Paint.Characters.Animation
{
    /// <summary>
    /// Класс, отвечающий за анимацию стандартного персонажа
    /// </summary>
    public class Animation_StandartCharacter : MonoBehaviour, iAnimation
    {
        public Actions AnimationActionsController;
        public Animator AnimationController;

        [Header("Arsenal")]
        public Transform rightGunBone;
        public Transform leftGunBone;
        public Arsenal[] ArsenalList;

        private bool m_IsMoving = false;


        public void Init()
        {
            if (ArsenalList.Length > 0)
                SetArsenal(ArsenalList[0].name);
        }


        public void PlayMoveAnimation()
        {
            if (!m_IsMoving)
            {
                AnimationActionsController.Run();
                m_IsMoving = true;
            }
        }

        public void PlayStayAnimation()
        {
            if (m_IsMoving)
            {
                AnimationActionsController.Stay();
                m_IsMoving = false;
            }
        }


        public void PlayAimAnimation()
        {
            if (m_IsMoving)
                m_IsMoving = false;

            AnimationActionsController.Aiming();
        }

        public void PlayShootAnimation() => AnimationActionsController.Attack();

        public void PlayCooldownAnimation() { }

        public void PlayFinishShootAnimation() { AnimationActionsController.Stay(); }


        public void PlayDestroyAnimation() => AnimationActionsController.Death();

        public void PlayDamageAnimation() => AnimationActionsController.Damage();


        public void PlayShieldActivatedAnimation() => AnimationActionsController.Sitting();

        public void PlayShieldDeactivatedAnimation() => AnimationActionsController.Sitting();


        public void SetArsenal(string name)
        {
            foreach (Arsenal hand in ArsenalList)
            {
                if (hand.name == name)
                {
                    if (rightGunBone.childCount > 0)
                        Destroy(rightGunBone.GetChild(0).gameObject);

                    if (leftGunBone.childCount > 0)
                        Destroy(leftGunBone.GetChild(0).gameObject);

                    if (hand.rightGun != null)
                    {
                        GameObject newRightGun = (GameObject)Instantiate(hand.rightGun);
                        newRightGun.transform.parent = rightGunBone;
                        newRightGun.transform.localPosition = Vector3.zero;
                        newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    }

                    if (hand.leftGun != null)
                    {
                        GameObject newLeftGun = (GameObject)Instantiate(hand.leftGun);
                        newLeftGun.transform.parent = leftGunBone;
                        newLeftGun.transform.localPosition = Vector3.zero;
                        newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    }

                    AnimationController.runtimeAnimatorController = hand.controller;

                    return;
                }
            }
        }


        [System.Serializable]
        public struct Arsenal
        {
            public string name;
            public GameObject rightGun;
            public GameObject leftGun;
            public RuntimeAnimatorController controller;
        }
    }
}
