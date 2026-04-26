using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("playerattacking");
            PlayerVisuals.instance.animator.SetTrigger("Attack");
            PlayerVisuals.instance.stopOtherAnims = true;
        }
    }


}
