using System;
using UnityEditor;
using UnityEngine;

namespace JuicyChickenGames.Editor
{
	public static class AssetUtilities
	{
		public static T EnsureAssetExists<T>(string path) where T : ScriptableObject
		{
			T asset = AssetDatabase.LoadAssetAtPath<T>(path);

			if (asset == null)
			{
				// If the asset doesn't exist, create a new instance of type T
				asset = ScriptableObject.CreateInstance<T>();

				// Create the asset at the specified path
				AssetDatabase.CreateAsset(asset, path);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}

			return asset;
		}

		private static string SanitizeFileName(UnityEngine.Object asset)
		{
			var invalids = System.IO.Path.GetInvalidFileNameChars();
			string origFileName = asset.name;
			var newName = String.Join("_", origFileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
			return newName;
		}
	}
}
