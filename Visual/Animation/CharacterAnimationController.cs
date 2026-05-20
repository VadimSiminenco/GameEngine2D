using System;

namespace GameEngine2D.Visual.Animation
{
    public class CharacterAnimationController
    {
        private float animationTime;

        public float AnimationTime => animationTime;

        public void Update(float elapsedSeconds)
        {
            animationTime += elapsedSeconds;
        }
    }
}