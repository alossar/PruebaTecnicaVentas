using VentasAMSOFT.Model;

namespace VentasAMSOFT.DAL
{
    /// <summary>
    /// Clase para gestionar los repositorios usando un solo contexto.
    /// </summary>
    public class VentasUnitOfWork
    {

        #region Atributos

        private VentasContext contexto = new VentasContext();
        private Repositorio<Cliente> repositorioClientes;
        private Repositorio<Telefono> repositorioTelefonos;
        private Repositorio<Venta> repositorioVentas;

        #endregion

        #region Propiedades

        public Repositorio<Cliente> RepositorioClientes
        {
            get
            {

                if (this.repositorioClientes == null)
                {
                    this.repositorioClientes = new Repositorio<Cliente>(contexto);
                }
                return repositorioClientes;
            }
        }

        public Repositorio<Telefono> RepositorioTelefonos
        {
            get
            {
                if (this.repositorioTelefonos == null)
                {
                    this.repositorioTelefonos = new Repositorio<Telefono>(contexto);
                }
                return repositorioTelefonos;
            }
        }


        public Repositorio<Venta> RepositorioVentas
        {
            get
            {
                if (this.repositorioVentas == null)
                {
                    this.repositorioVentas = new Repositorio<Venta>(contexto);
                }
                return repositorioVentas;
            }
        }

        #endregion

        #region Metodos

        public void Guardar()
        {
            contexto.SaveChanges();
        }

        #endregion
    }
}
