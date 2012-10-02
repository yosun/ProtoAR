using UnityEngine;
using System.Collections;

public class TheMaker : MonoBehaviour {
	
	public Camera cam; public GUIText guitext;
	
	enum TheMakeModes{
		Default,
		BrowseObj,
		Share
	}
	TheMakeModes theMakeMode = TheMakeModes.BrowseObj;
	Vector2 touchStart=new Vector2(-1000,-1000);
	Vector2 touchEnd = new Vector2(-1000,-1000);
	
	public int selectedChair=-1;
	public int selectedLamp=-1;
	
	public Texture[] texChairs = new Texture[5];
	public Texture[] texLamps = new Texture[5];
	
	public Texture[] texChairsGreen = new Texture[5];
	public Texture[] texLampsGreen = new Texture[5];	
	public int currentChair=-1;
	public int currentLamp=-1;
	
	public GameObject curActive;
	public GameObject curChair; public GameObject[] goChairs = new GameObject[5];
	public GameObject curLamp; public GameObject[] goLamps = new GameObject[5];
	public GameObject goSewers;
	
	public GUISkin guiskin; public GUISkin guiblackish;
	float margin = 10f; float topbtnsize=60f; float socialbtnsize=128f;
	float btnSize=10f;
	public Texture texTopStrip;
	public Texture texBrowseObj;
	public Texture texBack;
	public Texture texSearch;
	public Texture texSeeThru; int seeThruVisible=1;
	public Texture texShare;
	public Texture texTwitter; public Texture texFacebook; public Texture texGoogle; public Texture texTumblr; public Texture texFlickr;
	
	Rect[] rects;
	Rect rectTopRight;
	Rect rectTopRight2;
	Rect rectTopRight3;
	Rect rectTopLeft;
	Rect rectTopStrip;
	Rect rectBottom1;
	Rect rectBottom2;
	Rect rectBottom3;
	Rect rectBottom4;
	Rect rectBottom5;
	
	// Use this for initialization
	void Start () {
		btnSize = Screen.width/5f;
		socialbtnsize = Screen.width/5f;
		ToggleSeeThru ();
		
		rects = new Rect[9];
		rectTopLeft=new Rect(margin,margin,topbtnsize,topbtnsize);
		rectTopRight=new Rect(Screen.width - margin - topbtnsize,margin,topbtnsize,topbtnsize);
		rectTopRight2=new Rect(Screen.width - margin*2 - 2*topbtnsize,margin,topbtnsize,topbtnsize);
		rectTopRight3=new Rect(Screen.width - margin*3 - 3*topbtnsize,margin,topbtnsize,topbtnsize);
		rectTopStrip = new Rect(0,0,Screen.width,80);
		rectBottom1=new Rect(0,Screen.height - socialbtnsize,socialbtnsize,socialbtnsize);
		rectBottom2=new Rect(socialbtnsize,Screen.height - socialbtnsize,socialbtnsize,socialbtnsize);
		rectBottom3=new Rect(socialbtnsize*2,Screen.height - socialbtnsize,socialbtnsize,socialbtnsize);
		rectBottom4=new Rect(socialbtnsize*3,Screen.height - socialbtnsize,socialbtnsize,socialbtnsize);
		rectBottom5=new Rect(socialbtnsize*4,Screen.height - socialbtnsize,socialbtnsize,socialbtnsize);
		rects[0] = rectTopRight; rects[1] = rectTopRight2; rects[2] = rectTopRight3; rects[3] = rectTopLeft; rects[4] = rectTopStrip;
		rects[5]=rectBottom1;rects[6]=rectBottom2;rects[7]=rectBottom3;rects[8]=rectBottom4;rects[9]=rectBottom5;
	}
	
	// Update is called once per frame
	void Update () {
		 Vector2 pos = Input.mousePosition;
		 if (!TouchHelper.NotInRects(rects,pos))
			return;
		
		 Ray ray = cam.ScreenPointToRay(pos);
	     RaycastHit hit;
       	 if (Physics.Raycast(ray, out hit, 5000f)){
			string name = hit.transform.gameObject.name;
			if(name=="Floor"){
				//print (hit.point);
	     	    Move(curActive.transform , hit.point);
			}/*else if(name=="curChair"){
				curActive = goChair;
			}else if(name=="curLamp"){
					
			}*/
		 }
	}
	
