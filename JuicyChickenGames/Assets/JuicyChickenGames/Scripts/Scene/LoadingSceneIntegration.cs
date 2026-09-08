using UnityEngine;
using UnityEngine.SceneManagement;

namespace JuicyChickenGames
{
	// Lets you press Play on any scene in the Editor and still go through your
	// preload/bootstrap flow: if the scene you hit Play on isn't build index 0,
	// this redirects to scene 0 and remembers which scene you meant to open.
	// Call TryLoadOtherScene() from your preload scene once it's done
	// initializing to load that scene instead of whatever it would load next.
	//
	// Requires your preload/bootstrap scene to be first (build index 0) in
	// File > Build Settings > Scenes In Build. Editor-only - a no-op in
	// builds, where build index 0 is already what boots.
	public static class LoadingSceneIntegration
	{
#if UNITY_EDITOR
		private const int NoOtherScene = -1;
		private static int otherScene = NoOtherScene;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void InitLoadingScene()
		{
			// Reset unconditionally: with Domain Reload disabled (Enter Play
			// Mode Options), static fields survive between Play sessions, so a
			// stale value from a previous session must not leak into this one.
			otherScene = NoOtherScene;

			int sceneIndex = SceneManager.GetActiveScene().buildIndex;
			// <= 0 covers both "already the preload scene" (0) and "not yet
			// added to Build Settings" (-1, which can't be loaded back to).
			if (sceneIndex <= 0) return;

			otherScene = sceneIndex;
			SceneManager.LoadScene(0);
		}
#endif

		// Call once your preload scene has finished initializing. Loads the
		// scene that redirected here and clears the stored value so it can't
		// be consumed twice. Returns false (and does nothing) if there's no
		// scene to return to, or outside the Editor.
		public static bool TryLoadOtherScene()
		{
#if UNITY_EDITOR
			if (otherScene == NoOtherScene)
			{
				return false;
			}

			int sceneToLoad = otherScene;
			otherScene = NoOtherScene;
			SceneManager.LoadScene(sceneToLoad);
			return true;
#else
			return false;
#endif
		}
	}
}
