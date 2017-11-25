using System;
using Android.Animation;

namespace ImageSlider
{
	public class AnimationListener :  Java.Lang.Object, Animator.IAnimatorListener
	{
		private readonly Action _animationStartListener;
		private readonly Action _animationRepeatListener;
		private readonly Action _animationEndListener;

		public AnimationListener(Action animationStartListener, Action animationRepeatListener, Action animationEndListener)
		{
			_animationStartListener = animationStartListener;
			_animationRepeatListener = animationRepeatListener;
			_animationEndListener = animationEndListener;
		}

        public void OnAnimationCancel(Animator animation)
        {
            animation.RemoveListener(this);
        }

        public void OnAnimationEnd(Animator animation)
        {
            animation.RemoveListener(this);
            _animationEndListener?.Invoke();
        }

        public void OnAnimationRepeat(Animator animation)
        {
            _animationRepeatListener?.Invoke();
        }

        public void OnAnimationStart(Animator animation)
        {
            _animationStartListener?.Invoke();
        }
    }
}