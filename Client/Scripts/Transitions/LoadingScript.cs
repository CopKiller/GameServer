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
	private Label? _lblTaskName;
	
	private readonly List<(Func<Task>, string?)> _loadingSteps = [];

	public override void _Ready()
	{
		_progressBar = GetNode<ProgressBar>("%ProgressBar");
		_lblTaskName = GetNode<Label>("%lblTaskName");
	}
	
	public void AddTask(Func<Task> task, string? taskName = null)
	{
		_loadingSteps.Add((task, taskName));
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
			if (_lblTaskName != null)
				_lblTaskName.Text = step.Item2;
			
			try
			{
				await step.Item1();
			}
			catch (Exception ex)
			{
				GD.PrintErr($"Erro ao executar tarefa: {step.Item2}. Exception: {ex.Message}");
			}

			currentStep++;
			_progressBar.Value = (currentStep / (float)totalSteps) * _progressBar.MaxValue;
		}
		
		if (_lblTaskName != null)
			_lblTaskName.Text = "Carregamento concluído!";
		
		onLoadingComplete?.Invoke();
	}
}