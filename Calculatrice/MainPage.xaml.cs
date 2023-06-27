using Calculatrice.Helpers;
using System.Security.AccessControl;

namespace Calculatrice;

public partial class MainPage : ContentPage
{
    string currentEntry = "";
    int currentState = 1;
    string mathOperator;
    double firstNumber, secondNumber;
    string decimalFormat = "N0";

	public MainPage()
	{
		InitializeComponent();
        OnClear_Clicked(this, null);
	}

    private void OnClear_Clicked(object sender, EventArgs e)
    {
        firstNumber = 0;
        secondNumber = 0;
        currentState = 1;
        decimalFormat = "N0";
        resultText.Text = "0";
        currentEntry = string.Empty;
    }

    private void OnNavigate_Clicked(object sender, EventArgs e)
    {
        if (currentState == 1)
        {
            secondNumber = -1;
            mathOperator = "x";
            currentState = 2;
            OnCalculate(this, null);
        }
    }

    private void OnPercentage_Clicked(object sender, EventArgs e)
    {
        if (currentState == 1)
        {
            AffectNumberValue(resultText.Text);
            decimalFormat = "N2";
            secondNumber = 0.01;
            mathOperator = "x";
            currentState = 2;
            OnCalculate(this, null);
        }
    }
    private void OnCalculate(object sender, EventArgs e)
    {
        if (currentState == 2)
        {
            if (secondNumber == 0)
            {
                AffectNumberValue(resultText.Text);
            }

            double result = Calculator.Calculate(firstNumber, secondNumber, mathOperator);
            CurrentCalculation.Text = $"{firstNumber} {mathOperator} {secondNumber}";
            resultText.Text = result.ToTimmedString(decimalFormat);
            firstNumber = result;
            secondNumber = 0;
            currentState = -1;
            currentEntry = string.Empty;
        }
    }
    private void OnSelectNumber_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;

        string pressed = button.Text;
        currentEntry += pressed;

        if ((resultText.Text == "0" && pressed == "0") 
            || (currentEntry.Length <= 1 && pressed != "0")
            || currentState < 0)
        {
            resultText.Text = "";
            if (currentState < 0)
            {
                currentState *= -1;
            }
        }

        if (pressed == "." && decimalFormat != "N2")
        {
            decimalFormat = "N2";
        }

        resultText.Text += pressed;
    }

    private void OnSelectOperator_Clicked(object sender, EventArgs e)
    {
        AffectNumberValue(resultText.Text);
        currentState = -2;
        Button button = (Button )sender;
        string pressed = button.Text;
        mathOperator = pressed;
    }

    private void AffectNumberValue(string texteEntree)
    {
        double number;
        if (double.TryParse(texteEntree, out number))
        {
            if (currentState == 1)
            {
                firstNumber = number;
            }
            else
            {
                secondNumber = number;
            }

            currentEntry = string.Empty;
        }
    }
}

