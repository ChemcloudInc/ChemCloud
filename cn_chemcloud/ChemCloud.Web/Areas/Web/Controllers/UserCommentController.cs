using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
	public class UserCommentController : BaseMemberController
	{
		public UserCommentController()
		{
		}

		public JsonResult AddComment(long subOrderId, int star, string content)
		{
			ProductCommentInfo productCommentInfo = new ProductCommentInfo()
			{
				ReviewDate = DateTime.Now,
				ReviewContent = content,
				UserId = base.CurrentUser.Id,
				UserName = base.CurrentUser.UserName,
				Email = base.CurrentUser.Email,
				SubOrderId = new long?(subOrderId),
				ReviewMark = star
			};
			ServiceHelper.Create<ICommentService>().AddComment(productCommentInfo);
			MemberIntegralRecord memberIntegralRecord = new MemberIntegralRecord()
			{
				UserName = base.CurrentUser.UserName,
				MemberId = base.CurrentUser.Id,
				RecordDate = new DateTime?(DateTime.Now),
				TypeId = MemberIntegral.IntegralType.Comment
			};
			MemberIntegralRecordAction memberIntegralRecordAction = new MemberIntegralRecordAction()
			{
				VirtualItemTypeId = new MemberIntegral.VirtualItemType?(MemberIntegral.VirtualItemType.Comment),
				VirtualItemId = productCommentInfo.ProductId
			};
			memberIntegralRecord.ChemCloud_MemberIntegralRecordAction.Add(memberIntegralRecordAction);
			IConversionMemberIntegralBase conversionMemberIntegralBase = ServiceHelper.Create<IMemberIntegralConversionFactoryService>().Create(MemberIntegral.IntegralType.Comment, 0);
			ServiceHelper.Create<IMemberIntegralService>().AddMemberIntegral(memberIntegralRecord, conversionMemberIntegralBase);
			Result result = new Result()
			{
				success = true,
				msg = "发表成功"
			};
			return Json(result);
		}

		public ActionResult Index(int pageSize = 10, int pageNo = 1)
		{
			CommentQuery commentQuery = new CommentQuery()
			{
				UserID = base.CurrentUser.Id,
				PageSize = pageSize,
				PageNo = pageNo,
				Sort = "PComment"
			};
			PageModel<ProductEvaluation> productEvaluation = ServiceHelper.Create<ICommentService>().GetProductEvaluation(commentQuery);
			PagingInfo pagingInfo = new PagingInfo()
			{
				CurrentPage = pageNo,
				ItemsPerPage = pageSize,
				TotalItems = productEvaluation.Total
			};
			ViewBag.pageInfo = pagingInfo;
			return View(productEvaluation.Models);
		}
        public JsonResult GetCommentDetail(long id)
        {
            ProductCommentInfo comment = ServiceHelper.Create<ICommentService>().GetComment(id);
            return Json(new { ConsulationContent = comment.ReviewContent, ReplyContent = comment.ReplyContent });
        }
	}
}