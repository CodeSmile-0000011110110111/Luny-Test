using Luny.Engine.Bridge;
using Luny.Engine.Services;
using NUnit.Framework;
using System;

namespace Luny.Test.Engine
{
	/// <summary>
	/// Concrete test service to test LunyInputServiceBase behavior directly.
	/// </summary>
	internal sealed class TestInputService : LunyInputServiceBase, ILunyInputService
	{
		public void SimulatePostUpdate() => OnServicePostUpdate();
	}

	[TestFixture]
	public sealed class LunyInputServiceTests
	{
		private TestInputService _service;

		[SetUp]
		public void SetUp() => _service = new TestInputService();

		[Test]
		public void GetAxisValue_Unknown_Action_Returns_Default() =>
			Assert.That(_service.GetDirection("Unknown"), Is.EqualTo(default(LunyVector2)));

		[Test]
		public void GetAxisValue_Returns_Last_Known_Value()
		{
			var vec = new LunyVector2(0.5f, -0.3f);
			_service.SimulateDirectionalInput("Move", vec);

			Assert.That(_service.GetDirection("Move"), Is.EqualTo(vec));
		}

		[Test]
		public void GetAxisValue_NotCleared_On_PostUpdate()
		{
			var vec = new LunyVector2(1f, 0f);
			_service.SimulateDirectionalInput("Move", vec);
			_service.SimulatePostUpdate();

			Assert.That(_service.GetDirection("Move"), Is.EqualTo(vec));
		}

		[Test]
		public void GetButtonPressed_Returns_True_While_Held()
		{
			_service.SimulateButtonInput("Fire", true);

			Assert.That(_service.GetButtonPressed("Fire"), Is.True);
		}

		[Test]
		public void GetButtonPressed_Returns_False_After_Release()
		{
			_service.SimulateButtonInput("Fire", true);
			_service.SimulateButtonInput("Fire", false);

			Assert.That(_service.GetButtonPressed("Fire"), Is.False);
		}

		[Test]
		public void GetButtonJustPressed_True_On_Press_Frame()
		{
			_service.SimulateButtonInput("Jump", true);

			Assert.That(_service.GetButtonJustPressed("Jump"), Is.True);
		}

		[Test]
		public void GetButtonJustPressed_False_After_PreUpdate()
		{
			_service.SimulateButtonInput("Jump", true);
			_service.SimulatePostUpdate();

			Assert.That(_service.GetButtonJustPressed("Jump"), Is.False);
		}

		[Test]
		public void GetButtonJustPressed_False_On_Sustained_Hold_After_Reset()
		{
			_service.SimulateButtonInput("Jump", true);
			_service.SimulatePostUpdate();
			_service.SimulateButtonInput("Jump", true);

			// After PreUpdate, state resets, so the next press is treated as a new press
			Assert.That(_service.GetButtonJustPressed("Jump"), Is.False);
			Assert.That(_service.GetButtonPressed("Jump"), Is.True);
		}

		[Test]
		public void GetButtonJustPressed_True_After_Release_And_Repress()
		{
			_service.SimulateButtonInput("Jump", true);
			_service.SimulatePostUpdate();
			_service.SimulateButtonInput("Jump", false);
			_service.SimulatePostUpdate();
			_service.SimulateButtonInput("Jump", true);

			Assert.That(_service.GetButtonJustPressed("Jump"), Is.True);
		}

		[Test]
		public void GetButtonStrength_Returns_Analog_Value()
		{
			_service.SimulateButtonInput("Trigger", true, 0.75f);

			Assert.That(_service.GetButtonStrength("Trigger"), Is.EqualTo(0.75f));
		}

		[Test]
		public void GetButtonStrength_Returns_Zero_After_Release()
		{
			_service.SimulateButtonInput("Trigger", true, 0.75f);
			_service.SimulateButtonInput("Trigger", false);

			Assert.That(_service.GetButtonStrength("Trigger"), Is.EqualTo(0f));
		}

		[Test]
		public void GetButtonValue_Unknown_Action_Returns_Zero() => Assert.That(_service.GetAxis("Unknown"), Is.EqualTo(0f));

		[Test]
		public void OnInputAction_Event_Fires_For_Axis()
		{
			LunyInputEvent received = default;
			_service.OnInputAction += e => received = e;

			_service.SimulateDirectionalInput("Move", new LunyVector2(1f, 0f));

			Assert.That(received.ActionName, Is.EqualTo("Move"));
			Assert.That(received.ActionType, Is.EqualTo(LunyInputActionType.Directional));
			Assert.That(received.Direction, Is.EqualTo(new LunyVector2(1f, 0f)));
		}

		[Test]
		public void OnInputAction_Event_Fires_For_Button()
		{
			LunyInputEvent received = default;
			_service.OnInputAction += e => received = e;

			_service.SimulateButtonInput("Fire", true);

			Assert.That(received.ActionName, Is.EqualTo("Fire"));
			Assert.That(received.ActionType, Is.EqualTo(LunyInputActionType.Button));
			Assert.That(received.IsPressed, Is.True);
			Assert.That(received.IsJustPressed, Is.True);
		}

		[Test]
		public void OnInputAction_Event_JustPressed_False_On_Sustained()
		{
			_service.SimulateButtonInput("Fire", true);

			LunyInputEvent received = default;
			_service.OnInputAction += e => received = e;

			_service.SimulateButtonInput("Fire", true);

			Assert.That(received.IsPressed, Is.True);
			Assert.That(received.IsJustPressed, Is.False);
		}
	}
}
