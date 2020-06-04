//http://doctrina-kharkov.blogspot.com/
using UnityEngine;
using UnityEditor;

public class TerrainTexturBlur : EditorWindow
{
	Terrain ter;
	int blurRadius=1;

	[MenuItem("Doctrina/Terrain Texturing blur")]
	public static void ShowWindow ()
	{
		//Создать инстанс окна
		EditorWindow.GetWindow (typeof(TerrainTexturBlur));
	}
	
	//Обрабатываем код окна
	void OnGUI ()
	{
		//Выводим текст
		GUILayout.Label ("Blur terrain texturing");

        GUILayout.Label ("Terrain:");
		ter = (Terrain) EditorGUILayout.ObjectField (ter, typeof(Terrain), true);

		if (ter!=null) {
			

			blurRadius = EditorGUILayout.IntField ("Blur radius", blurRadius);
			blurRadius = Mathf.Clamp (blurRadius, 1, int.MaxValue);

			if (GUILayout.Button ("Blur splatmap")) {

				TerrainData terData = ter.terrainData;

				float[,,] splatMapData = terData.GetAlphamaps(0, 0, terData.alphamapWidth, terData.alphamapHeight);

				BlurSplatMap( splatMapData );

				terData.SetAlphamaps(0, 0, splatMapData);

			}
		}

	}

	void BlurSplatMap( float[,,] splats ){

		for (int y = 0; y < splats.GetLength(0); y++) {
			for (int x = 0; x < splats.GetLength(1); x++) {

				//neighbours
				float[] c = new float[splats.GetLength(2)];
				float[] cr = new float[splats.GetLength(2)];
				float[] cl = new float[splats.GetLength(2)];
				float[] cu = new float[splats.GetLength(2)];
				float[] cd = new float[splats.GetLength(2)];

				for (int i = 0; i < c.Length; i++) {
					c [i] = splats [y, x, i];
				}

				for (int i = 0; i < cr.Length; i++) {
					cr [i] = splats [y, Mathf.Clamp( x+blurRadius, 0, splats.GetLength(1)-1), i];
				}

				for (int i = 0; i < cl.Length; i++) {
					cl [i] = splats [y, Mathf.Clamp( x-blurRadius, 0, splats.GetLength(1)-1), i];
				}

				for (int i = 0; i < cu.Length; i++) {
					cu [i] = splats [Mathf.Clamp( y-blurRadius, 0, splats.GetLength(0)-1), x, i];
				}

				for (int i = 0; i < cd.Length; i++) {
					cd [i] = splats [Mathf.Clamp( y+blurRadius, 0, splats.GetLength(0)-1), x, i];
				}

				for (int i = 0; i < c.Length; i++) {
					c [i] = ( c [i] + cr [i] + cl [i] + cu [i] + cd [i] ) / 5;
				}

				for (int i = 0; i < c.Length; i++) {
					splats [y, x, i] = c [i];
				}

			}
		}
			

	}

}


