namespace ArribaEats
{
    /// <summary>
    /// An abstract class for every menu
    /// </summary>
    public abstract class MenuBase
    {
        public abstract MenuBase? Show();

        protected bool InputParser(int range, out int choice)
        {
            string input = Console.ReadLine();
            try
            {
                choice = int.Parse(input);
                if (choice > range || choice < 1)
                {
                    throw new InvalidInputException("Invalid input");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input");
                choice = -1;
                return false;
            }
        }

    }

}