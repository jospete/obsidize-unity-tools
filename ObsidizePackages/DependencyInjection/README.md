# Obsidize Dependency Injection

Utilities for enhanced unity app control flow via DI tokens.

**Note**: this is an over-engineered solution meant for projects with many sub-systems that
need to interact with each other. If you're working on a small project with 10 or less systems,
you probably don't need this.

The main benefits of this module are:

1. Faster than traditional "search the scene" methods (by alot) when looking for a shared object
2. Duplicate instance disambiguation / handling out-the-box
3. Better alternative to the singleton pattern (in my opinion)
4. Elimination of circular assembly definition reference errors (when used with bridge interface tokens)
5. Elimination of dependency creation racing conditions (i.e. module A needs module B, but module A was created before module B)
6. Allows for runtime token reference hot-swapping (i.e. overwriting a token value will update all watchers)
7. Independent sub-system operation - systems don't know when (or if) a token will be provided, and must operate in isolation with this in mind (their default behaviour should be to do "nothing" instead of explode on reference error)
8. Allows for true prefab isolation by eliminating shared scene references from the inspector

TL;DR:

```csharp

// Bad - searches entire scene heirarchy
CustomScript scriptRef = FindObjectOfType<CustomScript>();

// Good - direct reference to explicitly provided instance 
CustomScript scriptRef = Injector.Main.Get<CustomScript>();

// Better - also allows for interface-based povisioning to further decouple modules
ICustomBehaviour interfaceRef = Injector.Main.Get<ICustomBehaviour>();
```

## Installation

This git repo is directly installable via the unity package manager.
Simply paste the repo url into the package manager "add" menu and unity will do the rest.

## Usage

The below examples are pseudo-code for core concepts.

See the project samples for working code examples.

### Token Consumer Usage

For behaviours that want to use DI to get references, the easiest way is with an ```BehaviourInjectionContext``` instance:

```csharp
using Obsidize.DependencyInjection;
using UnityEngine;

public class Consumer : MonoBehaviour
{
	
	private BehaviourInjectionContext _injectionContext;
	private TokenAType _tokenA;
	private TokenBType _tokenB;
	private TokenCType _tokenC;
	private TokenDType _tokenD;
	
	private void Awake()
	{
		_injectionContext = new BehaviourInjectionContext(this)
			.Inject<TokenAType>(v => _tokenA = v)
			.Inject<TokenBType>(v => _tokenB = v, 10f) // can also pass a custom max-wait-time before the DI system will complain
			.InjectOptional<TokenCType>(v => _tokenC = v) // will not complain if no token is provided
			.Inject<TokenDType>(OnUpdateTokenD)
			.Inject<TokenDType>(OnUpdateTokenD); // !!! ERROR !!! - TokenDType already being consumed by this context (from previous line)
	}
	
	private void OnDestroy()
	{
		// Be sure to dispose the context when you're done with it to avoid memory leaks
		_injectionContext.Dispose();
	}
	
	private void OnUpdateTokenD(TokenDType value)
	{
		_tokenD = value;
		// do other stuff now that _tokenD is updated...
	}
}
```

### Token Source Usage

For custom non-behaviour tokens, extend ```InjectionTokenSource```:

```csharp
using Obsidize.DependencyInjection;
using UnityEngine;

public interface MyCustomTokenType
{
	int SomeData { get; }
	void DoTheThing();
}

[DisallowMultipleComponent]
public class CustomTokenProvider : InjectionTokenSource<MyCustomTokenType>, MyCustomTokenType
{

	[SerializeField] private int _someData;

	public int SomeData => _someData;
	
	// Optional override - defaults to casting the sub-class as the token
	protected override MyCustomTokenType GetInjectionTokenValue() => this;

	public void DoTheThing()
	{
		Debug.Log("Did the thing");
	}
}
```

If you want to provide an existing behaviour as a token, extend ```SiblingComponentInjectionTokenSource```
and add this behaviour to the GameObject containing the original behaviour:

```csharp
using Obsidize.DependencyInjection;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerHealth 
{

	[SerializeField] private int _value;
	
	public int Value => _value;	
}

[DisallowMultipleComponent]
public class PlayerHealthProvider : SiblingComponentInjectionTokenSource<PlayerHealth>
{
}
```

### (Advanced) Token Source Module Usage

Token source modules are best used for cases where the same set of token sources are required across multiple scenes.

A token source module will take in one or more prefabs of ```InjectionTokenSource<T>``` components via the ```Provide()``` method,
and instantiate the prefab when the DI system detects a request for that specific token type.

**NOTE:** - Do not glob all of your token source prefabs into one giant module script - nothing will be instantiated this way.
At least _one_ DI consumer must be active in the current scene to send requests to the DI system, which will trigger
prefab instantiation.

```csharp
using Obsidize.DependencyInjection;
using UnityEngine;

[DisallowMultipleComponent]
public class FeatureInjectionModule : MonoBehaviour
{
	
	private InjectionTokenSourceModuleContext _moduleContext;
	
	// NOTE: all provided items must inherit from InjectionTokenSource<T>
	[SerializeField] private BulletPool _bulletPoolPrefab;
	[SerializeField] private Player _playerPrefab;
	[SerializeField] private ModalDialog _modalDialogPrefab;
	
	private void Awake()
	{
		// NOTE: Prefabs are lazily instantiated, and will not be spawned
		// unless they are explicitly asked for via some DI consumer
		// ( such as BehaviourInjectionContext.Inject<T>() ).
		//
		// NOTE: At least one injection context must be active in the scene to start
		// this "chain reaction" of instantiation - if nothing asks the DI system for
		// a token, nothing will be instantiated.
		_moduleContext = new InjectionTokenSourceModuleContext()
			.Provide(_bulletPoolPrefab)
			.Provide(_playerPrefab)
			.Provide(_modalDialogPrefab);
	}
	
	private void OnDestroy()
	{
		// Be sure to clean up resources when this module is done
		_moduleContext.Dispose();
	}
}
```