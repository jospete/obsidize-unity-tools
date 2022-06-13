using UnityEngine;

namespace Obsidize.DependencyInjection
{
	/// <summary>
	/// Base class for Monobehaviours that want to provide a token to the DI system.
	/// </summary>
	/// <typeparam name="T">The type of token that will be provided by this behaviour</typeparam>
	public abstract class InjectionTokenSource<T> : MonoBehaviour, IInjectionTokenSource
		where T : class
	{

		public System.Type TokenType => typeof(T);
		public IInjectionTokenProvider Provider => TargetProvider;
		protected virtual bool DestroyOnProvisionFailure => true;
		protected virtual bool LogProvisioningLifeCylce => false;
		protected virtual InjectionTokenProvider<T> TargetProvider => Injector.Main.GetProvider<T>();

		protected virtual T GetInjectionTokenValue() => this as T;

		protected virtual void Awake()
		{
			ProvideInjectionToken();
		}

		protected virtual void OnDestroy()
		{
			DisposeInjectionToken();
		}

		protected virtual void OnTokenProvisionSuccess(InjectionToken<T> token)
		{
			if (LogProvisioningLifeCylce)
			{
				Debug.Log($"Successfully provisioned token -> {token}");
			}
		}

		protected virtual void OnTokenProvisionFailure(InjectionToken<T> token)
		{

			if (!DestroyOnProvisionFailure)
			{
				return;
			}

			if (LogProvisioningLifeCylce)
			{
				Debug.LogWarning($"Destroying {this} due to provision failure");
			}

			Destroy(gameObject);
		}

		protected void DisposeInjectionToken()
		{
			TargetProvider.DisposeCurrentToken();
		}

		protected void ProvideInjectionToken()
		{

			var token = new InjectionToken<T>(GetInjectionTokenValue);
			var provisioned = TargetProvider.Provide(token);

			if (provisioned)
			{
				OnTokenProvisionSuccess(token);
			}
			else
			{
				OnTokenProvisionFailure(token);
			}
		}
	}
}
