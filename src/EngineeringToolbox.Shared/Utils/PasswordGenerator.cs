namespace EngineeringToolbox.Shared.Utils
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword(int length = 8)
        {
            length = Math.Min(length, 32);
            var validLower = "abcdefghijklmnopqrstuvwxyz";
            var validUpper = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            var validNumber = "1234567890";
            var validSpecialChar = "!@#$%^&*?_-";

            Random random = new Random();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {

                if (i < 3)
                    chars[i] = validLower[random.Next(0, validLower.Length)];
                else if (i < 5)
                    chars[i] = validNumber[random.Next(0, validNumber.Length)];
                else if (i < 7)
                    chars[i] = validSpecialChar[random.Next(0, validSpecialChar.Length)];
                else
                    chars[i] = validUpper[random.Next(0, validUpper.Length)];

            }
            return new string(chars);

        }
    }
}
