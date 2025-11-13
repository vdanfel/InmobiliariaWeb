using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Programa;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.ViewModels.Programa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;
using System.Runtime.CompilerServices;

namespace InmobiliariaWeb.Servicios
{
    public class ProgramaService: IProgramaService
    {
        private readonly SqlConnection _connection;
        public ProgramaService(SqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> RegistrarPrograma(CrearViewModel crearViewModel, LoginResult loginResult)
        {
            var IdentPrograma = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_PROGRAMA_INSERT", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISNOMBRE", crearViewModel.ProgramaModel.Nombre);
                    command.Parameters.AddWithValue("@ISNUMERO_PARTIDA", crearViewModel.ProgramaModel.Numero_Partida);
                    command.Parameters.AddWithValue("@ISDIRECCION", crearViewModel.ProgramaModel.Direccion);
                    command.Parameters.AddWithValue("@ISREFERENCIA", crearViewModel.ProgramaModel.Referencia ?? "");
                    command.Parameters.AddWithValue("@ISAREA_TOTAL", crearViewModel.ProgramaModel.AreaTotal);
                    command.Parameters.AddWithValue("@ISAREA_LOTIZADA", crearViewModel.ProgramaModel.AreaLotizada);
                    command.Parameters.AddWithValue("@ISCANTIDAD_MANZANAS", crearViewModel.ProgramaModel.CantidadManzanas);
                    command.Parameters.AddWithValue("@ISSUMINISTRO", crearViewModel.ProgramaModel.Suministro ?? "");
                    command.Parameters.AddWithValue("@ISIdent_017_TipoContrato", crearViewModel.ProgramaModel.Ident_017_TipoContrato);
                    command.Parameters.AddWithValue("@ISClausula1", crearViewModel.ProgramaModel.Clausula1);
                    command.Parameters.AddWithValue("@ISPorcentajeLiquidacion", crearViewModel.ProgramaModel.PorcentajeLiquidacion);
                    command.Parameters.AddWithValue("@ISUSUARIO", loginResult.IdentUsuario);
                    
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        IdentPrograma = Int32.Parse(reader["IDENT_PROGRAMA"].ToString());
                    }
                    return IdentPrograma;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task RegistrarManzanaInicial(int Ident_Programa, int usuario)
        {
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_Manzana_Registrar_Inicial", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", Ident_Programa);
                    command.Parameters.AddWithValue("@ISUsuario", usuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> RegistrarManzanas(ProgramaModel programaModel, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Manzana_Registrar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", programaModel.Ident_Programa);
                    command.Parameters.AddWithValue("@ISCantidad", programaModel.CantidadManzanas);
                    command.Parameters.AddWithValue("@ISManzanaInicial", programaModel.ManzanaInicial);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    
                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return "error";
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<ActualizarViewModel> BuscarProgramaIdentPrograma(int identPrograma)
        {
            ActualizarViewModel actualizarViewModel= new ActualizarViewModel();
            ProgramaModel programaModel = new ProgramaModel();
            actualizarViewModel.ProgramaModel = programaModel;
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_Programa_BIdentPrograma", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", identPrograma);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        actualizarViewModel.ProgramaModel.Nombre = reader["NOMBRE"].ToString();
                        actualizarViewModel.ProgramaModel.Numero_Partida = reader["NUMERO_PARTIDA"].ToString();
                        actualizarViewModel.ProgramaModel.Codigo = reader["CODIGO"].ToString();
                        actualizarViewModel.ProgramaModel.Direccion = reader["DIRECCION"].ToString();
                        actualizarViewModel.ProgramaModel.Referencia = reader["REFERENCIA"].ToString();
                        actualizarViewModel.ProgramaModel.AreaTotal = decimal.Parse(reader["AREA_TOTAL"].ToString());
                        actualizarViewModel.ProgramaModel.AreaLotizada = decimal.Parse(reader["AREA_LOTIZADA"].ToString());
                        actualizarViewModel.ProgramaModel.CantidadManzanas = Int32.Parse(reader["CANTIDAD_MANZANAS"].ToString());
                        actualizarViewModel.ProgramaModel.Suministro = reader["SUMINISTRO"].ToString();
                        actualizarViewModel.ProgramaModel.Ident_017_TipoContrato = Int32.Parse(reader["Ident_017_TipoContrato"].ToString());
                        actualizarViewModel.ProgramaModel.ManzanaInicial = Int32.Parse(reader["MANZANA_INICIAL"].ToString());
                        actualizarViewModel.ProgramaModel.Clausula1 = reader["Clausula1"].ToString();
                        actualizarViewModel.ProgramaModel.PorcentajeLiquidacion = decimal.Parse(reader["PorcentajeLiquidacion"].ToString());
                    }
                    return actualizarViewModel;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<List<ProgramaList>> BandejaPrograma(string buscar)
        { 
            var programas = new List<ProgramaList>();
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_PROGRAMAS_BANDEJA", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISBuscar", buscar ?? "");
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var programaList = new ProgramaList();
                        programaList.Indice = Int32.Parse(reader["INDICE"].ToString());
                        programaList.IdentPrograma = Int32.Parse(reader["IDENT_PROGRAMA"].ToString());
                        programaList.Codigo = reader["CODIGO"].ToString();
                        programaList.NombrePrograma = reader["NOMBRE"].ToString();
                        programaList.LotesUsados = Int32.Parse(reader["UTILIZADO"].ToString());
                        programaList.LotesLibres = Int32.Parse(reader["LIBRE"].ToString());
                        programaList.LotesTotales = Int32.Parse(reader["TOTAL"].ToString());
                        programas.Add(programaList);
                    }
                    return programas;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<int> RegistrarPropietario(ViewPropietario viewPropietario,LoginResult loginResult)
        {
            var identProgramaPropietario = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_ProgramaPropietarios_Insert", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_programa", viewPropietario.IdentPrograma);
                    command.Parameters.AddWithValue("@ISIdent_Persona", viewPropietario.IdentPersona);
                    command.Parameters.AddWithValue("@ISIdent_011_TipoPropietario", viewPropietario.Ident011TipoPropietario);
                    command.Parameters.AddWithValue("@ISNumeroPartida", viewPropietario.NumeroPartida);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        identProgramaPropietario = Int32.Parse(reader["IDENT_PROGRAMAPROPIETARIO"].ToString());

                    }
                    return identProgramaPropietario;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<List<PropietarioList>> ListarPropietario(int identPrograma)
        { 
            var propietarios = new List<PropietarioList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_ProgramaPropietarios_List", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_programa", identPrograma);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var propietario = new PropietarioList();
                        propietario.Indice = Int32.Parse(reader["INDICE"].ToString());
                        propietario.IdentProgramaPropietario = Int32.Parse(reader["IDENT_PROGRAMAPROPIETARIO"].ToString());
                        propietario.IdentPrograma = Int32.Parse(reader["IDENT_PROGRAMA"].ToString());
                        propietario.IdentPersona = Int32.Parse(reader["IDENT_PERSONA"].ToString());
                        propietario.NombreCompleto = reader["NOMBRE_COMPLETO"].ToString();
                        propietario.Ident011TipoPropietario = Int32.Parse(reader["IDENT_011_TIPOPROPIETARIO"].ToString());
                        propietario.TipoPropietario = reader["TIPOPROPIETARIO"].ToString();
                        propietario.NumeroPartida = reader["NUMEROPARTIDA"].ToString();
                        propietario.Ident004Estado = Int32.Parse(reader["IDENT_004_ESTADO"].ToString());
                        propietario.Estado = reader["ESTADO"].ToString();
                        propietarios.Add(propietario);
                    }
                    return propietarios;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<List<ManzanaList>> ListarManzanasPrograma(int ident_Programa)
        { 
            var manzanaList = new List<ManzanaList>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_Manzana_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Programa", ident_Programa);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var manzana = new ManzanaList();
                        manzana.Indice = Int32.Parse(reader["INDICE"].ToString());
                        manzana.Ident_Manzana = Int32.Parse(reader["IDENT_MANZANA"].ToString());
                        manzana.Correlativo = Int32.Parse(reader["CORRELATIVO"].ToString());
                        manzana.Letra = reader["LETRA"].ToString();
                        manzana.CantidadLotes = Int32.Parse(reader["CANTIDADLOTES"].ToString());
                        manzanaList.Add(manzana);
                    }
                    return manzanaList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> ValidarManzanaInicial(int Ident_Programa, int ManzanaInicial, int CantidadManzanas)
        {
            var mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_MANZANAS_MANZANAINICIAL", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIDENT_PROGRAMA", Ident_Programa);
                    command.Parameters.AddWithValue("@ISMANZANA_INICIAL", ManzanaInicial);
                    command.Parameters.AddWithValue("@ISCANTIDAD_MANZANAS", CantidadManzanas);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read()) 
                    {
                        mensaje = reader["MENSAJE"].ToString();
                    }
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> AnularPrograma(int Ident_Programa)
        {
            var mensaje = "";
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_PROGRAMA_ANULAR", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIDENT_PROGRAMA", Ident_Programa);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    mensaje = "OK";
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> ActualizarPrograma(ProgramaModel programaModel, LoginResult loginResult)
        {
            var mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Programa_Actualizar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Programa", programaModel.Ident_Programa);
                    command.Parameters.AddWithValue("@ISNombre", programaModel.Nombre);
                    command.Parameters.AddWithValue("@ISNumero_Partida", programaModel.Numero_Partida);
                    command.Parameters.AddWithValue("@ISDireccion", programaModel.Direccion);
                    command.Parameters.AddWithValue("@ISReferencia", programaModel.Referencia ?? "");
                    command.Parameters.AddWithValue("@ISAreaTotal", programaModel.AreaTotal);
                    command.Parameters.AddWithValue("@ISAreaLotizada", programaModel.AreaLotizada);
                    command.Parameters.AddWithValue("@ISCantidadManzanas", programaModel.CantidadManzanas);
                    command.Parameters.AddWithValue("@ISSuministro", programaModel.Suministro ?? "");
                    command.Parameters.AddWithValue("@ISIdent_017_TipoContrato", programaModel.Ident_017_TipoContrato);
                    command.Parameters.AddWithValue("@ISUsuarioModificacion", loginResult.IdentUsuario);
                    command.Parameters.AddWithValue("@ISClausula1", programaModel.Clausula1);
                    command.Parameters.AddWithValue("@ISPorcentajeLiquidacion", programaModel.PorcentajeLiquidacion);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    mensaje = "Se actualizó con Éxito";
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> AnularManzanasList(int IdentPrograma,int IdentUsuario)
        {
            var mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_MANZANAS_ANULARLIST", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIDENT_PROGRAMA", IdentPrograma);
                    command.Parameters.AddWithValue("@ISUSUARIO", IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    mensaje = "OK";
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> ActualizarCantidadLotes(int IdentManzana, int CantidadLotes)
        {
            var mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_MANZANA_CANTIDADLOTES", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIDENT_MANZANA", IdentManzana);
                    command.Parameters.AddWithValue("@ISCANTIDAD_LOTES", CantidadLotes);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    mensaje = "OK";
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> AnularPropietario(int IdentProgramPropietario,int identUsuario)
        {
            var mensaje = "";
            try
            {
                using (SqlCommand command = new SqlCommand("SP_PROPIETARIOS_ANULAR", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIDENT_PROGRAMAPROPIETARIO", IdentProgramPropietario);
                    command.Parameters.AddWithValue("@ISIDENT_USUARIO", identUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    mensaje = "OK";
                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> RegistrarLote(int Ident_Manzana,int CantidadLotes, LoginResult loginResult, int Ident_014_Ubicacionlote)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Lotes_Registrar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Manzana", Ident_Manzana);
                    command.Parameters.AddWithValue("@ISCantidad", CantidadLotes);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    command.Parameters.AddWithValue("@ISIdent_014_Ubicacionlote", Ident_014_Ubicacionlote);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return "error";
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<List<LotesList>> ListarLotes(int identManzana)
        {
            var lotesList = new List<LotesList>();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_Lotes_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_Manzana", identManzana);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var lotes = new LotesList();
                        lotes.Indice = Int32.Parse(reader["INDICE"].ToString());
                        lotes.IdentLote = Int32.Parse(reader["IDENT_LOTES"].ToString());
                        lotes.Correlativo = Int32.Parse(reader["CORRELATIVO"].ToString());
                        lotes.CantidadLados = Int32.Parse(reader["CANTIDAD_LADOS"].ToString());
                        lotes.Ident010TipoLote = Int32.Parse(reader["IDENT_010_TIPOLOTE"].ToString());
                        lotes.PrecioM2 = decimal.Parse(reader["PRECIOM2"].ToString());
                        lotes.Area = decimal.Parse(reader["AREA"].ToString());
                        lotes.PrecioTotal = decimal.Parse(reader["PRECIO_TOTAL"].ToString());
                        lotes.Ident012EstadoLote = Int32.Parse(reader["IDENT_012_ESTADOLOTE"].ToString());
                        lotes.Ident014UbicacionLote = Int32.Parse(reader["IDENT_014_UBICACIONLOTE"].ToString());
                        lotes.TipoUbicacion = reader["TIPO_UBICACION"].ToString();
                        lotes.Flag_ReservadoPropietarpio = (bool)reader["FLAG_RESERVADOPROPIETARIO"];
                        lotesList.Add(lotes);
                    }
                    return lotesList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> LadoRegularRegistrar(ViewLadoregular viewLadoregular,LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_LadosRegulares_Registrar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_lote", viewLadoregular.Ident_Lote);
                    command.Parameters.AddWithValue("@ISIzquierda", viewLadoregular.Izquierda);
                    command.Parameters.AddWithValue("@ISColindaIzquierda", viewLadoregular.ColindaIzquierda);
                    command.Parameters.AddWithValue("@ISDerecha", viewLadoregular.Derecha);
                    command.Parameters.AddWithValue("@ISColindaDerecha", viewLadoregular.ColindaDerecha);
                    command.Parameters.AddWithValue("@ISFrente", viewLadoregular.Frente);
                    command.Parameters.AddWithValue("@ISColindaFrente", viewLadoregular.ColindaFrente);
                    command.Parameters.AddWithValue("@ISFondo", viewLadoregular.Fondo);
                    command.Parameters.AddWithValue("@ISColindaFondo", viewLadoregular.ColindaFondo);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);

                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return "error";
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<ViewLadoregular> LadoRegularSelect(int IdentLote)
        {
            var viewLadoRegular = new ViewLadoregular();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_LadosRegulares_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_lote", IdentLote);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        viewLadoRegular.Izquierda= decimal.Parse(reader["IZQUIERDA"].ToString());
                        viewLadoRegular.ColindaIzquierda = reader["COLINDAIZQUIERDA"].ToString();
                        viewLadoRegular.Derecha = decimal.Parse(reader["DERECHA"].ToString());
                        viewLadoRegular.ColindaDerecha = reader["COLINDADERECHA"].ToString();
                        viewLadoRegular.Frente = decimal.Parse(reader["FRENTE"].ToString());
                        viewLadoRegular.ColindaFrente = reader["COLINDAFRENTE"].ToString();
                        viewLadoRegular.Fondo = decimal.Parse(reader["FONDO"].ToString());
                        viewLadoRegular.ColindaFondo = reader["COLINDAFONDO"].ToString();
                        viewLadoRegular.PrecioM2 = decimal.Parse(reader["PRECIOM2"].ToString());
                        viewLadoRegular.Area = decimal.Parse(reader["AREA"].ToString());
                        viewLadoRegular.PrecioTotal = decimal.Parse(reader["PRECIOTOTAL"].ToString());
                        viewLadoRegular.Porcentaje = decimal.Parse(reader["PORCENTAJE"].ToString());
                        viewLadoRegular.Ident_012_EstadoLote = Int32.Parse(reader["IDENT_012_ESTADOLOTE"].ToString());
                        viewLadoRegular.FlagCheked = (bool)reader["FLAG_CHECKED"];
                        viewLadoRegular.Ident_014_UbicacionLote = Int32.Parse(reader["IDENT_014_UBICACIONLOTE"].ToString());
                        viewLadoRegular.Flag_ReservadoPropietarpio = (bool)reader["FLAG_RESERVADOPROPIETARIO"];
                    }
                    return viewLadoRegular;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> LoteActualizar(int Ident_Lote,int Ident_010_TipoLote,decimal PrecioM2,decimal Area,decimal PrecioTotal, 
            LoginResult loginResult,int Ident_012_EstadoLote, int Ident_014_Ubicacionlote, bool Flag_ReservadoPropietarpio)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_LOTES_ACTUALIZAR", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_lote", Ident_Lote);
                    command.Parameters.AddWithValue("@ISIdent_010_TipoLote", Ident_010_TipoLote);
                    command.Parameters.AddWithValue("@ISPrecioM2", PrecioM2);
                    command.Parameters.AddWithValue("@ISArea", Area);
                    command.Parameters.AddWithValue("@ISPrecioTotal", PrecioTotal);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    command.Parameters.AddWithValue("@ISIdent_012_EstadoLote", Ident_012_EstadoLote);
                    command.Parameters.AddWithValue("@ISIdent_014_Ubicacionlote", Ident_014_Ubicacionlote);
                    command.Parameters.AddWithValue("@ISFlag_ReservadoPropietarpio", Flag_ReservadoPropietarpio);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return "error";
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<string> LadoEspecialRegistrar(ViewLadoEspecial viewLadoEspecial, LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_LadoEspecial_Registrar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_lote", viewLadoEspecial.Ident_Lote);
                    command.Parameters.AddWithValue("@L1", viewLadoEspecial.L1);
                    command.Parameters.AddWithValue("@L1_Ident_009_Lado", viewLadoEspecial.L1_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL1", viewLadoEspecial.ColindaL1 ?? string.Empty);
                    command.Parameters.AddWithValue("@L2", viewLadoEspecial.L2);
                    command.Parameters.AddWithValue("@L2_Ident_009_Lado", viewLadoEspecial.L2_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL2", viewLadoEspecial.ColindaL2 ?? string.Empty);
                    command.Parameters.AddWithValue("@L3", viewLadoEspecial.L3);
                    command.Parameters.AddWithValue("@L3_Ident_009_Lado", viewLadoEspecial.L3_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL3", viewLadoEspecial.ColindaL3 ?? string.Empty);
                    command.Parameters.AddWithValue("@L4", viewLadoEspecial.L4);
                    command.Parameters.AddWithValue("@L4_Ident_009_Lado", viewLadoEspecial.L4_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL4", viewLadoEspecial.ColindaL4 ?? string.Empty);
                    command.Parameters.AddWithValue("@L5", viewLadoEspecial.L5);
                    command.Parameters.AddWithValue("@L5_Ident_009_Lado", viewLadoEspecial.L5_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL5", viewLadoEspecial.ColindaL5 ?? string.Empty);
                    command.Parameters.AddWithValue("@L6", viewLadoEspecial.L6);
                    command.Parameters.AddWithValue("@L6_Ident_009_Lado", viewLadoEspecial.L6_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL6", viewLadoEspecial.ColindaL6 ?? string.Empty);
                    command.Parameters.AddWithValue("@L7", viewLadoEspecial.L7);
                    command.Parameters.AddWithValue("@L7_Ident_009_Lado", viewLadoEspecial.L7_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL7", viewLadoEspecial.ColindaL7 ?? string.Empty);
                    command.Parameters.AddWithValue("@L8", viewLadoEspecial.L8);
                    command.Parameters.AddWithValue("@L8_Ident_009_Lado", viewLadoEspecial.L8_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL8", viewLadoEspecial.ColindaL8 ?? string.Empty);
                    command.Parameters.AddWithValue("@L9", viewLadoEspecial.L9);
                    command.Parameters.AddWithValue("@L9_Ident_009_Lado", viewLadoEspecial.L9_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL9", viewLadoEspecial.ColindaL9 ?? string.Empty);
                    command.Parameters.AddWithValue("@L10", viewLadoEspecial.L10);
                    command.Parameters.AddWithValue("@L10_Ident_009_Lado", viewLadoEspecial.L10_Ident_009_Lado);
                    command.Parameters.AddWithValue("@ColindaL10", viewLadoEspecial.ColindaL10 ?? string.Empty);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return "error";
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<ViewLadoEspecial> LadoEspecialSelect(int IdentLote)
                {
            var viewLadoEspecial = new ViewLadoEspecial();
            try
            {
                using (SqlCommand command = new SqlCommand("SP_LadoEspecial_Select", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_lote", IdentLote);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        viewLadoEspecial.L1 = decimal.Parse(reader["LADO1"].ToString());
                        viewLadoEspecial.L1_Ident_009_Lado = Int32.Parse(reader["L1_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL1 = reader["ColindaL1"].ToString();
                        viewLadoEspecial.L2 = decimal.Parse(reader["LADO2"].ToString());
                        viewLadoEspecial.L2_Ident_009_Lado = Int32.Parse(reader["L2_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL2 = reader["ColindaL2"].ToString();
                        viewLadoEspecial.L3 = decimal.Parse(reader["LADO3"].ToString());
                        viewLadoEspecial.L3_Ident_009_Lado = Int32.Parse(reader["L3_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL3 = reader["ColindaL3"].ToString();
                        viewLadoEspecial.L4 = decimal.Parse(reader["LADO4"].ToString());
                        viewLadoEspecial.L4_Ident_009_Lado = Int32.Parse(reader["L4_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL4 = reader["ColindaL4"].ToString();
                        viewLadoEspecial.L5 = decimal.Parse(reader["LADO5"].ToString());
                        viewLadoEspecial.L5_Ident_009_Lado = Int32.Parse(reader["L5_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL5 = reader["ColindaL5"].ToString();
                        viewLadoEspecial.L6 = decimal.Parse(reader["LADO6"].ToString());
                        viewLadoEspecial.L6_Ident_009_Lado = Int32.Parse(reader["L6_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL6 = reader["ColindaL6"].ToString();
                        viewLadoEspecial.L7 = decimal.Parse(reader["LADO7"].ToString());
                        viewLadoEspecial.L7_Ident_009_Lado = Int32.Parse(reader["L7_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL7 = reader["ColindaL7"].ToString();
                        viewLadoEspecial.L8 = decimal.Parse(reader["LADO8"].ToString());
                        viewLadoEspecial.L8_Ident_009_Lado = Int32.Parse(reader["L8_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL8 = reader["ColindaL8"].ToString();
                        viewLadoEspecial.L9 = decimal.Parse(reader["LADO9"].ToString());
                        viewLadoEspecial.L9_Ident_009_Lado = Int32.Parse(reader["L9_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL9 = reader["ColindaL9"].ToString();
                        viewLadoEspecial.L10 = decimal.Parse(reader["LADO10"].ToString());
                        viewLadoEspecial.L10_Ident_009_Lado = Int32.Parse(reader["L10_Ident_009_Lado"].ToString());
                        viewLadoEspecial.ColindaL10 = reader["ColindaL10"].ToString();
                        viewLadoEspecial.PrecioM2 = decimal.Parse(reader["PRECIOM2"].ToString());
                        viewLadoEspecial.Area = decimal.Parse(reader["AREA"].ToString());
                        viewLadoEspecial.PrecioTotal = decimal.Parse(reader["PRECIOTOTAL"].ToString());
                        viewLadoEspecial.Porcentaje = decimal.Parse(reader["PORCENTAJE"].ToString());
                        viewLadoEspecial.Ident_012_EstadoLote = Int32.Parse(reader["IDENT_012_ESTADOLOTE"].ToString());
                        viewLadoEspecial.FlagCheked = (bool)reader["FLAG_CHECKED"];
                        viewLadoEspecial.Flag_ReservadoPropietarpio = (bool)reader["FLAG_RESERVADOPROPIETARIO"];
                        viewLadoEspecial.Ident_014_UbicacionLote = Int32.Parse(reader["IDENT_014_UBICACIONLOTE"].ToString());
                    }
                    return viewLadoEspecial;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<List<ReporteProgramasEstado>> ReporteProgramasxEstado(int Ident_Programa,int TipoReporte)
        {
            var reporteProgramasEstadoList = new List<ReporteProgramasEstado>();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_REPORTE_LOTES_XTIPO", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Programa", Ident_Programa);
                    command.Parameters.AddWithValue("@nTipoReporte", TipoReporte);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var reporteProgramaEstado = new ReporteProgramasEstado();
                        reporteProgramaEstado.Indice = Int32.Parse(reader["INDICE"].ToString());
                        reporteProgramaEstado.Programa = reader["PROGRAMA"].ToString();
                        reporteProgramaEstado.Manzana = reader["MZ"].ToString();
                        reporteProgramaEstado.Ident_Lote = Int32.Parse(reader["IDENT_LOTE"].ToString());
                        reporteProgramaEstado.Ident010TipoLote = Int32.Parse(reader["IDENT_010_TIPOLOTE"].ToString());
                        reporteProgramaEstado.Lote = Int32.Parse(reader["LT"].ToString());
                        reporteProgramasEstadoList.Add(reporteProgramaEstado);
                    }
                    return reporteProgramasEstadoList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
        public async Task<ProManLotList> DatosProManLot(int Ident_Programa, int Ident_Manzana, int Ident_Lote)
        {
            var promanlotList = new ProManLotList();
            try
            {
                using (SqlCommand command = new SqlCommand("usp_DatosLote", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_Programa", Ident_Programa);
                    command.Parameters.AddWithValue("@nIdent_Manzana", Ident_Manzana);
                    command.Parameters.AddWithValue("@nIdent_Lote", Ident_Lote);

                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        promanlotList.Ident_Programa = Int32.Parse(reader["nIdent_Programa"].ToString());
                        promanlotList.Nombre_Programa = reader["sNombrePrograma"].ToString();
                        promanlotList.Ident_Manzana = Int32.Parse(reader["nIdent_Manzana"].ToString());
                        promanlotList.Letra_Manzana = reader["sManzana"].ToString();
                        promanlotList.Ident_Lote = Int32.Parse(reader["nIdent_Lote"].ToString());
                        promanlotList.Lote = Int32.Parse(reader["sLote"].ToString());
                    }
                    return promanlotList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
