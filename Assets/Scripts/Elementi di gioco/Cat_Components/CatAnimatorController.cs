using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimatorController : MonoBehaviour
{
    [SerializeField] Animator myAnimController;
    private AnimCatState catActState;

    public enum AnimCatState {
        IDLE,
        REST,
        DRAG,
        WORK,
        BACKTOBED,
        };
    private void Update() {
        
        }
    public void ChangeAnim(AnimCatState newState) {
        if (newState == catActState)
            return;
        myAnimController.Play(newState.ToString());
        catActState= newState;
        }
    }
