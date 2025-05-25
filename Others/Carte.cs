namespace ArribaEats
{ 
    public class Carte
    {
        public List<Plate> Plates { get; } = new List<Plate>();
        public void AddtoMenu(Plate plate) => Plates.Add(plate);
    }

}