﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleCalculator.Common.Styles;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace SimpleCalculator
{
    public partial class MainPage : ContentPage
    {
        int currentState = 1;
        string mathOperator;
        double firstNumber, secondNumber;

        public MainPage()
        {
            InitializeComponent();
            OnClear(this, null);

            gridMain.Children.ForEach(_ =>
            {
                if (_ is Button button)
                {
                    button.Clicked += async (s, e) =>
                    {
                        await button.ScaleTo(1.3);
                        await button.ScaleTo(1);
                    };
                }
            });
        }

        void OnSelectNumber(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string pressed = button.Text;

            if (this.resultText.Text == "0" || currentState < 0)
            {
                this.resultText.Text = "";
                if (currentState < 0)
                    currentState *= -1;
            }

            if (pressed == ".")
                pressed = ".00";// not optimistic

            this.resultText.Text += pressed;

            double number;
            if (double.TryParse(this.resultText.Text, out number))
            {
                this.resultText.Text = number.ToString("N0");
                if (currentState == 1)
                {
                    firstNumber = number;
                }
                else
                {
                    secondNumber = number;
                }
            }
        }

        void OnSelectOperator(object sender, EventArgs e)
        {
            currentState = -2;
            Button button = (Button)sender;
            string pressed = button.Text;
            mathOperator = pressed;
        }

        void OnClear(object sender, EventArgs e)
        {
            firstNumber = 0;
            secondNumber = 0;
            currentState = 1;
            this.resultText.Text = "0";
        }

        void OnCalculate(object sender, EventArgs e)
        {
            if (currentState == 2)
            {
                double result = Calculator.Calculate(firstNumber, secondNumber, mathOperator);

                this.CurrentCalculation.Text = $"{firstNumber} {mathOperator} {secondNumber}";

                this.resultText.Text = result.ToTrimmedString();
                firstNumber = result;
                currentState = -1;


            }
        }



        void OnNegative(object sender, EventArgs e)
        {
            if (currentState == 1)
            {
                secondNumber = -1;
                mathOperator = "×";
                currentState = 2;
                OnCalculate(this, null);
            }
        }

        void OnPercentage(object sender, EventArgs e)
        {
            if (currentState == 1)
            {
                secondNumber = 0.01;
                mathOperator = "×";
                currentState = 2;
                OnCalculate(this, null);
            }

        }

        int themeIndex = 0;

        ResourceDictionary[] themes = new ResourceDictionary[]
        {
            new DesertTheme(),
            new LavaTheme(),
            new OceanTheme(),
            new SunTheme(),
            new ClayTheme()
        };

        async void ThemeSwitcher_Clicked(System.Object sender, System.EventArgs e)
        {
            themeIndex += 1;
            if (themeIndex >= themes.Length)
            {
                themeIndex = 0;
            }

            ellipse.Scale = 0;
            ellipse.HeightRequest = Height * 2;
            ellipse.WidthRequest = Width * 2;
            ellipse.TranslationX = -Width / 2;
            ellipse.TranslationY = -Height / 2;

            App.Current.Resources = themes[themeIndex];

            var primaryColor = (Color)themes[themeIndex]["PrimaryColor"];

            ellipse.Fill = new SolidColorBrush(primaryColor);

            await ellipse.ScaleTo(3, 850);

            BackgroundColor = primaryColor;
        }
    }
}
