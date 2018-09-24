using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public string technicalName;
    public string displayName;

    //The number of charges that can be stored from the charge time.
    public int charges = 1;

    //The number of times the ability can be set off when the cooldown is up.
    public int salvo = 1;

    //Directly applied damage to target
    //Area damage splash damage

    //Damage dealt
    //Effects applied

    public void UseAttack()
    {
        //Ability stuff happens here in the inheritting classes...
    }
}



