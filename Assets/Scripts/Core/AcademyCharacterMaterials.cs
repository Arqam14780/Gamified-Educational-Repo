using System.Collections.Generic;
using UnityEngine;

namespace AR
{
    // Convert legacy character materials on instances, preserving vendor assets.
    public sealed class AcademyCharacterMaterials : MonoBehaviour
    {
        private readonly List<Material> owned=new List<Material>();
        public static void Prepare(GameObject character)
        {
            if(character.GetComponent<AcademyCharacterMaterials>()!=null)return;
            character.AddComponent<AcademyCharacterMaterials>().Convert();
        }
        private void Convert()
        {
            var replacements=new Dictionary<Material,Material>();
            foreach(var renderer in GetComponentsInChildren<Renderer>(true))
            {
                var materials=renderer.sharedMaterials;
                for(int i=0;i<materials.Length;i++)
                {
                    var original=materials[i];
                    if(original==null || original.shader==null)continue;
                    string shader=original.shader.name;
                    if(shader!="Standard" && !shader.StartsWith("Legacy Shaders/") && !shader.StartsWith("Mobile/"))continue;
                    if(!replacements.TryGetValue(original,out var converted))
                    {
                        converted=new Material(Shader.Find("Universal Render Pipeline/Lit"));
                        converted.name=original.name+" (academy instance)";
                        converted.mainTexture=original.mainTexture;
                        converted.color=original.HasProperty("_Color")?original.color:Color.white;
                        converted.SetFloat("_Smoothness",.2f);
                        replacements.Add(original,converted);owned.Add(converted);
                    }
                    materials[i]=converted;
                }
                renderer.sharedMaterials=materials;
            }
        }
        private void OnDestroy(){foreach(var material in owned)Destroy(material);}
    }
}
