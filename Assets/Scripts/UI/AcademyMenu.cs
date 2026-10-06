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


    }
}
