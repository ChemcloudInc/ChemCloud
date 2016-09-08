using ChemCloud.Core;
using ChemCloud.Core.Helper;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Data;
using ChemCloud.DBUtility;

namespace ChemCloud.Service
{
    public class CASInfoService : ServiceBase, ICASInfoService, IService, IDisposable
    {
        public CASInfoService()
        {
        }

        /*根据CASNO查询casinfo*/
        public CASInfo GetCASByNo(string CASNo)
        {
            CASInfo _CASInfo = new CASInfo();

            string strsql = string.Format("SELECT * FROM ChemCloud_CAS with(nolock) where CAS='{0}'", CASNo);
            DataTable dt = DbHelperSQLCAS.QueryDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                _CASInfo = GetEntity<CASInfo>(dt);
            }
            return _CASInfo;
        }

        /*根据PUB_CID 检索casifno*/
        public CASInfo GetCASByPUB_CID(int PUB_CID)
        {
            CASInfo _CASInfo = new CASInfo();

            string strsql = string.Format(@"SELECT 
            PUB_CID,CAS,CHINESE,Record_Title,CHINESE_ALIAS,Record_Description,Molecular_Formula,Molecular_Weight,
            Density,Boiling_Point,Flash_Point,Vapor_Pressure,Exact_Mass,LogP,
            Physical_Description,Melting_Point,Solubility,Storage_Conditions,
            Safety_and_Hazards,HS_CODE,[2D_Structure] as C2D_Structure
            FROM ChemCloud_CAS with(nolock) where Pub_CID='{0}'", PUB_CID);
            DataTable dt = DbHelperSQLCAS.QueryDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                _CASInfo = GetEntity<CASInfo>(dt);
            }
            return _CASInfo;
        }


