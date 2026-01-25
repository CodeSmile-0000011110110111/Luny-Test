using Luny.Engine.Bridge;
using Luny.Engine.Services;
using System;
using System.Collections.Generic;

namespace Luny.Test
{
	public sealed class MockSceneService : LunySceneServiceBase, ILunySceneService
	{
		private MockLunyScene _currentScene;
		private Dictionary<String, Object> _nativeObjects = new();

		public void ReloadScene()
		{
			_nativeObjects.Clear();
			InvokeOnSceneUnloaded(_currentScene);

			_currentScene = new MockLunyScene(new MockNativeScene("MockScene", "MockAssets/MockScene.scene"));
			InvokeOnSceneLoaded(_currentScene);
		}

		public IReadOnlyList<ILunyObject> GetObjects(IReadOnlyList<String> objectNames)
		{
			if (objectNames == null || objectNames.Count == 0)
				return Array.Empty<ILunyObject>();

			var foundObjects = new ILunyObject[objectNames.Count];
			for (var i = 0; i < objectNames.Count; i++)
			{
				var name = objectNames[i];
				if (_nativeObjects.TryGetValue(name, out var nativeObject))
					foundObjects[i] = MockLunyObject.ToLunyObject((MockNativeObject)nativeObject);
			}
			return foundObjects;
		}

		public void AddNativeObject(MockNativeObject mockObject) => _nativeObjects.Add(mockObject.Name, mockObject);

		public Boolean RemoveNativeObject(MockNativeObject mockObject) => _nativeObjects.Remove(mockObject.Name);

		protected override void OnServiceInitialize() =>
			_currentScene = new MockLunyScene(new MockNativeScene("MockScene", "MockAssets/MockScene.scene"));

		protected override void OnServiceStartup() => InvokeOnSceneLoaded(_currentScene);
	}
}
