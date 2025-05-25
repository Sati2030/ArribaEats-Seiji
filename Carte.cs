namespace ArribaEats
{ 
    /// <summary>
    /// Class for the Menu of a restaurant
    /// </summary>
    public class Carte
    {
        /// <summary>
        /// List of plates
        /// </summary>
        public List<Plate> Plates { get; } = new List<Plate>();
        /// <summary>
        /// Method to add to menu a plate
        /// </summary>
        /// <param name="plate">Plate to add to menu</param>
        public void AddtoMenu(Plate plate) => Plates.Add(plate);
    }

}