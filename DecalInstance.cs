using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Knife.RealBlood
{
    public class DecalInstance : MonoBehaviour
    {
        [SerializeField] private DecalProjector decal;

        public Material Material { get => decal.material; set => decal.material = value; }

        public void Init()
        {
            decal.material = Instantiate(decal.material);
        }

        private void OnDestroy()
        {
            DestroyImmediate(decal.material, true);
        }
    }
}