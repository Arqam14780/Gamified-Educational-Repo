using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ToonKids
{

    public class animatorshow : MonoBehaviour
    {
        public GameObject[] Boys;
        public GameObject[] Girls;
        public GameObject chair;
        string[] animations;
        GameObject RandomBoy;
        GameObject RandomGirl;
        int animN;
        int set = 0;
        GameObject newchair1;
        GameObject newchair2;
        bool rootON;
        public Texture[] texts;
        public GUIStyle newGUIStyle;
        public bool showUI;
        float Cturn;
        bool OnPosition;
        public Transform CamPos1;
        public Transform CamPos2;
        Transform CamStartPosition;
        Transform CamEndPosition;

        void Start()
        {
            if (transform.childCount > 0) Destroy(transform.GetChild(2).gameObject);
            rootON = false;
            animN = 0;
            animations = new string[34] { "walk1","walk2","walkbackwards","strafeR", "strafeL", "stairsUP" , "stairsDOWN", "run", "runR", "runL", "runbackwards", "runstrafeR",
                                                    "runstrafeL", "runINOUT", "sprint", "brake", "runturn180", "jump", "runjumpIN","freefall","turnR45","turnR90","turnL45","turnL90",
                                                    "turn180","hitforward","fallforwardIN","fallbackwardsIN","crouchIN","pushIN","stepforward","stepbackwards","stepR","stepL" };

            RandomBoy = Instantiate(Boys[Random.Range(0, 5)],transform);
            RandomGirl = Instantiate(Girls[Random.Range(0, 5)],transform);
            RandomBoy.transform.position += transform.right * 0.2f;
            RandomGirl.transform.position += transform.right * -0.2f;

            RandomBoy.GetComponent<TKBoyPrefabMaker>().Getready();
            RandomBoy.GetComponent<TKBoyPrefabMaker>().Randomize();
            RandomGirl.GetComponent<TKGirlPrefabMaker>().Getready();
            RandomGirl.GetComponent<TKGirlPrefabMaker>().Randomize();

            RandomBoy.GetComponent<Animator>().applyRootMotion = false;
            RandomGirl.GetComponent<Animator>().applyRootMotion = false;

            RandomBoy.GetComponent<Playanimation>().enabled = false;
            RandomGirl.GetComponent<Playanimation>().enabled = false;

            RandomBoy.GetComponent<Animator>().Play("TK_" + animations[0]);
            RandomGirl.GetComponent<Animator>().Play("TK_" + animations[0]);

            CamStartPosition = Camera.main.transform;
            CamEndPosition = Camera.main.transform;
            OnPosition = true;
        }

        void Update()
        {
            if (!OnPosition) MoveCam();
            if (Input.GetKeyDown("w")) changeset(1);
            if (Input.GetKeyDown("s")) changeset(-1);

            if (Input.GetKeyDown("d"))
            {
                changecharacter();
                animN++;
                changeanimation();
            }
            if (Input.GetKeyDown("a"))
            {
                changecharacter();
                animN--;
                changeanimation();
            }
            if (Input.GetKeyDown("space")) changecharacter();
            if (Input.GetKeyDown("r")) activeroot();
            if (Input.GetKeyDown("x")) showUI = !showUI;
            if (Input.GetKeyDown("left")) { Cturn += 90; turncharacter(); }
            if (Input.GetKeyDown("right")) { Cturn -= 90; turncharacter(); }
        }

        void changeanimation()
        {
            if (animN > animations.Length - 1) animN = 0;
            else if (animN < 0) animN = animations.Length -1 ;

            RandomBoy.GetComponent<Playanimation>().enabled = false;
            RandomGirl.GetComponent<Playanimation>().enabled = false;
            RandomBoy.GetComponent<Animator>().Play("TK_" + animations[animN]);
            RandomGirl.GetComponent<Animator>().Play("TK_" + animations[animN]);
        }

        void changeset(int todo)
        {
            set += todo;
            if (set > 5) set = 0;
            else if (set < 0) set = 5;
            animations = new string[34] { "walk1","walk2","walkbackwards","strafeR", "strafeL", "stairsUP" , "stairsDOWN", "run", "runR", "runL", "runbackwards", "runstrafeR",
                                                    "runstrafeL", "runINOUT", "sprint", "brake", "runturn180", "jump", "runjumpIN","freefall","turnR45","turnR90","turnL45","turnL90",
                                                    "turn180","hitforward","fallforwardIN","fallbackwardsIN","crouchIN","pushIN","stepforward","stepbackwards","stepR","stepL" };
            if (set == 1) animations = new string[13] { "idle1","idle2","idle3","idle4", "idle5", "idlehappy" , "idlesad", "idleafraid", "idleangry", "idleamazed", "idleembarrased", "idletired","idledizzy" };
            if (set == 2) animations = new string[18] { "talk1","talk2","clap","wave", "salute1", "salute2" , "laugh", "cry", "telloff", "scream", "sneeze", "grabUP", "grabDOWN", "victory1",
                                                        "victory2", "defeat1", "defeat2","throwcatch" };
            if (set == 3) animations = new string[7] { "lookback", "sitdownIN", "sitidle1", "sitidle2", "sitidle3", "sitdownmovechairIN", "sitdownmovechairOUT" };
            if (set == 4) animations = new string[18] { "DR_idle1", "DR_idle2", "DR_randomize1", "DR_randomize2", "DR_hair1", "DR_hair2", "DR_eyes1", "DR_eyes2", "DR_glasses1", "DR_glasses2",
                                                        "DR_chest1", "DR_chest2", "DR_legs1", "DR_legs2", "DR_feet1", "DR_feet2", "DR_skin1", "DR_skin2"};
            if (set == 5) animations = new string[7] { "liedownIN", "sunbath", "sitdownliedownRIN", "sitdownliedownLIN", "sleep1", "sleep2", "sleep3" };
                          
            animN = 0;
            changecharacter();
            changeanimation();
            if (set == 3 || set == 5) rootONOFF(true);
            else rootONOFF(false);
            if (set == 3)
            {
                Cturn = 0f;
                turncharacter();
                addchair();
            }
            else deletechair();
            if (set == 5) CamPos(1);            
            else CamPos(0);   
        }

        void changecharacter()
        {
            Destroy(RandomBoy);
            Destroy(RandomGirl);

            RandomBoy = Instantiate(Boys[Random.Range(0, 5)], transform);
            RandomGirl = Instantiate(Girls[Random.Range(0, 5)],transform);
            RandomBoy.transform.position += transform.right * 0.2f;
            RandomGirl.transform.position += transform.right * -0.2f;

            RandomBoy.GetComponent<TKBoyPrefabMaker>().Getready();
            RandomBoy.GetComponent<TKBoyPrefabMaker>().Randomize();

            RandomGirl.GetComponent<TKGirlPrefabMaker>().Getready();
            RandomGirl.GetComponent<TKGirlPrefabMaker>().Randomize();

            RandomBoy.GetComponent<Animator>().applyRootMotion = rootON;
            RandomGirl.GetComponent<Animator>().applyRootMotion = rootON;

            RandomBoy.GetComponent<Playanimation>().enabled = false;
            RandomGirl.GetComponent<Playanimation>().enabled = false;
            RandomBoy.GetComponent<Animator>().Play("TK_" + animations[animN]);
            RandomGirl.GetComponent<Animator>().Play("TK_" + animations[animN]);
            if (set == 3) addchair();
            turncharacter();
        }
        void addchair()
        {
            newchair1 = Instantiate(chair);
            newchair1.transform.position = RandomBoy.transform.Find("ROOT").position;
            newchair1.transform.parent = RandomBoy.transform.Find("ROOT").transform;

            newchair2 = Instantiate(chair);
            newchair2.transform.position = RandomGirl.transform.Find("ROOT").position;
            newchair2.transform.parent = RandomGirl.transform.Find("ROOT").transform;
        }
        void deletechair()
        {
            Destroy(newchair1);
            Destroy(newchair2);
        }
        void turncharacter()
        {
            RandomBoy.transform.rotation = Quaternion.Euler(new Vector3(0f, Cturn, 0f));
            RandomGirl.transform.rotation = Quaternion.Euler(new Vector3(0f, Cturn, 0f));
        }
        
        void activeroot()
        {
            rootON = !rootON;
            RandomBoy.GetComponent<Animator>().applyRootMotion = rootON;
            RandomGirl.GetComponent<Animator>().applyRootMotion = rootON;
        }
        void rootONOFF(bool ONOFF)
        {
            rootON = ONOFF;
            RandomBoy.GetComponent<Animator>().applyRootMotion = rootON;
            RandomGirl.GetComponent<Animator>().applyRootMotion = rootON;
        }
        void OnGUI()
        {
            if (showUI)
            {
                GUI.Label(new Rect(1300, 40, 300, 300), texts[0]);
                GUI.Label(new Rect(320, -100, 256, 256), animations[animN], newGUIStyle);
                GUI.Label(new Rect(300, 40, 256, 128), texts[set + 1]);
            }
        }

        public void CamPos(int index)
        {
            if (index == 0) CamEndPosition = CamPos1;
            else CamEndPosition = CamPos2;
            CamStartPosition = Camera.main.transform;           
            Vector3 Distance = CamEndPosition.position - Camera.main.transform.position;
            if (Distance.magnitude > 0.0125f) OnPosition = false;
        }
        void MoveCam()
        {
            Vector3 Distance = CamEndPosition.position - Camera.main.transform.position;
            Camera.main.transform.position = Vector3.Lerp(CamStartPosition.position, CamEndPosition.position, 0.01f);
            Camera.main.transform.rotation = Quaternion.Lerp(CamStartPosition.rotation, CamEndPosition.rotation, 0.01f);
            if (Distance.magnitude < 0.0125f) OnPosition = true;
        }
    }
}