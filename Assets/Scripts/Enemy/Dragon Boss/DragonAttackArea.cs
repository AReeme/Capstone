using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackArea : MonoBehaviour
{
    [SerializeField]
    public DragonController dragonController;
    private PolygonCollider2D polygonCollider;

    private void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        SetSortingOrderAndRotation();
    }

    private void Update()
    {
        SetSortingOrderAndRotation();
    }

    private void SetSortingOrderAndRotation()
    {
        float horizontal = dragonController.animator.GetFloat("Last_Horizontal");
        float vertical = dragonController.animator.GetFloat("Last_Vertical");

        int sortingOrder;

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            sortingOrder = horizontal > 0 ?
                dragonController.GetComponent<SpriteRenderer>().sortingOrder + 1 :
                dragonController.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else
        {
            sortingOrder = vertical > 0 ?
                dragonController.GetComponent<SpriteRenderer>().sortingOrder + 1 :
                dragonController.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }

        // Rotate the entire GameObject based on the dragon's facing direction
        float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (player.GetComponent<Health>() != null)
        {
            Health health = player.GetComponent<Health>();
            health.Damage(dragonController.damage);
        }
    }
}