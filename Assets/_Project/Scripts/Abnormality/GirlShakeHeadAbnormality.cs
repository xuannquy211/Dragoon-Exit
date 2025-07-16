using UnityEngine;

public class GirlShakeHeadAbnormality : Abnormality
{
    [SerializeField] private Animator _animator;
    
    public override void Active()
    {
        _animator.SetLayerWeight(1, 1f);
    }

    public override void Deactive()
    {
        _animator.SetLayerWeight(1, 0f);
    }
}