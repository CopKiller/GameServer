using System;
using Godot;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Extensions;

namespace Game.Scripts.Transitions
{
    public partial class LoadingScript : Control
    {
        private TextureRect? _background;
        private ProgressBar? _progressBar;
        private Label? _lblTaskName;
        private Tween? _tween;

        private readonly List<(Func<Task>, string?)> _loadingSteps = [];

        public override void _Ready()
        {
            _background = GetNode<TextureRect>("%Background");
            _progressBar = GetNode<ProgressBar>("%ProgressBar");
            _lblTaskName = GetNode<Label>("%lblTaskName");
            
            TestAddTask();
        }

        public void AddTask(Func<Task> task, string? taskName = null)
        {
            _loadingSteps.Add((task, taskName));
        }

        public async void StartLoading(Action? onLoadingComplete = null)
        {
            await this.FadeIn();
            
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
                double targetValue = (currentStep / (float)totalSteps) * _progressBar.MaxValue;
                
                SmoothUpdateProgressBar(targetValue, 0.5f);
            }

            if (_lblTaskName != null)
                _lblTaskName.Text = "Carregamento concluído!";
            
            await this.FadeOut();

            onLoadingComplete?.Invoke();
        }

        private void SmoothUpdateProgressBar(double targetValue, float duration)
        {
            _tween = CreateTween();

            if (_progressBar == null || _tween == null) return;

            _tween.SetEase(Tween.EaseType.InOut);
            _tween.SetTrans(Tween.TransitionType.Cubic);
            _tween.TweenProperty(_progressBar, "value", targetValue, duration);
        }
        
        private void TestAddTask()
        {
            AddTask(async () =>
            {
                await Task.Delay(200);
            }, "Teste 1");

            AddTask(async () =>
            {
                await Task.Delay(100);
            }, "Teste 2");

            AddTask(async () =>
            {
                await Task.Delay(1000);
            }, "Teste 3");
            
            AddTask(async () =>
            {
                await Task.Delay(600);
            }, "Teste 4");
            
            AddTask(async () =>
            {
                await Task.Delay(300);
            }, "Teste 5");
            
            AddTask(async () =>
            {
                await Task.Delay(1000);
            }, "Teste 6");

            StartLoading(() =>
            {
                if (_lblTaskName != null)
                    _lblTaskName.Text = "Carregamento concluído!";
            });
        }
    }
}