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

        if(enemy == null)
        {
            return false;
        }

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

    private void reAllignStatusObjects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] != null) {
                effects[i].statusObject.transform.position = transform.position + new Vector3(0f, (i+1) * offset, 0f);

            }
        }
    }

    public void RemoveAll(Enemy enemy)
    {
        //call remove on all effects
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].remove(enemy);
            effects.Remove(effects[i]);
        }
    }

    public void removeDeathMarker(Enemy enemy)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i].statusEffectName == "NearDeath")
            {
                effects[i].remove(enemy);
                effects.Remove(effects[i]);
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i] == null)
            {
                effects.RemoveAt(i);
                reAllignStatusObjects();
            }
            else
            {
                effects[i].applyUpdate(enemy);
                effects[i].tick(enemy);
            }
        }
    }
}
