using System.Data.Entity;
using VentasAMSOFT.Model;

namespace VentasAMSOFT.DAL
{
    /// <summary>
    /// Clase que representa el dominio de la solucion, y permite el acceso de las entidades que se persisten en la base de datos.
    /// </summary>
    public class VentasContext : DbContext
    {
        #region Constructores

        /// <summary>
        /// Especifica el nombre de la conexion a la base de datos a Entity Framework.
        /// </summary>
        public VentasContext() : base("name=VentasAMSOFTDB")
        {
            bool existeInstancia = System.Data.Entity.SqlServer.SqlProviderServices.Instance != null;
        }

        #endregion

        #region Propiedades: Entidades

        public virtual DbSet<Venta> Ventas { get; set; }
        public virtual DbSet<DetalleVenta> Detalles { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Telefono> Telefonos { get; set; }

        #endregion

        #region Metodos 

        /// <summary>
        /// Metodo utilizado cuando se crea la base de datos, para establecer las relaciones entre las tablas, de acuerdo a las entidades del modelo.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Se especifica la clave primaria de la entidad Venta, y la relacion uno a muchos 
            //entre la entidad Venta y la entidad DetalleVenta, especificando explicitamente la clave foranea de la relacion.
            modelBuilder.Entity<Venta>()
                .HasKey(v => v.IdVenta);
            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Detalles)
                .WithRequired(d => d.Venta)
                .HasForeignKey(d => d.IdVenta);

            //Se especifica la clave primaria de la entidad Cliente, y la relacion uno a muchos 
            //entre la entidad Cliente y la entidad Venta, especificando explicitamente la clave foranea de la relacion.
            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.IdCliente);
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Ventas)
                .WithRequired(v => v.Cliente)
                .HasForeignKey(v => v.IdCliente);

            //Se especifica la clave primaria de la entidad Telefono, y la relacion uno a muchos 
            //entre la entidad Telefono y la entidad Venta, especificando explicitamente la clave foranea de la relacion.
            modelBuilder.Entity<Telefono>()
                .HasKey(t => t.IdTelefono);
            modelBuilder.Entity<Telefono>()
                .HasMany(t => t.Ventas)
                .WithRequired(v => v.Telefono)
                .HasForeignKey(v => v.IdTelefono);

            //Se especifica la clave primaria de la entidad DetalleVenta
            modelBuilder.Entity<DetalleVenta>()
              .HasKey(t => t.IdDetalleVenta);
        }

        #endregion
    }
}
