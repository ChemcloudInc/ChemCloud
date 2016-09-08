using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChemCloud.Model;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model.Common;

namespace ChemCloud.IServices
{
    public interface ITransactionRecordService : IService, IDisposable
    {
        Result_List1<Result_TransactionRecord> GetTransactionRecordList_Web(int count);
        Result_List_Pager<Result_TransactionRecord> GetTransactionRecordList(TransactionRecordQuery query);

        Result_Msg DeleteById(int Id, long userId);

        Result_TransactionRecord GetObjectById(int Id);
        Result_Model<TransactionRecord> ComputeFun(TransactionRecordCompute query);


        Result_Msg TransactionRecordAdd(TransactionRecordAddQuery query);

        Result_Msg ModifyTransactionRecord(TransactionRecordModifyQuery query);

    }
}
