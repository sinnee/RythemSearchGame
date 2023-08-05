using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using DG.DemiEditor;
using UnityEngine.PlayerLoop;
using Debug = UnityEngine.Debug;

public enum Mode
{
	None = 0,
	Create,
	Edit
}

public class BeatMakerWindow : EditorWindow
{
	private Vector2 scrollPosition = new Vector2(0000, 0000);
	private Vector2 scrollPosition2 = new Vector2(0000, 0000);
	private GUIStyle buttonStyle;
	private GUIStyle topStatusStyle;
	private NoteScore _noteScore;

	private int _buttonNumber;

	//dropdown variable
	private List<string> _dropDownOptionList = new List<string>();
	private string[] _dropDownOptionArray;

	private bool isClickedCtlr;

	private const float NOTE_ZOME_INOUT_WEIGHT = 2;
	private const float SCROLLING_WEIGHT = 10;

	private GUIStyle _fieldStyle;


	[MenuItem("MyTool/BeatMaker %b")]
	static void Open()
	{
		Debug.Log("Test open");

		var window = GetWindow<BeatMakerWindow>();
		window.title = "Beat Maker";
	}

	private void OnEnable()
	{
		Debug.Log("Test oneenable");


		//노트 버튼 설정
		buttonStyle = new GUIStyle();
		buttonStyle.alignment = TextAnchor.UpperCenter;
		buttonStyle.fixedWidth = 30;
		//buttonStyle.fixedHeight = 30;
		buttonStyle.Height(30);
		buttonStyle.padding = new RectOffset(0, 0, 0, 0);
		buttonStyle.normal.background = new Texture2D(1, 1);

		//노트 버튼 설정
		topStatusStyle = new GUIStyle();
		topStatusStyle.Height(10);
		topStatusStyle.alignment = TextAnchor.UpperCenter;
		topStatusStyle.fixedWidth = 30;
		topStatusStyle.padding = new RectOffset(0, 0, 0, 0);
		//topStatusStyle.normal.background = new Texture2D(1, 1);

		_noteScore = null;



		_fieldStyle = new GUIStyle();
		_fieldStyle.Width(10);
		_fieldStyle.Margin(10);

		watch = new Stopwatch();
		



	}

	private Stopwatch watch;

