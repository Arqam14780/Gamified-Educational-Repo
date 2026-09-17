using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AR
{
    // External packs use the same relative paths as Resources, with .png appended.
    public static class StudyContent
    {
        public static string Root { get; internal set; }
        private static readonly Dictionary<string, Texture2D> images = new Dictionary<string, Texture2D>();

        public static Texture2D Texture(string resource)
        {
            if (Root == null) return Resources.Load<Texture2D>(resource);
            string path=Path.GetFullPath(Path.Combine(Root,resource+".png"));
            string prefix=Path.GetFullPath(Root)+Path.DirectorySeparatorChar;
            if (!path.StartsWith(prefix,StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning("Content image must stay inside the Academy folder."); return null;
            }
            if (images.TryGetValue(path,out var saved)) return saved;
            try
            {
                if (!File.Exists(path)) return null;
                var texture=new Texture2D(2,2,TextureFormat.RGBA32,false);
                if (!texture.LoadImage(File.ReadAllBytes(path))) { UnityEngine.Object.Destroy(texture); return null; }
                texture.name=resource; texture.wrapMode=TextureWrapMode.Clamp;
                images.Add(path,texture); return texture;
            }
            catch (Exception error) { Debug.LogWarning("Cannot read learning image: "+error.Message); return null; }
        }

        public static void Release()
        {
            foreach(var texture in images.Values) if(texture!=null) UnityEngine.Object.Destroy(texture);
            images.Clear();
        }
    }
}
