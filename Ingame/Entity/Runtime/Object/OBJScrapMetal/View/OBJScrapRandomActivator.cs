using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [ExecuteInEditMode]
    public class OBJScrapRandomActivator : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            SetActiveRandomMetal();
        }

        private void SetActiveRandomMetal()
        {
            List<Transform> metals = Enumerable.Range(0, transform.childCount)
                                     .Select(i => transform.GetChild(i))
                                     .ToList();
            foreach (var metal in metals)
                metal.gameObject.SetActive(false);

            Transform selected = metals.Choice(GameRng.Game);
            selected.gameObject.SetActive(true);
        }
    }
}
