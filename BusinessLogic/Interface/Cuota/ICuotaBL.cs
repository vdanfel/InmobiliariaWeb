namespace BusinessLogic.Interface.Cuota
{
    public interface ICuotaBL
    {
        Task<int> CuotasCompletas(int nIdent_Kardex);
    }
}
