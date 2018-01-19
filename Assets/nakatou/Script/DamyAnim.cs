using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamyAnim : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Weapon_Type type = 
            animator.gameObject.GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype;
        //ハンドガン
        if (type == Weapon_Type.Gun)
        {
            animator.CrossFade("HundGun_Idle", 0.0f);
        }
        //斧
        else if (type == Weapon_Type.Axe)
        {
            animator.CrossFade("Axe_Idle", 0.0f);
        }
        //拳
        else if (type == Weapon_Type.Fist)
        {
            animator.CrossFade("Punch_Idle", 0.0f);
        }
        //ナイフ
        else if (type == Weapon_Type.Knife)
        {
            animator.CrossFade("Knife_Idle", 0.0f);
        }
        //ライフル
        else if (type == Weapon_Type.Rifle)
        {
            animator.CrossFade("Rifle_Idle", 0.0f);
        }
        //槍
        else if (type == Weapon_Type.Spear)
        {
            animator.CrossFade("Lance_Idle", 0.0f);
        }
        //素手
        else
        {
            animator.CrossFade("Punch_Idle", 0.0f);
        }

        ////ハンドガン
        //if(type == Weapon_Type.Gun)
        //{
        //    animator.CrossFade("HundGun_Atk", 0.0f);
        //}
        ////斧
        //else if(type == Weapon_Type.Axe)
        //{
        //    animator.CrossFade("Axe_Atk", 0.0f);
        //}
        ////拳
        //else if (type == Weapon_Type.Fist)
        //{
        //    animator.CrossFade("Punch_Atk", 0.0f);
        //}
        ////ナイフ
        //else if (type == Weapon_Type.Knife)
        //{
        //    animator.CrossFade("Knife_Atk", 0.0f);
        //}
        ////ライフル
        //else if (type == Weapon_Type.Rifle)
        //{
        //    animator.CrossFade("Rifle_Atk", 0.0f);
        //}
        ////槍
        //else if (type == Weapon_Type.Spear)
        //{
        //    animator.CrossFade("Lance_Atk", 0.0f);
        //}
        ////素手
        //else
        //{
        //    animator.CrossFade("Punch_Atk", 0.0f);
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
