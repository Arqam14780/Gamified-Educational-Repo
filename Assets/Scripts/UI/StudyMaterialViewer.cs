using System;
using UnityEngine;
using UnityEngine.UI;

namespace AR
{
    public sealed class StudyMaterialViewer : MonoBehaviour
    {
        private Font font;
        private StudyLesson lesson;
        private StudyFrameKind kind;
        private int page;
        private float zoom = 1;
        private Text title, pageLabel, notice;
        private RawImage picture;
        private RectTransform viewport, content;
        private ScrollRect scroll;
        private Button previous, next, zoomOut, zoomIn;
        private Action closed;
        public bool IsOpen => gameObject.activeSelf;
        public int PageIndex => page;
        public float Zoom => zoom;
        public Texture CurrentTexture => picture.texture;

        public void Initialize(Transform parent, Font uiFont, Action onClose)
        {
            font=uiFont; closed=onClose;
            var root=gameObject.GetComponent<RectTransform>();
            root.SetParent(parent,false); root.anchorMin=Vector2.zero; root.anchorMax=Vector2.one;
            root.offsetMin=root.offsetMax=Vector2.zero;
            gameObject.AddComponent<Image>().color=new Color(.02f,.04f,.06f,.72f);
            var card=Panel("Learning reader",transform,Vector2.zero,new Vector2(1120,730),new Color(.06f,.12f,.17f,.98f));
            title=Label(card,"",new Vector2(-30,315),new Vector2(960,52),28,Color.white);
            Button(card,"X",new Vector2(515,320),new Vector2(48,44),Close);
            viewport=Panel("Page viewport",card,new Vector2(0,15),new Vector2(1040,520),new Color(.94f,.94f,.91f));
            viewport.gameObject.AddComponent<RectMask2D>();
            scroll=viewport.gameObject.AddComponent<ScrollRect>(); scroll.viewport=viewport;
            scroll.movementType=ScrollRect.MovementType.Clamped; scroll.scrollSensitivity=28;
            var imageObject=new GameObject("Learning page",typeof(RectTransform),typeof(RawImage));
            content=Place(imageObject,viewport,Vector2.zero,new Vector2(1040,520));
            picture=imageObject.GetComponent<RawImage>(); scroll.content=content;
            notice=Label(viewport,"",Vector2.zero,new Vector2(920,100),28,new Color(.14f,.2f,.25f));
            previous=Button(card,"< Previous",new Vector2(-415,-288),new Vector2(185,44),()=>ChangePage(-1));
            pageLabel=Label(card,"",new Vector2(-208,-288),new Vector2(220,44),21,Color.white);
            next=Button(card,"Next >",new Vector2(-10,-288),new Vector2(155,44),()=>ChangePage(1));
            zoomOut=Button(card,"-",new Vector2(120,-288),new Vector2(50,44),()=>SetZoom(zoom-.5f));
            Button(card,"Fit",new Vector2(187,-288),new Vector2(70,44),()=>SetZoom(1));
            zoomIn=Button(card,"+",new Vector2(254,-288),new Vector2(50,44),()=>SetZoom(zoom+.5f));
            Label(card,"Take your time. Zoom in to explore, then return to the classroom.",new Vector2(0,-337),new Vector2(900,36),18,AcademyUI.Muted);
            gameObject.SetActive(false);
        }

        public void Open(StudyLesson selected, StudyFrameKind type)
        {
            lesson=selected; kind=type; page=0; zoom=1;
            gameObject.SetActive(true); transform.SetAsLastSibling();
            title.text=lesson.subject+" / "+(kind==StudyFrameKind.Pdf?"PDF reading pack":"Image lesson")+" - "+lesson.title;
            RefreshPage();
        }
        public void ChangePage(int delta)
        {
            int count=kind==StudyFrameKind.Image?1:lesson.pdfPages.Length;
            int target=Mathf.Clamp(page+delta,0,count-1);
            if(target==page) return;
            page=target; zoom=1; RefreshPage();
        }
        public void SetZoom(float value)
        {
            zoom=Mathf.Clamp(value,1,3); FitContent();
        }
        public void Close()
        {
            gameObject.SetActive(false); scroll.StopMovement(); closed?.Invoke();
        }
        private void RefreshPage()
        {
            int count=kind==StudyFrameKind.Image?1:lesson.pdfPages.Length;
            string path=kind==StudyFrameKind.Image?lesson.imageResource:lesson.pdfPages[page];
            picture.texture=StudyContent.Texture(path);
            notice.text=picture.texture==null?"This learning page is unavailable.":"";
            picture.enabled=picture.texture!=null;
            pageLabel.text=kind==StudyFrameKind.Image?"Image lesson":"PDF page "+(page+1)+" / "+count;
            previous.interactable=page>0; next.interactable=page<count-1;
            previous.gameObject.SetActive(kind==StudyFrameKind.Pdf);
            next.gameObject.SetActive(kind==StudyFrameKind.Pdf);
            FitContent();
        }
        private void FitContent()
        {
            Canvas.ForceUpdateCanvases();
            if(picture.texture!=null)
            {
                float fit=Mathf.Min(viewport.rect.width/picture.texture.width,viewport.rect.height/picture.texture.height);
                content.sizeDelta=new Vector2(picture.texture.width,picture.texture.height)*fit*zoom;
            }
            scroll.StopMovement(); content.anchoredPosition=Vector2.zero;
            zoomOut.interactable=zoom>1; zoomIn.interactable=zoom<3;
        }
        private RectTransform Place(GameObject obj,Transform parent,Vector2 position,Vector2 size)
        {
            var rect=obj.GetComponent<RectTransform>(); rect.SetParent(parent,false);
            rect.anchorMin=rect.anchorMax=new Vector2(.5f,.5f); rect.anchoredPosition=position; rect.sizeDelta=size;
            return rect;
        }
        private RectTransform Panel(string name,Transform parent,Vector2 pos,Vector2 size,Color color)
        {
            var obj=new GameObject(name,typeof(RectTransform),typeof(Image));
            AcademyUI.Style(obj.GetComponent<Image>(),color); return Place(obj,parent,pos,size);
        }
        private Text Label(Transform parent,string text,Vector2 pos,Vector2 size,int fontSize,Color color)
        {
            var obj=new GameObject("Label",typeof(RectTransform),typeof(Text)); Place(obj,parent,pos,size);
            var label=obj.GetComponent<Text>(); label.font=font; label.text=text; label.fontSize=fontSize;
            label.color=color; label.alignment=TextAnchor.MiddleCenter; label.raycastTarget=false;
            label.resizeTextForBestFit=true; label.resizeTextMinSize=14; label.resizeTextMaxSize=fontSize;
            return label;
        }
        private Button Button(Transform parent,string text,Vector2 pos,Vector2 size,Action click)
        {
            var rect=Panel(text,parent,pos,size,new Color(.08f,.48f,.47f));
            var button=rect.gameObject.AddComponent<Button>(); button.onClick.AddListener(()=>click());
            AcademyUI.StyleButton(button);
            Label(rect,text,Vector2.zero,size,21,Color.white); return button;
        }
    }
}
