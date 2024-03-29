﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace business
{
    public class DiscoNegocio
    {
        DataAccess dataAccess = new DataAccess();
        public List<Disco> toList()
        {
            List<Disco> list = new List<Disco>();
            
            try
            {
                dataAccess.setConsultation("select d.Id,artista,titulo, FechaLanzamiento, CantidadCanciones,UrlImagenTapa,E.Descripcion as EstiloDescripcion,T.Descripcion as TipoEdicionDescripcion,D.IdEstilo,D.IdTipoEdicion from Discos D,ESTILOS E,TIPOSEDICION T  where E.Id = D.IdEstilo and T.Id = D.IdTipoEdicion");
                dataAccess.executeReading();
                while (dataAccess.Reader.Read()) 
                {
                    Disco aux = new Disco();
                    aux.Id = (int)dataAccess.Reader["Id"];
                    aux.Artista = (string)dataAccess.Reader["Artista"];
                    aux.Titulo = (string)dataAccess.Reader["Titulo"];
                    aux.FechaDeLanzamiento = (DateTime)dataAccess.Reader["FechaLanzamiento"];
                    aux.CantidadDeCanciones = (int)dataAccess.Reader["CantidadCanciones"];
                    aux.UrlImagenTapa = (string)dataAccess.Reader["UrlImagenTapa"];
                    aux.Estilo = new Estilo();
                    aux.Estilo.Descripcion = (string)dataAccess.Reader["EstiloDescripcion"];
                    aux.Estilo.Id = (int)dataAccess.Reader["IdEstilo"];
                    aux.TipoEdicion = new TipoEdicion();
                    aux.TipoEdicion.Descripcion = (string)dataAccess.Reader["TipoEdicionDescripcion"];
                    aux.TipoEdicion.Id = (int)dataAccess.Reader["IdTipoEdicion"];
                    list.Add(aux);
                }
                dataAccess.closeConecction();
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void add(Disco newDisk)
        {
            
            try
            {
                dataAccess.setConsultation("insert into DISCOS (Artista,Titulo,FechaLanzamiento,CantidadCanciones,UrlImagenTapa,idEstilo,IdTipoEdicion) values (@Artista,@Titulo,@FechaDeLanzamiento,@CantidadCanciones,@UrlImagenTapa,@idEstilo,@idTipoEdicion)");
                dataAccess.setParameters("Artista", newDisk.Artista);
                dataAccess.setParameters("@Titulo", newDisk.Titulo);
                dataAccess.setParameters("@FechaDeLanzamiento", newDisk.FechaDeLanzamiento);
                dataAccess.setParameters("@CantidadCanciones", newDisk.CantidadDeCanciones);
                dataAccess.setParameters("@UrlImagenTapa",newDisk.UrlImagenTapa);
                dataAccess.setParameters("@idEstilo",newDisk.Estilo.Id);
                dataAccess.setParameters("@idTipoEdicion", newDisk.TipoEdicion.Id);
                dataAccess.ExecuteAction();           
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { dataAccess.closeConecction(); }
        } 
        
        public void update(Disco updateDisk) 
        {
            
            try
            {
                dataAccess.setConsultation("update DISCOS set Artista = @Artista,Titulo = @Titulo, FechaLanzamiento = @FechaLanzamiento,CantidadCanciones = @CantidadCanciones,UrlImagenTapa = @UrlImagenTapa,IdEstilo = @IdEstilo, IdTipoEdicion = @IdTipoEdicion where id = @id");
                dataAccess.setParameters("@Artista", updateDisk.Artista);
                dataAccess.setParameters("@Titulo", updateDisk.Titulo);
                dataAccess.setParameters("@FechaLanzamiento", updateDisk.FechaDeLanzamiento);
                dataAccess.setParameters("@CantidadCanciones", updateDisk.CantidadDeCanciones);
                dataAccess.setParameters("@UrlImagenTapa", updateDisk.UrlImagenTapa);
                dataAccess.setParameters("@IdEstilo", updateDisk.Estilo.Id);
                dataAccess.setParameters("@IdTipoEdicion", updateDisk.TipoEdicion.Id);
                dataAccess.setParameters("@id", updateDisk.Id);
                dataAccess.ExecuteAction();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { dataAccess.closeConecction(); }
        }

        public void delete(int id) 
        {
            
            try
            {
                dataAccess.setConsultation("delete from Discos where id = @id");
                dataAccess.setParameters("@id",id);
                dataAccess.ExecuteAction();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { dataAccess.closeConecction(); } 
        }

        public List<Disco> filtrar(string campo, string criterio, string filtro)
        {
            List<Disco> list = new List<Disco>();

            try
            {
                string consulta = "select d.Id,Artista,titulo, FechaLanzamiento, CantidadCanciones,UrlImagenTapa,E.Descripcion as EstiloDescripcion,T.Descripcion as TipoEdicionDescripcion,D.IdEstilo,D.IdTipoEdicion from Discos D,ESTILOS E,TIPOSEDICION T  where E.Id = D.IdEstilo and T.Id = D.IdTipoEdicion and ";

                switch (campo)
                {
                    case "Cantidad de Canciones":
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "CantidadCanciones > @filtro";
                                break;
                            case "Menor a":
                                consulta += "CantidadCanciones < @filtro";
                                break;
                            default:
                                consulta += "CantidadCanciones = @filtro";
                                break;
                        }
                        break;
                    case "Titulo":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "Titulo LIKE @filtro + '%' ";
                                break;
                            case "Termina con":
                                consulta += "Titulo LIKE '%' + @filtro ";
                                break;
                            default:
                                consulta += "Titulo LIKE '%' + @filtro + '%' ";
                                break;
                        }
                        break;
                    case "Artista":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "Artista LIKE @filtro + '%' ";
                                break;
                            case "Termina con":
                                consulta += "Artista LIKE '%' + @filtro ";
                                break;
                            default:
                                consulta += "Artista LIKE '%' + @filtro + '%' ";
                                break;
                        }
                        break;
                    case "Estilo":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "E.Descripcion LIKE @filtro + '%' ";
                                break;
                            case "Termina con":
                                consulta += "E.Descripcion LIKE '%' + @filtro ";
                                break;
                            default:
                                consulta += "E.Descripcion LIKE '%' + @filtro + '%' ";
                                break;
                        }
                        break;
                    default:
                        break;
                }

                dataAccess.setConsultation(consulta);
                dataAccess.setParameters("@filtro", filtro);
                dataAccess.executeReading();

                while (dataAccess.Reader.Read())
                {
                    Disco aux = new Disco();
                    aux.Id = (int)dataAccess.Reader["Id"];
                    aux.Artista = (string)dataAccess.Reader["Artista"];
                    aux.Titulo = (string)dataAccess.Reader["Titulo"];
                    aux.FechaDeLanzamiento = (DateTime)dataAccess.Reader["FechaLanzamiento"];
                    aux.CantidadDeCanciones = (int)dataAccess.Reader["CantidadCanciones"];
                    aux.UrlImagenTapa = (string)dataAccess.Reader["UrlImagenTapa"];
                    aux.Estilo = new Estilo();
                    aux.Estilo.Descripcion = (string)dataAccess.Reader["EstiloDescripcion"];
                    aux.Estilo.Id = (int)dataAccess.Reader["IdEstilo"];
                    aux.TipoEdicion = new TipoEdicion();
                    aux.TipoEdicion.Descripcion = (string)dataAccess.Reader["TipoEdicionDescripcion"];
                    aux.TipoEdicion.Id = (int)dataAccess.Reader["IdTipoEdicion"];
                    list.Add(aux);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dataAccess.closeConecction();
            }
        }

    }
}
