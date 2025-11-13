using InmobiliariaWeb.Interfaces;
using InmobiliariaWeb.Models.Roles;
using InmobiliariaWeb.Result;
using InmobiliariaWeb.Result.Programa;
using InmobiliariaWeb.Result.Roles;
using Microsoft.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace InmobiliariaWeb.Servicios
{
    public class RolesService: IRolesService
    {
        private readonly SqlConnection _connection;
        public RolesService(SqlConnection connection) 
        {
            _connection = connection;
        }
        public async Task<List<RolesList>> ListarRoles(int Ident_005_TipoUsuario)
        { 
            var rolesList = new List<RolesList>();
            try 
            {
                using (SqlCommand command = new SqlCommand("usp_ListarRolesUsuario", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nIdent_005_TipoUsuario", Ident_005_TipoUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read()) 
                    {
                        var roles = new RolesList();
                        roles.Indice = Int32.Parse(reader["INDICE"].ToString());
                        roles.Ident_005_Rolusuario = Int32.Parse(reader["IDENT_005_TIPOUSUARIO"].ToString());
                        roles.Descripcion = reader["DESCRIPCION"].ToString();
                        rolesList.Add(roles);
                    }
                    return rolesList;
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
        public async Task<RolesIndexViewModel> CrearRol(RolesIndexViewModel rolesIndexViewModel)
        {
            
            try
            {
                using (SqlCommand command = new SqlCommand("usp_TipoUsuarioCrear", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@sNombreRol", rolesIndexViewModel.NombreRol);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        rolesIndexViewModel.Mensaje = reader["MENSAJE"].ToString();
                        rolesIndexViewModel.Ident_005_TipoUsuario = Int32.Parse(reader["IDENT_005_TIPOUSUARIO"].ToString());
                    }
                    return rolesIndexViewModel;
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
        public async Task<List<PaginasList>> ListarAccesos(int ident_005_TipoUsuario)
        {
            var paginasList = new List<PaginasList>();
            try 
            {
                using (SqlCommand command = new SqlCommand("SP_Accesos_Listar", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_005_TipoUsuario", ident_005_TipoUsuario);
                    await _connection.OpenAsync();
                    // Ejecuta el procedimiento almacenado y obtén el resultado
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var paginas = new PaginasList();
                        paginas.Indice = Int32.Parse(reader["INDICE"].ToString());
                        paginas.ident_Accesos = Int32.Parse(reader["IDENT_ACCESOS"].ToString());
                        paginas.Modulo = reader["MODULO"].ToString();
                        paginas.Pagina = reader["PAGINA"].ToString();
                        paginas.Flag_Visualizar = Convert.ToInt32(reader["FLAG_VISUALIZAR"]) != 0;
                        paginas.Flag_Lectura = Convert.ToInt32(reader["FLAG_LECTURA"]) != 0;
                        paginas.Flag_Escritura = Convert.ToInt32(reader["FLAG_ESCRITURA"]) != 0;
                        paginasList.Add(paginas);
                    }
                    return paginasList;
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
        public async Task CrearAccesos(RolesIndexViewModel rolesIndexViewModel,LoginResult loginResult)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SP_ACCESOS_CREAR", _connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ISIdent_005_TipoUsuario", rolesIndexViewModel.Ident_005_TipoUsuario);
                    command.Parameters.AddWithValue("@ISUsuario", loginResult.IdentUsuario);
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
        
    }
}
