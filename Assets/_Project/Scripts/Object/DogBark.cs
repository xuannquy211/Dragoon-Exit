using UnityEngine;

public class DogBark : InteractiveObject
{
    [SerializeField] private Animator animator;
    
    public override void Activate()
    {
        animator.SetTrigger("Bark");
    }
}