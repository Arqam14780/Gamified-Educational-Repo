using System.Collections.Generic;
using UnityEngine;

namespace AR
{
    public sealed class StudyRoomEnvironment : MonoBehaviour
    {
        public readonly List<Material> GeneratedMaterials = new List<Material>();
        private readonly Dictionary<Color, Material> palette = new Dictionary<Color, Material>();
        private Transform group;
        private static readonly Color Gold = new Color(.83f, .55f, .16f);
        private static readonly Color GoldLight = new Color(1f, .82f, .42f);
        private static readonly Color Wood = new Color(.29f, .13f, .07f);
        private static readonly Color Cream = new Color(.96f, .93f, .82f);

        public void Build()
        {
            if (transform.childCount > 0) return;
            group = transform;
            BuildShell();
            BuildFrames();
        }

        private void BuildShell()
        {
            Box("Foundation", new Vector3(0, -.18f, 0), new Vector3(14.4f, .36f, 12.4f), Wood);
            var wall = new Color(.28f, .49f, .24f);
            Box("North wall", new Vector3(0, 2.8f, 6.15f), new Vector3(14.6f, 5.6f, .3f), wall);
            Box("South wall", new Vector3(0, 2.8f, -6.15f), new Vector3(14.6f, 5.6f, .3f), wall);
            Box("West wall", new Vector3(-7.15f, 2.8f, 0), new Vector3(.3f, 5.6f, 12.6f), wall);
            Box("East wall", new Vector3(7.15f, 2.8f, 0), new Vector3(.3f, 5.6f, 12.6f), wall);
            for (int row = 0; row < 24; row++)
            for (int col = 0; col < 7; col++)
            {
                float shade = ((row * 7 + col * 3) % 5) * .025f;
                Box("Parquet board", new Vector3(-6f + col * 2, .005f, -5.75f + row * .5f),
                    new Vector3(1.985f, .015f, .485f), new Color(.47f + shade, .27f + shade, .13f + shade), false);
            }
            for (int side = 0; side < 4; side++)
            {
                var trim = new GameObject("Wall panelling " + side).transform;
                trim.SetParent(transform, false);
                trim.localPosition = side < 2 ? new Vector3(0, 0, side == 0 ? 5.96f : -5.96f) : new Vector3(side == 2 ? -6.96f : 6.96f, 0, 0);
                trim.localRotation = Quaternion.Euler(0, side == 0 ? 0 : side == 1 ? 180 : side == 2 ? -90 : 90, 0);
                group = trim;
                float length = side < 2 ? 14 : 12;
                Box("Walnut wainscot", new Vector3(0, .5f, 0), new Vector3(length, 1, .08f), Wood);
                Box("Brass chair rail", new Vector3(0, 1.02f, -.045f), new Vector3(length, .065f, .09f), Gold);
                Box("Skirting", new Vector3(0, .09f, -.035f), new Vector3(length, .18f, .12f), Wood);
                Box("Crown moulding", new Vector3(0, 5.38f, -.07f), new Vector3(length, .16f, .22f), Cream, false);
                for (float x = -length / 2 + .6f; x < length / 2; x += 1.2f)
                    Box("Panel stile", new Vector3(x, .5f, -.06f), new Vector3(.045f, .8f, .04f), Gold, false);
            }
            group = transform;
            Box("Ceiling", new Vector3(0, 5.65f, 0), new Vector3(14.6f, .2f, 12.6f), new Color(.14f, .19f, .18f));
            for (int x = -4; x <= 4; x += 4)
            for (int z = -4; z <= 4; z += 4)
            {
                Box("Skylight brass surround", new Vector3(x, 5.48f, z), new Vector3(3.1f, .12f, 2.8f), Gold, false);
                Box("Skylight diffuser", new Vector3(x, 5.39f, z), new Vector3(2.9f, .04f, 2.6f), new Color(1, .98f, .89f), false);
            }
            for (int x = -3; x <= 3; x += 6)
            {
                Box("Gallery bench seat", new Vector3(x, .6f, -1.7f), new Vector3(2.5f, .18f, .75f), Cream);
                Box("Bench pedestal", new Vector3(x - .85f, .26f, -1.7f), new Vector3(.26f, .52f, .55f), Wood);
                Box("Bench pedestal", new Vector3(x + .85f, .26f, -1.7f), new Vector3(.26f, .52f, .55f), Wood);
            }
            Box("Entry door surround", new Vector3(0, 1.7f, -5.9f), new Vector3(2.15f, 3.4f, .15f), Gold);
            Box("Entry door", new Vector3(0, 1.6f, -5.78f), new Vector3(1.9f, 3.2f, .12f), Wood);
            Box("Door handle", new Vector3(.65f, 1.5f, -5.65f), new Vector3(.08f, .32f, .08f), GoldLight, false);
        }
        private void BuildFrames()
        {
            var lessons = StudyLessonCatalog.Load();
            var math = lessons.Find("Math");
            var english = lessons.Find("English");
            Frame("Math", new Vector3(-3.1f, 2.9f, 5.91f), 0, "1 + 1", "NUMBERS & COUNTING", new Color(.08f, .40f, .65f));
            Frame("English", new Vector3(3.1f, 2.9f, 5.91f), 0, "A B C", "LETTERS & WORDS", new Color(.63f, .24f, .14f));
            Frame("Math", new Vector3(-6.91f, 2.9f, 2.2f), -90, "", "", Color.white, StudyFrameKind.Image, math.imageResource);
            Frame("Math", new Vector3(-6.91f, 2.9f, -2.2f), -90, "", "", Color.white, StudyFrameKind.Pdf, math.pdfPages[0]);
            Frame("English", new Vector3(6.91f, 2.9f, 2.2f), 90, "", "", Color.white, StudyFrameKind.Image, english.imageResource);
            Frame("English", new Vector3(6.91f, 2.9f, -2.2f), 90, "", "", Color.white, StudyFrameKind.Pdf, english.pdfPages[0]);
        }

