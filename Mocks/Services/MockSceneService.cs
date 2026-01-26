using Luny.Engine.Bridge;
using Luny.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Luny.Test
{
	public sealed class MockSceneService : LunySceneServiceBase, ILunySceneService
	{
		private MockLunyScene _currentScene;
		private Dictionary<String, MockNativeObject> _nativeObjects = new();

		public void ReloadScene()
		{
			_nativeObjects.Clear();
			InvokeOnSceneUnloaded(_currentScene);

			_currentScene = new MockLunyScene(new MockNativeScene("MockScene", "MockAssets/MockScene.scene"));
			InvokeOnSceneLoaded(_currentScene);
		}

		public IReadOnlyList<ILunyObject> GetObjects(IReadOnlyCollection<String> objectNames)
		{
			if (objectNames == null || objectNames.Count == 0)
				return Array.Empty<ILunyObject>();

			var foundObjects = new List<ILunyObject>();
			foreach (var nativeObject in _nativeObjects.Values)
			{
				if (objectNames.Contains(nativeObject.Name))
					foundObjects.Add(MockLunyObject.ToLunyObject(nativeObject));
			}

			return foundObjects.AsReadOnly();
		}

		public ILunyObject FindObjectByName(String name) => throw new NotImplementedException();

		public void AddNativeObject(MockNativeObject mockObject) => _nativeObjects.Add(mockObject.Name, mockObject);

		public Boolean RemoveNativeObject(MockNativeObject mockObject) => _nativeObjects.Remove(mockObject.Name);

		protected override void OnServiceInitialize() =>
			_currentScene = new MockLunyScene(new MockNativeScene("MockScene", "MockAssets/MockScene.scene"));

		protected override void OnServiceStartup() => InvokeOnSceneLoaded(_currentScene);
	}
}
