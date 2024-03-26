using UnityEngine;

public abstract class StatusEffect : ScriptableObject
{
    public Sprite icon;

    public GameObject statusObject;

    public string statusEffectName;

    public string description;

    public float duration;

    public float timeElapsed;

    public abstract void applyFirst(Enemy enemy);

    public abstract void applyUpdate(Enemy enemy);

    public abstract void applyRemove(Enemy enemy);

    public virtual bool tick(Enemy enemy)
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed> duration)
        {
            remove(enemy);
            return false;
        }
        return true;
    }

    public void remove(Enemy enemy)
    {
        Destroy(statusObject);
        statusObject= null;
        applyRemove(enemy);
        Destroy(this);
    }

    public override bool Equals(object obj)
    {
        if(this == null)
        {
            return false;
        }
        var item = obj as StatusEffect;

        if (item == null)
        {
            return false;
        }

        return this.name == item.name;
    }

    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
}
