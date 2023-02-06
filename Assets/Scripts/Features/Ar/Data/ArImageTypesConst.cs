namespace Features.Ar.Data
{
    public class ArImageTypesConst
    {
        public static string InkGod = "InkGod";
        public static string Ferrous = "Ferrous";
        public static string RootAndBloom = "RootAndBloom";
        public static string BrokenDreams = "BrokenDreams";
        public static string Forrest = "Forrest";
        public static string Robot = "Robot";

        public static string[] GetAllImageTypes()
        {
            return new[] { InkGod, Ferrous, RootAndBloom, BrokenDreams, Forrest, Robot };
        }
    }
}