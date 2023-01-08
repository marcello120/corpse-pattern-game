using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holster : MonoBehaviour
{

    public Vector3 mousePosition = Vector3.zero;

    public bool canAttack = true;
    
    public Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canAttack && weapon!=null && !weapon.isAttacking())
        {
            //update mouse postion
            mousePosition = Input.mousePosition;

            Vector3 lookDir = (mousePosition - Camera.main.WorldToScreenPoint(transform.parent.position)).normalized;

            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, 0, angle);

            //Debug.Log("ANGLE:" + angle);

            //flip w
            Vector3 aimLocalScale = Vector3.one;
            if (angle > 90 || angle < -90)
            {
                aimLocalScale.y = -1f;
            }
            else
            {
                aimLocalScale.y = +1f;
            }
            transform.localScale = aimLocalScale;

            //make weapon appear above/below player

                SpriteRenderer weaponSprite= weapon.GetComponent<SpriteRenderer>();
                if (weaponSprite != null)
                {
                    int layer = 0;
                    if (angle > 180 || angle < 0)
                    {
                        layer = +1;
                    }
                    else
                    {
                        layer = -1;
                    }
                    weaponSprite.sortingOrder = layer;
                }


        }

    }


    public void Attack()
    {
        if(weapon != null )
        {
            weapon.Attack();
        }
    }
}
