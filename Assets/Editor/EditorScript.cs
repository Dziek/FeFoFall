using UnityEditor;
using UnityEngine;
using System.Collections;

public class EditorScript : MonoBehaviour {
	
	//BuildOptions.ShowBuiltPlayer
	//BuildOptions.AutoRunPlayer
	
	// EDIT /////////////////////////////////////////////////////////////
	public static string GetName () {
		return "FeFoFall";
	}
	
	public static string[] GetScenes () {
		return new string[] {"Assets/Testing/CanvasTesting.unity"};
	}
	
	public static void PerformBuild() {
		BuildStandaloneWin();
		BuildWebGL();
		// BuildAndroid();
	}
	// TO HERE /////////////////////////////////////////////////////////////
	
    public static void BuildStandaloneWin()
    {      
        BuildPipeline.BuildPlayer(GetScenes(), "Builds/PC/" + GetName() + "StandaloneWin.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
	}
	
	public static void BuildWebGL()
    {          
        BuildPipeline.BuildPlayer(GetScenes(), "Builds/WebGL", BuildTarget.WebGL, BuildOptions.None);
	}
	
	public static void BuildAndroid()
    {          
        BuildPipeline.BuildPlayer(GetScenes(), "Builds/Android/" + GetName() + ".apk", BuildTarget.Android, BuildOptions.None);
	}
}
