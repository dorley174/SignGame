using UnityEngine;

[RequireComponent(typeof(LayerMask))]
public class GeneralEnemyBehaviour
{
    public static bool LookingDirectlyAtPlayer(Vector3 p1, Vector3 p2, LayerMask masks, string tag)
    {
        RaycastHit2D hit = Physics2D.Linecast(p1, p2, masks);
        if (hit)
        {
            if (hit.collider.tag == tag)
            {
                return true;
            }
        }
        return false;
    }
}
