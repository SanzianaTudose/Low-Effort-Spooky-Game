using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollAnimationHandler : MonoBehaviour
{
    public Animator animator;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenScroll()
    {
        animator.SetBool("MinigameStarted", true);
    }

    public void CloseScroll()
    {
        animator.SetBool("MinigameStarted", false);
    }
}