        public static T GetEntity<T>(DataTable table) where T : new()
        {
            T entity = new T();
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                        }

                    }
                }
            }

            return entity;
        }

        public static IList<T> GetEntities<T>(DataTable table) where T : new()
        {
            IList<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                }
                entities.Add(entity);
            }
            return entities;
        }

        public Model.PageModel<Model.CASInfo> GetCasList(QueryModel.CASInfoQuery casQuery)
        {
            int num = 0;

            IQueryable<CASInfo> cas = context.CASInfo.AsQueryable<CASInfo>();
            if (!string.IsNullOrWhiteSpace(casQuery.CHINESE))
            {
                cas =
                    from d in cas
                    where d.CHINESE.Equals(casQuery.CHINESE)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(casQuery.Molecular_Formula))
            {
                cas =
                    from d in cas
                    where d.Molecular_Formula.Equals(casQuery.Molecular_Formula)
                    select d;
            }
            if (!string.IsNullOrWhiteSpace(casQuery.CAS))
            {
                cas =
                    from d in cas
                    where d.CAS.Equals(casQuery.CAS)
                    select d;
            }
            cas = cas.GetPage(out num, casQuery.PageNo, casQuery.PageSize, (IQueryable<CASInfo> d) =>
                from o in d
                orderby o.Pub_CID ascending
                select o);
            return new PageModel<CASInfo>()
            {

                Models = cas,
                Total = num
            };
        }

        public Model.PageModel<Model.CASInfo> PageSearchCasList(QueryModel.CASInfoQuery casQuery)
        {
            int num = 0;
            try
            {
                IQueryable<CASInfo> cas = context.CASInfo.AsQueryable<CASInfo>();

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBER))
                {
                    cas =
                        from d in cas
                        where d.Pub_CID.Equals(casQuery.CAS_NUMBER) || d.CHINESE.Equals(casQuery.CAS_NUMBER) || d.CHINESE_ALIAS.Equals(casQuery.CAS_NUMBER) || d.Record_Title.Equals(casQuery.CAS_NUMBER)
                        select d;
                }

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBERList))
                {
                    List<string> listcas = new List<string>();
                    string[] array = casQuery.CAS_NUMBERList.Split('~');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(array[i]))
                        {
                            listcas.Add(array[i]);
                        }
                    }
                    cas =
                        from d in cas
                        where (from p in listcas select p).Contains(d.Pub_CID.ToString())
                        select d;
                }


                cas = cas.GetPage(out num, casQuery.PageNo, casQuery.PageSize, (IQueryable<CASInfo> d) =>
                    from o in d
                    orderby o.Pub_CID ascending
                    select o);
                return new PageModel<CASInfo>()
                {

                    Models = cas,
                    Total = num
                };
            }
            catch (Exception) { return null; }
        }

        public CASInfo GetCASByCAS_NO(string CAS_NO)
        {
            CASInfo _CASInfo = new CASInfo();

            string strsql = string.Format("SELECT * FROM ChemCloud_CAS with(nolock) where CAS='{0}'", CAS_NO);
            DataTable dt = DbHelperSQLCAS.QueryDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                _CASInfo = GetEntity<CASInfo>(dt);
            }
            return _CASInfo;
        }

        public CASInfo GetCASInfoByCid(int cid) {
            CASInfo _CASInfo = new CASInfo();
            string strsql = string.Format("SELECT * FROM ChemCloud_CAS with(nolock) where Pub_CID={0}", cid);
            DataTable dt = DbHelperSQLCAS.QueryDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                _CASInfo = GetEntity<CASInfo>(dt);
            }
            return _CASInfo;
        }

        public CASInfo GetCASByCASNO(string CAS_NO)
        {
            CASInfo _CASInfo = new CASInfo();

            string strsql = string.Format("SELECT * FROM ChemCloud_CAS with(nolock) where CAS='{0}'", CAS_NO);
            DataTable dt = DbHelperSQLCAS.QueryDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                _CASInfo = GetEntity<CASInfo>(dt);
            }

            if (_CASInfo == null)
            {
                CASInfo info = new CASInfo();
                info.Pub_CID = 0;
                return info; ;
            }
            else
            {
                return _CASInfo;
            }

        }

        public void AddCAS(CASInfo casinfo)
        {
            context.CASInfo.Add(casinfo);
            context.SaveChanges();
        }

        public void UpdateCAS(CASInfo casinfo)
        {
            CASInfo cas = context.CASInfo.FirstOrDefault((CASInfo m) => m.Pub_CID == casinfo.Pub_CID);
            cas.CHINESE = casinfo.CHINESE;
            cas.CHINESE_ALIAS = casinfo.CHINESE_ALIAS;
            cas.HS_CODE = casinfo.HS_CODE;
            cas.SAFE_DESC = casinfo.SAFE_DESC;
            cas.Pub_CID = casinfo.Pub_CID;
            cas.C2D_Structure = casinfo.C2D_Structure;
            cas.Names_and_Identifiers = casinfo.Names_and_Identifiers;
            cas.Record_Title = casinfo.Record_Title;
            cas.Record_Description = casinfo.Record_Description;
            cas.Computed_Descriptors = casinfo.Computed_Descriptors;
            cas.IUPAC_Name = casinfo.IUPAC_Name;
            cas.InChI = casinfo.InChI;
            cas.InChI_Key = casinfo.InChI_Key;
            cas.Canonical_SMILES = casinfo.Canonical_SMILES;
            cas.Other_Identifiers = casinfo.Other_Identifiers;
            cas.Synonyms = casinfo.Synonyms;
            cas.MeSH_Synonyms = casinfo.MeSH_Synonyms;
            cas.Depositor_Supplied_Synonyms = casinfo.Depositor_Supplied_Synonyms;
            cas.Chemical_and_Physical_Properties = casinfo.Chemical_and_Physical_Properties;
            cas.Computed_Properties = casinfo.Computed_Properties;
            cas.Molecular_Weight = casinfo.Molecular_Weight;
            cas.Molecular_Formula = casinfo.Molecular_Formula;
            cas.XLogP3 = casinfo.XLogP3;
            cas.Exact_Mass = casinfo.Exact_Mass;
            cas.Monoisotopic_Mass = casinfo.Monoisotopic_Mass;
            cas.Topological_Polar_Surface_Area = casinfo.Topological_Polar_Surface_Area;
            cas.Experimental_Properties = casinfo.Experimental_Properties;
            cas.Solubility = casinfo.Solubility;
            cas.LogP = casinfo.LogP;
            cas.Related_Records = casinfo.Related_Records;
            cas.Related_Compounds_with_Annotation = casinfo.Related_Compounds_with_Annotation;
            cas.Parent_Compound = casinfo.Parent_Compound;
            cas.Related_Compounds = casinfo.Related_Compounds;
            cas.Substances = casinfo.Substances;
            cas.Related_Substances = casinfo.Related_Substances;
            cas.Substances_by_Category = casinfo.Substances_by_Category;
            cas.Absorption = casinfo.Absorption;
            cas.Identification = casinfo.Identification;
            cas.Safety_and_Hazards = casinfo.Safety_and_Hazards;
            cas.Accidental_Release_Measures = casinfo.Accidental_Release_Measures;
            cas.Disposal_Methods = casinfo.Disposal_Methods;
            cas.Regulatory_Information = casinfo.Regulatory_Information;
            cas.Toxicity = casinfo.Toxicity;
            cas.Toxicological_Information = casinfo.Toxicological_Information;
            cas.Interactions = casinfo.Interactions;
            cas.Antidote_and_Emergency_Treatment = casinfo.Antidote_and_Emergency_Treatment;
            cas.Human_Toxicity_Excerpts = casinfo.Human_Toxicity_Excerpts;
            cas.Non_Human_Toxicity_Excerpts = casinfo.Non_Human_Toxicity_Excerpts;
            cas.Populations_at_Special_Risk = casinfo.Populations_at_Special_Risk;
            cas.Depositor_Provided_PubMed_Citations = casinfo.Depositor_Provided_PubMed_Citations;
            cas.NLM_Curated_PubMed_Citations = casinfo.NLM_Curated_PubMed_Citations;
            cas.Classification = casinfo.Classification;
            cas.C3D_Status = casinfo.C3D_Status;
            cas.EC_Number = casinfo.EC_Number;
            cas.Spectral_Properties = casinfo.Spectral_Properties;
            cas.GC_MS = casinfo.GC_MS;
            cas.GC_MS_Fields = casinfo.GC_MS_Fields;
            cas.GC_MS_Images = casinfo.GC_MS_Images;
            cas.MS_MS = casinfo.MS_MS;
            cas.MS_MS_Fields = casinfo.MS_MS_Fields;
            cas.MS_MS_Images = casinfo.MS_MS_Images;
            cas.CAS = casinfo.CAS;
            cas.UNII = casinfo.UNII;
            cas.Physical_Description = casinfo.Physical_Description;
            cas.Kovats_Retention_Index = casinfo.Kovats_Retention_Index;
            cas.Crystal_Structures = casinfo.Crystal_Structures;
            cas.CCDC_Number = casinfo.CCDC_Number;
            cas.Crystal_Structure_Data = casinfo.Crystal_Structure_Data;
            cas.Protein_Bound_3_D_Structures = casinfo.Protein_Bound_3_D_Structures;
            cas.Biologic_Line_Notation = casinfo.Biologic_Line_Notation;
            cas.Boiling_Point = casinfo.Boiling_Point;
            cas.Melting_Point = casinfo.Melting_Point;
            cas.pKa = casinfo.pKa;
            cas.ICSC_Number = casinfo.ICSC_Number;
            cas.RTECS_Number = casinfo.RTECS_Number;
            cas.UN_Number = casinfo.UN_Number;
            cas.Color = casinfo.Color;
            cas.Odor = casinfo.Odor;
            cas.Taste = casinfo.Taste;
            cas.Flash_Point = casinfo.Flash_Point;
            cas.Density = casinfo.Density;
            cas.Vapor_Density = casinfo.Vapor_Density;
            cas.Vapor_Pressure = casinfo.Vapor_Pressure;
            cas.LogS = casinfo.LogS;
            cas.Stability = casinfo.Stability;
            cas.Auto_Ignition = casinfo.Auto_Ignition;
            cas.Decomposition = casinfo.Decomposition;
            cas.Viscosity = casinfo.Viscosity;
            cas.Corrosivity = casinfo.Corrosivity;
            cas.Heat_of_Combustion = casinfo.Heat_of_Combustion;
            cas.Heat_of_Vaporization = casinfo.Heat_of_Vaporization;
            cas.Surface_Tension = casinfo.Surface_Tension;
            cas.Chemical_Classes = casinfo.Chemical_Classes;
            cas.OSHA_Chemical_Sampling = casinfo.OSHA_Chemical_Sampling;
            cas.NIOSH_Analytical_Methods = casinfo.NIOSH_Analytical_Methods;
            cas.Hazards_Identification = casinfo.Hazards_Identification;
            cas.GHS_Classification = casinfo.GHS_Classification;
            cas.Health_Hazard = casinfo.Health_Hazard;
            cas.Fire_Hazard = casinfo.Fire_Hazard;
            cas.Explosion_Hazard = casinfo.Explosion_Hazard;
            cas.Hazards_Summary = casinfo.Hazards_Summary;
            cas.Fire_Potential = casinfo.Fire_Potential;
            cas.Safety_and_Hazard_Properties = casinfo.Safety_and_Hazard_Properties;
            cas.LEL = casinfo.LEL;
            cas.UEL = casinfo.UEL;
            cas.Flammability = casinfo.Flammability;
            cas.Critical_Temperature = casinfo.Critical_Temperature;
            cas.Critical_Pressure = casinfo.Critical_Pressure;
            cas.NFPA_Hazard_Classification = casinfo.NFPA_Hazard_Classification;
            cas.NFPA_Fire_Rating = casinfo.NFPA_Fire_Rating;
            cas.NFPA_Health_Rating = casinfo.NFPA_Health_Rating;
            cas.Physical_Dangers = casinfo.Physical_Dangers;
            cas.Chemical_Dangers = casinfo.Chemical_Dangers;
            cas.Explosive_Limits_and_Potential = casinfo.Explosive_Limits_and_Potential;
            cas.OSHA_Standards = casinfo.OSHA_Standards;
            cas.NIOSH_Recommendations = casinfo.NIOSH_Recommendations;
            cas.Fire_Fighting_Measures = casinfo.Fire_Fighting_Measures;
            cas.Fire_Fighting = casinfo.Fire_Fighting;
            cas.Explosion_Fire_Fighting = casinfo.Explosion_Fire_Fighting;
            cas.Other_Fire_Fighting_Hazards = casinfo.Other_Fire_Fighting_Hazards;
            cas.Spillage_Disposal = casinfo.Spillage_Disposal;
            cas.Cleanup_Methods = casinfo.Cleanup_Methods;
            cas.Other_Preventative_Measures = casinfo.Other_Preventative_Measures;
            cas.Handling_and_Storage = casinfo.Handling_and_Storage;
            cas.Nonfire_Spill_Response = casinfo.Nonfire_Spill_Response;
            cas.Safe_Storage = casinfo.Safe_Storage;
            cas.Storage_Conditions = casinfo.Storage_Conditions;
            cas.Air_and_Water_Reactions = casinfo.Air_and_Water_Reactions;
            cas.Reactive_Group = casinfo.Reactive_Group;
            cas.Reactivity_Alerts = casinfo.Reactivity_Alerts;
            cas.Reactivities_and_Incompatibilities = casinfo.Reactivities_and_Incompatibilities;
            cas.Transport_Information = casinfo.Transport_Information;
            cas.DOT_Emergency_Guidelines = casinfo.DOT_Emergency_Guidelines;
            cas.Shipment_Methods_and_Regulations = casinfo.Shipment_Methods_and_Regulations;
            cas.DOT_ID_and_Guide = casinfo.DOT_ID_and_Guide;
            cas.DOT_Label = casinfo.DOT_Label;
            cas.Packaging_and_Labelling = casinfo.Packaging_and_Labelling;
            cas.EC_Classification = casinfo.EC_Classification;
            cas.UN_Classification = casinfo.UN_Classification;
            cas.Emergency_Response = casinfo.Emergency_Response;
            cas.DOT_Emergency_Response_Guide = casinfo.DOT_Emergency_Response_Guide;
            cas.Federal_Drinking_Water_Standards = casinfo.Federal_Drinking_Water_Standards;
            cas.CERCLA_Reportable_Quantities = casinfo.CERCLA_Reportable_Quantities;
            cas.TSCA_Requirements = casinfo.TSCA_Requirements;
            cas.RCRA_Requirements = casinfo.RCRA_Requirements;
            cas.Other_Safety_Information = casinfo.Other_Safety_Information;
            cas.Toxic_Combustion_Products = casinfo.Toxic_Combustion_Products;
            cas.Isomeric_SMILES = casinfo.Isomeric_SMILES;
            cas.Status = casinfo.Status;
            cas.Federal_Drinking_Water_Guidelines = casinfo.Federal_Drinking_Water_Guidelines;
            cas.Dissociation_Constants = casinfo.Dissociation_Constants;
            cas.pH = casinfo.pH;
            cas.Drug_Warning = casinfo.Drug_Warning;
            cas.Over_the_Counter_Drug_Products = casinfo.Over_the_Counter_Drug_Products;
            cas.OTC_Drug_Ingredient = casinfo.OTC_Drug_Ingredient;
            cas.OTC_Proprietary_Name = casinfo.OTC_Proprietary_Name;
            cas.OTC_Applicant = casinfo.OTC_Applicant;
            cas.FIFRA_Requirements = casinfo.FIFRA_Requirements;
            cas.Other_Hazardous_Reactions = casinfo.Other_Hazardous_Reactions;
            cas.Prescription_Drug_Products = casinfo.Prescription_Drug_Products;
            cas.RX_Drug_Ingredient = casinfo.RX_Drug_Ingredient;
            cas.RX_Proprietary_Name = casinfo.RX_Proprietary_Name;
            cas.RX_Applicant = casinfo.RX_Applicant;
            cas.NFPA_Reactivity_Rating = casinfo.NFPA_Reactivity_Rating;
            cas.FDA_Orange_Book_Patents = casinfo.FDA_Orange_Book_Patents;
            cas.FDA_Orange_Book_Patent_ID = casinfo.FDA_Orange_Book_Patent_ID;
            cas.FDA_Orange_Book_Patent_Expiration = casinfo.FDA_Orange_Book_Patent_Expiration;
            cas.FDA_Orange_Book_Patent_Applicant = casinfo.FDA_Orange_Book_Patent_Applicant;
            cas.FDA_Orange_Book_Patent_Drug_Application = casinfo.FDA_Orange_Book_Patent_Drug_Application;
            cas.Isolation_Name = casinfo.Isolation_Name;
            cas.Isolation_Distance = casinfo.Isolation_Distance;
            context.SaveChanges();
        }


        public PageModel<CASInfo> SearchCasInfoList(QueryModel.CASInfoQuery casQuery, string islike)
        {
            int num = 0;
            try
            {
                IQueryable<CASInfo> cas = context.CASInfo.AsQueryable<CASInfo>();

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBER))
                {
                    string search = casQuery.CAS_NUMBER;
                    #region 模糊查询
                    if (islike == "0")
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.Pub_CID.ToString().Contains(search)
                                   orderby u.Pub_CID ascending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Contains(search) || u.CHINESE_ALIAS.Contains(search)
                                   orderby u.Pub_CID ascending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else
                        {
                            cas = (from u in cas
                                   where u.Record_Title.Contains(search) || u.Record_Title.Contains(search)
                                   orderby u.Pub_CID ascending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                    }
                    #endregion
                    #region 精确查询
                    else
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.Pub_CID.ToString().Equals(search)
                                   orderby u.Pub_CID ascending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Equals(search) || u.CHINESE_ALIAS.Equals(search)
                                   orderby u.Pub_CID ascending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                        else
                        {
                            cas = (from u in cas
                                   where u.Record_Title.Equals(search) || u.Record_Title.Equals(search)
                                   orderby u.Pub_CID ascending
                                   select u).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                        }
                    }
                    #endregion
                }
                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBERList))
                {
                    List<string> listcas = new List<string>();
                    string[] array = casQuery.CAS_NUMBERList.Split('~');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(array[i]))
                        {
                            listcas.Add(array[i]);
                        }
                    }
                    cas =
                        (from d in cas
                         where (from p in listcas select p).Contains(d.Pub_CID.ToString())
                         orderby d.Pub_CID ascending
                         select d).Skip(casQuery.PageSize * (casQuery.PageNo - 1)).Take(casQuery.PageSize);
                }
                return new PageModel<CASInfo>()
                {
                    Models = cas,
                    Total = cas.Count<CASInfo>()
                };
            }
            catch (Exception) { return null; }
        }


        public int SearchCasInfoListCount(QueryModel.CASInfoQuery casQuery, string islike)
        {
            int num = 0;
            try
            {
                IQueryable<CASInfo> cas = context.CASInfo.AsQueryable<CASInfo>();

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBER))
                {

                    string search = casQuery.CAS_NUMBER;

                    if (islike == "0")
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.Pub_CID.ToString().Contains(search)
                                   orderby u.Pub_CID descending
                                   select u);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Contains(search) || u.CHINESE_ALIAS.Contains(search)
                                   orderby u.Pub_CID descending
                                   select u);
                        }
                        //else if (Regex.IsMatch(search, @"[A-Za-z0-9]+"))
                        else
                        {
                            cas = (from u in cas
                                   where u.Record_Title.Contains(search) || u.Record_Title.Contains(search)
                                   orderby u.Pub_CID descending
                                   select u);
                        }
                    }
                    else //精确查询
                    {
                        if (Regex.IsMatch(search, @"[0-9?-]") && !Regex.IsMatch(search, @"[A-Za-z]+"))
                        {
                            cas = (from u in cas
                                   where u.Pub_CID.Equals(search)
                                   orderby u.Pub_CID descending
                                   select u);
                        }
                        else if (Regex.IsMatch(search, @"[\u4E00-\u9FA5]"))
                        {
                            cas = (from u in cas
                                   where u.CHINESE.Equals(search) || u.CHINESE_ALIAS.Equals(search)
                                   orderby u.Pub_CID descending
                                   select u);
                        }
                        //else if (Regex.IsMatch(search, @"[A-Za-z0-9]+"))
                        else
                        {
                            cas = (from u in cas
                                   where u.Record_Title.Equals(search) || u.Record_Title.Equals(search)
                                   orderby u.Pub_CID descending
                                   select u);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(casQuery.CAS_NUMBERList))
                {
                    List<string> listcas = new List<string>();
                    string[] array = casQuery.CAS_NUMBERList.Split('~');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(array[i]))
                        {
                            listcas.Add(array[i]);
                        }
                    }
                    cas =
                        from d in cas
                        where (from p in listcas select p).Contains(d.Pub_CID.ToString())
                        select d;
                }
                cas = cas.GetPage(out num, casQuery.PageNo, casQuery.PageSize, (IQueryable<CASInfo> d) =>
                    from o in d
                    orderby o.Pub_CID descending
                    select o);
                return num;
            }
            catch (Exception) { return 0; }
        }

        /*查询casinfo*/
        public CASInfo GetCASByCID(string CID)
        {
            CASInfo _CASInfo = new CASInfo();

            string strsql = string.Format("SELECT * FROM ChemCloud_CAS with(nolock) where Pub_CID='{0}'", CID);
            DataTable dt = DbHelperSQLCAS.QueryDataTable(strsql);
            if (dt != null && dt.Rows.Count > 0)
            {
                _CASInfo = GetEntity<CASInfo>(dt);
            }
            return _CASInfo;
        }


        public string GetCASByInchikey(string Inchikey)
        {
            string pub_cid = "0";

            if (!string.IsNullOrEmpty(Inchikey))
            {
                string sql = "select Pub_CID From ChemCloud_CAS with(nolock) where InChI_Key='" + Inchikey + "'";
                DataTable dt = ChemCloud.DBUtility.DbHelperSQLCAS.QueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    pub_cid = dt.Rows[0]["Pub_CID"] == null ? "" : dt.Rows[0]["Pub_CID"].ToString();
                }
            }
            return pub_cid;
        }
    }
}



