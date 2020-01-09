namespace ProductionCode
{
    public class SimpleZipCodeValidator
    {
        public bool Validate(string zipcode)
        {
            if (!int.TryParse(zipcode, out var zip))
                return false;

            if (zip > 99999)
                return false;

            if (zip <= 0)
                return false;

            return true;
        }
    }
}
