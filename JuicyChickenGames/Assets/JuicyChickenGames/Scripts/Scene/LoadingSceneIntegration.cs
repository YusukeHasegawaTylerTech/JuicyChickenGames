using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuicyChickenGames
{
	// Lets you press Play on any scene in the Editor and still go through your
	// preload/bootstrap flow: if the scene you hit Play on isn't build index 0,
	// this redirects to scene 0 and records which scene you meant to open in
	// `otherScene`. Have your preload scene's script read `LoadingSceneIntegration.otherScene`
	// once it's done initializing, and load that scene instead of falling through
	// to whatever the preload scene would normally load next.
	//
	// Requires your preload/bootstrap scene to be first (build index 0) in
	// File > Build Settings > Scenes In Build.
	public class LoadingSceneIntegration
	{
#if UNITY_EDITOR
		public static int otherScene = -2;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void InitLoadingScene()
		{
			int sceneIndex = SceneManager.GetActiveScene().buildIndex;
			if (sceneIndex == 0) return;

			otherScene = sceneIndex;
			SceneManager.LoadScene(0);
		}
#endif
	}
}
