using System;
using UnityEditor;
using UnityEngine;

namespace Obsidize.DependencyInjection.EditorTools
{

	public class InjectionTokenInspectorEditorWindow : EditorWindow
	{

		private static readonly Type _unityObjectType = typeof(UnityEngine.Object);

		[MenuItem("Window/Obsidize/Dependency Injection/Token Inspector")]
		public static InjectionTokenInspectorEditorWindow ShowWindow()
		{
			return GetWindow<InjectionTokenInspectorEditorWindow>("DI Token Inspector");
		}

		protected static bool IsUnityObjectType(Type type)
		{
			return type != null && (type.IsAssignableFrom(_unityObjectType) || type.IsSubclassOf(_unityObjectType));
		}

		protected static bool IsUnityObject(object value)
		{
			return value != null && IsUnityObjectType(value.GetType());
		}

		protected virtual Injector TargetInjector => Injector.Main;

		protected virtual void Awake()
		{
			TargetInjector.Registry.OnProviderAdd += HandleProviderAdd;
			TargetInjector.Registry.OnProviderRemove += HandleProviderRemove;

			foreach (var provider in TargetInjector.Registry.Providers)
			{
				provider.OnTokenChange += HandleProviderTokenChange;
			}
		}

		protected virtual void OnDestroy()
		{
			TargetInjector.Registry.OnProviderAdd -= HandleProviderAdd;
			TargetInjector.Registry.OnProviderRemove -= HandleProviderRemove;

			foreach (var provider in TargetInjector.Registry.Providers)
			{
				provider.OnTokenChange -= HandleProviderTokenChange;
			}
		}

		private void HandleProviderAdd(IInjectionTokenProvider provider)
		{
			provider.OnTokenChange += HandleProviderTokenChange;
			Repaint();
		}

		private void HandleProviderRemove(IInjectionTokenProvider provider)
		{
			provider.OnTokenChange -= HandleProviderTokenChange;
			Repaint();
		}

		private void HandleProviderTokenChange(IInjectionTokenProvider _)
		{
			Repaint();
		}

		protected virtual void OnGUI()
		{

			var providers = TargetInjector.Registry.Providers;

			if (providers.Count <= 0)
			{
				EditorGUILayout.LabelField("No active providers registered");
				return;
			}

			EditorGUILayout.LabelField("Active Providers", EditorStyles.boldLabel);

			var previousLabelWidth = EditorGUIUtility.labelWidth;
			var previousEnabledState = GUI.enabled;

			EditorGUIUtility.labelWidth = Mathf.Max(175f, position.width / 3f);
			GUI.enabled = false;

			foreach (var provider in providers)
			{
				RenderProviderState(provider);
			}

			EditorGUIUtility.labelWidth = previousLabelWidth;
			GUI.enabled = previousEnabledState;
		}

		protected virtual void RenderProviderState(IInjectionTokenProvider provider)
		{

			if (provider == null)
			{
				EditorGUILayout.LabelField("Missing provider reference");
				return;
			}

			var tokenValue = provider.DynamicTokenValue;
			var tokenType = provider.TokenType;
			var label = tokenType.Name;

			if (IsUnityObject(tokenValue))
			{
				EditorGUILayout.ObjectField(label, (UnityEngine.Object)tokenValue, tokenValue.GetType(), true);
			}
			else if (provider.HasToken)
			{
				EditorGUILayout.TextField(label, tokenValue != null ? tokenValue.ToString() : "null");
			}
			else
			{
				EditorGUILayout.TextField(label, "(No token assigned)");
			}
		}
	}
}