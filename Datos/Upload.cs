﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Datos
{

    public class Upload
    {
        suaEntities db = new suaEntities();

        public Upload() { }

        /**
         * Actualizamos los patrones 
         * 
         */

        public int RefreshBoss()
        {
            SUAHelper sua = null;
            int count = 0;
            Boolean isError = false;
            try
            {
                //Realizamos la conexion
                sua = new SUAHelper();

                String sSQL = "SELECT REG_PAT, RFC_PAT, NOM_PAT, ACT_PAT, DOM_PAT, " +
                              "       MUN_PAT, CPP_PAT, ENT_PAT, TEL_PAT, REM_PAT, " +
                              "       ZON_PAT, DEL_PAT, CAR_ENT, NUM_DEL, CAR_DEL, " +
                              "       NUM_SUB, CAR_SUB, TIP_CON, CON_VEN, INI_AFIL," +
                              "       PAT_REP, CLASE  , FRACCION, STyPS  " +
                              "  FROM Patron   " +
                              "  ORDER BY REG_PAT ";

                //Ejecutamos nuestra consulta
                DataTable dt = sua.ejecutarSQL(sSQL);

                foreach (DataRow rows in dt.Rows)
                {
                    //Revisamos la existencia del registro

                    String patronDescripcion = rows["REG_PAT"].ToString();
                    Patrones patron = new Patrones();
                    if (!patronDescripcion.Equals(""))
                    {
                        var patronTemp = from b in db.Patrones
                                         where b.registro.Equals(patronDescripcion.Trim())
                                         select b;


                        if (patronTemp != null && patronTemp.Count() > 0)
                        {
                            foreach (var patronItem in patronTemp)
                            {
                                patron = patronItem;
                                break;
                            }//Definimos los valores para la plaza
                        }
                        else
                        {
                            patron.registro = "";
                        }

                    }


                    if (!patron.registro.Equals(""))
                    {
                        String plazaDescripcion = rows["CAR_ENT"].ToString();
                        if (!plazaDescripcion.Equals(""))
                        {
                            var plazaTemp = from b in db.Plazas
                                            where b.descripcion.Equals(plazaDescripcion.Trim())
                                            select b;

                            Plazas plaza = new Plazas();

                            if (plazaTemp.Count() > 0)
                            {
                                foreach (var plazaItem in plazaTemp)
                                {
                                    plaza.id = plazaItem.id;
                                    plaza.descripcion = plazaItem.descripcion;
                                    plaza.indicador = "P";
                                    break;
                                }//Definimos los valores para la plaza
                            }
                            else
                            {
                                plaza.descripcion = plazaDescripcion.Trim();
                                plaza.indicador = "P";
                                db.Plazas.Add(plaza);
                                db.SaveChanges();

                            }//Ya existen datos con esta plaza?                            

                            //Modificamos los datos del patron existente
                            patron.registro = rows["REG_PAT"].ToString();
                            patron.rfc = rows["RFC_PAT"].ToString();
                            patron.nombre = rows["NOM_PAT"].ToString();
                            patron.actividad = rows["ACT_PAT"].ToString();
                            patron.domicilio = rows["DOM_PAT"].ToString();
                            patron.municipio = rows["MUN_PAT"].ToString();
                            patron.codigoPostal = rows["CPP_PAT"].ToString();
                            patron.entidad = rows["ENT_PAT"].ToString();
                            patron.telefono = rows["TEL_PAT"].ToString();
                            patron.remision = ((Boolean.Parse(rows["REM_PAT"].ToString()) == true) ? "V" : "F");
                            patron.zona = rows["ZON_PAT"].ToString();
                            patron.delegacion = rows["DEL_PAT"].ToString();
                            patron.carEnt = rows["CAR_ENT"].ToString();
                            patron.numeroDelegacion = Int32.Parse(rows["NUM_DEL"].ToString());
                            patron.carDel = rows["CAR_DEL"].ToString();
                            patron.numSub = Int32.Parse(rows["NUM_SUB"].ToString());
                            patron.Plaza_id = plaza.id;
                            patron.tipoConvenio = Decimal.Parse(rows["TIP_CON"].ToString());
                            patron.convenio = rows["CON_VEN"].ToString();
                            patron.inicioAfiliacion = rows["INI_AFIL"].ToString();
                            patron.patRep = rows["PAT_REP"].ToString();
                            patron.clase = rows["CLASE"].ToString();
                            patron.fraccion = rows["FRACCION"].ToString();
                            patron.STyPS = rows["STyPS"].ToString();

                            //Ponemos la entidad en modo modficada y guardamos cambios
                            try
                            {
                                db.Entry(patron).State = EntityState.Modified;
                                db.SaveChanges();
                                count++;
                            }
                            catch (DbEntityValidationException ex)
                            {
                                StringBuilder sb = new StringBuilder();

                                foreach (var failure in ex.EntityValidationErrors)
                                {
                                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                                    foreach (var error in failure.ValidationErrors)
                                    {
                                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                        sb.AppendLine();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        String plazaDescripcion = rows["CAR_ENT"].ToString();
                        if (!plazaDescripcion.Equals(""))
                        {
                            var plazaTemp = from b in db.Plazas
                                            where b.descripcion.Equals(plazaDescripcion.Trim())
                                            select b;

                            Plazas plaza = new Plazas();

                            if (plazaTemp.Count() > 0)
                            {
                                foreach (var plazaItem in plazaTemp)
                                {
                                    plaza.id = plazaItem.id;
                                    plaza.descripcion = plazaItem.descripcion;
                                    plaza.indicador = "P";
                                    break;
                                }//Definimos los valores para la plaza
                            }
                            else
                            {
                                plaza.descripcion = plazaDescripcion.Trim();
                                plaza.indicador = "P";
                                db.Plazas.Add(plaza);
                                db.SaveChanges();

                            }//Ya existen datos con esta plaza?
                            //Creamos el nuevo patron
                            patron = new Patrones();

                            patron.registro = rows["REG_PAT"].ToString();
                            patron.rfc = rows["RFC_PAT"].ToString();
                            patron.nombre = rows["NOM_PAT"].ToString();
                            patron.actividad = rows["ACT_PAT"].ToString();
                            patron.domicilio = rows["DOM_PAT"].ToString();
                            patron.municipio = rows["MUN_PAT"].ToString();
                            patron.codigoPostal = rows["CPP_PAT"].ToString();
                            patron.entidad = rows["ENT_PAT"].ToString();
                            patron.telefono = rows["TEL_PAT"].ToString();
                            patron.remision = ((Boolean.Parse(rows["REM_PAT"].ToString()) == true) ? "V" : "F");
                            patron.zona = rows["ZON_PAT"].ToString();
                            patron.delegacion = rows["DEL_PAT"].ToString();
                            patron.carEnt = rows["CAR_ENT"].ToString();
                            patron.numeroDelegacion = Int32.Parse(rows["NUM_DEL"].ToString());
                            patron.carDel = rows["CAR_DEL"].ToString();
                            patron.numSub = Int32.Parse(rows["NUM_SUB"].ToString());
                            patron.Plaza_id = plaza.id;
                            patron.tipoConvenio = Decimal.Parse(rows["TIP_CON"].ToString());
                            patron.convenio = rows["CON_VEN"].ToString();
                            patron.inicioAfiliacion = rows["INI_AFIL"].ToString();
                            patron.patRep = rows["PAT_REP"].ToString();
                            patron.clase = rows["CLASE"].ToString();
                            patron.fraccion = rows["FRACCION"].ToString();
                            patron.STyPS = rows["STyPS"].ToString();

                            //Guardamos el patron
                            try
                            {
                                db.Patrones.Add(patron);
                                db.SaveChanges();
                                count++;
                            }
                            catch (DbEntityValidationException ex)
                            {
                                StringBuilder sb = new StringBuilder();

                                foreach (var failure in ex.EntityValidationErrors)
                                {
                                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                                    foreach (var error in failure.ValidationErrors)
                                    {
                                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                        sb.AppendLine();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                if (ex.Source != null)
                {
                    Console.WriteLine(ex.Source);
                    isError = true;
                }
            }
            finally
            {
                if (sua != null)
                {
                    sua.cerrarConexion();
                }
            }

            //String msg = "";
            //if (isError)
            //{
            //    msg = "Ocurrio un error al intentar cargar el archivo";
            //}
            //else
            //{

            //    msg = "Se ha realizado la actualización de los Patrones con exito!";
            //}

            return count;
        }

        /**
         * Realizamos la carga de los acreditados
         */
        public void uploadAcreditado(String path)
        {
            SUAHelper sua = null;
            Boolean isError = false;
            try
            {
                sua = new SUAHelper(path);

                ParametrosHelper parameterHelper = new ParametrosHelper();

                Parametros smdfParameter = parameterHelper.getParameterByKey("SMDF");
                Parametros sinfonParameter = parameterHelper.getParameterByKey("SINFON");

                //Preparamos la consulta
                String sSQL = "SELECT a.REG_PATR , a.NUM_AFIL, a.CURP      , a.RFC_CURP, a.NOM_ASEG, " +
                              "       a.SAL_IMSS , a.SAL_INFO, a.FEC_ALT   , a.FEC_BAJ , a.TIP_TRA , " +
                              "       a.SEM_JORD , a.PAG_INFO, a.TIP_DSC   , a.VAL_DSC , a.CVE_UBC , " +
                              "       a.TMP_NOM  , a.FEC_DSC , a.FEC_FinDsc, a.ARTI_33 , a.SAL_AR33," +
                              "       a.TRA_PENIV, a.ESTADO  , a.CVE_MUN   , b.OCUPA   , b.LUG_NAC  " +
                              "  FROM Asegura a LEFT JOIN Afiliacion b  " +
                              "    ON a.REG_PATR = b.REG_PATR AND  a.NUM_AFIL = b.NUM_AFIL " +
                              "  WHERE a.PAG_INFO <> '' " +
                              "  ORDER BY a.REG_PATR, a.NUM_AFIL ";


                DataTable dt = sua.ejecutarSQL(sSQL);

                foreach (DataRow rows in dt.Rows)
                {
                    String patronDescripcion = rows["REG_PATR"].ToString();
                    Patrones patron = new Patrones();
                    if (!patronDescripcion.Equals(""))
                    {
                        var patronTemp = from b in db.Patrones
                                         where b.registro.Equals(patronDescripcion.Trim())
                                         select b;


                        if (patronTemp != null && patronTemp.Count() > 0)
                        {
                            foreach (var patronItem in patronTemp)
                            {
                                patron = patronItem;
                                break;
                            }//Definimos los valores para la plaza
                        }
                        else
                        {
                            patron.registro = "";
                        }

                    }

                    if (!patron.registro.Trim().Equals(""))
                    {
                        Boolean bExist = false;

                        //Creamos el nuevo asegurado      
                        Acreditados acreditado = new Acreditados();
                        String numAfil = rows["NUM_AFIL"].ToString().Trim();
                        String numCred = rows["PAG_INFO"].ToString().Trim();

                        //Revisamos la existencia del registro
                        var acreditadoExist = from b in db.Acreditados
                                              where b.Patrones.registro.Equals(patron.registro.Trim())
                                                && b.numeroAfiliacion.Equals(numAfil)
                                                && b.numeroCredito.Equals(numCred)
                                              select b;

                        if (acreditadoExist != null && acreditadoExist.Count() > 0)
                        {
                            foreach (var acred in acreditadoExist)
                            {
                                acreditado = acred;
                                bExist = true;
                                break;
                            }//Borramos cada registro.
                        }//Ya existen datos con este patron?

                        String tipoDescuento = rows["TIP_DSC"].ToString();

                        acreditado.PatroneId = patron.Id;
                        acreditado.Patrones = patron;
                        acreditado.numeroAfiliacion = rows["NUM_AFIL"].ToString();
                        acreditado.CURP = rows["CURP"].ToString();
                        acreditado.RFC = rows["RFC_CURP"].ToString();

                        String cliente = rows["CVE_UBC"].ToString();

                        var clienteTemp = db.Clientes.Where(b => b.claveCliente == cliente.Trim()).FirstOrDefault();

                        if (clienteTemp != null)
                        {
                            acreditado.Clientes = (Clientes)clienteTemp;
                            acreditado.clienteId = clienteTemp.Id;
                        }
                        else
                        {
                            Clientes clienteNuevo = new Clientes();
                            clienteNuevo.claveCliente = cliente;
                            clienteNuevo.rfc = "PENDIENTE";
                            clienteNuevo.claveSua = "PENDIENTE";
                            clienteNuevo.descripcion = "PENDIENTE";
                            clienteNuevo.ejecutivo = "PENDIENTE";
                            clienteNuevo.Plaza_id = 1;
                            clienteNuevo.Grupo_id = 1;

                            try
                            {
                                db.Clientes.Add(clienteNuevo);
                                db.SaveChanges();
                                acreditado.clienteId = clienteNuevo.Id;
                            }
                            catch (DbEntityValidationException dbEx)
                            {
                                foreach (var validationErrors in dbEx.EntityValidationErrors)
                                {
                                    foreach (var validationError in validationErrors.ValidationErrors)
                                    {
                                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                                    }
                                }
                            }

                        }

                        String nombrePattern = rows["NOM_ASEG"].ToString();
                        nombrePattern = nombrePattern.Replace("$", ",");

                        string[] substrings = Regex.Split(nombrePattern, ",");

                        acreditado.nombre = substrings[2];
                        acreditado.apellidoPaterno = substrings[0];
                        acreditado.apellidoMaterno = substrings[1];
                        acreditado.nombreCompleto = substrings[0] + " " + substrings[1] + " " + substrings[2];
                        acreditado.ocupacion = rows["OCUPA"].ToString();
                        acreditado.fechaAlta = DateTime.Parse(rows["FEC_ALT"].ToString());

                        if (rows["FEC_BAJ"].ToString().Equals(""))
                        {
                            acreditado.fechaBaja = null;
                        }
                        else
                        {
                            acreditado.fechaBaja = DateTime.Parse(rows["FEC_BAJ"].ToString());
                        }//Trae fecha valida?

                        acreditado.idGrupo = "";
                        acreditado.numeroCredito = rows["PAG_INFO"].ToString();

                        if (rows["FEC_DSC"].ToString().Equals(""))
                        {
                            acreditado.fechaInicioDescuento = null;
                        }
                        else
                        {
                            acreditado.fechaInicioDescuento = DateTime.Parse(rows["FEC_DSC"].ToString());
                        }//Trae fecha valida?

                        if (rows["FEC_FinDsc"].ToString().Equals(""))
                        {
                            acreditado.fechaFinDescuento = null;
                        }
                        else
                        {
                            acreditado.fechaFinDescuento = DateTime.Parse(rows["FEC_FinDsc"].ToString());
                        }//Trae fecha valida?

                        DateTime date = DateTime.Now;

                        //Validamos que el ultimo movimiento no sea por baja o suspención.
                        if (sua.esValidoActualizarPorMovimiento(acreditado.Patrones.registro, acreditado.numeroAfiliacion))
                        {
                            //Validamos que el valor de los parametros sea mayor a cero.
                            if (sinfonParameter.valorMoneda > 0 && smdfParameter.valorMoneda > 0)
                            {

                                if (bExist)
                                {
                                    if (acreditado.fechaUltimoCalculo != null)
                                    {
                                        //Validamos que se haya modificado el valor de los parametros para el calculo
                                        if (DateTime.Compare((DateTime)acreditado.fechaUltimoCalculo, smdfParameter.fechaCreacion) <= 0
                                        && DateTime.Compare((DateTime)acreditado.fechaUltimoCalculo, sinfonParameter.fechaCreacion) <= 0)
                                        {
                                            acreditado = calcularInfonavitInfo(acreditado, rows, tipoDescuento, Decimal.Parse(sinfonParameter.valorMoneda.ToString()), Decimal.Parse(smdfParameter.valorMoneda.ToString()));

                                        } //Se ha cambiado los parametros desde la ultima actualización ?
                                    }
                                    else
                                    {
                                        acreditado = calcularInfonavitInfo(acreditado, rows, tipoDescuento, Decimal.Parse(sinfonParameter.valorMoneda.ToString()), Decimal.Parse(smdfParameter.valorMoneda.ToString()));
                                    }
                                }
                                else
                                {
                                    acreditado = calcularInfonavitInfo(acreditado, rows, tipoDescuento, Decimal.Parse(sinfonParameter.valorMoneda.ToString()), Decimal.Parse(smdfParameter.valorMoneda.ToString()));
                                }
                            }//Los parametros son mayores a cero en su valor moneda ?
                        }//El movimiento es por baja o suspención ?

                        acreditado.Plaza_id = patron.Plaza_id;

                        if (!bExist)
                        {
                            acreditado.fechaCreacion = date;
                        }
                        else
                        {
                            acreditado.fechaModificacion = date;
                        }

                        //Guardamos el asegurado
                        try
                        {
                            if (!bExist)
                            {
                                db.Acreditados.Add(acreditado);
                            }
                            else
                            {
                                db.Entry(acreditado).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            StringBuilder sb = new StringBuilder();

                            foreach (var failure in ex.EntityValidationErrors)
                            {
                                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                                foreach (var error in failure.ValidationErrors)
                                {
                                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                    sb.AppendLine();
                                }
                            }
                        }
                    }
                }

            }
            catch (OleDbException ex)
            {
                if (ex.Source != null)
                {
                    Console.WriteLine(ex.Source);
                    isError = true;
                }
            }
            finally
            {
                if (isError)
                {
                    //TempData["viewMessage"] = "Ocurrio un error al intentar cargar el archivo de los Acreditados";
                }
                else
                {
                    //TempData["viewMessage"] = "Se ha realizado la actualización con exito!";
                }
                if (sua != null)
                {
                    sua.cerrarConexion();
                }
            }
        }

        private Acreditados calcularInfonavitInfo(Acreditados acreditado, DataRow rows, String tipoDescuento, Decimal sinfon, Decimal smdf)
        {
            DateTime date = DateTime.Now;

            Decimal valueToCalculate = Decimal.Parse(rows["VAL_DSC"].ToString());
            acreditado.sdi = Double.Parse(rows["SAL_IMSS"].ToString());
            Decimal sdi = Decimal.Parse(rows["SAL_IMSS"].ToString());
            acreditado.smdv = Double.Parse(smdf.ToString());


            Decimal newValue = Decimal.Parse("0.0");
            //Empezamos con los calculos
            if (tipoDescuento.Trim().Equals("1"))
            {

                // Descuento tipo porcentaje
                acreditado.sd = 0;
                acreditado.cuotaFija = 0;
                //                acreditado.smdv = 0.0;
                acreditado.vsm = 0;
                acreditado.porcentaje = valueToCalculate / 100;


                newValue = (sdi * 60);
                newValue = newValue * (valueToCalculate / 100);
                newValue = newValue + sinfon;

                acreditado.descuentoBimestral = newValue;

            }
            else if (tipoDescuento.Trim().Equals("2"))
            {
                // Descuento tipo cuota fija
                acreditado.sd = 0;
                acreditado.cuotaFija = valueToCalculate;
                //               acreditado.smdv = 0.0;
                acreditado.vsm = 0;
                acreditado.porcentaje = 0;

                newValue = valueToCalculate * 2;
                acreditado.descuentoBimestral = newValue;


            }
            else if (tipoDescuento.Trim().Equals("3"))
            {
                // Descuento tipo VSM
                acreditado.sd = 0;
                acreditado.cuotaFija = 0;
                //               acreditado.smdv = 0.0;
                acreditado.vsm = Math.Round(valueToCalculate, 3);
                acreditado.porcentaje = 0;
                newValue = valueToCalculate * smdf * 2;
                newValue = newValue + sinfon;
                newValue = Math.Round(newValue, 3);
                acreditado.descuentoBimestral = newValue;
            }

            acreditado.descuentoMensual = Math.Round(acreditado.descuentoBimestral / 2, 3);
            Decimal newValue2 = acreditado.descuentoMensual * Decimal.Parse((7 / 30.4).ToString());
            newValue2 = Math.Round(newValue2, 3);
            acreditado.descuentoSemanal = newValue2;

            newValue2 = acreditado.descuentoMensual * Decimal.Parse((14 / 30.4).ToString());
            newValue2 = Math.Round(newValue2, 3);
            acreditado.descuentoCatorcenal = newValue2;
            acreditado.descuentoQuincenal = Math.Round(acreditado.descuentoBimestral / 4, 3);
            acreditado.descuentoVeintiochonal = Math.Round(acreditado.descuentoMensual * Decimal.Parse((28 / 30.4).ToString()), 3);
            acreditado.descuentoDiario = Math.Round(acreditado.descuentoBimestral / Decimal.Parse("60.1"), 3);
            acreditado.fechaUltimoCalculo = date.Date;

            return acreditado;
        }

        /**
         *  Hacemos la carga de los asegurados
         * */
        public void uploadAsegurado(String path)
        {
            SUAHelper sua = null;
            Boolean isError = false;
            try
            {
                //Realizamos la conexión
                sua = new SUAHelper(path);

                String sSQL = "SELECT a.REG_PATR , a.NUM_AFIL, a.CURP      , a.RFC_CURP, a.NOM_ASEG, " +
                              "       a.SAL_IMSS , a.SAL_INFO, a.FEC_ALT   , a.FEC_BAJ , a.TIP_TRA , " +
                              "       a.SEM_JORD , a.PAG_INFO, a.TIP_DSC   , a.VAL_DSC , a.CVE_UBC , " +
                              "       a.TMP_NOM  , a.FEC_DSC , a.FEC_FinDsc, a.ARTI_33 , a.SAL_AR33," +
                              "       a.TRA_PENIV, a.ESTADO  , a.CVE_MUN   , b.OCUPA   , b.LUG_NAC  " +
                              "  FROM Asegura a LEFT JOIN Afiliacion b  " +
                              "    ON a.REG_PATR = b.REG_PATR AND  a.NUM_AFIL = b.NUM_AFIL " +
                              "  ORDER BY a.NUM_AFIL ";

                //Ejecutamos la consulta
                DataTable dt = sua.ejecutarSQL(sSQL);
                foreach (DataRow rows in dt.Rows)
                {

                    String patronDescripcion = rows["REG_PATR"].ToString();
                    Patrones patron = new Patrones();
                    if (!patronDescripcion.Equals(""))
                    {
                        var patronTemp = from b in db.Patrones
                                         where b.registro.Equals(patronDescripcion.Trim())
                                         select b;


                        if (patronTemp != null && patronTemp.Count() > 0)
                        {
                            foreach (var patronItem in patronTemp)
                            {
                                patron = patronItem;
                                break;
                            }//Definimos los valores para la plaza
                        }
                        else
                        {
                            patron.registro = "";
                        }

                    }

                    if (!patron.registro.Trim().Equals(""))
                    {
                        Boolean bExist = false;

                        //Creamos el nuevo asegurado      
                        Asegurados asegurado = new Asegurados();
                        String numAfil = rows["NUM_AFIL"].ToString().Trim();

                        //Revisamos la existencia del registro
                        var aseguradoExist = from b in db.Asegurados
                                             where b.Patrones.registro.Equals(patron.registro.Trim())
                                               && b.numeroAfiliacion.Equals(numAfil)
                                             select b;

                        if (aseguradoExist.Count() > 0)
                        {
                            foreach (var aseg in aseguradoExist)
                            {
                                asegurado = aseg;
                                bExist = true;
                                break;
                            }//Borramos cada registro.
                        }//Ya existen datos con este patron?


                        //Creamos el nuevo asegurado                 

                        asegurado.PatroneId = patron.Id;
                        asegurado.numeroAfiliacion = rows["NUM_AFIL"].ToString();
                        asegurado.CURP = rows["CURP"].ToString();
                        asegurado.RFC = rows["RFC_CURP"].ToString();
                        asegurado.nombre = rows["NOM_ASEG"].ToString();
                        asegurado.salarioImss = Decimal.Parse(rows["SAL_IMSS"].ToString());
                        if (rows["SAL_INFO"].ToString().Equals(""))
                        {
                            asegurado.salarioInfo = 0;
                        }
                        else
                        {
                            asegurado.salarioInfo = Decimal.Parse(rows["SAL_INFO"].ToString());
                        }

                        asegurado.fechaAlta = DateTime.Parse(rows["FEC_ALT"].ToString());

                        if (rows["FEC_BAJ"].ToString().Equals(""))
                        {
                            asegurado.fechaBaja = null;
                        }
                        else
                        {
                            asegurado.fechaBaja = DateTime.Parse(rows["FEC_BAJ"].ToString());
                        }//Trae fecha valida?
                        asegurado.tipoTrabajo = rows["TIP_TRA"].ToString();
                        asegurado.semanaJornada = rows["SEM_JORD"].ToString();
                        asegurado.paginaInfo = rows["PAG_INFO"].ToString();
                        asegurado.tipoDescuento = rows["TIP_DSC"].ToString();
                        asegurado.valorDescuento = Decimal.Parse(rows["VAL_DSC"].ToString());

                        String cliente = rows["CVE_UBC"].ToString();

                        var clienteTemp = db.Clientes.Where(b => b.claveCliente == cliente.Trim()).FirstOrDefault();

                        if (clienteTemp != null)
                        {
                            asegurado.Clientes = (Clientes)clienteTemp;
                            asegurado.ClienteId = clienteTemp.Id;
                        }
                        else
                        {
                            Clientes clienteNuevo = new Clientes();
                            clienteNuevo.claveCliente = cliente;
                            clienteNuevo.rfc = "PENDIENTE";
                            clienteNuevo.claveSua = "PENDIENTE";
                            clienteNuevo.descripcion = "PENDIENTE";
                            clienteNuevo.ejecutivo = "PENDIENTE";
                            clienteNuevo.Plaza_id = 1;
                            clienteNuevo.Grupo_id = 1;

                            try
                            {
                                db.Clientes.Add(clienteNuevo);
                                db.SaveChanges();
                                asegurado.ClienteId = clienteNuevo.Id;
                            }
                            catch (DbEntityValidationException dbEx)
                            {
                                foreach (var validationErrors in dbEx.EntityValidationErrors)
                                {
                                    foreach (var validationError in validationErrors.ValidationErrors)
                                    {
                                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                                    }
                                }
                            }

                        }

                        asegurado.nombreTemporal = rows["TMP_NOM"].ToString();

                        if (rows["FEC_DSC"].ToString().Equals(""))
                        {
                            asegurado.fechaDescuento = null;
                        }
                        else
                        {
                            asegurado.fechaDescuento = DateTime.Parse(rows["FEC_DSC"].ToString());
                        }//Trae fecha valida?

                        if (rows["FEC_FinDsc"].ToString().Equals(""))
                        {
                            asegurado.finDescuento = null;
                        }
                        else
                        {
                            asegurado.finDescuento = DateTime.Parse(rows["FEC_FinDsc"].ToString());
                        }//Trae fecha valida?
                        asegurado.articulo33 = rows["ARTI_33"].ToString();
                        if (rows["SAL_AR33"].ToString().Equals(""))
                        {
                            asegurado.salarioArticulo33 = 0;
                        }
                        else
                        {
                            asegurado.salarioArticulo33 = Decimal.Parse(rows["SAL_AR33"].ToString());
                        }
                        asegurado.trapeniv = rows["TRA_PENIV"].ToString();
                        asegurado.estado = rows["ESTADO"].ToString();
                        asegurado.claveMunicipio = rows["CVE_MUN"].ToString();
                        asegurado.Plaza_id = patron.Plaza_id;
                        asegurado.ocupacion = rows["OCUPA"].ToString();
                        if (rows["OCUPA"].ToString().Equals("EXTRANJERO"))
                        {
                            asegurado.extranjero = "SI";
                        }
                        else
                        {
                            asegurado.extranjero = "NO";
                        }

                        DateTime date = DateTime.Now;
                        if (!bExist)
                        {
                            asegurado.fechaCreacion = date;
                        }
                        else
                        {
                            asegurado.fechaModificacion = date;
                        }


                        //Guardamos el asegurado
                        try
                        {
                            if (bExist)
                            {
                                db.Entry(asegurado).State = EntityState.Modified;
                            }
                            else
                            {
                                db.Asegurados.Add(asegurado);
                            }
                            db.SaveChanges();
                            if (asegurado.id > 0)
                            {
                                uploadIncapacidades(asegurado.Patrones.registro, asegurado.numeroAfiliacion, asegurado.id, path);
                                uploadMovimientos(asegurado.Patrones.registro, asegurado.numeroAfiliacion, asegurado.id, path);
                                accionesAdicionalesAsegurados(asegurado);
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            isError = true;
                            StringBuilder sb = new StringBuilder();

                            foreach (var failure in ex.EntityValidationErrors)
                            {
                                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                                foreach (var error in failure.ValidationErrors)
                                {
                                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                    sb.AppendLine();
                                }
                            }
                        }
                    }
                }

            }
            catch (OleDbException ex)
            {
                isError = true;
                if (ex.Source != null)
                {
                    Console.WriteLine(ex.Source);
                }
            }
            finally
            {
                if (isError)
                {

                    //TempData["viewMessage"] = "Ocurrio un error al intentar cargar el archivo de Asegurados";
                }
                else
                {

                    //TempData["viewMessage"] = "Se ha realizado la actualización con exito!";
                }
                if (sua != null)
                {
                    sua.cerrarConexion();
                }
            }
        }

        public void uploadIncapacidades(String registro, String numeroAfiliacion, int aseguradoId, String path)
        {
            SUAHelper sua = null;
            try
            {
                //Realizamos la conexión
                sua = new SUAHelper(path);

                String sSQL = "SELECT a.REG_PAT  , a.NUM_AFI      , a.FEC_ACC   , a.FOL_INC , a.GRUPO_INC , " +
                              "       a.REC_REV  , a.CONSECUENCIA , a.TIP_RIE   , a.RAM_SEG , a.SECUELA   , " +
                              "       a.CON_INC  , a.DIA_SUB      , a.POR_INC   , a.IND_DEF , a.FEC_TER     " +
                              "  FROM Incapacidades a  " +
                              "  WHERE a.REG_PAT = '" + registro + "'" +
                              "    AND a.NUM_AFI = '" + numeroAfiliacion + "'" +
                              "  ORDER BY a.NUM_AFI ";

                //Ejecutamos la consulta
                DataTable dt = sua.ejecutarSQL(sSQL);
                foreach (DataRow rows in dt.Rows)
                {

                    Boolean bExist = false;

                    String folio = rows["FOL_INC"].ToString();
                    Incapacidades incapacidad = new Incapacidades();
                    var incapacidadTemp = from b in db.Incapacidades
                                          where b.folioIncapacidad.Equals(folio.Trim())
                                          select b;

                    if (incapacidadTemp != null && incapacidadTemp.Count() > 0)
                    {
                        foreach (var incapacidadItem in incapacidadTemp)
                        {
                            incapacidad = incapacidadItem;
                            bExist = true;
                            break;
                        }//Definimos los valores para la plaza
                    }

                    //Creamos la nueva incapacidad     
                    if (!bExist)
                    {
                        incapacidad.aseguradoId = aseguradoId;
                        incapacidad.folioIncapacidad = rows["FOL_INC"].ToString();
                    }
                    incapacidad.fechaAcc = DateTime.Parse(rows["FEC_ACC"].ToString());
                    incapacidad.grupoIncapacidad = rows["GRUPO_INC"].ToString();
                    incapacidad.recRev = rows["REC_REV"].ToString();
                    incapacidad.consecuencia = rows["CONSECUENCIA"].ToString();

                    incapacidad.tieRie = rows["TIP_RIE"].ToString();
                    incapacidad.ramSeq = rows["RAM_SEG"].ToString();
                    incapacidad.secuela = rows["SECUELA"].ToString();
                    incapacidad.conInc = rows["CON_INC"].ToString();
                    incapacidad.diaSub = int.Parse(rows["DIA_SUB"].ToString());
                    incapacidad.porcentajeIncapacidad = Decimal.Parse(rows["POR_INC"].ToString());
                    incapacidad.indDef = rows["IND_DEF"].ToString();
                    incapacidad.fecTer = DateTime.Parse(rows["FEC_TER"].ToString());

                    //Guardamos la incapacidad
                    try
                    {
                        if (bExist)
                        {
                            db.Entry(incapacidad).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Incapacidades.Add(incapacidad);
                        }
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (var failure in ex.EntityValidationErrors)
                        {
                            sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                            foreach (var error in failure.ValidationErrors)
                            {
                                sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                sb.AppendLine();
                            }
                        }
                    }
                }

            }
            catch (OleDbException ex)
            {
                if (ex.Source != null)
                {
                    Console.WriteLine(ex.Source);
                }
            }
            finally
            {
                if (sua != null)
                {
                    sua.cerrarConexion();
                }
            }
        }

        public void uploadMovimientos(String registro, String numeroAfiliacion, int aseguradoId, String path)
        {
            SUAHelper sua = null;
            try
            {
                //Realizamos la conexión
                sua = new SUAHelper(path);

                String sSQL = "SELECT a.REG_PATR , a.NUM_AFIL , a.TIP_MOVS  , a.FEC_INIC , a.CON_SEC  , " +
                              "       a.NUM_DIAS , a.SAL_MOVT , a.SAL_MOVT2 , a.MES_ANO  , a.FEC_REAL , " +
                              "       a.FOL_INC  , a.CVE_MOVS , a.SAL_MOVT3 , a.TIP_INC  , a.EDO_MOV  , " +
                              "       a.FEC_EXT  , a.SAL_ANT1 , a.SAL_ANT2  , a.SAL_ANT3 , a.ART_33   , " +
                              "       a.TIP_SAL  , a.TIP_RIE  , a.TIP_REC   , a.NUM_CRE  , a.VAL_DES  , " +
                              "       a.TIP_DES  , a.TAB_DISM " +
                              "  FROM Movtos a  " +
                              "  WHERE a.REG_PATR = '" + registro + "'" +
                              "    AND a.NUM_AFIL = '" + numeroAfiliacion + "'" +
                              "  ORDER BY a.NUM_AFIL ";

                //Ejecutamos la consulta
                DataTable dt = sua.ejecutarSQL(sSQL);
                foreach (DataRow rows in dt.Rows)
                {

                    String folio = rows["FOL_INC"].ToString();
                    MovimientosAseguradoes movimiento = new MovimientosAseguradoes();

                    movimiento.fechaInicio = DateTime.Parse(rows["FEC_INIC"].ToString());
                    movimiento.aseguradoId = aseguradoId;
                    movimiento.sdi = rows["SAL_MOVT"].ToString();
                    String tipoMov = "01";
                    if (!string.IsNullOrEmpty(rows["TIP_MOVS"].ToString().Trim()))
                    {
                        tipoMov = rows["TIP_MOVS"].ToString().Trim();
                    }

                    //Validamos que ese movimiento no se haya guardado anteriormente
                    var movTemp = (from s in db.MovimientosAseguradoes
                                  .Where(s => s.aseguradoId.Equals(aseguradoId)
                                  && s.catalogoMovimientos.tipo.Equals(tipoMov.Trim())
                                  && s.fechaInicio.Equals(movimiento.fechaInicio))
                                   select s).FirstOrDefault();

                    if (movTemp == null)
                    {

                        if (rows["NUM_DIAS"].ToString() != null && !rows["NUM_DIAS"].ToString().Equals(""))
                        {
                            movimiento.numeroDias = int.Parse(rows["NUM_DIAS"].ToString());
                        }

                        if (folio != null && !folio.Equals(""))
                        {

                            var incapacidadTemp = from b in db.Incapacidades
                                                  where b.folioIncapacidad.Equals(folio.Trim())
                                                  select b;

                            if (incapacidadTemp != null && incapacidadTemp.Count() > 0)
                            {
                                foreach (var incapacidadItem in incapacidadTemp)
                                {
                                    movimiento.Incapacidades = incapacidadItem;
                                    movimiento.incapacidadId = incapacidadItem.id;
                                    break;
                                }//Definimos los valores para la plaza
                            }
                        }

                        var tipoTemp = db.catalogoMovimientos.Where(b => b.tipo == tipoMov).FirstOrDefault();

                        if (tipoTemp != null)
                        {
                            movimiento.catalogoMovimientos = (catalogoMovimientos)tipoTemp;
                        }
                        else
                        {
                            catalogoMovimientos catMov = new catalogoMovimientos();
                            catMov.id = 1;
                            catMov.tipo = "01";
                            movimiento.catalogoMovimientos = catMov;
                        }

                        movimiento.credito = rows["NUM_CRE"].ToString();
                        movimiento.estatus = rows["EDO_MOV"].ToString();

                        //Guardamos el movimiento
                        try
                        {

                            db.MovimientosAseguradoes.Add(movimiento);
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            StringBuilder sb = new StringBuilder();

                            foreach (var failure in ex.EntityValidationErrors)
                            {
                                sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                                foreach (var error in failure.ValidationErrors)
                                {
                                    sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                    sb.AppendLine();
                                }
                            }
                        }
                    }
                }//Ya existe ese movimiento for fecha y tipo?
            }
            catch (OleDbException ex)
            {
                if (ex.Source != null)
                {
                    Console.WriteLine(ex.Source);
                }
            }
            finally
            {
                if (sua != null)
                {
                    sua.cerrarConexion();
                }
            }
        }

        /**
         * Realizamos el calculo del salario diario y la fecha de entrada 
         */
        private void accionesAdicionalesAsegurados(Asegurados asegurado)
        {

            int aseguradoId = asegurado.id;
            DateTime ahora = DateTime.Now;

            //obtenemos el ultimo reingreso, si existe.
            var movTemp = (from s in db.MovimientosAseguradoes
                                  .Where(s => s.aseguradoId.Equals(aseguradoId)
                                   && s.catalogoMovimientos.tipo.Equals("08"))
                                  .OrderBy(s => s.fechaInicio)
                           select s).FirstOrDefault();

            if (movTemp != null)
            {
                asegurado.fechaAlta = movTemp.fechaInicio;
            }

            //obtenemos el ultimo movimiento para saber como se calcula
            //el salario Diario.
            movTemp = (from s in db.MovimientosAseguradoes
                                  .Where(s => s.aseguradoId.Equals(aseguradoId))
                                  .OrderBy(s => s.fechaInicio)
                       select s).FirstOrDefault();

            if (movTemp != null)
            {
                if (asegurado.salarioDiario == null)
                {
                    asegurado.salarioDiario = 0;
                }
                if (movTemp.catalogoMovimientos.tipo.Trim().Equals("08"))
                {
                    asegurado.salarioDiario = Decimal.Parse(movTemp.sdi.Trim());
                }
                else if (movTemp.catalogoMovimientos.tipo.Trim().Equals("01") || movTemp.catalogoMovimientos.tipo.Trim().Equals("07"))
                {
                    long annos = DatesHelper.DateDiffInYears(asegurado.fechaAlta, ahora);
                    if (annos.Equals(0))
                    {
                        annos = 1;
                    }
                    Factores factor = (db.Factores.Where(x => x.anosTrabajados == annos).FirstOrDefault());
                    if (factor != null)
                    {
                        asegurado.salarioDiario = Decimal.Parse(movTemp.sdi.Trim()) / factor.factorIntegracion;
                    }
                    else
                    {
                        asegurado.salarioDiario = 0;
                    }
                }
                else if (movTemp.catalogoMovimientos.tipo.Trim().Equals("02"))
                {
                    asegurado.salarioDiario = 0;

                }

                db.Entry(asegurado).State = EntityState.Modified;
                db.SaveChanges();

                Acreditados acreditado = (from s in db.Acreditados
                                          where s.PatroneId == asegurado.PatroneId
                                             && s.numeroAfiliacion.Equals(asegurado.numeroAfiliacion)
                                          select s).FirstOrDefault();

                if (acreditado != null)
                {
                    acreditado.fechaAlta = asegurado.fechaAlta;
                    acreditado.sd = Decimal.Parse(asegurado.salarioDiario.ToString());

                    db.Entry(acreditado).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

        }
    }
}
