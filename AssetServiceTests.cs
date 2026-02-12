using Luny.ContractTest.Mocks;
using Luny.Engine.Bridge;
using NUnit.Framework;

namespace Luny.Test
{
	[TestFixture]
	public class AssetServiceTests
	{
		private MockAssetService _assetService;
		private MockPathConverter _pathConverter;

		[SetUp]
		public void Setup()
		{
			_pathConverter = new MockPathConverter();
			LunyPath.Converter = _pathConverter;
			_assetService = new MockAssetService();
		}

		[TearDown]
		public void TearDown() => LunyPath.Converter = null;

		[Test]
		public void Path_Normalization_Works()
		{
			var path = LunyAssetPath.FromNative("Assets\\Resources\\Prefabs\\Player");
			Assert.That(path.AgnosticPath, Is.EqualTo("Prefabs/Player"));

			var path2 = LunyAssetPath.FromNative("res://Prefabs/Player");
			Assert.That(path2.AgnosticPath, Is.EqualTo("Prefabs/Player"));
		}

		[Test]
		public void Asset_TieredLookup_LunyFolder_Works()
		{
			// Setup mock asset in Tier 1 location
			var nativePath = "res://Luny/Prefab/Prefabs/Player.prefab";
			var assetPath = LunyAssetPath.FromNative(nativePath);
			var prefab = new MockPrefab(1, assetPath);
			_assetService.AddAsset(prefab);

			// Load by agnostic path
			var loaded = _assetService.Load<ILunyPrefab>("Prefabs/Player");

			Assert.That(loaded, Is.Not.Null);
			Assert.That(loaded.AssetPath.NativePath, Is.EqualTo(nativePath));
		}

		[Test]
		public void Asset_TieredLookup_RootFolder_Works()
		{
			// Setup mock asset in Tier 2 location
			var nativePath = "res://Prefabs/Player.prefab";
			var assetPath = LunyAssetPath.FromNative(nativePath);
			var prefab = new MockPrefab(1, assetPath);
			_assetService.AddAsset(prefab);

			// Load by agnostic path
			var loaded = _assetService.Load<ILunyPrefab>("Prefabs/Player");

			Assert.That(loaded, Is.Not.Null);
			Assert.That(loaded.AssetPath.NativePath, Is.EqualTo(nativePath));
		}

		[Test]
		public void Asset_Load_ReturnsPlaceholder_OnFailure()
		{
			var loaded = _assetService.Load<LunyPrefab>("NonExistent");

			Assert.That(loaded, Is.Not.Null);
			Assert.That(loaded, Is.InstanceOf<MockPrefab>());
			Assert.That(((MockPrefab)loaded).IsPlaceholder, Is.True);
		}

		[Test]
		public void Asset_Cache_Works()
		{
			var nativePath = "res://Prefabs/Player.prefab";
			var assetPath = LunyAssetPath.FromNative(nativePath);
			var prefab = new MockPrefab(1, assetPath);
			_assetService.AddAsset(prefab);

			var load1 = _assetService.Load<ILunyPrefab>("Prefabs/Player");
			var load2 = _assetService.Load<ILunyPrefab>("Prefabs/Player");

			Assert.That(load1, Is.SameAs(load2));
		}
	}
}