	void Move(Transform what,Vector3 loc){
		what.position = new Vector3(loc.x,what.position.y,loc.z);
	}
	
	void MoveElsewhereBut(GameObject[] g,int c){
		for(int i=0;i<g.Length;i++){
			if(i!=c){
				g[i].transform.position = new Vector3(-9999,0,-9999);
			}
		}
	}
	
	void OnGUI(){ //guitext.text = theMakeMode.ToString();
		if(theMakeMode==TheMakeModes.BrowseObj){
			GUI.skin = guiskin;
			
			selectedChair = GUI.SelectionGrid(new Rect(0,0,Screen.width,btnSize),selectedChair,texChairs,5);
			selectedLamp = GUI.SelectionGrid(new Rect(0,btnSize,Screen.width,btnSize),selectedLamp,texLamps,5);
			
			if(selectedChair!=-1){
				currentChair = selectedChair;
				Move (goChairs[currentChair].transform,curChair.transform.position);
				curChair = goChairs[currentChair];
				MoveElsewhereBut(goChairs,currentChair);
				curActive = curChair;
				selectedChair=-1;
			}
			if(selectedLamp!=-1){
				currentLamp = selectedLamp;
				Move (goLamps[currentLamp].transform,curLamp.transform.position);
				curLamp = goLamps[currentLamp];
				MoveElsewhereBut(goLamps,currentLamp);
				curActive = curLamp;
				selectedLamp=-1;
			}
			
			if(currentChair!=-1){
				GUI.DrawTexture(new Rect(currentChair*btnSize,0,btnSize,btnSize),texChairsGreen[currentChair]);
			}
			if(currentLamp!=-1){
				GUI.DrawTexture(new Rect(currentLamp*btnSize,btnSize,btnSize,btnSize),texLampsGreen[currentLamp]);
			}
			
			if(Input.touchCount==1){
				if(Input.GetTouch(0).phase==TouchPhase.Began){
					touchStart = Input.GetTouch(0).position;
					touchEnd = touchStart;
				}else if(Input.GetTouch (0).phase == TouchPhase.Ended){
					touchEnd = Input.GetTouch (0).position;
				
					float diff = (touchEnd.y-touchStart.y);
					if(diff>280f){
						theMakeMode = TheMakeModes.Default;
						//guitext.text = touchEnd.ToString()+" "+touchStart.ToString()+" "+diff.ToString();
					}
					touchStart=touchEnd;
				}
				
			}
			/*
			GUI.skin = guiblackish;
			if(GUI.Button (new Rect(margin,Screen.height - margin - btnSize,btnSize,btnSize),"<")){
				theMakeMode=TheMakeModes.Default;
			}*/
			
		}else{
			GUI.DrawTexture(rectTopStrip,texTopStrip);
			
			
			GUI.skin = guiblackish;
		
				if(GUI.Button (rectTopRight2,texBrowseObj)){
					theMakeMode=TheMakeModes.BrowseObj;
				}	
			
			
			if(GUI.Button(rectTopLeft,texSearch)){
				// TODO **************** back to model search screen
				
			}			
		
			
			if(GUI.Button (rectTopRight3,texSeeThru)){
				ToggleSeeThru();
			}
			
			if(theMakeMode == TheMakeModes.Share){
				if(GUI.Button (rectTopRight,texShare)){
					theMakeMode=TheMakeModes.Default;
				}
				
				// TODO ****************** add social
				if(GUI.Button (rectBottom1,texTwitter)){
					
				}
				if(GUI.Button (rectBottom2,texFacebook)){
					
				}
				if(GUI.Button (rectBottom3,texGoogle)){
					
				}
				if(GUI.Button (rectBottom4,texTumblr)){
					
				}
				if(GUI.Button (rectBottom5,texFlickr)){
					
				}				
			}else{
				if(GUI.Button (rectTopRight,texShare)){
					theMakeMode=TheMakeModes.Share;
				}				
			}
		}
	}
	
	void ToggleSeeThru(){
		seeThruVisible*=-1;
		if(seeThruVisible<0){
			// hide
			goSewers.SetActiveRecursively(false);
		}else{
			// show
			goSewers.SetActiveRecursively(true);
		}
	}
	
}
