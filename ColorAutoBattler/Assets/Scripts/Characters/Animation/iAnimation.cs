namespace Paint.Characters.Animation
{
    public interface iAnimation
    {
        void Init();

        void PlayMoveAnimation();
        void PlayStayAnimation();

        void PlayAimAnimation();
        void PlayShootAnimation();
        void PlayCooldownAnimation();
        void PlayFinishShootAnimation();

        void PlayDamageAnimation();
        void PlayDestroyAnimation();

        void PlayShieldActivatedAnimation();
        void PlayShieldDeactivatedAnimation();
    }
}
