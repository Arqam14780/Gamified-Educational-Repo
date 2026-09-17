using UnityEngine;
using UnityEngine.UI;

namespace AR
{
    // Shared visual tokens keep every screen consistent and easy to restyle.
    public static class AcademyUI
    {
        public static readonly Color Navy = new Color(.07f,.14f,.22f);
        public static readonly Color Teal = new Color(.06f,.43f,.47f);
        public static readonly Color Paper = new Color(.96f,.97f,.95f);
        public static readonly Color Muted = new Color(.63f,.75f,.79f);
        private static Sprite rounded;

        public static void Style(Image image, Color color)
        {
            if(rounded == null)
            {
                const int size=32;
                var texture=new Texture2D(size,size,TextureFormat.RGBA32,false);
                texture.name="Academy rounded panel";
                for(int y=0;y<size;y++) for(int x=0;x<size;x++)
                {
                    float dx=Mathf.Max(8-x, x-23),dy=Mathf.Max(8-y,y-23);
                    float distance=new Vector2(Mathf.Max(0,dx),Mathf.Max(0,dy)).magnitude;
                    texture.SetPixel(x,y,new Color(1,1,1,Mathf.Clamp01(8-distance)));
                }
                texture.Apply();
                rounded=Sprite.Create(texture,new Rect(0,0,size,size),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,new Vector4(9,9,9,9));
            }
            image.sprite=rounded; image.type=Image.Type.Sliced; image.color=color;
        }

        public static void StyleButton(Button button)
        {
            Style(button.GetComponent<Image>(),Teal);
            var colors=button.colors;
            colors.highlightedColor=new Color(.8f,1,1);
            colors.pressedColor=new Color(.6f,.85f,.86f);
            colors.disabledColor=new Color(.8f,.85f,.85f,.65f);
            button.colors=colors;
        }
    }
}
