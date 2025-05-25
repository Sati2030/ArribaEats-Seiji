namespace ArribaEats
{
    /// <summary>
    /// An abstract class for every menu.
    /// </summary>
    public abstract class MenuBase
    {
        /// <summary>
        /// Method for displaying the menu options.
        /// </summary>
        /// <returns>The next menu that should be displayed.</returns>
        public abstract MenuBase? Show();

        /// <summary>
        /// Method for parcing the options that a user chooses from a list.
        /// </summary>
        /// <param name="range">The maximum number that the list of options has.</param>
        /// <param name="choice">The parsed choice, if successful.</param>
        /// <returns>True if parsing was successful: otherwise, false.</returns>
        /// <exception cref="InvalidInputException">Thrown when input is out of range</exception>
        protected bool InputParser(int range, out int choice)
        {
            string? input = Console.ReadLine();
            try
            {
                choice = int.Parse(input!);

                ///If out of range
                if (choice > range || choice < 1)
                {
                    throw new InvalidInputException("Invalid input");
                }
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid input");
                choice = -1;
                return false;
            }
        }

    }

}