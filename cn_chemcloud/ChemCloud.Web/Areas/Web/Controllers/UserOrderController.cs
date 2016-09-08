using ChemCloud.Core;
using ChemCloud.Core.Plugins;
using ChemCloud.IServices;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using ChemCloud.Web.Areas.Web.Models;
using ChemCloud.Web.Framework;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;

namespace ChemCloud.Web.Areas.Web.Controllers
{
    public class UserOrderController : BaseMemberController
    {
        public UserOrderController()
        {
        }

        [HttpPost]
        public JsonResult CloseOrder(long orderId)
        {
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(orderId, base.CurrentUser.Id);
            if (order == null)
            {
                Result result = new Result()
                {
                    success = false,
                    msg = "取消失败，该订单已删除或者不属于当前用户！"
                };
                return Json(result);
            }
            ServiceHelper.Create<IOrderService>().MemberCloseOrder(orderId, base.CurrentUser.UserName, false);

            Result result1 = new Result()
            {
                success = true,
                msg = "取消成功"
            };
            return Json(result1);
        }

        [HttpPost]
        public JsonResult ConfirmOrder(long orderId)
        {
            ServiceHelper.Create<IOrderService>().MembeConfirmOrder(orderId, base.CurrentUser.UserName);
            Result result = new Result()
            {
                success = true,
                msg = "操作成功！"
            };
            return Json(result);
        }

        [ChildActionOnly]
        public ActionResult CustmerServices(long shopId)
        {
            List<CustomerServiceInfo> list = (
                from m in ServiceHelper.Create<ICustomerService>().GetCustomerService(shopId)
                orderby m.Tool
                select m).ToList();
            List<CustomerServiceInfo> customerServiceInfos = new List<CustomerServiceInfo>();
            CustomerServiceInfo customerServiceInfo = list.Where((CustomerServiceInfo a) =>
            {
                if (a.Tool != CustomerServiceInfo.ServiceTool.QQ)
                {
                    return false;
                }
                return a.Type == CustomerServiceInfo.ServiceType.AfterSale;
            }).OrderBy<CustomerServiceInfo, string>((CustomerServiceInfo t) => Guid.NewGuid().ToString()).FirstOrDefault();
            CustomerServiceInfo customerServiceInfo1 = list.Where((CustomerServiceInfo a) =>
            {
                if (a.Tool != CustomerServiceInfo.ServiceTool.Wangwang)
                {
                    return false;
                }
                return a.Type == CustomerServiceInfo.ServiceType.AfterSale;
            }).OrderBy<CustomerServiceInfo, string>((CustomerServiceInfo t) => Guid.NewGuid().ToString()).FirstOrDefault();
            if (customerServiceInfo != null)
            {
                customerServiceInfos.Add(customerServiceInfo);
            }
            if (customerServiceInfo1 != null)
            {
                customerServiceInfos.Add(customerServiceInfo1);
            }
            return base.PartialView(customerServiceInfos);
        }

