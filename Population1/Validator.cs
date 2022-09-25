//Author: Thuy Giang Nguyen

using System;
using System.Globalization;
using System.Windows.Controls;

namespace Population1
{
    public class NumericValidationRule : ValidationRule
    {
        public string ValidationType { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Please enter a positive number");

            switch (ValidationType)
            {
                case "Positive_Double":
                    bool canConvertDouble = float.TryParse(strValue, out float doubleVal);
                    if (canConvertDouble)
                        canConvertDouble = doubleVal >= 0;
                    return canConvertDouble ? new ValidationResult(true, null) : new ValidationResult(false, $"Please enter a positive double");
                default:
                    throw new InvalidCastException($"{ValidationType} is not supported");
            }
        }
    }
}
