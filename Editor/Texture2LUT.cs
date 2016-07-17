using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

namespace Sigtrap.T2LUT {
	public class Texture2LUT : EditorWindow {
		Texture2D _palette;
		bool _pValid = true;
		string _path;
		string[] _exts = new string[]{".png",".jpg"};
		int _extIndex = 0;
		int[] _ress = new int[]{8,16,32};
		string[] _resNames;
		int _resIndex = 1;

		long _lastGenTime = -1;
		string _lastGenPath = "";
		int _lastGenRes = -1;
		string _lastTexPath = "";
		int _lastTexX = -1;
		int _lastTexY = -1;

		[MenuItem("Window/Texture2LUT")]
		public static void ShowWindow(){
			EditorWindow.GetWindow<Texture2LUT>();
		}

		double _lastUpdateTime = 0;
		void Update(){
			// Check (once per second) if palette can be read... using stupid try/catch as there's no exposed property.
			if (EditorApplication.timeSinceStartup - _lastUpdateTime > 1f){
				_lastUpdateTime = EditorApplication.timeSinceStartup;
				if (_palette != null){
					_pValid = true;
					try {
						_palette.GetPixel(0,0);
					} catch {
						_pValid = false;
					}
				}
				Repaint();
			}
		}
		void OnEnable(){
			titleContent = new GUIContent("Texture To LUT");
			_resNames = new string[_ress.Length];
			for (int i=0; i<_resNames.Length; ++i){
				_resNames[i] = string.Format("[{0} x {1}]", _ress[i]*_ress[i], _ress[i]);
			}
		}

		void OnGUI(){
			EditorGUILayout.Space();
			if (_lastGenTime > 0){
				EditorGUILayout.HelpBox(string.Format(
					"LUT {0} [{1} x {2}]\nGenerated in {3}s\nFrom {4} [{5} x {6}]",
					"Assets/" + _lastGenPath, 
					(_lastGenRes * _lastGenRes).ToString(), _lastGenRes.ToString(),
					(_lastGenTime/1000).ToString(), _lastTexPath, _lastTexX.ToString(), _lastTexY.ToString()
				), MessageType.Info);
			} else {
				EditorGUILayout.HelpBox("Generation can take a long time!\n   Avoid large input textures\n   Avoid output higher than 256 x 16", MessageType.Warning);
			}
			EditorGUILayout.Space();

			_palette = EditorGUILayout.ObjectField("Palette", _palette, typeof(Texture2D), false) as Texture2D;
			if (!_pValid){
				++EditorGUI.indentLevel;
				EditorGUILayout.HelpBox("Texture read/write must be enabled in import settings!", MessageType.Error);
				--EditorGUI.indentLevel;
				EditorGUILayout.Space();
			}

			_path = EditorGUILayout.TextField(new GUIContent("Output Path", "Relative to Assets, without extension"), _path);
			_extIndex = EditorGUILayout.Popup("Format", _extIndex, _exts);
			_resIndex = EditorGUILayout.Popup("Resolution", _resIndex, _resNames);
			int res = _ress[_resIndex];

			Color gc = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);
			EditorGUILayout.Space();
			if (_palette != null && _pValid && GUILayout.Button("\nGENERATE\n")){
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				sw.Start();

				Texture2D _lut = new Texture2D(res*res,res,TextureFormat.RGB24,false);
				_lut.filterMode = FilterMode.Point;

				Color[] ppx = _palette.GetPixels();
				Color[] lpx = new Color[res*res*res];
				for (int b=0; b<res; ++b){
					for (int g=0; g<res; ++g){
						for (int r=0; r<res; ++r){
							// Scan palette image for closest match and store
							float lowestDelta = 999;
							Color result = Color.clear;
							float lr = ((float)r/(float)res);
							float lg = ((float)g/(float)res);
							float lb = ((float)b/(float)res);
							foreach (Color c in ppx){
								float delta = Mathf.Abs(c.r - lr) + Mathf.Abs(c.g - lg) + Mathf.Abs(c.b - lb);
								if (delta < lowestDelta){
									lowestDelta = delta;
									result = c;
								}
							}
							lpx[(((res-1)-g)*res*res) + (b*res) + r] = result;
						}
					}
				}
				_lut.SetPixels(lpx);
				_lut.Apply();

				string path = _path + _exts[_extIndex];
				byte[] output = null;
				switch (_extIndex){
				case 0:
					output = _lut.EncodeToPNG();
					break;
				case 1:
					output = _lut.EncodeToJPG();
					break;
				}
				File.WriteAllBytes(Application.dataPath + "/" + path, output);

				sw.Stop();

				_lastGenTime = sw.ElapsedMilliseconds;
				_lastGenPath = path;
				_lastGenRes = _ress[_resIndex];
				_lastTexPath = AssetDatabase.GetAssetPath(_palette);
				_lastTexX = _palette.width;
				_lastTexY = _palette.height;

				AssetDatabase.Refresh();
			}
			GUI.backgroundColor = gc;
		}
	}
}