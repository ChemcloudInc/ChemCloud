using ChemCloud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemCloud.Model
{
    /// <summary>
    /// 平台CAS库主表  add xue
    /// </summary>
    [Serializable]
    public class CASInfo : BaseModel
    {
        public CASInfo() { }

        #region Model
        /// <summary>
        /// CID
        /// </summary>
        private int _pub_cid;
        public int Pub_CID
        {
            set { _pub_cid = value; }
            get { return _pub_cid; }
        }
        /// <summary>
        /// 中文名
        /// </summary>
        public string CHINESE
        {
            get;
            set;
        }
        /// <summary>
        /// 中文别名
        /// </summary>
        public string CHINESE_ALIAS
        {
            get;
            set;
        }
        /// <summary>
        /// HS编码
        /// </summary>
        public string HS_CODE
        {
            get;
            set;
        }
        /// <summary>
        /// 安全说明
        /// </summary>
        public string SAFE_DESC
        {
            get;
            set;
        }
        /// <summary>
        /// 二维结构图
        /// </summary>
        public string C2D_Structure
        {
            get;
            set;
        }
        /// <summary>
        /// 名称和标识符
        /// </summary>
        public string Names_and_Identifiers
        {
            get;
            set;
        }
        /// <summary>
        /// 记录标题
        /// </summary>
        public string Record_Title
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Record_Description
        {
            get;
            set;
        }
        /// <summary>
        /// 化学名
        /// </summary>
        public string Computed_Descriptors
        {
            get;
            set;
        }
        /// <summary>
        /// 化学名
        /// </summary>
        public string IUPAC_Name
        {
            get;
            set;
        }
        /// <summary>
        /// InChI
        /// </summary>
        public string InChI
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string InChI_Key
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Canonical_SMILES
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other_Identifiers
        {
            get;
            set;
        }
        /// <summary>
        /// 同义词
        /// </summary>
        public string Synonyms
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MeSH_Synonyms
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Depositor_Supplied_Synonyms
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Chemical_and_Physical_Properties
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Computed_Properties
        {
            get;
            set;
        }
        /// <summary>
        /// 分子量
        /// </summary>
        public string Molecular_Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 分子式
        /// </summary>
        public string Molecular_Formula
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string XLogP3
        {
            get;
            set;
        }
        /// <summary>
        /// 精确质量
        /// </summary>
        public string Exact_Mass
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Monoisotopic_Mass
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Topological_Polar_Surface_Area
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Experimental_Properties
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Solubility
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LogP
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Related_Records
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Related_Compounds_with_Annotation
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Parent_Compound
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Related_Compounds
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Substances
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Related_Substances
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Substances_by_Category
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Absorption
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Identification
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Safety_and_Hazards
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Accidental_Release_Measures
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Disposal_Methods
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Regulatory_Information
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Toxicity
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Toxicological_Information
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Interactions
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Antidote_and_Emergency_Treatment
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Human_Toxicity_Excerpts
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Non_Human_Toxicity_Excerpts
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Populations_at_Special_Risk
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Depositor_Provided_PubMed_Citations
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NLM_Curated_PubMed_Citations
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Classification
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string C3D_Status
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string EC_Number
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Spectral_Properties
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string GC_MS
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string GC_MS_Fields
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string GC_MS_Images
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MS_MS
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MS_MS_Fields
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string MS_MS_Images
        {
            get;
            set;
        }
        /// <summary>
        /// CAS
        /// </summary>
        public string CAS
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UNII
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Physical_Description
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Kovats_Retention_Index
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Crystal_Structures
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CCDC_Number
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Crystal_Structure_Data
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Protein_Bound_3_D_Structures
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Biologic_Line_Notation
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Boiling_Point
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Melting_Point
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string pKa
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ICSC_Number
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RTECS_Number
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UN_Number
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Color
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Odor
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Taste
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Flash_Point
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Density
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Vapor_Density
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Vapor_Pressure
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LogS
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Stability
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Auto_Ignition
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Decomposition
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Viscosity
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Corrosivity
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Heat_of_Combustion
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Heat_of_Vaporization
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Surface_Tension
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Chemical_Classes
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OSHA_Chemical_Sampling
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NIOSH_Analytical_Methods
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Hazards_Identification
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string GHS_Classification
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Health_Hazard
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fire_Hazard
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Explosion_Hazard
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Hazards_Summary
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fire_Potential
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Safety_and_Hazard_Properties
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string LEL
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UEL
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Flammability
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Critical_Temperature
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Critical_Pressure
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NFPA_Hazard_Classification
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NFPA_Fire_Rating
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NFPA_Health_Rating
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Physical_Dangers
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Chemical_Dangers
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Explosive_Limits_and_Potential
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OSHA_Standards
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NIOSH_Recommendations
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fire_Fighting_Measures
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fire_Fighting
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Explosion_Fire_Fighting
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other_Fire_Fighting_Hazards
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Spillage_Disposal
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Cleanup_Methods
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other_Preventative_Measures
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Handling_and_Storage
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Nonfire_Spill_Response
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Safe_Storage
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Storage_Conditions
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Air_and_Water_Reactions
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Reactive_Group
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Reactivity_Alerts
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Reactivities_and_Incompatibilities
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Transport_Information
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOT_Emergency_Guidelines
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Shipment_Methods_and_Regulations
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOT_ID_and_Guide
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOT_Label
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Packaging_and_Labelling
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string EC_Classification
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string UN_Classification
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Emergency_Response
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string DOT_Emergency_Response_Guide
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Federal_Drinking_Water_Standards
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CERCLA_Reportable_Quantities
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string TSCA_Requirements
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RCRA_Requirements
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other_Safety_Information
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Toxic_Combustion_Products
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Isomeric_SMILES
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Federal_Drinking_Water_Guidelines
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Dissociation_Constants
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string pH
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Drug_Warning
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Over_the_Counter_Drug_Products
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OTC_Drug_Ingredient
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OTC_Proprietary_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string OTC_Applicant
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string FIFRA_Requirements
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Other_Hazardous_Reactions
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Prescription_Drug_Products
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RX_Drug_Ingredient
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RX_Proprietary_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string RX_Applicant
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string NFPA_Reactivity_Rating
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string FDA_Orange_Book_Patents
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string FDA_Orange_Book_Patent_ID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string FDA_Orange_Book_Patent_Expiration
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string FDA_Orange_Book_Patent_Applicant
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string FDA_Orange_Book_Patent_Drug_Application
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Isolation_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Isolation_Distance
        {
            get;
            set;
        }
        #endregion
    }
}
