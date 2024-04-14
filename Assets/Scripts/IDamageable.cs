using System.Collections;
using UnityEngine;

public interface IDamageable
{
    float health { get; set; }

    void Kill();
    void Damage(float damageTaken);
    void Heal(float damageHealed);

}