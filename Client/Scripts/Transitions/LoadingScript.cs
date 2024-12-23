using System;
using Core.Client.Resource;
using Godot;

namespace Game.Scripts.Transitions;

using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class LoadingScript : Control
{
	private ProgressBar? _progressBar;
	
	private readonly List<Task> _loadingSteps = [];

	public override void _Ready()
	{
		_progressBar = GetNode<ProgressBar>("ProgressBar");
	}
	
	public void AddTask(Action task)
	{
		_loadingSteps.Add(new Task(task));
	}

	public async Task StartLoading(Action? onLoadingComplete = null)
	{
		int totalSteps = _loadingSteps.Count;
		int currentStep = 0;
		
		if (_progressBar == null)
		{
			GD.PrintErr("ProgressBar não encontrado!");
			return;
		}

		foreach (var step in _loadingSteps)
		{
			await step;
			
			currentStep++;
			_progressBar.Value = (currentStep / (float)totalSteps) * 100;
		}

		GD.Print("Carregamento concluído!");
		
		onLoadingComplete?.Invoke();
	}
}