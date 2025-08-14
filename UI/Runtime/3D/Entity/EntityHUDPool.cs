using System.Collections.Generic;
using UnityEngine;
using Ingame;

namespace UI
{
    public class EntityHUDPool : MonoBehaviour
    {
        [SerializeField] private string prefabPath = "Ingame/HUD/EntityHUD";
        private readonly Stack<UIEntityHUD> pool = new();
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Monster HUD prefab not found at Resources/{prefabPath}");
            }
        }

        public UIEntityHUD Spawn(AgentController controller)
        {
            if (prefab == null || controller == null)
                return null;

            UIEntityHUD hud = pool.Count > 0 ? pool.Pop() : Instantiate(prefab, transform).GetComponent<UIEntityHUD>();
            hud.gameObject.SetActive(true);
            hud.Bind(controller, this);
            return hud;
        }

        public void Despawn(UIEntityHUD hud)
        {
            if (hud == null)
                return;

            hud.Unbind();
            hud.gameObject.SetActive(false);
            hud.transform.SetParent(transform);
            pool.Push(hud);
        }
    }
}
