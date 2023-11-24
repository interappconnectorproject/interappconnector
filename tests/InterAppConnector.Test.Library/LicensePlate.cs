using System.Text.RegularExpressions;

namespace InterAppConnector.Test.Library
{
    public class LicensePlate
    {
        private string _plate = "";
        public string Plate
        {
            get
            {
                return _plate;
            }
        }

        public LicensePlate(string plate)
        {
            if (Regex.IsMatch(plate, @"[a-zA-Z0-9]+"))
            {
                _plate = plate;
            }
            else
            {
                throw new ArgumentNullException("Plate must not be null or an empty string");
            }
        }

        public static LicensePlate ParseExact(string value, string format, IFormatProvider culture)
        {
            LicensePlate plate;
            string licensePlate = "";

            if (format.Length == value.Length)
            {
                string loweredFormat = format.ToLower();
                for (int i = 0; i < value.Length; i++)
                {
                    switch (loweredFormat[i])
                    {
                        case 'l':
                            if (Regex.IsMatch("" + value[i], @"[a-zA-Z]"))
                            {
                                licensePlate += value[i];
                            }
                            else
                            {
                                throw new FormatException("Character " + i + ": Wrong character '" + value[i] + "'. It should be a letter");
                            }
                            break;
                        case 'n':
                            if (Regex.IsMatch("" + value[i], @"[0-9]"))
                            {
                                licensePlate += value[i];
                            }
                            else
                            {
                                throw new FormatException("Character " + i + ": Wrong character '" + value[i] + "'. It should be a number");
                            }
                            break;
                        case 'x':
                            if (Regex.IsMatch("" + value[i], @"[a-zA-Z0-9]"))
                            {
                                licensePlate += value[i];
                            }
                            else
                            {
                                throw new FormatException("Character " + i + ": Wrong character '" + value[i] + "'. It should be an alphanumeric character");
                            }
                            break;
                        default:
                            throw new FormatException("Character " + i + ": Unrecognised character in format. The allowed characters are l for letters, n for numbers and x for alphanumerical characters");
                    }
                }
            }
            else
            {
                throw new FormatException("Value and format must have the same length");
            }

            plate = new(licensePlate);
            return plate;
        }
    }
}
