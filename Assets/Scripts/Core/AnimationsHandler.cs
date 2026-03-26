using UnityEngine;

public class AnimationsHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayAnimation(string animationName)
    {
        if (_animator == null)
        {
            return;
        }
        _animator.Play(animationName);
    }
    public void StopAnimation()
    {
        _animator.StopPlayback();
    }
}

