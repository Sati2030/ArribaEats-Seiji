namespace ArribaEats
{

    public class CuisineType
    {
        private static readonly Dictionary<string, int> TypeMapping = new()
        {
            ["italian"] = 0,
            ["french"] = 1,
            ["chinese"] = 2,
            ["japanese"] = 3,
            ["american"] = 4,
            ["australian"] = 5
        };

        public string Name { get; }
        public int Id => TypeMapping[Name.ToLower()];

        public CuisineType(string name)
        {
            if (!TypeMapping.ContainsKey(name.ToLower()))
                throw new InvalidInputException("Unknown cuisine type");
            Name = name;
        }
    }

}

