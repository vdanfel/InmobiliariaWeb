namespace Repository.Interface.Cuota
{
    public interface ICuotaRepository
    {
        Task<int> CuotasCompletas(int nIdent_Kardex);
    }
}
