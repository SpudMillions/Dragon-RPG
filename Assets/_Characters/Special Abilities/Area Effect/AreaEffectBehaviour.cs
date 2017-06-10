using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;

public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
{
    AreaEffectConfig config;

    public void SetConfig(AreaEffectConfig configToSet)
    {
        this.config = configToSet;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void Use(AbilityUseParams useParams)
    {
        //create static sphere to detect targets
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());

        foreach(RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}

