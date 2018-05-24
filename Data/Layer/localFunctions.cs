using System;
using System.Collections.Generic;
using System.Text;
using UpayDll;
using System.Reflection;
using System.Diagnostics;
using UPay.Operations;

namespace UPay.Data.Layer
{    
        internal class trnRequestValues
        {
            public object this[string propertyName]
            {
                get
                {
                    Type myType = typeof(trnRequestValues);
                    PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                    return myPropInfo.GetValue(this, null);
                }
                set
                {
                    Type myType = typeof(trnRequestValues);
                    PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                    myPropInfo.SetValue(this, value, null);
                }
            }
            public string trn_mer_id { get; set; }
            public string trn_cargo_extra { get; set; }
            public string chk_trn_tarifa { get; set; }
            public string trn_amount { get; set; }
            public string trn_auth_code { get; set; }
            public string trn_key { get; set; }
            public string trn_new_room { get; set; }
            public string trn_orig_id { get; set; }
            public string trn_tip_amount { get; set; }
            public string dcs_form { get; set; }
            public string longitud { get; set; }
            public string trn_usr_id { get; set; }
            public string trn_password { get; set; }
            public string trn_cashback_amount { get; set; }
            public string chk_trn_fecha_ingreso { get; set; }
            public string chk_trn_fecha_egreso { get; set; }
            public string trn_qty_pay { get; set; }
            public string trn_referencia_1 { get; set; }
            public string trn_cur_id1 { get; set; }
            public string trn_contrato { get; set; }
            public string trn_card_number { get; set; }
            public string trn_external_mer_id { get; set; }
            public string trn_exp_date { get; set; }
            public string trn_referencia_2 { get; set; }
            public string HOST { get; set; }
            public string PORT { get; set; }
            public string hostStunnel { get; set; }
            public string portStunnel { get; set; }
            public string llave { get; set; }
            public string ter_id { get; set; }
            public string mer_id { get; set; }
            public string terUSD_id { get; set; }
            public string merUSD_id { get; set; }
            public string puertoCom { get; set; }
            public string TIMEOUT { get; set; }
            public string WSFiltrosBM { get; set; }

            private readonly ILogger _logger;

            public trnRequestValues(ILogger logger)
            {
                _logger = logger;
            }

            public bool hasProperty(string methodName)
            {
                object o = this;
                var type = o.GetType();
                return type.GetProperty(methodName) != null;
            }
            public void clear()
            {
                trn_cargo_extra = string.Empty;
                chk_trn_tarifa = string.Empty;
                trn_amount = string.Empty;
                trn_auth_code = string.Empty;
                trn_key = string.Empty;
                trn_new_room = string.Empty;
                trn_orig_id = string.Empty;
                trn_tip_amount = "0";
                dcs_form = string.Empty;
                longitud = string.Empty;
                trn_usr_id = string.Empty;
                trn_password = string.Empty;
                trn_cashback_amount = string.Empty;
                chk_trn_fecha_ingreso = string.Empty;
                chk_trn_fecha_egreso = string.Empty;
                trn_qty_pay = "1";
                trn_referencia_1 = string.Empty;
                trn_cur_id1 = string.Empty;
                trn_mer_id = string.Empty;
                HOST = string.Empty;
                PORT = string.Empty;
                hostStunnel = string.Empty;
                portStunnel = string.Empty; 
                llave = string.Empty; 
                ter_id = string.Empty;
                mer_id = string.Empty;
                terUSD_id = string.Empty; 
                merUSD_id = string.Empty;
                puertoCom = string.Empty;
                TIMEOUT = string.Empty;
                WSFiltrosBM = string.Empty;
            }
        }
    internal class localFunctions
    {
        private readonly ILogger _logger;

        public localFunctions(ILogger logger)
        {
            _logger = logger;
        }

