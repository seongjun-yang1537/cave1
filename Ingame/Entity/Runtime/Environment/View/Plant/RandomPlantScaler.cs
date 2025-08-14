using System.Collections.Generic;
using System.Linq;
using Core;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [ExecuteInEditMode]
    public class RandomPlantScaler : MonoBehaviour
    {
        public Light light;
        public Transform plant;

        protected virtual void OnEnable()
        {
            RandomMetal();
        }

        private void RandomMetal()
        {
            MT19937 rng = GameRng.Game;

            float scale = rng.NextFloat(6f, 10f);

            if (plant != null)
                plant.transform.localScale = Vector3.one * scale;

            if (light != null)
            {
                light.intensity = (scale - 6) / 4f + 1;
                light.intensity *= 2;
                light.range = scale * 2;
            }
        }
    }
}