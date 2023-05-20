namespace Features.Ar.Data
{
    public class ArImageTypesConst
    {
        public static string Robot = "Robot";
        public static string Woman = "Woman";

        public static string[] GetAllImageTypes()
        {
            return new[] {Robot, Woman};
        }
    }
}