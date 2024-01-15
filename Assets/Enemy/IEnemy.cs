using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{

    public void getHit(float damage, Vector2 knockback);

    public void Death();

    public void Remove();

}
