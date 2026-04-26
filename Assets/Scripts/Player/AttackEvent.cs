using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public void Attack()
    {
        PlayerDamager.instance.Attack();
    }
}