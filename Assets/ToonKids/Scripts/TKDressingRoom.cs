using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ToonKids
{
    public class TKDressingRoom : MonoBehaviour
    {
        public Camera Mcam;
        bool MovilCam;
        public GameObject[] Boys;
        public GameObject[] Girls;
        GameObject[] ActiveChars;
        public GameObject Top;
        public GameObject Head;
        public GameObject Chest;
        public GameObject Legs;
        public GameObject Feet;
        public GameObject TopGirl;
        public GameObject HeadGirl;
        public GameObject ChestGirl;
        public GameObject LegsGirl;
        public GameObject FeetGirl;
        public GameObject GirlLegs;
        GameObject Character;
        public RuntimeAnimatorController DRAnimations;
        Animator anim;
        public Transform[] campositions;
        int Cam2Pos;
        bool OnPosition;
        Vector3 Distance;
        Transform CamStartPosition;
        Transform CamEndPosition;
        GameObject[] Panels;
        int CharIndex;
        int gender;
        int panel;
        float[] LookAt;
        Transform Target;
        float idletime;
        float nextidle;

        void Start()
        {
            MovilCam = true;
            idletime = 0f;
            nextidle = 2f;
            panel = 0;
            Mcam.transform.position = campositions[5].position;
            Mcam.transform.rotation = campositions[5].rotation;
            CharIndex = 0;
            Character = transform.GetChild(1).gameObject;
            if (Character.TryGetComponent(out Playanimation temp))
                temp.enabled = false;
            Character.GetComponent<TKBoyPrefabMaker>().Getready();
            anim = Character.GetComponent<Animator>();
            anim.runtimeAnimatorController = DRAnimations;
            anim.Play("idle1");
            OnPosition = true;
            Panels = new GameObject[10] { Top, Head, Chest, Legs, Feet, TopGirl, HeadGirl, ChestGirl, LegsGirl, FeetGirl };
            for (int forAUX = 0; forAUX < Panels.Length - 1; forAUX++)
                Panels[forAUX].SetActive(false);
            ActiveChars = Boys;
            gender = 0;
            Target = transform.GetChild(0);
            LookAt = new float[6] { 0.735f, 0.735f, 0.5f, 0.25f, 0.1f, 0.5f };
            Target.transform.position = transform.position + Vector3.up * LookAt[0];
            CamStartPosition = Mcam.transform;
            CamEndPosition = campositions[5];
        }

        void Update()
        {
            if (!OnPosition) MoveCam();
            idletime += Time.deltaTime;
            if (idletime > nextidle)
            {
                if (Cam2Pos == 5 || !MovilCam) anim.SetInteger("idles", Random.Range(0, 24));
                else anim.SetInteger("idles", 0);
                nextidle = Random.Range(2f, 4f);
                idletime = 0f;
            }

            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * -90f);
            if (Input.GetKey("s")) transform.rotation = Quaternion.Euler(Vector3.up);
            if (Input.GetKey("w")) { transform.rotation = Quaternion.Euler(Vector3.up); transform.Rotate(Vector3.up * 180f); }
        }


        //BOYS
        //Hairs & hats
        public void SetHair(int index)
        {
            Character.GetComponent<TKBoyPrefabMaker>().Sethair(index);
            if (Coin(8)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void NextHair()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nexthair();
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void PrevHair()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Prevhair();
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void NextHairColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nexthaircolor(0);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void PrevHairColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nexthaircolor(1);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void NextHatColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nexthatcolor(0);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void PrevHatColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nexthatcolor(1);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        //Head
        public void NextSkinColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextskincolor(0);
            if (Coin(8)) anim.Play("skin" + Random.Range(0, 2));
        }
        public void PrevSkinColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextskincolor(1);
            if (Coin(8)) anim.Play("skin" + Random.Range(0, 2));
        }
        public void NextEyesColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nexteyescolor(0);
            if (Coin(8)) anim.Play("eyes" + Random.Range(0, 2));
        }
        public void PrevEyesColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nexteyescolor(1);
            if (Coin(8)) anim.Play("eyes" + Random.Range(0, 2));
        }
        public void NextTeethColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextteethcolor(0);
            anim.Play("teeth");
        }
        public void PrevTeethColor()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextteethcolor(1);
            anim.Play("teeth");
        }
        public void NextGlasses()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextglasses(0);
        }
        public void PrevGlasses()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextglasses(1);
        }
        public void GlassesONOFF()
        {
            Character.GetComponent<TKBoyPrefabMaker>().Glasseson();
            if (Character.GetComponent<TKBoyPrefabMaker>().glassesactive) anim.Play("glasses" + Random.Range(0, 2));
        }
        //Chest
        public void SetChest(int index)
        {
            Character.GetComponent<TKBoyPrefabMaker>().Setchest(index);
            if (Coin(8)) anim.Play("chest" + Random.Range(0, 2));
        }
        public void NextChestColor(int index)
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextchestcolor(index);
        }
        //Legs
        public void SetLegs(int index)
        {
            Character.GetComponent<TKBoyPrefabMaker>().Setlegs(index);
            if (Coin(8)) anim.Play("legs" + Random.Range(0, 2));
        }
        public void NextLegsColor(int index)
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextlegscolor(index);
        }
        //Feet
        public void SetFeet(int index)
        {
            Character.GetComponent<TKBoyPrefabMaker>().Setfeet(index);
            if (Coin(8)) anim.Play("feet" + Random.Range(0, 2));
        }
        public void NextFeetColor(int index)
        {
            Character.GetComponent<TKBoyPrefabMaker>().Nextfeetcolor(index);
        }

        //GIRLS
        //Hairs & hats
        public void SetHairGirl(int index)
        {
            Character.GetComponent<TKGirlPrefabMaker>().Sethair(index);
            if (Coin(8)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void NextHairGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nexthair();
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void PrevHairGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Prevhair();
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void NextHairColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nexthaircolor(0);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void PrevHairColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nexthaircolor(1);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void NextHatColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nexthatcolor(0);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        public void PrevHatColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nexthatcolor(1);
            if (Coin(10)) anim.Play("hair" + Random.Range(0, 2));
        }
        //Head
        public void NextSkinColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextskincolor(0);
            if (Coin(8)) anim.Play("skin" + Random.Range(0, 2));
        }
        public void PrevSkinColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextskincolor(1);
            if (Coin(8)) anim.Play("skin" + Random.Range(0, 2));
        }
        public void NextEyesColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nexteyescolor(0);
            if (Coin(8)) anim.Play("eyes" + Random.Range(0, 2));
        }
        public void PrevEyesColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nexteyescolor(1);
            if (Coin(8)) anim.Play("eyes" + Random.Range(0, 2));
        }
        public void NextTeethColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextteethcolor(0);
            anim.Play("teeth");
        }
        public void PrevTeethColorGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextteethcolor(1);
            anim.Play("teeth");
        }
        public void NextGlassesGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextglasses(0);
        }
        public void PrevGlassesGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextglasses(1);
        }
        public void GlassesONOFFGirl()
        {
            Character.GetComponent<TKGirlPrefabMaker>().Glasseson();
            if (Character.GetComponent<TKGirlPrefabMaker>().glassesactive) anim.Play("glasses" + Random.Range(0, 2));
        }
        //Chest
        public void SetChestGirl(int index)
        {
            GirlLegs.SetActive(true);
            Character.GetComponent<TKGirlPrefabMaker>().Setchest(index);
            if (Character.GetComponent<TKGirlPrefabMaker>().Dress()) GirlLegs.SetActive(false);
            else GirlLegs.SetActive(true);
            if (Coin(8)) anim.Play("chest" + Random.Range(0, 2));
        }
        public void NextChestColorGirl(int index)
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextchestcolor(index);
        }
        //Legs
        public void SetLegsGirl(int index)
        {
            Character.GetComponent<TKGirlPrefabMaker>().Setlegs(index);
            if (Coin(8)) anim.Play("legs" + Random.Range(0, 2));

        }
        public void NextLegsColorGirl(int index)
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextlegscolor(index);
        }
        //Feet
        public void SetFeetGirl(int index)
        {
            Character.GetComponent<TKGirlPrefabMaker>().Setfeet(index);
            if (Coin(8)) anim.Play("feet" + Random.Range(0, 2));
        }
        public void NextFeetColorGirl(int index)
        {
            Character.GetComponent<TKGirlPrefabMaker>().Nextfeetcolor(index);
        }


        public void ActivePanel(int index)
        {
            panel = index;
            int aux = 5 * gender;
            for (int forAUX = 0; forAUX < Panels.Length; forAUX++)
                Panels[forAUX].SetActive(false);
            if (index < Panels.Length)
            {
                Panels[index + aux].SetActive(true);
            }
        }
        public void Randomize()
        {
            if (gender == 0) Character.GetComponent<TKBoyPrefabMaker>().Randomize();
            else Character.GetComponent<TKGirlPrefabMaker>().Randomize();
            CamPos(5);
            if (Coin(8)) anim.Play("randomize" + Random.Range(0, 2));

            if (gender == 1)
            {
                if (Character.GetComponent<TKGirlPrefabMaker>().Dress()) GirlLegs.SetActive(false);
                else GirlLegs.SetActive(true);
            }
        }
        public void Nude()
        {
            if (gender == 0) Character.GetComponent<TKBoyPrefabMaker>().Nude();
            else Character.GetComponent<TKGirlPrefabMaker>().Nude();
        }
        public void CamPos(int index)
        {
            Cam2Pos = index;
            CamStartPosition = Mcam.transform;
            if (MovilCam) CamEndPosition = campositions[Cam2Pos];
            else CamEndPosition = campositions[5];
            Distance = CamEndPosition.position - Mcam.transform.position;
            if (Distance.magnitude > 0.0125f) OnPosition = false;
            Target.transform.position = transform.position + Vector3.up * LookAt[index];
        }
        public void ChangeGender()
        {
            if (gender == 0)
            {
                gender = 1;
                ActiveChars = Girls;
                NextCharacter(0);
                //if (panel!=0) 
                    ActivePanel(panel);
            }
            else
            {
                gender = 0;
                ActiveChars = Boys;
                NextCharacter(0);
                //if (panel != 0)
                ActivePanel(panel);
            }
        }
        public void LockZoomONOFF()
        {
            MovilCam = !MovilCam;
            if (MovilCam) CamEndPosition = campositions[Cam2Pos];
            else CamEndPosition = campositions[5];
            OnPosition = false;
            MoveCam();
        }
        public void NextCharacter(int next)
        {
            Destroy(Character);
            CharIndex += next;
            if (CharIndex < 0) CharIndex = 4;
            else if (CharIndex > 4) CharIndex = 0;
            Character = Instantiate(ActiveChars[CharIndex], transform);
            if (Character.TryGetComponent(out Playanimation temp))
                temp.enabled = false;
            if (gender == 0) Character.GetComponent<TKBoyPrefabMaker>().Getready();
            else Character.GetComponent<TKGirlPrefabMaker>().Getready();

            anim = Character.GetComponent<Animator>();
            anim.runtimeAnimatorController = DRAnimations;

            int coin = Random.Range(0, 7);
            if (coin == 0) anim.Play("salute1");
            else if (coin == 1) anim.Play("salute2");
            else anim.Play("wave");
            if (gender == 1)
            {
                if (Character.GetComponent<TKGirlPrefabMaker>().Dress()) GirlLegs.SetActive(false);
                else GirlLegs.SetActive(true);
            }
        }
        public void Embarrasment()
        {
            anim.CrossFade("embarrassed", 0.05f);
        }
        void MoveCam()
        {
            Distance = CamEndPosition.position - Mcam.transform.position;
            Mcam.transform.position = Vector3.Lerp(CamStartPosition.position, CamEndPosition.position, 0.0125f);
            Mcam.transform.rotation = Quaternion.Lerp(CamStartPosition.rotation, CamEndPosition.rotation, 0.0125f);
            if (Distance.magnitude < 0.0125f) OnPosition = true;
        }
        bool Coin(int range)
        {
            if (Random.Range(0, range) < 5)
                return true;
            else return false;

        }
    }
}