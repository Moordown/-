//http://doctrina-kharkov.blogspot.com/
using UnityEngine;
using UnityEditor;

public class TerrainTexturer : EditorWindow{
	
	Terrain ter;
	float slope=0.7f;

	public Vector2[] heightDistr; //public according to SerializedProperty display

	[MenuItem("Doctrina/Terrain Texturer")]
	public static void ShowWindow (){
		//Создать инстанс окна
		EditorWindow.GetWindow (typeof(TerrainTexturer));
	}
	
	//Обрабатываем код окна
	void OnGUI (){
		
		//Выводим текст
		GUILayout.Label ("Procedurally texture terrain");

        GUILayout.Label ("Terrain:");
		ter = (Terrain) EditorGUILayout.ObjectField (ter, typeof(Terrain), true);

		if (ter!=null) {

			slope = EditorGUILayout.FloatField ("Slope(y normalized):", slope);
			slope = Mathf.Clamp01 (slope);

			TerrainData terData = ter.terrainData;

			//
			if (heightDistr == null) {
				heightDistr = new Vector2[terData.alphamapLayers-1]; //-1 because of slope texture is not depending on height
				FillDistrubution( heightDistr );
			} else if(heightDistr.Length != terData.alphamapLayers-1 ){
				heightDistr = new Vector2[terData.alphamapLayers-1];
				FillDistrubution (heightDistr);
			}

			DisplayArray ("heightDistr");
		
			if (GUILayout.Button("Texturize")) {

				int swidth = terData.alphamapResolution;

				float[,,] splatMapData = terData.GetAlphamaps(0, 0, swidth, swidth);

				//Find actual maxHeight
				float maxHeight = 0.001f; //prevent zero div
				for (int y = 0; y < swidth; y++) {
					for (int x = 0; x < swidth; x++) {
						if (terData.GetInterpolatedHeight ((float)x / swidth, (float)y / swidth) > maxHeight) {
							maxHeight = terData.GetInterpolatedHeight ((float)x / swidth, (float)y / swidth);
						}
					}
				}

				for (int y = 0; y < swidth; y++) {
					for (int x = 0; x < swidth; x++) {

						Vector3 nrm = terData.GetInterpolatedNormal ( (float) x / swidth, (float) y / swidth ); 					

						int splats = terData.alphamapTextures.Length;

						if (nrm.y < slope ) {
							SetSplatValue (splatMapData, y, x, 0);
						} else {

							//Texturize by height
							float h = terData.GetInterpolatedHeight( (float) x / swidth, (float) y / swidth );
							float nh = h / maxHeight;

							for (int i = 0; i < heightDistr.Length; i++) {
								if (nh >= heightDistr [i].x && nh <= heightDistr [i].y) {	
									SetSplatValue (splatMapData, y, x, i+1);
								}
							}
						}
					}	
				}
					
				//TODO make button to make height data correct for textures

				//
				terData.SetAlphamaps(0, 0, splatMapData);

			}
				
		}

	}

	void SetSplatValue( float[,,] splats, int y, int x, int splat ){
		for (int i = 0; i < splats.GetLength(2); i++) {
			if (i == splat) {
				splats [y, x, i] = 1;
			} else {
				splats [y, x, i] = 0;
			}
		}
	}

	void DisplayArray(string propertyName){
	      
		ScriptableObject target = this;
		SerializedObject so = new SerializedObject(target);
		SerializedProperty stringsProperty = so.FindProperty(propertyName);

		EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
		so.ApplyModifiedProperties(); 

	}

	void FillDistrubution(Vector2[] distrubution){
		float step = 1f / distrubution.Length;
		for (int i = 0; i < distrubution.Length; i++) {
			distrubution [i].x = i * step;
			distrubution [i].y = (i+1) * step;
		}
	}

}


