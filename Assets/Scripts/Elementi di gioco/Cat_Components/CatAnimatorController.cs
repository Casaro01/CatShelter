using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimatorController : MonoBehaviour
{
    [SerializeField] Animator myAnimController;
    private AnimCatState catActState;
    [SerializeField] enum AnimCatState {
        IDLE,
        REST,
        DRAG,
        WORK,
        BACKTOBED,
        };
    private void Update() {
        
        }
    public void ChangeAnim(string anim) {
        myAnimController.Play("");
        }
    }
