using System.Collections.Generic;
using Core;
using UnityEngine;

namespace UI
{
    public class DamageIndicatorPool : MonoBehaviour
    {
        [SerializeField] private string prefabPath = "Ingame/UI/DamageIndicator";
        private readonly Stack<DamageIndicator> pool = new();
        private GameObject prefab;

        private readonly Vector3 POSITION_NOISE = Vector3.one * 0.1f;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Damage indicator prefab not found at Resources/{prefabPath}");
            }
        }

        public DamageIndicator Spawn(Vector3 position, float damage)
        {
            if (prefab == null)
                return null;

            position += GameRng.UI.NextVector3(-POSITION_NOISE, POSITION_NOISE);

            DamageIndicator indicator = pool.Count > 0 ? pool.Pop() : Instantiate(prefab, transform).GetComponent<DamageIndicator>();
            indicator.gameObject.SetActive(true);
            indicator.Initialize(this, position, damage);
            return indicator;
        }

        public void Despawn(DamageIndicator indicator)
        {
            if (indicator == null)
                return;

            indicator.gameObject.SetActive(false);
            indicator.transform.SetParent(transform);
            pool.Push(indicator);
        }
    }
}
