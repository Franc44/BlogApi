using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BlogApi.Models
{
    public partial class BlogHCContext : DbContext
    {
        public BlogHCContext()
        {
        }

        public BlogHCContext(DbContextOptions<BlogHCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccionesUsuario> AccionesUsuarios { get; set; }
        public virtual DbSet<Documento> Documentos { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<ForoPregunta> ForoPreguntas { get; set; }
        public virtual DbSet<ForoPreguntasLike> ForoPreguntasLikes { get; set; }
        public virtual DbSet<ForoPreguntasRespuesta> ForoPreguntasRespuestas { get; set; }
        public virtual DbSet<ForoPreguntasRespuestasLike> ForoPreguntasRespuestasLikes { get; set; }
        public virtual DbSet<Publicacione> Publicaciones { get; set; }
        public virtual DbSet<Recuperacion> Recuperacions { get; set; }
        public virtual DbSet<RfcMatricula> RfcMatriculas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccionesUsuario>(entity =>
            {
                entity.HasKey(e => e.IdAccion)
                    .HasName("PK__Acciones__7BB9ED82B0BFAB96");

                entity.ToTable("Acciones_Usuarios");

                entity.Property(e => e.IdAccion).HasColumnName("Id_Accion");

                entity.Property(e => e.FechaAccion)
                    .HasColumnType("smalldatetime")
                    .HasColumnName("Fecha_Accion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdTablaAccion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Id_Tabla_Accion");

                entity.Property(e => e.NombreTablaAccion)
                    .IsRequired()
                    .HasMaxLength(26)
                    .IsUnicode(false)
                    .HasColumnName("NombreTabla_Accion");

                entity.Property(e => e.OperacionAccion)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("Operacion_Accion")
                    .IsFixedLength(true);

                entity.Property(e => e.UsuarioAccion)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Usuario_Accion");
            });

            modelBuilder.Entity<Documento>(entity =>
            {
                entity.HasKey(e => e.IdDocumento)
                    .HasName("PK__Document__995E161386962108");

                entity.ToTable("Documento");

                entity.Property(e => e.IdDocumento).HasColumnName("Id_Documento");

                entity.Property(e => e.DescripcionDoc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Descripcion_Doc");

                entity.Property(e => e.EmpresaDoc).HasColumnName("Empresa_Doc");

                entity.Property(e => e.FechaDoc)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Doc");

                entity.Property(e => e.FileDoc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("File_Doc");

                entity.Property(e => e.TipoDoc)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Tipo_Doc");

                entity.Property(e => e.TituloDoc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Titulo_Doc");

                entity.Property(e => e.UsuarioDoc)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Usuario_Doc");

                entity.HasOne(d => d.EmpresaDocNavigation)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.EmpresaDoc)
                    .HasConstraintName("FK__Documento__Empre__1AD3FDA4");

                entity.HasOne(d => d.UsuarioDocNavigation)
                    .WithMany(p => p.Documentos)
                    .HasForeignKey(d => d.UsuarioDoc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Documento__Usuar__19DFD96B");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.HasKey(e => e.IdEmpresa)
                    .HasName("PK__Empresa__F4BB6039C3B1D851");

                entity.ToTable("Empresa");

                entity.Property(e => e.IdEmpresa).HasColumnName("ID_Empresa");

                entity.Property(e => e.DescripcionEmp)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Descripcion_Emp");

                entity.Property(e => e.EstatusEmp)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Estatus_Emp");

                entity.Property(e => e.FechaAltaEmp)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Alta_Emp");

                entity.Property(e => e.NombreEmp)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Nombre_Emp");
            });

            modelBuilder.Entity<ForoPregunta>(entity =>
            {
                entity.HasKey(e => e.IdPregunta)
                    .HasName("PK__Foro_Pre__1298DD2D27FA59BE");

                entity.ToTable("Foro_Preguntas");

                entity.HasIndex(e => e.IdPregunta, "ui_ukPregunta")
                    .IsUnique();

                entity.Property(e => e.IdPregunta).HasColumnName("Id_Pregunta");

                entity.Property(e => e.DescripcionPregunta)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("Descripcion_Pregunta");

                entity.Property(e => e.EmpresaPregunta).HasColumnName("Empresa_Pregunta");

                entity.Property(e => e.EstatusPregunta)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Estatus_Pregunta");

                entity.Property(e => e.EtiquetasPregunta)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Etiquetas_Pregunta");

                entity.Property(e => e.FechaCierre)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Cierre");

                entity.Property(e => e.FechaPregunta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Pregunta");

                entity.Property(e => e.LikesPregunta).HasColumnName("Likes_Pregunta");

                entity.Property(e => e.TituloPregunta)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Titulo_Pregunta");

                entity.Property(e => e.UsuarioPregunta)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Usuario_Pregunta");

                entity.HasOne(d => d.EmpresaPreguntaNavigation)
                    .WithMany(p => p.ForoPregunta)
                    .HasForeignKey(d => d.EmpresaPregunta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Empre__693CA210");

                entity.HasOne(d => d.UsuarioPreguntaNavigation)
                    .WithMany(p => p.ForoPregunta)
                    .HasForeignKey(d => d.UsuarioPregunta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Usuar__68487DD7");
            });

            modelBuilder.Entity<ForoPreguntasLike>(entity =>
            {
                entity.HasKey(e => e.IdFplikes)
                    .HasName("PK__Foro_Pre__4D4D19557BA90BD5");

                entity.ToTable("Foro_Preguntas_Likes");

                entity.Property(e => e.IdFplikes).HasColumnName("Id_FPLikes");

                entity.Property(e => e.FechaFlp)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_FLP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IdPreguntaFpl).HasColumnName("Id_Pregunta_FPL");

                entity.Property(e => e.IdUsuarioFlp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Id_Usuario_FLP");

                entity.HasOne(d => d.IdPreguntaFplNavigation)
                    .WithMany(p => p.ForoPreguntasLikes)
                    .HasForeignKey(d => d.IdPreguntaFpl)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Id_Pr__4E53A1AA");

                entity.HasOne(d => d.IdUsuarioFlpNavigation)
                    .WithMany(p => p.ForoPreguntasLikes)
                    .HasForeignKey(d => d.IdUsuarioFlp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Id_Us__4F47C5E3");
            });

            modelBuilder.Entity<ForoPreguntasRespuesta>(entity =>
            {
                entity.HasKey(e => e.IdRespuesta)
                    .HasName("PK__Foro_Pre__4F54537D0C11373F");

                entity.ToTable("Foro_Preguntas_Respuestas");

                entity.Property(e => e.IdRespuesta).HasColumnName("Id_Respuesta");

                entity.Property(e => e.CorrectaRespuesta)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Correcta_Respuesta");

                entity.Property(e => e.EstatusRespuesta)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Estatus_Respuesta");

                entity.Property(e => e.FechaRespuesta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Respuesta")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LikesRespuesta).HasColumnName("Likes_Respuesta");

                entity.Property(e => e.PreguntaRespuesta).HasColumnName("Pregunta_Respuesta");

                entity.Property(e => e.TextoRespuesta)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("Texto_Respuesta");

                entity.Property(e => e.UsuarioRespuesta)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Usuario_Respuesta");

                entity.HasOne(d => d.PreguntaRespuestaNavigation)
                    .WithMany(p => p.ForoPreguntasRespuesta)
                    .HasForeignKey(d => d.PreguntaRespuesta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Pregu__6D0D32F4");

                entity.HasOne(d => d.UsuarioRespuestaNavigation)
                    .WithMany(p => p.ForoPreguntasRespuesta)
                    .HasForeignKey(d => d.UsuarioRespuesta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Usuar__6C190EBB");
            });

            modelBuilder.Entity<ForoPreguntasRespuestasLike>(entity =>
            {
                entity.HasKey(e => e.IdFprlikes)
                    .HasName("PK__Foro_Pre__92239DE3C2082B26");

                entity.ToTable("Foro_Preguntas_Respuestas_Likes");

                entity.Property(e => e.IdFprlikes).HasColumnName("Id_FPRLikes");

                entity.Property(e => e.FechaFlrp)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_FLRP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IdRespuestaFprl).HasColumnName("Id_Respuesta_FPRL");

                entity.Property(e => e.IdUsuarioFlrp)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Id_Usuario_FLRP");

                entity.HasOne(d => d.IdRespuestaFprlNavigation)
                    .WithMany(p => p.ForoPreguntasRespuestasLikes)
                    .HasForeignKey(d => d.IdRespuestaFprl)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Id_Re__5224328E");

                entity.HasOne(d => d.IdUsuarioFlrpNavigation)
                    .WithMany(p => p.ForoPreguntasRespuestasLikes)
                    .HasForeignKey(d => d.IdUsuarioFlrp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Foro_Preg__Id_Us__531856C7");
            });

            modelBuilder.Entity<Publicacione>(entity =>
            {
                entity.HasKey(e => e.IdPublicacion)
                    .HasName("PK__Publicac__BE870757C66F3721");

                entity.Property(e => e.IdPublicacion).HasColumnName("Id_Publicacion");

                entity.Property(e => e.EmpresaPubli).HasColumnName("Empresa_Publi");

                entity.Property(e => e.EstatusPubli)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Estatus_Publi");

                entity.Property(e => e.FechaPubli)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Publi");

                entity.Property(e => e.ImagenNombrePubli)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Imagen_Nombre_Publi");

                entity.Property(e => e.TextoPubli)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("Texto_Publi");

                entity.Property(e => e.TipoPubli)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Tipo_Publi");

                entity.Property(e => e.TituloPubli)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Titulo_Publi");

                entity.Property(e => e.UsuarioPubli)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Usuario_Publi");

                entity.HasOne(d => d.EmpresaPubliNavigation)
                    .WithMany(p => p.Publicaciones)
                    .HasForeignKey(d => d.EmpresaPubli)
                    .HasConstraintName("FK__Publicaci__Empre__17036CC0");

                entity.HasOne(d => d.UsuarioPubliNavigation)
                    .WithMany(p => p.Publicaciones)
                    .HasForeignKey(d => d.UsuarioPubli)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Publicaci__Usuar__160F4887");
            });

            modelBuilder.Entity<Recuperacion>(entity =>
            {
                entity.HasKey(e => e.IdRecuperacion)
                    .HasName("PK__Recupera__9079D6B4E199D7F3");

                entity.ToTable("Recuperacion");

                entity.Property(e => e.IdRecuperacion).HasColumnName("ID_Recuperacion");

                entity.Property(e => e.ClaveRec)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Clave_Rec");

                entity.Property(e => e.ExpiracionRec)
                    .HasColumnType("datetime")
                    .HasColumnName("Expiracion_Rec");

                entity.Property(e => e.FechaRec)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Rec");

                entity.Property(e => e.UsuarioRec)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Usuario_Rec");

                entity.HasOne(d => d.UsuarioRecNavigation)
                    .WithMany(p => p.Recuperacions)
                    .HasForeignKey(d => d.UsuarioRec)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Recuperac__Usuar__72910220");
            });

            modelBuilder.Entity<RfcMatricula>(entity =>
            {
                entity.HasKey(e => e.Matricula)
                    .HasName("PK__RFC_Matr__0FB9FB4EC8ECFCD7");

                entity.ToTable("RFC_Matricula");

                entity.Property(e => e.Matricula)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Rfc)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("RFC");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__63C76BE26C80996B");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Id_Usuario");

                entity.Property(e => e.ApellidosUsua)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Apellidos_Usua");

                entity.Property(e => e.ContraUsua)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Contra_Usua");

                entity.Property(e => e.EmailUsua)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Email_Usua");

                entity.Property(e => e.EmpresaUsua).HasColumnName("Empresa_Usua");

                entity.Property(e => e.EstatusUsua)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Estatus_Usua");

                entity.Property(e => e.FechaAltaUsua)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Alta_Usua");

                entity.Property(e => e.MatriculaUsua)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Matricula_Usua");

                entity.Property(e => e.NombresUsua)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Nombres_Usua");

                entity.Property(e => e.ProfilePicture)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Profile_Picture");

                entity.Property(e => e.RfcUsua)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("RFC_Usua");

                entity.Property(e => e.TipoUsua)
                    .HasColumnType("numeric(1, 0)")
                    .HasColumnName("Tipo_Usua");

                entity.HasOne(d => d.EmpresaUsuaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.EmpresaUsua)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Usuarios__Empres__5EBF139D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
