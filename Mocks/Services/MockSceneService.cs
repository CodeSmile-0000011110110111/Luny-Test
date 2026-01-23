using Luny.Engine.Bridge;
using Luny.Engine.Services;
using System;
using System.Collections.Generic;

namespace Luny.Test
{
	public sealed class MockSceneService : LunySceneServiceBase, ILunySceneService
	{
		private MockLunyScene _currentScene;
		private List<ILunyObject> _sceneObjects = new();

		public void ReloadScene() => throw new NotImplementedException();

		public IReadOnlyList<ILunyObject> GetAllObjects() => _sceneObjects;
		public ILunyObject FindObjectByName(String name) => throw new NotImplementedException();

		public void AddSceneObject(MockLunyObject mockObject) => _sceneObjects.Add(mockObject);
		public Boolean RemoveSceneObject(MockLunyObject mockObject) => _sceneObjects.Remove(mockObject);

		protected override void OnServiceInitialize() =>
			_currentScene = new MockLunyScene(new MockNativeScene("MockScene", "MockAssets/MockScene.scene"));

		protected override void OnServiceStartup() => InvokeOnSceneLoaded(_currentScene);
	}
}
