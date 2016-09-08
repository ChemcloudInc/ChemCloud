using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
    public class ProductAnalysis : BaseModel
    {
        private long _id;

        public new long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                base.Id = value;
            }
        }

        /// <summary>
        /// 委托方姓名-委托方信息
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 报告抬头-委托方信息
        /// </summary>
        public string ReportHeader { get; set; }

        /// <summary>
        /// 联系人-委托方信息
        /// </summary>
        public string ClientLinkMan { get; set; }

        /// <summary>
        /// 手机-委托方信息
        /// </summary>
        public string ClientMobilePhone { get; set; }

        /// <summary>
        /// E-mail-委托方信息
        /// </summary>
        public string ClientEmail { get; set; }

        /// <summary>
        /// 固话-委托方信息
        /// </summary>
        public string ClientFixedLine { get; set; }

        /// <summary>
        /// 传真-委托方信息
        /// </summary>
        public string ClientFax { get; set; }

        /// <summary>
        /// 地址-委托方信息
        /// </summary>
        public string ClientAddress { get; set; }

        /// <summary>
        /// 样品名称
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// 样品数量
        /// </summary>
        public int SampleQuantity { get; set; }

        /// <summary>
        /// 样品规格型号
        /// </summary>
        public string SampleSpecifications { get; set; }

        /// <summary>
        /// 样品来源
        /// </summary>
        public string SampleSource { get; set; }

        /// <summary>
        /// 相关资料
        /// </summary>
        public string RelatedData { get; set; }

        /// <summary>
        /// 样品已知信息
        /// </summary>
        public string KnownInformation { get; set; }

        /// <summary>
        /// 性状项
        /// </summary>
        public int CharacterItem { get; set; }

        /// <summary>
        /// 性状描述
        /// </summary>
        public string CharacterItemDescribe { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string SampleColor { get; set; }

        /// <summary>
        /// 危险性项
        /// </summary>
        public int DangerItem { get; set; }

        /// <summary>
        /// 危险性描述
        /// </summary>
        public string DangerItemDescribe { get; set; }

        /// <summary>
        /// 保存条件
        /// </summary>
        public string StorageCondition { get; set; }

        /// <summary>
        /// 常规分析
        /// </summary>
        public int GeneralAnaly { get; set; }

        /// <summary>
        /// 常规分析周期
        /// </summary>
        public int GeneralAnalyCycle { get; set; }

        /// <summary>
        /// 深度分析
        /// </summary>
        public int DepthAnalysis { get; set; }

        /// <summary>
        /// 深度分析周期
        /// </summary>
        public int DepthAnalysisCycle { get; set; }

        /// <summary>
        /// 重大项目分析费用
        /// </summary>
        public decimal ProjectAnalysisCost { get; set; }

        /// <summary>
        /// 重大项目分析周期
        /// </summary>
        public string ProjectAnalysisCycle { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 余样处理
        /// </summary>
        public string ResidualSampleTreatment { get; set; }

        /// <summary>
        /// 退样地址
        /// </summary>
        public string ReturnSampleAddress { get; set; }

        /// <summary>
        /// 报告格式
        /// </summary>
        public string ReportFormat { get; set; }

        /// <summary>
        /// 服务费用
        /// </summary>
        public decimal ServiceCharge { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long Memberid { get; set; }

        [NotMapped]
        /// <summary>
        /// 用户名称
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 状态（1已提交2已确认3已支付4分析中5已完成）
        /// </summary>
        public int AnalysisStatus { get; set; }

        /// <summary>
        /// 平台分析附件
        /// </summary>
        public string AnalysisAttachments { get; set; }

        /// <summary>
        /// 查询所用页码
        /// </summary>
        [NotMapped]
        public int PageNo { get; set; }


        /// <summary>
        /// 查询所用页面条数
        /// </summary>
        [NotMapped]
        public int PageSize { get; set; }

    }
}
