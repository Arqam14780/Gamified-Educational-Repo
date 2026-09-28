using UnityEngine;
using UnityEngine.UI;

namespace AR
{
    // Uses the existing character models and selection persistence.
    public sealed class AcademyMenu : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private ChildSelection selection;
        [SerializeField] private Text characterLabel;
        private int index;

        public void Initialize(ChildSelection owner, int selected)
        {
            selection=owner; index=selected;
            Refresh();
        }

        public void PreviousLearner()
        {
            soundManager.PlayBtnSound();
            Select(-1);
        }
        public void NextLearner()
        {
            soundManager.PlayBtnSound();
            Select(1);
        }
        public void EnterAcademy()
        {
            soundManager.PlayBtnSound();
            selection.Play();
        }
#if UNITY_EDITOR
        // Authoring only: the generated hierarchy is saved into MainMenu.unity.
        public void BuildForEditor(ChildSelection owner)
        {
            selection=owner; index=0;
            var camera=FindFirstObjectByType<Camera>();
            if(camera!=null)
            {
                camera.tag="MainCamera";
                camera.clearFlags=CameraClearFlags.SolidColor;
                camera.backgroundColor=new Color(.76f,.85f,.84f);
            }
            var root=new GameObject("Academy menu",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
            root.transform.SetParent(transform,false);
            root.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
            var scaler=root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution=new Vector2(1280,800);
            scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.Expand;
            var top=Panel(root.transform,new Vector2(0,315),new Vector2(1180,130));
            Label(top,"LEARNING ACADEMY",new Vector2(0,23),new Vector2(1100,50),36,Color.white);
            Label(top,"Your classroom. Your curiosity. Your next discovery.",new Vector2(0,-28),new Vector2(1100,38),21,AcademyUI.Muted);
            var welcome=Panel(root.transform,new Vector2(-285,35),new Vector2(540,370));
            Label(welcome,"READY TO DISCOVER?",new Vector2(0,135),new Vector2(490,48),29,Color.white);
            Label(welcome,"Walk into a classroom built for curiosity.",new Vector2(0,83),new Vector2(490,48),20,AcademyUI.Muted);
            Label(welcome,"01   EXPLORE\nLook closely at the learning images.",new Vector2(0,12),new Vector2(490,70),22,Color.white);
            Label(welcome,"02   READ\nFind new ideas in each PDF lesson.",new Vector2(0,-68),new Vector2(490,70),22,Color.white);
            Label(welcome,"03   TRY\nPut what you learned into practice.",new Vector2(0,-145),new Vector2(490,65),22,Color.white);
            var bottom=Panel(root.transform,new Vector2(0,-280),new Vector2(1180,205));
            characterLabel=Label(bottom,"",new Vector2(-275,50),new Vector2(540,38),25,Color.white);
            Button(bottom,"<",new Vector2(-485,-5),new Vector2(65,54),()=>Select(-1));
            Button(bottom,">",new Vector2(-65,-5),new Vector2(65,54),()=>Select(1));
            Label(bottom,"Choose your learner",new Vector2(-275,-5),new Vector2(340,50),21,AcademyUI.Muted);
            Button(bottom,"Enter academy  >",new Vector2(285,20),new Vector2(440,64),selection.Play);
            Label(bottom,"01  Explore images     02  Read PDFs     03  Try a quiz",new Vector2(0,-68),new Vector2(1080,40),20,AcademyUI.Muted);
            Refresh();
        }
#endif

        private void Select(int direction)
        {
            index=(index+direction+selection.transform.childCount)%selection.transform.childCount;
            selection.SelectChild(index); Refresh();
        }
        private void Refresh()
        {
            characterLabel.text="LEARNER "+(index+1)+" / "+selection.transform.childCount;
            var learner=selection.transform.GetChild(index);
            var animator=learner.GetComponentInChildren<Animator>();
            if(Application.isPlaying && animator!=null && animator.isActiveAndEnabled)animator.Update(0);
            var renderers=learner.GetComponentsInChildren<Renderer>();
            var camera=Camera.main;
            if(renderers.Length==0 || camera==null)return;
            var bounds=renderers[0].bounds;
            foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
            float height=Mathf.Max(bounds.size.y,1);
            float distance=height/(2*Mathf.Tan(camera.fieldOfView*Mathf.Deg2Rad*.5f))*2.15f;
            var focus=bounds.center-camera.transform.right*height*.8f-camera.transform.up*height*.12f;
            camera.transform.position=focus-camera.transform.forward*distance;
        }
#if UNITY_EDITOR
        private RectTransform Panel(Transform parent,Vector2 position,Vector2 size)
        {
            var obj=new GameObject("Menu panel",typeof(RectTransform),typeof(Image));
            var rect=Place(obj,parent,position,size); AcademyUI.Style(obj.GetComponent<Image>(),AcademyUI.Navy); return rect;
        }
        private RectTransform Place(GameObject obj,Transform parent,Vector2 position,Vector2 size)
        {
            var rect=obj.GetComponent<RectTransform>();rect.SetParent(parent,false);
            rect.anchorMin=rect.anchorMax=Vector2.one*.5f;rect.sizeDelta=size;rect.anchoredPosition=position;return rect;
        }
        private Text Label(Transform parent,string value,Vector2 position,Vector2 size,int fontSize,Color color)
        {
            var obj=new GameObject("Menu text",typeof(RectTransform),typeof(Text)); Place(obj,parent,position,size);
            var label=obj.GetComponent<Text>();label.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.text=value;label.fontSize=fontSize;label.color=color;label.alignment=TextAnchor.MiddleCenter;label.raycastTarget=false;return label;
        }
        private void Button(Transform parent,string value,Vector2 position,Vector2 size,UnityEngine.Events.UnityAction action)
        {
            var obj=new GameObject(value,typeof(RectTransform),typeof(Image),typeof(Button));Place(obj,parent,position,size);
            var button=obj.GetComponent<Button>();AcademyUI.StyleButton(button);button.onClick.AddListener(action);
            Label(obj.transform,value,Vector2.zero,size,23,Color.white);
        }
#endif
    }
}