        public ActionResult Detail(long id)
        {
            OrderInfo order = ServiceHelper.Create<IOrderService>().GetOrder(id, base.CurrentUser.Id);
            IEnumerable<long> nums = (
                from d in order.OrderItemInfo
                select d.ProductId).AsEnumerable<long>();
            var list = (
                from d in ServiceHelper.Create<IProductService>().GetProductByIds(nums)
                select new { Id = d.Id, ProductCode = d.ProductCode }).ToList();
            foreach (OrderItemInfo orderItemInfo in order.OrderItemInfo)
            {
                var variable = list.Find((d) => d.Id == orderItemInfo.ProductId);
                if (variable == null)
                {
                    continue;
                }
                orderItemInfo.ProductCode = variable.ProductCode;
                orderItemInfo.Pub_CID = ServiceHelper.Create<IProductService>().GetProduct(orderItemInfo.ProductId) == null ? 0 :
                    (ServiceHelper.Create<IProductService>().GetProduct(orderItemInfo.ProductId).Pub_CID == null ? 0 : ServiceHelper.Create<IProductService>().GetProduct(orderItemInfo.ProductId).Pub_CID);

            }
            ViewBag.COA = "";
            if (ServiceHelper.Create<IChemCloud_OrderWithCoaService>().GetChemCloud_OrderWithCoaByOrderid(id) != null)
            {
                string strcoa = ServiceHelper.Create<IChemCloud_OrderWithCoaService>().GetChemCloud_OrderWithCoaByOrderid(id).CoaNo;
                if (!string.IsNullOrEmpty(strcoa))
                {
                    COAList Coa = ServiceHelper.Create<ICOAListService>().GetCoAReportInfo(strcoa);
                    if (Coa != null)
                    {
                        string coaurl = ChemCloud.Core.Common.GetRootUrl("") + "/search/Search_COA?key=" + Coa.CoANo;
                        ViewBag.COA = coaurl;
                    }
                }
            }


            ViewBag.ShopName = order.ShopName == null ? "" : order.ShopName;

            return View(order);
        }

        [HttpPost]
        public JsonResult GetExpressData(string expressCompanyName, string shipOrderNumber)
        {
            if (string.IsNullOrWhiteSpace(shipOrderNumber))
            {
                throw new HimallException("错误的订单信息");
            }
            OrderExpressQuery oe = ServiceHelper.Create<IOrderExpressQueryService>().GetOrderExpressById(shipOrderNumber);
            return Json(oe);
        }

