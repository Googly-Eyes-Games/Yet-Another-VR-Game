using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionsSceneManger : ScriptableObject
{
	private static TransitionsSceneManger instance;

	[Foldout("Transition")]
	[SerializeField]
	private GameObject transitionPrefab;

	[Foldout("Transition")]
	[SerializeField]
	private float transitionTime;

	public SceneManagerState State { get; private set; }

	private SceneTransition sceneTransitionInternal;
	private SceneTransition SceneTransition
	{
		get
		{
			if (sceneTransitionInternal)
				return sceneTransitionInternal;
			
			GameObject loadTransition = Instantiate(instance.transitionPrefab, Camera.main.transform);
			loadTransition.transform.localPosition = Vector3.forward * (Camera.main.nearClipPlane * 2f);
			
			sceneTransitionInternal = loadTransition.GetComponent<SceneTransition>();
			
			return sceneTransitionInternal;
		}
	}
	
	// ReSharper disable Unity.PerformanceAnalysis
	public static TransitionsSceneManger Get()
	{
		if (!instance)
		{
			instance = Resources.Load<TransitionsSceneManger>("SceneManagerConfig_SO");
		}

		return instance;
	}
	
	public void LoadLevel()
	{
		LoadScene(GameplaySettings.Global.LevelScene);
	}
	
	public void LoadMenu()
	{
		LoadScene(GameplaySettings.Global.MenuScene);
	}

	public void LoadGameOver()
	{
		LoadScene(GameplaySettings.Global.GameOverScene);
	}

	public async void LoadScene(string sceneName)
	{
		if (State != SceneManagerState.Ready)
			return;
		
		Time.timeScale = 1f;

		AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);
		loadingOperation.allowSceneActivation = false;
		
		State = SceneManagerState.SceneTransition;
		await PlaySceneUnloadTransition();

		loadingOperation.allowSceneActivation = true;
		
		State = SceneManagerState.LoadingScene;
		while (!loadingOperation.isDone)
		{
			await Task.Delay(1);
		}
		
		State = SceneManagerState.SceneTransition;
		await PlaySceneLoadTransition();

		State = SceneManagerState.Ready;
	}

	private async Task PlaySceneLoadTransition()
	{
		await SceneTransition.FadeOut(transitionTime);
	}

	private async Task PlaySceneUnloadTransition()
	{
		await SceneTransition.FadeIn(transitionTime);
	}
	
	public enum SceneManagerState
	{
		Ready,
		SceneTransition,
		LoadingScene,
	}	
}