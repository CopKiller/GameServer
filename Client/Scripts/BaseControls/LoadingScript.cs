using System;
using System.Threading.Tasks;
using Game.Scripts.Extensions.Attributes;
using Godot;

namespace Game.Scripts.BaseControls
{
    [ScenePath("res://Client/Scenes/Transitions/LoadingScene.tscn")]
    public partial class LoadingScript : Control
    {
        private TextureRect? _background;
        private ProgressBar? _progressBar;
        private Label? _lblTaskName;
        private Tween? _tween;

        public override void _Ready()
        {
            GD.Print("LoadingScript ready!");
            
            _background = GetNode<TextureRect>("%Background");
            _progressBar = GetNode<ProgressBar>("%ProgressBar");
            _lblTaskName = GetNode<Label>("%lblTaskName");

            if (_background != null && _progressBar != null && _lblTaskName != null) return;
            
            GD.PrintErr("Componentes obrigatórios não foram encontrados!");
        }
        
        public void UpdateMessage(string message)
        {
            if (_lblTaskName == null)
            {
                GD.PrintErr("Label para TaskName não foi encontrada.");
                return;
            }

            _lblTaskName.Text = message;
        }

        public async Task SmoothUpdateProgressBar(double targetValue, float duration)
        {
            if (_progressBar == null)
            {
                GD.PrintErr("ProgressBar não foi encontrada.");
                return;
            }

            _tween = CreateTween();

            if (_tween == null)
            {
                GD.PrintErr("Falha ao criar Tween.");
                return;
            }

            try
            {
                _tween.SetEase(Tween.EaseType.InOut);
                _tween.SetTrans(Tween.TransitionType.Cubic);
                _tween.TweenProperty(_progressBar, "value", targetValue * _progressBar.MaxValue, duration);
                await ToSignal(_tween, Tween.SignalName.Finished);
            }
            catch (Exception e)
            {
                GD.PrintErr($"Erro ao configurar Tween: {e.Message}");
            }
        }
    }
}