using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Lightable
{
    public Player Target;
    public float AttackPower;

    public abstract void FocusOn(Player target);
    public abstract void LoseFocus();
}
