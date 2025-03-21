﻿using InmobiliariaWeb.Models.Caja;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Contratos;

namespace InmobiliariaWeb.Interfaces
{
    public interface ICajaService
    {
        Task<int> Obtener_Ident_Ingresos(int Ident_021_TipoIngresos, int Ident_Origen);
        Task<List<CuentasBancariasList>> CuentasBancariasXBanco(int Ident_019_Banco);
        Task<int> Ingresos_Insert(IngresosModel ingresosModel, LoginResult loginResult);
        Task Ingresos_Update(IngresosModel ingresosModel, LoginResult loginResult);
        Task<int> IngresosDetalle_Insert(IngresosDetalleModel ingresosDetalleModel, LoginResult loginResult);
        Task<List<IngresosDetallesList>> IngresosDetalle_List(int Ident_Ingresos);
        Task IngresosDetalle_Delete(int Ident_IngresosDetalle, LoginResult loginResult);
        Task<decimal> IngresosDetalle_ImporteTotal(int Ident_Ingresos);
        Task Ingresos_ValidarImportes(int Ident_IngresosDetalle, int Ident_021_TipoIngresos);
    }
}
