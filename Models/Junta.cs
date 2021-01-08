using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE.Models
{
    public class Junta
    {
        public int COD_PROVINCIA { get; set; }
        public int COD_CANTON { get; set; }
        public int COD_PARROQUIA{ get; set; }
        public int COD_ZONA{ get; set; }
        public char SEXO{ get; set; }
        public int JUNTA{ get; set; }
        [Key]
        public int COD_JUNTA{ get; set; }
        public int NUMELE_JUNTA{ get; set; }
        public char STATUS_JUNTA{ get; set; }
        public string ESTJUNNIVJUN_JUNTA{ get; set; } //VARCHAR2(1 BYTE),
        public int NUMELENIVJUN_JUNTA{ get; set; }
        public string ESTJUNNIVREC_JUNTA{ get; set; }   //VARCHAR2(1 BYTE),
        public int NUMELENIVREC_JUNTA{ get; set; }
        public string ESTJUNNIVZON_JUNTA{ get; set; }   //VARCHAR2(1 BYTE),
        public int NUMELNIVZON_JUNTA{ get; set; }
        public string ESTJUNNIVPAD_JUNTA{ get; set; } //  VARCHAR2(1 BYTE),
        public int NUMELENIVPAD_JUNTA{ get; set; }
        public char CEDPRI_JUNTA{ get; set; }         //CHAR(9 BYTE),
        public char DIGPRI_JUNTA{ get; set; } //CHAR(1 BYTE),
        public string NOMPRI_JUNTA{ get; set; } //CHAR(50 BYTE),
        public string CEDULT_JUNTA{ get; set; } //CHAR(9 BYTE),
        public char DIGJULT_JUNTA{ get; set; } //CHAR(1 BYTE),
        public string NOMULT_JUNTA{ get; set; } //CHAR(50 BYTE),
        public string EST2A{ get; set; } //VARCHAR2(1 BYTE),
        public int NUM2A{ get; set; }
        public string EST2D{ get; set; } //             VARCHAR2(1 BYTE),
        public int NUM2D{ get; set; }
        public string EST3A{ get; set; }  //               VARCHAR2(1 BYTE),
        public int NUM3A{ get; set; }
        public string EST3D{ get; set; } //                VARCHAR2(1 BYTE),
        public int NUM3D{ get; set; }
        public string ESTJUNSEX{ get; set; } //           VARCHAR2(1 BYTE),
        public int NUMJUNSEX{ get; set; }
        public int COD_SEG_JUNTA{ get; set; }
        public int COD_SEG_NAC{ get; set; }
        public int COD_SEG_PRO{ get; set; }
        public int COD1{ get; set; }
        public int COD2{ get; set; }
        public int COD3{ get; set; }
        public int COD4{ get; set; }
        public int COD5{ get; set; }
        public int COD6{ get; set; }
        public int COD7{ get; set; }
        public int COD8{ get; set; }
        public int COD9{ get; set; }
        public int COD_RECINTO{ get; set; }
        public int COD_JIE{ get; set; }
        public int COD_PROCESO{ get; set; }
        public int COD_CIRCUNSCRIPCION{ get; set; }
    }
}
