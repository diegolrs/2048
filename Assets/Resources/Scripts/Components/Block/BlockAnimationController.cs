using UnityEngine;
using DG.Tweening;

public class BlockAnimationController : MonoBehaviour
{
    private const string MergeAnimation = "MergeAnimation";
    public const float SlideAnimationTime = 0.18f;

    public float GetSlideAnimationTime() => SlideAnimationTime;

    [SerializeField] Animator _anim;

    public void PlayMergeAnimation() => _anim?.Play(MergeAnimation);
    public void MakeSlideAnimation(Vector2 target) => transform.DOLocalMove(target, SlideAnimationTime);
}