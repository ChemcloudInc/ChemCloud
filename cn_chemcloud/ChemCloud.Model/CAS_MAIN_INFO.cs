using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class CAS_MAIN_INFO : BaseModel
    {
        #region Model
        private string CASNO;

        public new string CAS_NO
        {
            get { return CASNO; }
            set { CASNO = value; }
        }

        public string CAS_NUMBER
        {
            get;
            set;
        }

        public string PC_SID
        {
            get;
            set;
        }

        public string PUB_CID
        {
            get;
            set;
        }

        public string CHINESE
        {
            get;
            set;
        }



        public string CHINESE_ALIAS
        {
            get;
            set;
        }

        public string ENGLISH
        {
            get;
            set;
        }

        public string ENGLISH_ALIAS
        {
            get;
            set;
        }

        public string MOLECULAR_FORMULA
        {
            get;
            set;
        }

        public string MOLECULAR_WEIGHT
        {
            get;
            set;
        }
        public string PRECISE_QUALITY
        {
            get;
            set;
        }

        public string PSA
        {
            get;
            set;
        }

        public string DENSITY
        {
            get;
            set;
        }
        public string BOILING_POINT
        {
            get;
            set;
        }

        public string FLASH_POINT
        {
            get;
            set;
        }

        public string REFRACTIVE_INDEX
        {
            get;
            set;
        }
        public string VAPOR_PRESSURE
        {
            get;
            set;
        }

        public string CAS_CATEGORY
        {
            get;
            set;
        }

        public string SOURCE_FROM
        {
            get;
            set;
        }
        public string SOURCE_FROM_LINK
        {
            get;
            set;
        }

        public DateTime? AVAILABLE_DATE
        {
            get;
            set;
        }

        public string STRUCTURE_2D
        {
            get;
            set;
        }
        public string EXTERNAL_ID
        {
            get;
            set;
        }
        public string EXTERNAL_LINK
        {
            get;
            set;
        }
        public string COMMENT
        {
            get;
            set;
        }
        public int VERSION
        {
            get;
            set;
        }
        public DateTime? ADD_DATE
        {
            get;
            set;
        }
        public string ADD_AUTHOR
        {
            get;
            set;
        }
        public bool IS_DEL
        {
            get;
            set;
        }
        public long? DEL_UID
        {
            get;
            set;
        }
        public DateTime? DEL_TIME
        {
            get;
            set;
        }
        public bool IS_UPDATE
        {
            get;
            set;
        }
        public long? UPDATE_UID
        {
            get;
            set;
        }
        public DateTime? UPDATE_TIME
        {
            get;
            set;
        }
        public int IS_AUDIT
        {
            get;
            set;
        }
        public long? AUDIT_UID
        {
            get;
            set;
        }
        public DateTime? AUDIT_TIME
        {
            get;
            set;
        }
        public string AUDIT_DESC
        {
            get;
            set;
        }
        public bool IS_LOCK
        {
            get;
            set;
        }
        public int Status { get; set; }
        public string LOG_P { get; set; }
        public string HG_CODE { get; set; }

        public string WGK { get; set; }
        public string DEN_CODE { get; set; }
        public string SAFE_DESC { get; set; }
        public string DEN_SIGN { get; set; }
        public string RongDian { get; set; }
        public string WaiGuanXZ { get; set; }
        #endregion

        public CAS_MAIN_INFO()
        {
        }
        public enum CAS_Status
        {
            [Description("审核通过")]
            YES = 1,
            [Description("待审核")]
            WAIT = 0,
            [Description("审核不通过")]
            NO = -1
        }

        public CAS_MAIN_INFO(CAS_MAIN_INFO m)
            : this()
        {
            CASNO = m.CASNO;
            CAS_NUMBER = m.CAS_NUMBER;
            PC_SID = m.PC_SID;
            PUB_CID = m.PUB_CID;
            CHINESE = m.CHINESE;
            CHINESE_ALIAS = m.CHINESE_ALIAS;
            ENGLISH = m.ENGLISH;
            ENGLISH_ALIAS = m.ENGLISH_ALIAS;
            MOLECULAR_FORMULA = m.ENGLISH_ALIAS;
            MOLECULAR_WEIGHT = m.MOLECULAR_WEIGHT;
            PRECISE_QUALITY = m.ENGLISH_ALIAS;
            PSA = m.PSA;
            DENSITY = m.DENSITY;
            BOILING_POINT = m.BOILING_POINT;
            FLASH_POINT = m.FLASH_POINT;
            REFRACTIVE_INDEX = m.REFRACTIVE_INDEX;
            VAPOR_PRESSURE = m.VAPOR_PRESSURE;
            CAS_CATEGORY = m.CAS_CATEGORY;
            SOURCE_FROM = m.SOURCE_FROM;
            SOURCE_FROM_LINK = m.SOURCE_FROM_LINK;
            AVAILABLE_DATE = m.AVAILABLE_DATE;
            STRUCTURE_2D = m.STRUCTURE_2D;
            EXTERNAL_ID = m.EXTERNAL_ID;
            EXTERNAL_LINK = m.EXTERNAL_LINK;
            COMMENT = m.COMMENT;
            VERSION = m.VERSION;
            ADD_DATE = m.ADD_DATE;
            ADD_AUTHOR = m.ADD_AUTHOR;
            IS_DEL = m.IS_DEL;
            DEL_UID = m.DEL_UID;
            DEL_TIME = m.DEL_TIME;
            IS_UPDATE = m.IS_UPDATE;
            UPDATE_UID = m.UPDATE_UID;
            UPDATE_TIME = m.UPDATE_TIME;
            IS_AUDIT = m.IS_AUDIT;
            AUDIT_UID = m.AUDIT_UID;
            AUDIT_TIME = m.AUDIT_TIME;
            AUDIT_DESC = m.AUDIT_DESC;
            IS_LOCK = m.IS_LOCK;
            Status = m.Status;
            LOG_P = m.LOG_P;
            HG_CODE = m.HG_CODE;
            WGK = m.WGK;
            DEN_CODE = m.DEN_CODE;
            SAFE_DESC = m.SAFE_DESC;
            DEN_SIGN = m.DEN_SIGN;
            RongDian = m.RongDian;
            WaiGuanXZ = m.WaiGuanXZ;
        }
    }
}
