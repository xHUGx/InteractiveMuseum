namespace Features.Ar.Data
{
    public class ArImageTypesConst
    {
        public static string Ferrous = "Ferrous";
        public static string RootAndBloom = "RootAndBloom";

        public static string[] GetAllImageTypes()
        {
            return new[] {Ferrous, RootAndBloom};
        }
    }
}