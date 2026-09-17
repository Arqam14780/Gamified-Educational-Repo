using UnityEngine;
using UnityEngine.Rendering;

namespace AR
{
    public sealed partial class StudyRoomEnvironment
    {
        // All furniture dimensions are in metres. Keep the centre aisle clear.
        private void BuildAcademy()
        {
            group = transform;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.78f,.85f,.94f);
            RenderSettings.ambientEquatorColor = new Color(.56f,.64f,.65f);
            RenderSettings.ambientGroundColor = new Color(.32f,.28f,.24f);
            Box("Academy sign", new Vector3(0,4.75f,5.88f), new Vector3(8,.68f,.12f), Gold, false);
            Text("LEARNING ACADEMY",new Vector3(0,4.77f,5.79f),.17f,Color.white);
            Text("EXPLORE   /   READ   /   DISCOVER",new Vector3(0,4.24f,5.83f),.069f,Gold);
            Box("Central learning rug",new Vector3(0,.025f,1.3f),new Vector3(8,.018f,3.8f),new Color(.19f,.39f,.43f),false);
            for(int side=-1;side<=1;side+=2)
            {
                for(int z=-3;z<=3;z+=3)
                {
                    Box("Clerestory window surround",new Vector3(side*6.94f,4.82f,z),new Vector3(.09f,.78f,2.7f),Gold,false);
                    Box("Clerestory glazing",new Vector3(side*6.87f,4.82f,z),new Vector3(.04f,.64f,2.54f),new Color(.66f,.87f,.95f),false);
                    Box("Window mullion",new Vector3(side*6.83f,4.82f,z),new Vector3(.06f,.64f,.045f),Cream,false);
                }
                float x=side*2.7f;
                Box("Study desk oak top",new Vector3(x,.83f,1.8f),new Vector3(2.1f,.12f,1.05f),new Color(.69f,.51f,.32f));
                for(int dx=-1;dx<=1;dx+=2)
                for(int dz=-1;dz<=1;dz+=2)
                    Box("Desk steel leg",new Vector3(x+dx*.87f,.4f,1.8f+dz*.38f),new Vector3(.065f,.8f,.065f),Gold);
                Box("Chair cushion",new Vector3(x,.47f,.85f),new Vector3(.67f,.12f,.62f),Gold);
                Box("Chair back",new Vector3(x,.84f,.56f),new Vector3(.67f,.7f,.08f),Gold);
                for(int dx=-1;dx<=1;dx+=2)
                for(int dz=-1;dz<=1;dz+=2)
                    Box("Chair leg",new Vector3(x+dx*.26f,.23f,.85f+dz*.23f),new Vector3(.045f,.46f,.045f),Wood);
                Box("Notebook",new Vector3(x,.91f,1.8f),new Vector3(.55f,.035f,.4f),Cream,false);
                Box("Notebook spine",new Vector3(x-.26f,.935f,1.8f),new Vector3(.04f,.01f,.4f),GoldLight,false);
                Bookshelf(side*4.5f);
                Plant(new Vector3(side*6.25f,0,4.95f));
            }
            for(int x=-4;x<=4;x+=4)
            {
                var lamp=new GameObject("Ceiling soft light",typeof(Light));
                lamp.transform.SetParent(transform,false);
                lamp.transform.localPosition=new Vector3(x,4.9f,0);
                var light=lamp.GetComponent<Light>();
                light.type=x==0?LightType.Spot:LightType.Point; light.range=14; light.intensity=x==0?5f:2.3f;
                light.color=new Color(1,.94f,.82f); light.shadows=LightShadows.None;
                if(x==0)
                {
                    lamp.transform.localRotation=Quaternion.Euler(90,0,0);
                    light.spotAngle=140; light.innerSpotAngle=100; light.shadows=LightShadows.Soft;
                    light.shadowBias=.03f; light.shadowNormalBias=.2f;
                }
            }
        }

        private void Bookshelf(float x)
        {
            Box("Reading library back",new Vector3(x,1.05f,-5.78f),new Vector3(2.8f,2.1f,.18f),Gold);
            for(int level=0;level<4;level++)
            {
                float y=.12f+level*.62f;
                Box("Library shelf",new Vector3(x,y,-5.47f),new Vector3(2.9f,.08f,.68f),Wood);
                if(level==3)continue;
                for(int book=0;book<12;book++)
                {
                    Color color=Color.Lerp(GoldLight,new Color(.28f,.51f,.54f),(book%4)/3f);
                    Box("Library book",new Vector3(x-1.2f+book*.21f,y+.24f,-5.44f),new Vector3(.15f,.37f+(book%3)*.04f,.38f),color,false);
                }
            }
        }

        private void Plant(Vector3 position)
        {
            Box("Planter",position+Vector3.up*.3f,new Vector3(.5f,.6f,.5f),Cream);
            Box("Plant stem",position+Vector3.up*.95f,new Vector3(.07f,1.3f,.07f),Wood,false);
            for(int i=0;i<5;i++)
            {
                var leaf=GameObject.CreatePrimitive(PrimitiveType.Sphere);
                leaf.name="Plant foliage"; leaf.transform.SetParent(transform,false);
                float a=i*2.4f;
                leaf.transform.localPosition=position+new Vector3(Mathf.Cos(a)*.22f,1+i*.14f,Mathf.Sin(a)*.22f);
                leaf.transform.localScale=new Vector3(.5f,.3f,.48f);
                leaf.GetComponent<Collider>().enabled=false;
                // Reuse a generated palette material without adding another shader dependency.
                var sample=Box("Leaf material source",position,Vector3.one*.001f,new Color(.22f,.43f,.28f),false);
                leaf.GetComponent<Renderer>().sharedMaterial=sample.GetComponent<Renderer>().sharedMaterial;
                if(Application.isPlaying)Destroy(sample);else DestroyImmediate(sample);
            }
        }
    }
}