        [ValidateInput(false)]
        public ActionResult Index(string orderDate, string keywords, string orderids, DateTime? startDateTime, DateTime? endDateTime, int? orderStatus, int pageNo = 1, int pageSize = 10, string casno = "")
        {
            OrderInfo.OrderOperateStatus? nullable;
            DateTime? nullable1 = startDateTime;
            DateTime? nullable2 = endDateTime;
            if (!string.IsNullOrEmpty(orderDate) && orderDate.ToLower() != "all")
            {
                string lower = orderDate.ToLower();
                string str = lower;
                if (lower != null)
                {
                    if (str == "threemonth")
                    {
                        nullable1 = new DateTime?(DateTime.Now.AddMonths(-3));
                    }
                    else if (str == "halfyear")
                    {
                        nullable1 = new DateTime?(DateTime.Now.AddMonths(-6));
                    }
                    else if (str == "year")
                    {
                        nullable1 = new DateTime?(DateTime.Now.AddYears(-1));
                    }
                    else if (str == "yearago")
                    {
                        nullable2 = new DateTime?(DateTime.Now.AddYears(-1));
                    }
                }
            }
            if (orderStatus.HasValue)
            {
                int? nullable3 = orderStatus;
                if ((nullable3.GetValueOrDefault() != 0 ? false : nullable3.HasValue))
                {
                    orderStatus = null;
                }
            }
            OrderQuery orderQuery = new OrderQuery()
            {
                StartDate = nullable1,
                EndDate = nullable2
            };
            OrderQuery orderQuery1 = orderQuery;
            int? nullable4 = orderStatus;
            if (nullable4.HasValue)
            {
                nullable = new OrderInfo.OrderOperateStatus?((OrderInfo.OrderOperateStatus)nullable4.GetValueOrDefault());
            }
            else
            {
                nullable = null;
            }
            orderQuery1.Status = nullable;
            orderQuery.UserId = new long?(base.CurrentUser.Id);
            orderQuery.SearchKeyWords = keywords;
            orderQuery.PageSize = pageSize;
            orderQuery.PageNo = pageNo;

            orderQuery.CASNo = casno;

            PageModel<OrderInfo> orders = ServiceHelper.Create<IOrderService>().GetOrders<OrderInfo>(orderQuery, null);
            PagingInfo pagingInfo = new PagingInfo()
            {
                CurrentPage = pageNo,
                ItemsPerPage = pageSize,
                TotalItems = orders.Total
            };
            ViewBag.pageInfo = pagingInfo;
            ViewBag.UserId = base.CurrentUser.Id;

            List<OrderInfo> list = orders.Models.ToList();
            ICashDepositsService create = Instance<ICashDepositsService>.Create;
            IEnumerable<OrderListModel> orderListModel =
                from item in list
                select new OrderListModel()
                {
                    IsBehalfShip = item.IsBehalfShip,
                    BehalfShipNumber = item.BehalfShipNumber,
                    BehalfShipType = item.BehalfShipType,
                    Id = item.Id,
                    ActiveType = item.ActiveType,
                    OrderType = item.OrderType,
                    Address = item.Address,
                    CellPhone = item.CellPhone,
                    CloseReason = item.CloseReason,
                    CommisTotalAmount = item.CommisAmount,
                    DiscountAmount = item.DiscountAmount,
                    ExpressCompanyName = item.ExpressCompanyName,
                    FinishDate = item.FinishDate,
                    Freight = item.Freight,
                    GatewayOrderId = item.GatewayOrderId,
                    IntegralDiscount = item.IntegralDiscount,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    ShopId = item.ShopId,
                    ShopName = item.ShopName,
                    ShipTo = item.ShipTo,
                    CoinType = item.CoinType,
                    CoinTypeName = item.CoinTypeName,
                    OrderTotalAmount = item.OrderTotalAmount,
                    PaymentTypeName = item.PaymentTypeName,
                    OrderStatus = item.OrderStatus,
                    RefundStats = item.RefundStats,
                    OrderCommentInfo = item.OrderCommentInfo,
                    OrderDate = item.OrderDate,
                    OrderItemList =
                        from oItem in item.OrderItemInfo
                        select new OrderItemListModel()
                        {
                            ProductId = oItem.ProductId,
                            ProductName = oItem.ProductName,
                            CASNo = ServiceHelper.Create<IProductService>().GetProduct(oItem.ProductId) == null ? "" : ServiceHelper.Create<IProductService>().GetProduct(oItem.ProductId).CASNo,
                            SalePrice = oItem.SalePrice,
                            Quantity = oItem.Quantity,
                            Pub_CID = ServiceHelper.Create<IProductService>().GetProduct(oItem.ProductId) == null ? 0 : ServiceHelper.Create<IProductService>().GetProduct(oItem.ProductId).Pub_CID,
                            PackingUnit = oItem.PackingUnit,
                            Purity = oItem.Purity
                        }
                };
            List<long> nums = (
                from d in list
                select d.Id).ToList();
            if (nums.Count > 0)
            {
                RefundQuery refundQuery = new RefundQuery()
                {
                    OrderId = new long?(nums[0]),
                    MoreOrderId = nums,
                    PageNo = 1,
                    PageSize = list.Count
                };
                List<OrderRefundInfo> orderRefundInfos = (
                    from d in ServiceHelper.Create<IRefundService>().GetOrderRefunds(refundQuery).Models
                    where (int)d.RefundMode == 1
                    select d).ToList();
                if (orderRefundInfos.Count > 0)
                {
                    foreach (OrderRefundInfo orderRefundInfo in orderRefundInfos)
                    {
                        OrderInfo orderInfo = list.FirstOrDefault((OrderInfo d) => d.Id == orderRefundInfo.OrderId);
                        if (orderInfo == null)
                        {
                            continue;
                        }
                        orderInfo.RefundStats = (int)(orderRefundInfo.SellerAuditStatus);
                    }
                }
            }

            decimal totalamount = 0;
            if (orderListModel != null)
            {
                foreach (var item in orderListModel)
                {
                    totalamount += item.OrderTotalAmount;
                }
            }
            ViewBag.totalamount = totalamount.ToString("F2");


            return View(orderListModel.ToList());
        }

