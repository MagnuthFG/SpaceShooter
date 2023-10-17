using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.Mono
{
    public class ComponentPool<T> : Queue<T> where T : MonoBehaviour
    {
// PROPERTIES

        public int Max { get; private set; } = 0;
        public int Growth { get; set; } = 1;
        public T Component { get; private set; }
        public GameObject Prefab { get; private set; } = null;

// INITIALISATION

        public ComponentPool(GameObject prefab, int max) : base(max){
            Component = prefab.GetComponent<T>();
            Prefab    = prefab;
            Max       = max;
        }

        public ComponentPool(GameObject prefab, int growth, int max) : base(max){
            Component = prefab.GetComponent<T>();
            Prefab    = prefab;
            Growth    = growth;
            Max       = max;
        }

// POOLING

        public T Get(){
            T next = TryGet();

            if (next != null)
                return next;

            Populate(Growth);
            return TryGet();
        }

        public T TryGet(){
            int tries = 0;
              T next  = null;

            while (next == null && tries++ < base.Count){
                if (!base.TryDequeue(out next)) continue;

                if (!next.enabled){
                    next.enabled = true;

                    base.Enqueue(next);
                    return next;
                }
                base.Enqueue(next);
                next = null;
            }
            return null;
        }

        public void Populate(){
            Populate(Growth);
        }

        public void Populate(int amount){
            amount = Mathf.Clamp(
                amount, 0, this.Max - base.Count
            );
            for (int i = 0; i < amount; i++){
                var component = GameObject.Instantiate<T>(Component);
                    component.enabled = false;

                base.Enqueue(component);
            }
        }

    }
}