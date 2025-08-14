using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class RigAnchors
    {
        [HideInInspector] public Transform root;
        [SerializeField] private PSphereArea _head;
        [SerializeField] private PCapsuleArea _body;

        public PSphereArea head
        {
            get
            {
                if (_head == null && root != null)
                {
                    var headTransform = root.FindInAllChildren("rig@head", true);
                    if (headTransform != null)
                        _head = headTransform.GetComponent<PSphereArea>();
                }
                return _head;
            }
        }

        public PCapsuleArea body
        {
            get
            {
                if (_body == null && root != null)
                {
                    var bodyTransform = root.FindInAllChildren("rig@body", true);
                    if (bodyTransform != null)
                        _body = bodyTransform.GetComponent<PCapsuleArea>();
                }
                return _body;
            }
        }

        public void Init(Transform root)
        {
            this.root = root;
            Reload();
        }

        public void Reload()
        {
            _head = null;
            _body = null;
        }
    }
}