        private void Frame(string subject, Vector3 position, float yaw, string symbol, string caption, Color accent, StudyFrameKind kind = StudyFrameKind.Quiz, string previewResource = null)
        {
            var root = new GameObject(subject + " - " + kind + " wall frame");
            root.transform.SetParent(transform, false);
            root.transform.localPosition = position;
            root.transform.localRotation = Quaternion.Euler(0, yaw, 0);
            var frame = root.AddComponent<StudyFrame>();
            frame.subject = subject;
            frame.kind = kind;
            var hitbox = root.AddComponent<BoxCollider>();
            hitbox.center = new Vector3(0, 0, -.18f);
            hitbox.size = new Vector3(3.4f, 2.8f, .4f);
            group = root.transform;
            Box("Deep frame backing", Vector3.zero, new Vector3(3.2f, 2.6f, .15f), Wood, false);
            Rail(3.22f, 2.62f, .14f, -.12f, Gold);
            Rail(3.0f, 2.40f, .055f, -.25f, GoldLight);
            Rail(2.87f, 2.27f, .05f, -.24f, Wood);
            Rail(2.76f, 2.16f, .045f, -.25f, GoldLight);
            Box("Ivory picture mat", new Vector3(0, 0, -.13f), new Vector3(2.7f, 2.1f, .04f), Cream, false);
            var poster = Box("Subject poster", new Vector3(0, .12f, -.17f), new Vector3(2.4f, 1.58f, .025f), accent, false);
            if (kind == StudyFrameKind.Quiz)
            {
                Text("QUIZ / " + subject.ToUpperInvariant(), new Vector3(0, .67f, -.205f), .075f, Color.white);
                Text(symbol, new Vector3(0, .20f, -.215f), .21f, Color.white);
                Text(caption, new Vector3(0, -.39f, -.215f), .055f, Color.white);
            }
            else
            {
                var texture = Resources.Load<Texture2D>(previewResource);
                if (texture == null) throw new System.InvalidOperationException("Missing frame preview: " + previewResource);
                var material = new Material(Resources.Load<Shader>("StudyRoomSurface"));
                material.name = subject + " " + kind + " preview";
                material.color = Color.white; material.mainTexture = texture;
                // Unity's cube back face has both texture axes reversed.
                material.mainTextureScale = new Vector2(-1,-1);
                material.mainTextureOffset = Vector2.one;
                GeneratedMaterials.Add(material);
                poster.GetComponent<Renderer>().sharedMaterial = material;
                Text(kind.ToString().ToUpperInvariant()+" / "+subject.ToUpperInvariant(),new Vector3(0,.99f,-.215f),.047f,Wood);
            }
            Text(kind == StudyFrameKind.Quiz ? "TRY THE RELATED QUIZ" : kind == StudyFrameKind.Image ? "VIEW IMAGE LESSON" : "READ PDF LESSON", new Vector3(0, -.84f, -.21f), .057f, Wood);
            for (int x = -1; x <= 1; x += 2)
            for (int y = -1; y <= 1; y += 2)
            {
                var ornament = Box("Corner rosette", new Vector3(x * 1.53f, y * 1.23f, -.26f), new Vector3(.15f, .15f, .07f), GoldLight, false);
                ornament.transform.localRotation = Quaternion.Euler(0, 0, 45);
            }
            Box("Museum nameplate", new Vector3(0, -1.6f, 0), new Vector3(1.5f, .3f, .08f), Gold, false);
            Text(subject.ToUpperInvariant() + " / " + kind.ToString().ToUpperInvariant(), new Vector3(0, -1.6f, -.06f), .045f, Wood);
        }

        private void Rail(float width, float height, float thickness, float depth, Color color)
        {
            Box("Top moulding", new Vector3(0, height / 2, depth), new Vector3(width, thickness, .10f), color, false);
            Box("Bottom moulding", new Vector3(0, -height / 2, depth), new Vector3(width, thickness, .10f), color, false);
            Box("Left moulding", new Vector3(-width / 2, 0, depth), new Vector3(thickness, height, .10f), color, false);
            Box("Right moulding", new Vector3(width / 2, 0, depth), new Vector3(thickness, height, .10f), color, false);
        }

        private GameObject Box(string name, Vector3 position, Vector3 size, Color color, bool collision = true)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
            obj.transform.SetParent(group, false);
            obj.transform.localPosition = position;
            obj.transform.localScale = size;
            obj.GetComponent<Collider>().enabled = collision;
            if (!palette.TryGetValue(color, out Material material))
            {
                material = new Material(Resources.Load<Shader>("StudyRoomSurface"));
                material.name = "Gallery " + ColorUtility.ToHtmlStringRGB(color);
                material.color = color;
                palette.Add(color, material);
                GeneratedMaterials.Add(material);
            }
            obj.GetComponent<Renderer>().sharedMaterial = material;
            return obj;
        }

        private void Text(string value, Vector3 position, float size, Color color)
        {
            var obj = new GameObject(value, typeof(TextMesh));
            obj.transform.SetParent(group, false);
            obj.transform.localPosition = position;
            var text = obj.GetComponent<TextMesh>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            obj.GetComponent<MeshRenderer>().sharedMaterial = text.font.material;
            text.text = value;
            text.fontSize = 64;
            text.characterSize = size * .6f;
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.color = color;
        }

        private void OnDestroy()
        {
            foreach (var material in GeneratedMaterials)
                if (Application.isPlaying) Destroy(material);
        }
    }
}
