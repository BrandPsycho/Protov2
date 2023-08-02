using Microsoft.AspNetCore.Mvc;
using Protov2.Data;
using Protov2.DTO;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;


namespace Protov2.Controllers
{
    public class AccesoController : Controller
    {
        private readonly DbContext _dbContext;

        public AccesoController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(UsuariosDTO nuser, ClientesDTO nclient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool registrado;
                    string mensaje;

                    if (nuser.contrasena == nuser.confirmar_contrasena)
                    {
                        nuser.contrasena = ConvertirSha256(nuser.contrasena);
                    }
                    else
                    {
                        ViewData["Mensaje"] = "Las contraseñas no coinciden";
                        return View();
                    }

                    using (SqlConnection cn = new SqlConnection(_dbContext.Valor))
                    {
                        using (SqlCommand cmd = new SqlCommand("Login_RegistrarUsuario", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@correo_elec", SqlDbType.VarChar).Value = nuser.correo_elec;
                            cmd.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = nuser.contrasena;
                            cmd.Parameters.Add("@nombre_cliente", SqlDbType.VarChar).Value = nclient.nombre_cliente;
                            cmd.Parameters.Add("@apellido_cliente", SqlDbType.VarChar).Value = nclient.apellido_cliente;
                            cmd.Parameters.Add("@telefono_cliente", SqlDbType.VarChar).Value = nclient.telefono_cliente;

                            cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;

                            cn.Open();

                            cmd.ExecuteNonQuery();

                            registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                            cn.Close();
                        }
                    }

                    ViewData["Mensaje"] = mensaje;

                    if (registrado)
                    {
                        return RedirectToAction("Login", "Acceso");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception)
            { 
                return View("Registrar");
            }
            ViewData["error"] = "Error de credenciales";
            return View("Registrar");

        }

        public ActionResult Login()
        {
            return View() ; 
        }

        [HttpPost]
        public ActionResult Login(UsuariosDTO user)
        {
            user.contrasena = ConvertirSha256(user.contrasena);

            using (SqlConnection cn = new SqlConnection(_dbContext.Valor))
            {
                SqlCommand cmd = new SqlCommand("Login_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("correo_elec", user.correo_elec);
                cmd.Parameters.AddWithValue("contrasena", user.contrasena);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                user.id_usuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }

            if (user.id_usuario != 0)
            {
                Response.Cookies.Append("user", "Bienvenido" + user.correo_elec);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Response.Cookies.Delete("user");
            return RedirectToAction("Login", "Acceso");
        }


        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

    }

}


