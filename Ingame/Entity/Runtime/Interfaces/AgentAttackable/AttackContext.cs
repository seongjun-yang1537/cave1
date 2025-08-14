using UnityEngine;

namespace Ingame
{
    public enum DamageType
    {
        Physical,
        Magical,
        Fire,
        Ice,
        Poison,
        True
    }

    public enum StatusEffectType
    {
        None,
        Stun,
        Burn,
        Freeze,
        Bleed
    }

    public struct AttackContext
    {
        public float damage;
        public DamageType damageType;
        public Vector3 direction;
        public float knockback;
        public bool isCritical;
        public StatusEffectType statusEffect;
        public float statusEffectDuration;

        public static Builder CreateBuilder() => new Builder();

        public class Builder
        {
            private float _damage = 0f;
            private DamageType _damageType = DamageType.Physical;
            private Vector3 _direction = Vector3.zero;
            private float _knockback = 0f;
            private bool _isCritical = false;
            private StatusEffectType _statusEffect = StatusEffectType.None;
            private float _statusEffectDuration = 0f;

            public Builder SetDamage(float value)
            {
                _damage = value;
                return this;
            }

            public Builder SetDamageType(DamageType value)
            {
                _damageType = value;
                return this;
            }

            public Builder SetDirection(Vector3 value)
            {
                _direction = value;
                return this;
            }

            public Builder SetKnockback(float value)
            {
                _knockback = value;
                return this;
            }

            public Builder SetCritical(bool value)
            {
                _isCritical = value;
                return this;
            }

            public Builder SetStatusEffect(StatusEffectType value, float duration)
            {
                _statusEffect = value;
                _statusEffectDuration = duration;
                return this;
            }

            public AttackContext Build()
            {
                return new AttackContext
                {
                    damage = _damage,
                    damageType = _damageType,
                    direction = _direction,
                    knockback = _knockback,
                    isCritical = _isCritical,
                    statusEffect = _statusEffect,
                    statusEffectDuration = _statusEffectDuration
                };
            }
        }
    }
}