        public enum availableOperatives
        {
            error = 0,
            T028S003 = 1,       //check in
            T060S000 = 2,       //venta online
            T060S002 = 4,       //depósito en garantía
            T060S001 = 8,       //no show
            T028S004 = 16,      //Reautorización
            T132S002 = 32,      //Cambio de turno
            T056S003 = 64,      //check out
            T120S003 = 128,     //ajuste de check out
            T056S001 = 256,     //salida express
            T120S000 = 512,     //cancelación
            T120S002 = 1024,    //cancelación de check in
            T120S001 = 2048,    //ajuste de propina
            T028S005 = 4096,    //cambio de cuarto
            T024S003 = 8192,    //check in offline
            T056S000 = 16384,   //venta offline
            T040S000 = 32768,   //devolución
            PRINT = 65536,       //reimpresión
            CAB = 56,            //cargo Recurrente
            SHOWCONF = 112,         //show conflocal
            UPDATECONF = 224,    //update conflocal
            INIKEY = 448,       //inicializacion de llaves
            UPDATER = 896   //updater

            //KEYINIT = 33554432,
        }
        internal void procesaDatos(trnRequestValues requestValues, string data)
        {
            string[] trnRequest = data.Split('|');
            int contador = 0;
            requestValues.clear();

            foreach (string campo in trnRequest)
            {
                if (contador != 0 && campo != string.Empty)
                {
                    string[] detalle = campo.Split('=');
                    if (requestValues.hasProperty(detalle[0].ToString()))
                        requestValues[detalle[0].ToString()] = detalle[1];
                }
                contador++;
            }
        }
        internal string ejecutaLlamada(trnRequestValues requestValues)
        {
            //_logger.Info("Karen -> ejecutaLlamada");

            Upay u = new Upay();            
            availableOperatives operacion = new availableOperatives();

            bool exists = Enum.IsDefined(typeof(availableOperatives), requestValues.dcs_form);                       
           
            if (exists)
                operacion = (availableOperatives)System.Enum.Parse(typeof(availableOperatives), requestValues.dcs_form);
            else
                operacion = availableOperatives.error;

            string result = string.Empty;
            switch (operacion)
            {
                case availableOperatives.PRINT:                    
                    #region reimpresion
                    result = u.opspreimp(requestValues.trn_orig_id);                    
                    break;
                    #endregion
                case availableOperatives.INIKEY:
                    #region inicializacion
                    result = u.opcpiniciallave();
                    break;
                    #endregion
                case availableOperatives.T060S000:
                    #region venta
                    result = u.opcpventa(requestValues.trn_amount, requestValues.trn_qty_pay);                    
                    break;
                    #endregion                
                case availableOperatives.T040S000:
                    #region devolucion
                    result = u.opcpdevolucion(requestValues.trn_orig_id, requestValues.trn_auth_code, requestValues.trn_amount);                    
                    break;
                    #endregion
                case availableOperatives.T120S001:
                    #region ajuste
                    result = u.opspajuste(requestValues.trn_orig_id, requestValues.trn_amount);                    
                    break;
                    #endregion
                case availableOperatives.T120S000:
                    #region cancelación
                    result = u.opspcancel(requestValues.trn_orig_id);
                    break;
                    #endregion
                case availableOperatives.SHOWCONF:
                    #region showConflocal
                    result = u.opspconf();                    
                    break;
                    #endregion
                case availableOperatives.UPDATECONF:
                    #region updateConflocal                    
                    result = u.opspupdateconf(requestValues.HOST,requestValues.PORT,requestValues.hostStunnel,requestValues.portStunnel, requestValues.TIMEOUT, requestValues.llave,requestValues.mer_id,requestValues.ter_id,requestValues.merUSD_id,requestValues.terUSD_id,requestValues.puertoCom);                    
                    break;
                    #endregion                
                default:
                    result = "|trn_internal_respcode=-2|trn_msg_host=Operativa No Existe|dcs_status=ERROR|";
                    break;
                        
            }
            result = "|Respuesta="+ result + "|dcs_form=" + operacion.ToString() + "|";                     
            return result;
        }
    }


}

