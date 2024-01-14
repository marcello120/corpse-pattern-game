using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class StatusHolder : MonoBehaviour
{

    public List<StatusEffect> effects;
    public float offset = 0.2f;
    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public bool Add(StatusEffect effect)
    {
        int index = effects.FindIndex(f => f!=null &&  f.statusEffectName == effect.statusEffectName);

        if (index >= 0)
        {
            Debug.Log(effect.name + " aready in status for " + enemy.name);
            return false;
        }

        effects.Add(effect);
        effect.statusObject.GetComponent<SpriteRenderer>().sprite = effect.icon;
        effect.applyFirst(enemy);

        effect.statusObject = Instantiate(effect.statusObject, transform.position + new Vector3(0f,effects.Count * offset,0f), Quaternion.identity);
        effect.statusObject.transform.parent = transform;


        return true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i] == null)
            {
                effects.RemoveAt(i);
                
                //remove from view
            }
            else
            {
                effects[i].applyUpdate(enemy);
                effects[i].tick(enemy);
            }
        }
    }
}
