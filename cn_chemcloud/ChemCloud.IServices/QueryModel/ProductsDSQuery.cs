using ChemCloud.Model;
using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.IServices.QueryModel
{
    public class ProductsDSQuery : QueryBase
    {
        public ProductsDSQuery()
        {

        }
        /// <summary>
        /// ���۱��
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// ��Ʒ���
        /// </summary>
        public long productid { get; set; }
        /// <summary>
        /// ���̱��
        /// </summary>
        public long shopid { get; set; }
        /// <summary>
        /// �û����
        /// </summary>
        public long userid { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        public int usertype { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        public int dsstatus { get; set; }
        /// <summary>
        /// ʱ��
        /// </summary>
        public DateTime dstime { get; set; }
        /// <summary>
        /// CAS
        /// </summary>
        public string CAS { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string productcode { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public string productname { get; set; }
        /// <summary>
        /// ��Ӧ������
        /// </summary>
        public string sellerusername { get; set; }
    }
}