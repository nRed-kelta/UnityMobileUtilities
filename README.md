# nRed Unity Mobile Utilities
This is development utility of iOS/Android apps by Unity.

## Checked environment
- Unity2018.2.9f1
- FirebaseSDK for Unity 5.3.1
- Admob Google Mobile Ads Unity Plugin v3.15.1

## Usage

### Use utilities on your scripts(without FirebaseUtility)
1. Import nRed.unitypackage
2. Write to your script by "using nRed/SOME NAMESPACE"
3. Use nRed utility functions.

### Use FirebaseUtility
1. Import nRed.unitypackage
2. Import Firebase/Admob package by FirebaseSDK/AdmobSDK.
3. Drag & Drop "Assets/nRed/[nRed] FirebaseUtility" to hierarchy.
4. Bind your monobehaviour to FirebaseHelper prefab. ex) public FirebaseHelper fbutil;
5. Select "[nRed] FirebaseUtility" on hierarchy.
6. Select checkbox of your use utility and press Update & Save button on Inspector.
7. Setting your using Firebase modules on Inspector.
8. Use access module wrapper by FirebaseHelper class instance.
~~~
//ex) Use AdMob reward ad by utility.

[SerializeField] FirebaseHelper fbutil; // PREMISE: This value binded to nRed prefab on inspector already.

void YourGameAdmobViewTiming()
{
	// 2. Called when reward ad request finished.
	fbutil.admob.onRewardLoaded = ()=>{ // Optional
		// 3. Called when finished show interstitial.
		fbutil.admob.ShowRewardVideo(
			onYourRewardedCallback,
			onYourNotRewardedCallback,
			onYourRewardTouchedWithAppQuit
		);
	};

	// 1. Request reward ad by your ad unit id.
	fbutil.admob.RequestRewardVideo( YOUR_ADUNIT_ID );
}
~~~
### Utilities
--------------------------------------------------------------------------
#### ActionUtility
	Null check and call to methods.

~~~
// == if(YourAction!=null) YourAction();
YourAction.SafeCall(); 

// == if(YourAction1Param!=null) YourAction1Param(param1);
YourAction1Param.SafeCall(param1); 
~~~

#### CodeUtility
	Get code line, and method from StackFrames.

#### DebugUtility
	Debug.Log helper.

~~~
D.Log("This log only DEBUG build.");
~~~

#### FactoryUtility
	Cached array & list. Elements can release to cache pool.
	Recycle element is difficult management, but can reuse heap.
	Especially get convenience on mobile.

~~~
// Cacheable array container.
CArray<int> cacheArray = new CArray<int>();
int[] intArrayA = cacheArray.Alloc(10);

cacheArray.Release(ref intArrayA);
// intArrayA pool to cache.
// at next int[10] array allocation is return by cache pool.

int[] intArrayB = cacheArray.Alloc(100);
// Size 100 array has not exists in pool. That use allocation by new.

int[] intArrayC = cacheArray.Alloc(10);
// Size 10 array has exists in pool by above Release(). That not allocate heap. Return by cache pool of above.

// Caution!
// If array element is reference type,
// then you need timing management of release or destroy .


~~~

#### GameObjectUtility
	Shortcut access. No more write "gameObject.transform.localPosition.x" etc..
~~~
gobj.lx(2f);		// == gameObject.transform.localPosition =
			//        new Vector3(2f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
gobj.lp(Vector3.zero);	// == gameObject.transform.localPosition = Vector3.zero;
gobj.ls(1f,2f,3f);	// == gameObject.transform.localScale = new Vector3(1f, 2f, 3f);
floar localY = gobj.ly();// == gameObject.transform.localPosition.y;
~~~

#### GUIStyleBuilder
	Easy build GUIStyle.
~~~
// Settings can flexible order.
var style = new GUIStyle()
		.OnActiveColor( Color.red )
		.TextColor( Color.black )
		.RichText( true )
		.FixedWidth( 100 );
~~~

#### GuiUtility / EditorGuiUtility
	Easy group scope.
	No more write "Begin***** to End*****" etc..
~~~
using(DISABLE( !canButtonPress )) // Condition of disable parts.
{
	using(COLOR_BG(Color.Yellow)) // GUI color change scope.
	{
		using( XLAYOUT("Box") )	 // GUI Horizontal layout scope.
		{
			if( GUILayout.Button("LEFT") )
			{
				// Button action1
			}
						
			if( GUILayout.Button("RIGHT") )
			{
				// Button action2
			}
		}
	}
}
~~~

#### ICollectionUtility
	Safe methods.
	Safe call of ICollections. for example List and Distionary etc.
	if( list.IsReadable() ) // == if( (list!=null) && (list.Count>0) )

#### InputUtility
	Simple key bind action.
	A key can bind max to 2 actions.
	BindKey( int yourActionCode, KeyCode keyCode, int bindIndex=0, string actionNameOnNew="" );
~~~
// Bind key to your id
KeyAction.Instance.BindKey( MY_JUMP_ENUM_ID, KeyCode.Space, 0, "JumpAction" );

// Judge pressed keydown
if( KeyAction.Instance.IsKeyDown( MY_JUMP_ENUM_ID ) )
	Jump();
~~~

#### StringUtility
	String / StringBuilder shortcuts.

#### FirebaseUtility
	Simple setup for firebase some functions.
	Configure on inspector and easy init for firebase scripting.
	If use this utility, then need import FirebaseSDK or AdmobSDK for Unity, and "[nRed] FirebaseHelper" prefab drag to your hierarchy.
	after drag, then select prefab & check use firebase utilities.

##### Firebase Analytics
	Simple log wrapper.

~~~
public FirebaseHelper fbutil = YOUR_HIERARCHY_NRED_PREFAB;

// Available on all builds
fbutil.analytics.LogEvent("LevelClear","Easy", 5);

// Available on DEBUG build
fbutil.analytics.DLogEvent("DebugMode","ForceLevelClear", 5);
~~~

##### Firebase Authentication
	Support E-mail/Password auth and Anonymous auth sign-in.
	In editor use substitution by E-mail/Password auth.

##### Firebase Cloud Messaging
	Inspector can switch configure subscribe topics of DEBUG / RELEASE / PRODUCT BUILD.

##### Firebase RemoteConfig
	Setup default keyValue and developer mode on inspector.

##### Firebase Realtime database
	Inspector can setup information for run on editor.

##### Firebase Storage
	Setup buckets array on inspector.

##### AdMob
	Test devices registable.
	Setup of Banner/Reward/Interstitial ad on inspector.

##### Not supported below.
	- DynamicLink
	- Cloud Firestore(Not included in FirebaseSDK for Unity)
	- Cloud Funtions
	- Invites
	- 
#### Version History
##### 0.1
	Added nRed utility scripts.

--------------------------------------------------------------------------
## License
MIT