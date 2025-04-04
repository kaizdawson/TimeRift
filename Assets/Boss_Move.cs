﻿using UnityEngine;

public class Boss_Move : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange=3f;
    Transform player;
    Rigidbody2D rb;
    Boss boss;
    AudioSource audioSource;

    public AudioClip footstepClip;
    float footstepCooldown = 0.4f;
    float lastFootstepTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb =animator.GetComponent<Rigidbody2D>();
        boss=animator.GetComponent<Boss>();
        audioSource = animator.GetComponent<AudioSource>();
        lastFootstepTime = Time.time;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null || rb == null) return;
        boss.LookAtPlayer();
        Vector2 target = player.position; 
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Time.time - lastFootstepTime >= footstepCooldown)
        {
            if (footstepClip != null && audioSource != null)
            {
                audioSource.PlayOneShot(footstepClip);
                lastFootstepTime = Time.time;
            }
        }

        if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }

    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