	private void OnGUI() //update 같은 개념.
	{
		#region DrawMenuBar

		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		{


			if (GUILayout.Button("Load", EditorStyles.toolbarButton))
			{
				Load();
			}

			if (GUILayout.Button("Save as new name", EditorStyles.toolbarButton))
			{
				Save();
			}
			if (GUILayout.Button("Convert", EditorStyles.toolbarButton))
			{
				ConvertInstrumentObject();
			}
			GUILayout.FlexibleSpace();
		}
		EditorGUILayout.EndHorizontal();
		#endregion

	if (_noteScore == null) // create mode
		{
			if (GUILayout.Button("Create Score"))
			{
				Create();
			}
			if (GUILayout.Button("Load"))
			{
				Load();
			}
		}
		else // compose mode
		{
			EditorGUILayout.LabelField($"Score Name {_noteScore.name}");
			GUILayout.BeginHorizontal();
			{

				_noteScore.BPMValue = EditorGUILayout.IntField("BPM", _noteScore.BPMValue);
				_noteScore.BarCountValue = EditorGUILayout.IntField("BarCount", _noteScore.BarCountValue);

				GUILayout.BeginVertical();
				{
					_noteScore.NoteCountValue = EditorGUILayout.IntField("Count", _noteScore.NoteCountValue);

					EditorGUI.BeginChangeCheck();
					{
						_noteScore.selectedIndex = EditorGUILayout.Popup("Note", _noteScore.selectedIndex, _dropDownOptionArray);
					}
					if (EditorGUI.EndChangeCheck())
					{

						_buttonNumber = _noteScore.Scale();
						Debug.Log(_dropDownOptionList[_noteScore.selectedIndex]);
					}
				}
				GUILayout.EndVertical();


			}
			GUILayout.EndHorizontal();


			//악보 편집 영역

			using (var scrollViewScope = new ScrollViewScope(scrollPosition))
			{
				GUILayout.BeginHorizontal();
				{

					//info
					GUILayout.BeginVertical();
					{
						foreach (var item in _noteScore.items)
						{
							DrawInstInfo(item);
						}
					}
					GUILayout.EndVertical();

					//score
					GUILayout.BeginVertical();
					{
						GUIStyle scrollViewStyle = new GUIStyle(GUI.skin.scrollView);
						scrollViewStyle.overflow = new RectOffset(0, 0, 0, 0);


						scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2,true,false);
						{
							int idxCount = 0;

							GUILayout.BeginHorizontal();
							{

								for (int i = 0; i < _noteScore.BarCountValue; i++)
								{
									for (int j = 0; j < _noteScore.NoteCountValue; j++) //마디
									{
										for (int k = 0; k < _buttonNumber; k++) //음표 박자 수 그리기
										{
											

											//버튼 입력 판정
											if (GUILayout.Button(EditorGUIUtility.IconContent("Animation.EventMarker"), topStatusStyle))
											{
												

												Repaint();
											}
											GUI.backgroundColor = Color.white;
											idxCount++;
										}
										GUILayout.Label(EditorGUIUtility.IconContent("d_Animation.EventMarker"));
									}
									GUILayout.Label("한마디끝");
								}
							}
							GUILayout.EndHorizontal();
							
							foreach (var item in _noteScore.items)
							{
								DrawScore(item);
							}
						}
						GUILayout.EndScrollView();
						
						
							
						
					}
					GUILayout.EndVertical();
				}
				GUILayout.EndHorizontal();


				GUILayout.BeginHorizontal();
				{
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Add Score"))
					{
						_noteScore.items.Add(new NoteScoreItem());
					}
					GUILayout.FlexibleSpace();

				}
				GUILayout.EndHorizontal();
			}

			#region 마우스 이벤트 감지

			var mouseEvent = Event.current;
			var ctrlEvnet = Event.current.keyCode;

			if (Event.current.keyCode == KeyCode.LeftControl && Event.current.type == EventType.KeyDown)
				isClickedCtlr = true;
			else if (Event.current.keyCode == KeyCode.LeftControl && Event.current.type == EventType.KeyUp)
				isClickedCtlr = false;

			if (Event.current.type == EventType.ScrollWheel)
			{
				if (mouseEvent.delta.y > 0)
				{
					if (isClickedCtlr)
					{
						buttonStyle.fixedWidth -= NOTE_ZOME_INOUT_WEIGHT;
						topStatusStyle.fixedWidth -= NOTE_ZOME_INOUT_WEIGHT;
					}
					else
						scrollPosition2.x += SCROLLING_WEIGHT;
					Repaint();
				}
				else if (mouseEvent.delta.y < 0)
				{
					if (isClickedCtlr)
					{
						buttonStyle.fixedWidth += NOTE_ZOME_INOUT_WEIGHT;
						topStatusStyle.fixedWidth += NOTE_ZOME_INOUT_WEIGHT;
					}
					else
						scrollPosition2.x -= SCROLLING_WEIGHT;
					Repaint();
				}
			}

			#endregion

		}


	}

	void PlayScore()
	{
		watch.Start();
		
	}
	void DrawInstInfo(NoteScoreItem item)
	{
		GUILayout.BeginHorizontal();
		{
		EditorGUILayout.LabelField("악기",GUILayout.Height(30), GUILayout.Width(50));
		item.instrument = (GameObject)EditorGUILayout.ObjectField(item.instrument, typeof(GameObject), true, GUILayout.Height(30), GUILayout.Width(100));
		GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
	}

	void DrawScore(NoteScoreItem item)
	{
		int idxCount = 0;

		GUILayout.BeginHorizontal();
		{

			for (int i = 0; i < _noteScore.BarCountValue; i++)
			{
				for (int j = 0; j < _noteScore.NoteCountValue; j++) //마디
				{
					for (int k = 0; k < _buttonNumber; k++) //음표 박자 수 그리기
					{
						//노트 입력 여부 확인
						if (item.scoreDic.ContainsKey(idxCount))
						{
							GUI.backgroundColor = Color.yellow;
						}

						//버튼 입력 판정
						if (GUILayout.Button(EditorGUIUtility.IconContent("Animation.EventMarker"), buttonStyle))
						{
							if (item.scoreDic.ContainsKey(idxCount))
								item.scoreDic.Remove(idxCount);
							else
								item.scoreDic.Add(idxCount, 1); //1은 임시 값, 추후 연음? 그런 개념 넣을 것

							Repaint();
						}
						GUI.backgroundColor = Color.white;
						idxCount++;
					}
					GUILayout.Label(EditorGUIUtility.IconContent("d_Animation.EventMarker"));
				}
				GUILayout.Label("한마디끝");
			}
		}
		GUILayout.EndHorizontal();

	}

	private void Save()
	{
		var path = EditorUtility.SaveFilePanel("Score 데이터 저장", Application.dataPath, "BeatScore.asset", "asset");
		byte[] data = null;


		//버그 리포트 : 현재 project에 있는 Score을 인스턴스를 가지고 있을 때 저장하면 오류 발생
		if (string.IsNullOrEmpty(path) == false)
		{
			AssetDatabase.CreateAsset(_noteScore, path.Substring(path.IndexOf("Assets", StringComparison.Ordinal)));

			ShowNotification(new GUIContent("저장 성공!!!"), 3);
		}
	}

	private void Create()
	{
		_noteScore = ScriptableObject.CreateInstance<NoteScore>();
		foreach (DictionaryEntry entry in _noteScore.noteTable)
		{
			_dropDownOptionList.Add((string)entry.Key);
		}
		_dropDownOptionArray = _dropDownOptionList.ToArray();

		var path = EditorUtility.SaveFilePanel("Score 데이터 저장", Application.dataPath, "BeatScore.asset", "asset");
		byte[] data = null;


		//버그 리포트 : 현재 project에 있는 Score을 인스턴스를 가지고 있을 때 저장하면 오류 발생
		if (string.IsNullOrEmpty(path) == false)
		{
			AssetDatabase.CreateAsset(_noteScore, path.Substring(path.IndexOf("Assets", StringComparison.Ordinal)));

			ShowNotification(new GUIContent("저장 성공!!!"), 3);
		}
	}


	private void Load()
	{
		var path = EditorUtility.OpenFilePanel("악보 불러오기", Application.dataPath, "asset");

		if (string.IsNullOrEmpty(path) == false)
		{
			_noteScore = null;
			_noteScore = AssetDatabase.LoadAssetAtPath<NoteScore>(path.Substring(path.IndexOf("Assets", StringComparison.Ordinal)));
			if (_noteScore != null)
			{
				ShowNotification(new GUIContent($"{_noteScore.name}을 불러왔습니다."), 3);
			}

			//악보 넣을 시 한번 입력해줘야 됨. 이전에는 open 때 해서 문제가 없었는데 한번 고민 해봐야할 듯
			foreach (DictionaryEntry entry in _noteScore.noteTable)
			{
				_dropDownOptionList.Add((string)entry.Key);
			}
			_dropDownOptionArray = _dropDownOptionList.ToArray();

		}
		_buttonNumber = _noteScore.Scale();
		Repaint();
	}

	private void ConvertInstrumentObject()
	{
		List<Tuple<float, int>> instrumentBeatList = new List<Tuple<float, int>>();

		float btnTime = _noteScore.CaluBtnTime();
		foreach (var item in _noteScore.items)
		{
			instrumentBeatList.Clear();

			foreach (var entry in item.scoreDic)
			{
				instrumentBeatList.Add(Tuple.Create(entry.Key * btnTime, entry.Value));
			}

			instrumentBeatList.Sort();

			GameObject instrumentObject = Instantiate(item.instrument);
			instrumentObject.GetComponent<InstrumentController>().instrumentBeatList = instrumentBeatList.ToList();


			// 생성한 게임 오브젝트에 컴포넌트 추가

			//값 확인
			foreach (var VARIABLE in instrumentBeatList)
			{
				Debug.Log($"tuble <{VARIABLE.Item1},{VARIABLE.Item2}>");

			}

			//instrument 에 계산된 값 전달
			//item.instrument;
		}


	}
}