        [HttpPost]
        public JsonResult GetPayInfo(string orderId = "", string uid = "")
        {
            FQPayment fq = ServiceHelper.Create<IFQPaymentService>().GetFQPaymentInfo(long.Parse(orderId), long.Parse(uid));
            if (fq == null)
            {
                return Json("");
            }
            else
            {
                return Json(fq);
            }
        }
        /// <summary>
        /// 获取当前用户限额判断的操作
        /// </summary>
        /// <param name="price">订单价格</param>
        /// <param name="onum">订单编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckMyMoney(string price, string onum)
        {
            Organization oinfo = ServiceHelper.Create<IOrganizationService>().GetOrganizationByUserId(base.CurrentUser.Id);
            PurchaseRolesInfo roleInfo = ServiceHelper.Create<IPermissionGroupService>().GetPurchaseRole(oinfo.RoleId);
            if (roleInfo != null)
            {
                if ((roleInfo.RoleName == "管理员") || (roleInfo.RoleName == "Admin"))
                {
                    return Json("ok");
                }
                else
                {
                    if (oinfo != null)
                    {
                        OrderInfo o = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(onum));
                        LimitedAmount la = ServiceHelper.Create<IOrganizationService>().GetlimitedByRoleId(oinfo.RoleId, int.Parse(o.CoinType.ToString()));
                        if (la != null)
                        {
                            decimal usermoney = la.Money;
                            decimal ordermoney = decimal.Parse(price);
                            if (usermoney >= ordermoney)
                            {
                                return Json("ok");
                            }
                            else
                            {
                                //申请判断
                                ApplyAmountInfo aai = ServiceHelper.Create<IApplyAmountService>().GetApplyByUserId(base.CurrentUser.Id, long.Parse(onum));
                                if (aai != null)
                                {
                                    int status = aai.ApplyStatus;
                                    if (status == 0)
                                    {
                                        return Json("您已提交限额申请，请耐心等待");
                                    }
                                    else if (status == 1)
                                    {
                                        return Json("ok");
                                    }
                                    else
                                    {
                                        return Json("您提交限额申请没有通过,请进行重新提交");
                                    }
                                }
                                else
                                {
                                    return Json("你的限额受限");//你的限额受限，是否提交申请?
                                }
                            }
                        }
                        else
                        {
                            return Json("获取您的限额信息失败");
                        }
                    }
                    else
                    {
                        return Json("您现在还没有组织架构哦");
                    }
                }
            }
            else
            {
                return Json("您现在还没有权限组哦");
            }

        }
        [HttpPost]
        public JsonResult IsExitsApply(long orderId)
        {
            ApplyAmountInfo ApplyInfo = ServiceHelper.Create<IApplyAmountService>().GetApplyByOrderId(base.CurrentUser.Id, orderId);
            if (ApplyInfo != null)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }
        /// <summary>
        /// 添加限额申请
        /// </summary>
        /// <param name="orderAmount">订单价格</param>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddXian(string orderAmount, string orderId)
        {
            OrderInfo o = ServiceHelper.Create<IOrderService>().GetOrder(long.Parse(orderId));
            decimal dmmoney = decimal.Parse(orderAmount);
            int cointypes = int.Parse(o.CoinType.ToString());
            long oid = long.Parse(orderId);
            ApplyAmountInfo model = new ApplyAmountInfo();
            model.ApplyUserId = base.CurrentUser.Id;
            model.AuthorId = base.CurrentUser.ParentSellerId;
            model.ApplyAmount = dmmoney;
            model.OrderId = oid;
            model.ApplyDate = DateTime.Now;
            model.AuthDate = DateTime.Now;
            model.ApplyStatus = 0;
            model.CoinType = cointypes;
            bool flag = ServiceHelper.Create<IApplyAmountService>().AddApplyAmount(model);
            ServiceHelper.Create<ISiteMessagesService>().SendLimitedAmountMessage(base.CurrentUser.ParentSellerId);
            if (flag)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }

    }
}