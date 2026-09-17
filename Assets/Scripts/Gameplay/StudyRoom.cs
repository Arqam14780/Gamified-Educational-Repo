using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AR
{
    // Created by GameManager after the selected character has been activated.
    public sealed class StudyRoom : MonoBehaviour
    {
        private Vector3 origin = new Vector3(1000, 0, 1000);
        private readonly Color teal = AcademyUI.Teal;
        private int firstLesson;
        private StudyRoomEnvironment environment;
        private Font font;
        private Transform player;
        private CharacterController controller;
        private Animator animator;
        private Canvas canvas;
        private GameObject modal;
        private Text heading, progress, question, feedback, best;
        private readonly Button[] answers = new Button[4];
        private Button next;
        private Image quizProgress;
        private string subject;
        private StudyQuestion[] questions;
        private int index, score;
        private bool answered;
        private Vector2 touchMove;
        private Camera roomCamera;
        private float cameraYaw;
        private float cameraPitch = 12f;
        private StudyLessonCatalog lessonCatalog;
        private StudyMaterialViewer materialViewer;
        private bool IsReading => materialViewer != null && materialViewer.IsOpen;
        private bool IsOverlayOpen => (modal != null && modal.activeSelf) || IsReading;

        public void Initialize(GameObject selectedPlayer)
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            lessonCatalog = StudyLessonCatalog.Load();
            BuildRoom();
            player = selectedPlayer.transform;
            foreach (var motor in selectedPlayer.GetComponentsInChildren<TP_Motor>()) motor.enabled = false;
            foreach (var driver in selectedPlayer.GetComponentsInChildren<TP_Animator>()) driver.enabled = false;
            foreach (var body in selectedPlayer.GetComponentsInChildren<Rigidbody>()) body.isKinematic = true;
            foreach (var oldCollider in selectedPlayer.GetComponentsInChildren<Collider>()) oldCollider.enabled = false;
            player.SetPositionAndRotation(origin + new Vector3(0, .15f, -2.4f), Quaternion.identity);
            controller = selectedPlayer.GetComponent<CharacterController>();
            if (controller == null) controller = selectedPlayer.AddComponent<CharacterController>();
            controller.height = 1.8f; controller.radius = .3f; controller.center = new Vector3(0, .9f, 0); controller.enabled = true;
            animator = selectedPlayer.GetComponentInChildren<Animator>();
            if (animator != null) animator.applyRootMotion = false;
            foreach (var cam in FindObjectsByType<Camera>(FindObjectsSortMode.None)) cam.gameObject.SetActive(false);
            var cameraObject = new GameObject("Study room camera", typeof(Camera), typeof(AudioListener), typeof(PhysicsRaycaster));
            cameraObject.transform.SetParent(transform);
            cameraObject.tag = "MainCamera";
            roomCamera = cameraObject.GetComponent<Camera>();
            roomCamera.clearFlags = CameraClearFlags.SolidColor; roomCamera.backgroundColor = new Color(.87f, .92f, .93f);
            roomCamera.fieldOfView = 58f;
            roomCamera.nearClipPlane = .1f;
            foreach (var oldCanvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None)) oldCanvas.gameObject.SetActive(false);
            BuildUI();
            UpdateCamera(true);
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }

        private void BuildRoom()
        {
            environment = FindFirstObjectByType<StudyRoomEnvironment>();
            if (environment == null)
            {
                var prefab = Resources.Load<GameObject>("StudyGallery");
                if (prefab != null) environment = Instantiate(prefab, origin, Quaternion.identity).GetComponent<StudyRoomEnvironment>();
                else
                {
                    environment = new GameObject("Study Gallery").AddComponent<StudyRoomEnvironment>();
                    environment.transform.position = origin;
                    environment.Build();
                }
            }
            origin = environment.transform.position;
            RefreshFrames();
        }

        private void RefreshFrames()
        {
            environment.DisplayLessons(lessonCatalog, firstLesson);
            foreach (var frame in environment.GetComponentsInChildren<StudyFrame>())
            {
                var target = frame;
                frame.Activate = () => OpenFrame(target);
            }
        }
        private RectTransform Rect(GameObject obj, Transform parent, Vector2 anchor, Vector2 position, Vector2 size)
        {
            obj.transform.SetParent(parent,false); var r=obj.GetComponent<RectTransform>();
            r.anchorMin=r.anchorMax=anchor; r.anchoredPosition=position; r.sizeDelta=size; return r;
        }
        private Text Label(Transform parent,string value,Vector2 position,Vector2 size,int fontSize,Color color)
        {
            var obj=new GameObject("Label",typeof(RectTransform),typeof(Text)); Rect(obj,parent,new Vector2(.5f,.5f),position,size);
            var text=obj.GetComponent<Text>(); text.text=value; text.font=font; text.fontSize=fontSize; text.color=color;
            text.alignment=TextAnchor.MiddleCenter; text.raycastTarget=false;
            text.resizeTextForBestFit=true; text.resizeTextMinSize=16; text.resizeTextMaxSize=fontSize;
            return text;
        }
        private Button MakeButton(Transform parent,string value,Vector2 pos,Vector2 size,Action click)
        {
            var obj=new GameObject(value,typeof(RectTransform),typeof(Image),typeof(Button)); Rect(obj,parent,new Vector2(.5f,.5f),pos,size);
            obj.GetComponent<Image>().color=teal; var button=obj.GetComponent<Button>(); button.onClick.AddListener(()=>click());
            AcademyUI.StyleButton(button);
            Label(obj.transform,value,Vector2.zero,size-Vector2.one*8,23,Color.white); return button;
        }
        private void BuildUI()
        {
            if(EventSystem.current==null) new GameObject("Study EventSystem",typeof(EventSystem),typeof(InputSystemUIInputModule));
            var obj=new GameObject("Study UI",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster)); obj.transform.SetParent(transform);
            canvas=obj.GetComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay; canvas.sortingOrder=50;
            var scaler=obj.GetComponent<CanvasScaler>(); scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution=new Vector2(1280,800); scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.Expand;
            var header=new GameObject("Academy header",typeof(RectTransform),typeof(Image));
            Rect(header,canvas.transform,new Vector2(.5f,1),new Vector2(0,-64),new Vector2(1230,108));
            AcademyUI.Style(header.GetComponent<Image>(),AcademyUI.Navy);
            var banner=Label(canvas.transform,"LEARNING ACADEMY   /   Explore your classroom",new Vector2(-90,-42),new Vector2(800,55),26,Color.white);
            banner.rectTransform.anchorMin=banner.rectTransform.anchorMax=new Vector2(.5f,1);
            best=Label(canvas.transform,"",new Vector2(-90,-86),new Vector2(800,35),20,Color.white); best.rectTransform.anchorMin=best.rectTransform.anchorMax=new Vector2(.5f,1); RefreshBest();
            var footer=new GameObject("Control hint background",typeof(RectTransform),typeof(Image));
            Rect(footer,canvas.transform,new Vector2(.5f,0),new Vector2(0,35),new Vector2(740,48));
            AcademyUI.Style(footer.GetComponent<Image>(),AcademyUI.Navy);
            var hint=Label(canvas.transform,"WASD / arrows: move   |   Right-drag: look   |   Click a frame",new Vector2(0,35),new Vector2(730,40),19,Color.white);
            hint.rectTransform.anchorMin=hint.rectTransform.anchorMax=new Vector2(.5f,0);
            var menu=MakeButton(canvas.transform,"Characters",new Vector2(-110,-42),new Vector2(180,48),()=>SceneManager.LoadScene("MainMenu"));
            var mr=menu.GetComponent<RectTransform>(); mr.anchorMin=mr.anchorMax=Vector2.one;
            if(lessonCatalog.lessons.Length>2)
            {
                var more=MakeButton(canvas.transform,"Next subjects >",new Vector2(-110,-91),new Vector2(180,36),()=>
                {
                    if(IsOverlayOpen)return;
                    firstLesson=(firstLesson+2)>=lessonCatalog.lessons.Length?0:firstLesson+2;
                    RefreshFrames(); RefreshBest();
                });
                more.GetComponent<RectTransform>().anchorMin=more.GetComponent<RectTransform>().anchorMax=Vector2.one;
            }
            Direction("^",new Vector2(110,185),Vector2.up); Direction("v",new Vector2(110,65),Vector2.down);
            Direction("<",new Vector2(45,125),Vector2.left); Direction(">",new Vector2(175,125),Vector2.right);
            var lookPad = new GameObject("Touch look pad", typeof(RectTransform), typeof(Image), typeof(StudyLookPad));
            Rect(lookPad, canvas.transform, new Vector2(1,0), new Vector2(-120,115), new Vector2(195,150));
            AcademyUI.Style(lookPad.GetComponent<Image>(),new Color(.07f,.14f,.22f,.82f));
            Label(lookPad.transform,"DRAG TO LOOK",Vector2.zero,new Vector2(190,140),19,Color.white);
            lookPad.GetComponent<StudyLookPad>().Look = delta =>
            {
                if (IsOverlayOpen) return;
                cameraYaw += delta.x * .16f;
                cameraPitch = Mathf.Clamp(cameraPitch - delta.y * .10f, -8f, 28f);
            };
            modal=new GameObject("Quiz overlay",typeof(RectTransform),typeof(Image)); var overlay=Rect(modal,canvas.transform,new Vector2(.5f,.5f),Vector2.zero,Vector2.zero);
            overlay.anchorMin=Vector2.zero; overlay.anchorMax=Vector2.one; overlay.offsetMin=overlay.offsetMax=Vector2.zero; modal.GetComponent<Image>().color=new Color(.02f,.03f,.05f,.48f);
            var card=new GameObject("Quiz card",typeof(RectTransform),typeof(Image)); Rect(card,modal.transform,new Vector2(.5f,.5f),Vector2.zero,new Vector2(620,620)); card.GetComponent<Image>().color=new Color(.025f,.035f,.055f,.94f);
            AcademyUI.Style(card.GetComponent<Image>(),AcademyUI.Navy);
            heading=Label(card.transform,"",new Vector2(0,255),new Vector2(500,45),32,Color.white);
            progress=Label(card.transform,"",new Vector2(0,205),new Vector2(530,35),19,new Color(.40f,.85f,.82f));
            var track=new GameObject("Quiz progress track",typeof(RectTransform),typeof(Image));
            Rect(track,card.transform,Vector2.one*.5f,new Vector2(0,176),new Vector2(530,5));
            track.GetComponent<Image>().color=new Color(.18f,.28f,.35f);
            var fill=new GameObject("Quiz progress",typeof(RectTransform),typeof(Image));
            Rect(fill,track.transform,new Vector2(0,.5f),Vector2.zero,new Vector2(530,5));
            fill.GetComponent<RectTransform>().pivot=new Vector2(0,.5f);
            quizProgress=fill.GetComponent<Image>(); quizProgress.color=new Color(.38f,.81f,.73f);
            question=Label(card.transform,"",new Vector2(0,125),new Vector2(550,100),30,Color.white);
            for(int i=0;i<4;i++) { int answer=i; answers[i]=MakeButton(card.transform,"",new Vector2(i%2==0?-145:145,20-i/2*78),new Vector2(265,62),()=>Answer(answer)); }
            feedback=Label(card.transform,"",new Vector2(0,-135),new Vector2(550,70),23,Color.white);
            next=MakeButton(card.transform,"Next",new Vector2(0,-225),new Vector2(245,55),Next);
            MakeButton(card.transform,"X",new Vector2(275,270),new Vector2(40,40),CloseQuiz);
            modal.SetActive(false);
            var reader = new GameObject("Learning material reader", typeof(RectTransform), typeof(StudyMaterialViewer));
            materialViewer = reader.GetComponent<StudyMaterialViewer>();
            materialViewer.Initialize(canvas.transform, font, () => touchMove = Vector2.zero);
        }
        private void Direction(string label,Vector2 position,Vector2 direction)
        {
            var button=MakeButton(canvas.transform,label,position,new Vector2(58,52),()=>{});
            var rect=button.GetComponent<RectTransform>(); rect.anchorMin=rect.anchorMax=Vector2.zero;
            var hold=button.gameObject.AddComponent<StudyMoveButton>(); hold.Changed=value=>touchMove=value?direction:Vector2.zero;
        }
        private void Update()
        {
            if(player==null) return;
            bool paused=IsOverlayOpen;
            if(Keyboard.current!=null&&Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (IsReading) materialViewer.Close(); else CloseQuiz();
            }
            Vector2 move=touchMove;
            var k=Keyboard.current;
            if(k!=null) move+=new Vector2((k.dKey.isPressed||k.rightArrowKey.isPressed?1:0)-(k.aKey.isPressed||k.leftArrowKey.isPressed?1:0),(k.wKey.isPressed||k.upArrowKey.isPressed?1:0)-(k.sKey.isPressed||k.downArrowKey.isPressed?1:0));
            move=paused?Vector2.zero:Vector2.ClampMagnitude(move,1);
            if (Mouse.current != null && !paused && Mouse.current.rightButton.isPressed)
            {
                Vector2 look = Mouse.current.delta.ReadValue();
                cameraYaw += look.x * .16f;
                cameraPitch = Mathf.Clamp(cameraPitch - look.y * .10f, -8f, 28f);
            }
            Quaternion headingRotation = Quaternion.Euler(0, cameraYaw, 0);
            var velocity=headingRotation * new Vector3(move.x,0,move.y);
            controller.Move((velocity*3.2f+Vector3.down*4)*Time.deltaTime);
            if(velocity.sqrMagnitude>.01f) player.rotation=Quaternion.RotateTowards(player.rotation,Quaternion.LookRotation(velocity),540*Time.deltaTime);
            if(animator!=null) foreach(var parameter in animator.parameters)
                if(parameter.type==AnimatorControllerParameterType.Float&&(parameter.name=="Speed"||parameter.name=="Vertical"||parameter.name=="Horizontal")) animator.SetFloat(parameter.name,parameter.name=="Horizontal"?0:move.magnitude);
            if(player.position.y<origin.y-3) { controller.enabled=false; player.position=origin+new Vector3(0,.15f,-2.4f); controller.enabled=true; }
        }
        private void LateUpdate()
        {
            if (player != null && !IsOverlayOpen) UpdateCamera(false);
        }
        private void UpdateCamera(bool snap)
        {
            Vector3 focus = player.position + Vector3.up * 1.6f;
            Quaternion cameraRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0);
            Vector3 direction = -(cameraRotation * Vector3.forward);
            float distance = 3.7f;
            foreach (var hit in Physics.SphereCastAll(focus, .22f, direction, distance, ~0, QueryTriggerInteraction.Ignore))
                if (!hit.transform.IsChildOf(player)) distance = Mathf.Min(distance, Mathf.Max(.25f, hit.distance - .08f));
            Vector3 desiredCameraPosition = focus + direction * distance;
            // Retract immediately near geometry; smoothing must not carry the camera through a wall.
            if (snap || distance < 3.7f) roomCamera.transform.position = desiredCameraPosition;
            else roomCamera.transform.position = Vector3.Lerp(roomCamera.transform.position, desiredCameraPosition, 1f - Mathf.Exp(-12f * Time.deltaTime));
            Vector3 p = roomCamera.transform.position - origin;
            p.x = Mathf.Clamp(p.x,-6.65f,6.65f); p.z = Mathf.Clamp(p.z,-5.6f,5.6f); p.y = Mathf.Clamp(p.y,.3f,5.1f);
            roomCamera.transform.position = origin + p;
            roomCamera.transform.rotation = cameraRotation;
        }
        private void OpenFrame(StudyFrame frame)
        {
            if (IsOverlayOpen) return;
            touchMove = Vector2.zero;
            if (frame.kind == StudyFrameKind.Quiz) OpenQuiz(frame.subject);
            else materialViewer.Open(lessonCatalog.Find(frame.subject), frame.kind);
        }
        private void OpenQuiz(string chosen)
        {
            if (IsOverlayOpen) return;
            subject=chosen; questions=lessonCatalog.Find(chosen).questions; index=score=0; touchMove=Vector2.zero;
            modal.SetActive(true); ShowQuestion();
        }
        private void ShowQuestion()
        {
            answered=false; heading.text=subject+" Quiz"; progress.text="Question "+(index+1)+" / "+questions.Length+"   |   Score: "+score;
            quizProgress.rectTransform.sizeDelta=new Vector2(530f*(index+1)/questions.Length,5);
            question.text=questions[index].prompt; feedback.text="Choose one answer."; next.gameObject.SetActive(false);
            for(int i=0;i<answers.Length;i++) { answers[i].gameObject.SetActive(true); answers[i].interactable=true; answers[i].GetComponent<Image>().color=teal; answers[i].GetComponentInChildren<Text>().text=((char)('A'+i))+".  "+questions[index].options[i]; }
        }
        private void Answer(int selected)
        {
            if(answered) return; answered=true; bool correct=selected==questions[index].correct; if(correct) score++;
            feedback.text=(correct?"Correct! Great job!":"Correct answer: "+questions[index].options[questions[index].correct])+"\n"+questions[index].source;
            for(int i=0;i<answers.Length;i++) { answers[i].interactable=false; if(i==questions[index].correct) answers[i].GetComponent<Image>().color=new Color(.2f,.65f,.3f); }
            if(!correct) answers[selected].GetComponent<Image>().color=new Color(.65f,.25f,.25f);
            next.GetComponentInChildren<Text>().text=index==questions.Length-1?"See results":"Next question"; next.gameObject.SetActive(true);
        }
        private void Next()
        {
            if(index>=questions.Length) { index=score=0; ShowQuestion(); return; }
            if(!answered) return;
            index++;
            if(index<questions.Length) { ShowQuestion(); return; }
            string key="StudyRoom.Best."+subject; PlayerPrefs.SetInt(key,Mathf.Max(score,PlayerPrefs.GetInt(key,0))); PlayerPrefs.Save(); RefreshBest();
            heading.text="Well done!"; progress.text=subject+" complete"; question.text=score+" / "+questions.Length+" correct";
            feedback.text=score==questions.Length?"Perfect score! Try the other frame next.":"Keep learning. You can try again!";
            foreach(var button in answers) button.gameObject.SetActive(false); next.GetComponentInChildren<Text>().text="Try again";
        }
        private void CloseQuiz() { if(modal!=null) modal.SetActive(false); touchMove=Vector2.zero; }
        private void RefreshBest()
        {
            best.text="PERSONAL BEST";
            for(int i=firstLesson;i<Mathf.Min(firstLesson+2,lessonCatalog.lessons.Length);i++)
            {
                var lesson=lessonCatalog.lessons[i];
                best.text+="   |   "+lesson.subject+" "+Mathf.Min(lesson.questions.Length,PlayerPrefs.GetInt("StudyRoom.Best."+lesson.subject,0))+" / "+lesson.questions.Length;
            }
        }
        private void OnApplicationFocus(bool focused) { if(!focused) touchMove=Vector2.zero; }
        private void OnDestroy() { StudyContent.Release(); }
    }
}





