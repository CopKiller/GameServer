using System;
using Core.Database.Constants;
using Godot;

namespace Game.Scripts.BaseControls;

public partial class DatePicker : WindowBase
{
    private OptionButton _yearOptionButton;
    private OptionButton _monthOptionButton;
    private OptionButton _dayOptionButton;
    private Button _confirmButton;

    [Signal]
    public delegate void DateSelectedEventHandler(string dateString);

    private int _startYear = CharactersLength.MinYear;
    private int _endYear = CharactersLength.MaxYear;

    public override void _Ready()
    {
        _yearOptionButton = GetNode<OptionButton>("MarginContainer/VBoxContainer/HBoxContainer/YearOptionButton");
        _monthOptionButton = GetNode<OptionButton>("MarginContainer/VBoxContainer/HBoxContainer/MonthOptionButton");
        _dayOptionButton = GetNode<OptionButton>("MarginContainer/VBoxContainer/HBoxContainer/DayOptionButton");
        _confirmButton = GetNode<Button>("MarginContainer/VBoxContainer/ConfirmButton");
        
        PopulateYearOptionButton();
        PopulateMonthOptionButton();
        PopulateDayOptionButton();
    }

    private void PopulateYearOptionButton()
    {
        for (int year = _startYear; year <= _endYear; year++)
        {
            _yearOptionButton.AddItem(year.ToString());
        }
        _yearOptionButton.Selected = _yearOptionButton.GetItemCount() - 1;
    }

    private void PopulateMonthOptionButton()
    {
        string[] months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
        for (int i = 0; i < months.Length - 1; i++)
        {
            _monthOptionButton.AddItem(months[i]);
        }
        _monthOptionButton.Selected = DateTime.Now.Month - 1;
    }

    private void PopulateDayOptionButton()
    {
        int year = _startYear + _yearOptionButton.Selected;
        int month = _monthOptionButton.Selected + 1;
        int daysInMonth = DateTime.DaysInMonth(year, month);

        _dayOptionButton.Clear();
        for (int day = 1; day <= daysInMonth; day++)
        {
            _dayOptionButton.AddItem(day.ToString());
        }
        _dayOptionButton.Selected = 0;
    }

    private void OnYearOrMonthChanged(int index)
    {
        PopulateDayOptionButton();
    }

    private void OnConfirmButtonPressed()
    {
        int year = _startYear + _yearOptionButton.Selected;
        int month = _monthOptionButton.Selected + 1;
        int day = _dayOptionButton.Selected + 1;

        DateTime selectedDate = new DateTime(year, month, day);
        EmitSignal(SignalName.DateSelected, selectedDate.ToString("yyyy-MM-dd"));
        
        Hide(); 
    }
}