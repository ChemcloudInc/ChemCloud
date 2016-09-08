using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.QueryModel;
using ChemCloud.Web.Framework;
using ChemCloud.Web.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Admin.Controllers
{
    public class CASController : BaseAdminController
    {

        public ActionResult ManageMent()
        {
            return View();
        }
        public ActionResult DManageMent()
        {
            return View();
        }
        public ActionResult WManageMent()
        {
            return View();
        }
        // GET: Admin/CAS
        public ActionResult Index()
        {

            return View();
        }
        [Description("分页获取cas管理JSON数据")]
        [HttpPost]
        [UnAuthorize]
        public JsonResult List(int page, int rows, string CAS, string CHINESE, string Molecular_Formula, string type)
        {
            ICASInfoService casService = ServiceHelper.Create<ICASInfoService>();
            CASInfoQuery casQuery = new CASInfoQuery()
            {
                CAS = CAS,
                CHINESE = CHINESE,
                Molecular_Formula = Molecular_Formula,
                PageNo = page,
                PageSize = rows
            };
            PageModel<CASInfo> cas = casService.GetCasList(casQuery);
            IEnumerable<CASInfo> models =
                from item in cas.Models.ToArray()
                select new CASInfo()
                {
                    #region model
                    CHINESE = item.CHINESE,
                    CHINESE_ALIAS = item.CHINESE_ALIAS,
                    HS_CODE = item.HS_CODE,
                    SAFE_DESC = item.SAFE_DESC,
                    Pub_CID = item.Pub_CID,
                    C2D_Structure = item.C2D_Structure,
                    Names_and_Identifiers = item.Names_and_Identifiers,
                    Record_Title = item.Record_Title,
                    Record_Description = item.Record_Description,
                    Computed_Descriptors = item.Computed_Descriptors,
                    IUPAC_Name = item.IUPAC_Name,
                    InChI = item.InChI,
                    InChI_Key = item.InChI_Key,
                    Canonical_SMILES = item.Canonical_SMILES,
                    Other_Identifiers = item.Other_Identifiers,
                    Synonyms = item.Synonyms,
                    MeSH_Synonyms = item.MeSH_Synonyms,
                    Depositor_Supplied_Synonyms = item.Depositor_Supplied_Synonyms,
                    Chemical_and_Physical_Properties = item.Chemical_and_Physical_Properties,
                    Computed_Properties = item.Computed_Properties,
                    Molecular_Weight = item.Molecular_Weight,
                    Molecular_Formula = item.Molecular_Formula,
                    XLogP3 = item.XLogP3,
                    Exact_Mass = item.Exact_Mass,
                    Monoisotopic_Mass = item.Monoisotopic_Mass,
                    Topological_Polar_Surface_Area = item.Topological_Polar_Surface_Area,
                    Experimental_Properties = item.Experimental_Properties,
                    Solubility = item.Solubility,
                    LogP = item.LogP,
                    Related_Records = item.Related_Records,
                    Related_Compounds_with_Annotation = item.Related_Compounds_with_Annotation,
                    Parent_Compound = item.Parent_Compound,
                    Related_Compounds = item.Related_Compounds,
                    Substances = item.Substances,
                    Related_Substances = item.Related_Substances,
                    Substances_by_Category = item.Substances_by_Category,
                    Absorption = item.Absorption,
                    Identification = item.Identification,
                    Safety_and_Hazards = item.Safety_and_Hazards,
                    Accidental_Release_Measures = item.Accidental_Release_Measures,
                    Disposal_Methods = item.Disposal_Methods,
                    Regulatory_Information = item.Regulatory_Information,
                    Toxicity = item.Toxicity,
                    Toxicological_Information = item.Toxicological_Information,
                    Interactions = item.Interactions,
                    Antidote_and_Emergency_Treatment = item.Antidote_and_Emergency_Treatment,
                    Human_Toxicity_Excerpts = item.Human_Toxicity_Excerpts,
                    Non_Human_Toxicity_Excerpts = item.Non_Human_Toxicity_Excerpts,
                    Populations_at_Special_Risk = item.Populations_at_Special_Risk,
                    Depositor_Provided_PubMed_Citations = item.Depositor_Provided_PubMed_Citations,
                    NLM_Curated_PubMed_Citations = item.NLM_Curated_PubMed_Citations,
                    Classification = item.Classification,
                    C3D_Status = item.C3D_Status,
                    EC_Number = item.EC_Number,
                    Spectral_Properties = item.Spectral_Properties,
                    GC_MS = item.GC_MS,
                    GC_MS_Fields = item.GC_MS_Fields,
                    GC_MS_Images = item.GC_MS_Images,
                    MS_MS = item.MS_MS,
                    MS_MS_Fields = item.MS_MS_Fields,
                    MS_MS_Images = item.MS_MS_Images,
                    CAS = item.CAS,
                    UNII = item.UNII,
                    Physical_Description = item.Physical_Description,
                    Kovats_Retention_Index = item.Kovats_Retention_Index,
                    Crystal_Structures = item.Crystal_Structures,
                    CCDC_Number = item.CCDC_Number,
                    Crystal_Structure_Data = item.Crystal_Structure_Data,
                    Protein_Bound_3_D_Structures = item.Protein_Bound_3_D_Structures,
                    Biologic_Line_Notation = item.Biologic_Line_Notation,
                    Boiling_Point = item.Boiling_Point,
                    Melting_Point = item.Melting_Point,
                    pKa = item.pKa,
                    ICSC_Number = item.ICSC_Number,
                    RTECS_Number = item.RTECS_Number,
                    UN_Number = item.UN_Number,
                    Color = item.Color,
                    Odor = item.Odor,
                    Taste = item.Taste,
                    Flash_Point = item.Flash_Point,
                    Density = item.Density,
                    Vapor_Density = item.Vapor_Density,
                    Vapor_Pressure = item.Vapor_Pressure,
                    LogS = item.LogS,
                    Stability = item.Stability,
                    Auto_Ignition = item.Auto_Ignition,
                    Decomposition = item.Decomposition,
                    Viscosity = item.Viscosity,
                    Corrosivity = item.Corrosivity,
                    Heat_of_Combustion = item.Heat_of_Combustion,
                    Heat_of_Vaporization = item.Heat_of_Vaporization,
                    Surface_Tension = item.Surface_Tension,
                    Chemical_Classes = item.Chemical_Classes,
                    OSHA_Chemical_Sampling = item.OSHA_Chemical_Sampling,
                    NIOSH_Analytical_Methods = item.NIOSH_Analytical_Methods,
                    Hazards_Identification = item.Hazards_Identification,
                    GHS_Classification = item.GHS_Classification,
                    Health_Hazard = item.Health_Hazard,
                    Fire_Hazard = item.Fire_Hazard,
                    Explosion_Hazard = item.Explosion_Hazard,
                    Hazards_Summary = item.Hazards_Summary,
                    Fire_Potential = item.Fire_Potential,
                    Safety_and_Hazard_Properties = item.Safety_and_Hazard_Properties,
                    LEL = item.LEL,
                    UEL = item.UEL,
                    Flammability = item.Flammability,
                    Critical_Temperature = item.Critical_Temperature,
                    Critical_Pressure = item.Critical_Pressure,
                    NFPA_Hazard_Classification = item.NFPA_Hazard_Classification,
                    NFPA_Fire_Rating = item.NFPA_Fire_Rating,
                    NFPA_Health_Rating = item.NFPA_Health_Rating,
                    Physical_Dangers = item.Physical_Dangers,
                    Chemical_Dangers = item.Chemical_Dangers,
                    Explosive_Limits_and_Potential = item.Explosive_Limits_and_Potential,
                    OSHA_Standards = item.OSHA_Standards,
                    NIOSH_Recommendations = item.NIOSH_Recommendations,
                    Fire_Fighting_Measures = item.Fire_Fighting_Measures,
                    Fire_Fighting = item.Fire_Fighting,
                    Explosion_Fire_Fighting = item.Explosion_Fire_Fighting,
                    Other_Fire_Fighting_Hazards = item.Other_Fire_Fighting_Hazards,
                    Spillage_Disposal = item.Spillage_Disposal,
                    Cleanup_Methods = item.Cleanup_Methods,
                    Other_Preventative_Measures = item.Other_Preventative_Measures,
                    Handling_and_Storage = item.Handling_and_Storage,
                    Nonfire_Spill_Response = item.Nonfire_Spill_Response,
                    Safe_Storage = item.Safe_Storage,
                    Storage_Conditions = item.Storage_Conditions,
                    Air_and_Water_Reactions = item.Air_and_Water_Reactions,
                    Reactive_Group = item.Reactive_Group,
                    Reactivity_Alerts = item.Reactivity_Alerts,
                    Reactivities_and_Incompatibilities = item.Reactivities_and_Incompatibilities,
                    Transport_Information = item.Transport_Information,
                    DOT_Emergency_Guidelines = item.DOT_Emergency_Guidelines,
                    Shipment_Methods_and_Regulations = item.Shipment_Methods_and_Regulations,
                    DOT_ID_and_Guide = item.DOT_ID_and_Guide,
                    DOT_Label = item.DOT_Label,
                    Packaging_and_Labelling = item.Packaging_and_Labelling,
                    EC_Classification = item.EC_Classification,
                    UN_Classification = item.UN_Classification,
                    Emergency_Response = item.Emergency_Response,
                    DOT_Emergency_Response_Guide = item.DOT_Emergency_Response_Guide,
                    Federal_Drinking_Water_Standards = item.Federal_Drinking_Water_Standards,
                    CERCLA_Reportable_Quantities = item.CERCLA_Reportable_Quantities,
                    TSCA_Requirements = item.TSCA_Requirements,
                    RCRA_Requirements = item.RCRA_Requirements,
                    Other_Safety_Information = item.Other_Safety_Information,
                    Toxic_Combustion_Products = item.Toxic_Combustion_Products,
                    Isomeric_SMILES = item.Isomeric_SMILES,
                    Status = item.Status,
                    Federal_Drinking_Water_Guidelines = item.Federal_Drinking_Water_Guidelines,
                    Dissociation_Constants = item.Dissociation_Constants,
                    pH = item.pH,
                    Drug_Warning = item.Drug_Warning,
                    Over_the_Counter_Drug_Products = item.Over_the_Counter_Drug_Products,
                    OTC_Drug_Ingredient = item.OTC_Drug_Ingredient,
                    OTC_Proprietary_Name = item.OTC_Proprietary_Name,
                    OTC_Applicant = item.OTC_Applicant,
                    FIFRA_Requirements = item.FIFRA_Requirements,
                    Other_Hazardous_Reactions = item.Other_Hazardous_Reactions,
                    Prescription_Drug_Products = item.Prescription_Drug_Products,
                    RX_Drug_Ingredient = item.RX_Drug_Ingredient,
                    RX_Proprietary_Name = item.RX_Proprietary_Name,
                    RX_Applicant = item.RX_Applicant,
                    NFPA_Reactivity_Rating = item.NFPA_Reactivity_Rating,
                    FDA_Orange_Book_Patents = item.FDA_Orange_Book_Patents,
                    FDA_Orange_Book_Patent_ID = item.FDA_Orange_Book_Patent_ID,
                    FDA_Orange_Book_Patent_Expiration = item.FDA_Orange_Book_Patent_Expiration,
                    FDA_Orange_Book_Patent_Applicant = item.FDA_Orange_Book_Patent_Applicant,
                    FDA_Orange_Book_Patent_Drug_Application = item.FDA_Orange_Book_Patent_Drug_Application,
                    Isolation_Name = item.Isolation_Name,
                    Isolation_Distance = item.Isolation_Distance
                    #endregion
                };
            DataGridModel<CASInfo> dataGridModel = new DataGridModel<CASInfo>()
            {
                rows = models,
                total = cas.Total
            };
            return Json(dataGridModel);
        }

        public ActionResult Details(int cid)
        {
            CASInfo cas = new CASInfo();
            cas = ServiceHelper.Create<ICASInfoService>().GetCASInfoByCid(cid);
            return View(cas);
        }

        [Description("CAS审核")]
        [HttpPost]
        [UnAuthorize]
        public ActionResult Success(string CAS_NO, string CAS_STATUS)
        {
            //ServiceHelper.Create<ICASInfoService>().UpdateCASStatus(CAS_NO, CAS_STATUS);
            return Json(new { Successful = true });
        }

        [Description("CAS详情编辑")]
        public ActionResult Edit(string CAS_NO)
        {
            CASInfo casno = ServiceHelper.Create<ICASInfoService>().GetCASByCAS_NO(CAS_NO);
            return View(casno);
        }

        public ActionResult Editing(CASInfo casinfo)
        {
            ServiceHelper.Create<ICASInfoService>().UpdateCAS(casinfo);
            //IOperationLogService operationLogService = ServiceHelper.Create<IOperationLogService>();
            //LogInfo logInfo = new LogInfo()
            //{
            //    Date = DateTime.Now,
            //    Description = string.Concat("修改CAS信息，GUID=", casinfo.Pub_CID),
            //    IPAddress = base.Request.UserHostAddress,
            //    PageUrl = string.Concat("/CAS/Edit/", casinfo.CAS_NO),
            //    UserName = base.CurrentManager.UserName,
            //    ShopId = 0
            //};
            //operationLogService.AddPlatformOperationLog(logInfo);
            return Json(new { Successful = true });
        }

        public ActionResult Audit(string CAS_NO)
        {
            CASInfo casno = ServiceHelper.Create<ICASInfoService>().GetCASByCAS_NO(CAS_NO);
            return View(casno);
        }

    }
}
