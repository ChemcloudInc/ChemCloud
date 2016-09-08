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
using System.Threading.Tasks;
using System.Transactions;

namespace ChemCloud.Service
{
    public class MemberService : ServiceBase, IMemberService, IService, IDisposable
    {
        public MemberService()
        {

        }

        private void AddBindInergral(UserMemberInfo member)
        {
            if (context.MemberIntegralRecord.Any((MemberIntegralRecord a) => a.MemberId == member.Id && (int)a.TypeId == 6))
            {
                return;
            }
            try
            {
                MemberIntegralRecord memberIntegralRecord = new MemberIntegralRecord()
                {
                    UserName = member.UserName,
                    MemberId = member.Id,
                    RecordDate = new DateTime?(DateTime.Now),
                    //TypeId = MemberIntegral.IntegralType.BindWX,
                    //ReMark = "绑定微信"
                };
                //IConversionMemberIntegralBase conversionMemberIntegralBase = Instance<IMemberIntegralConversionFactoryService>.Create.Create(MemberIntegral.IntegralType.BindWX, 0);
                //Instance<IMemberIntegralService>.Create.AddMemberIntegral(memberIntegralRecord, conversionMemberIntegralBase);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        private void AddIntegel(UserMemberInfo member)
        {
            if (!Instance<IMemberIntegralService>.Create.HasLoginIntegralRecord(member.Id))
            {
                MemberIntegralRecord memberIntegralRecord = new MemberIntegralRecord()
                {
                    UserName = member.UserName,
                    MemberId = member.Id,
                    RecordDate = new DateTime?(DateTime.Now),
                    ReMark = "每天登录",
                    TypeId = MemberIntegral.IntegralType.Login
                };
                IConversionMemberIntegralBase conversionMemberIntegralBase = Instance<IMemberIntegralConversionFactoryService>.Create.Create(MemberIntegral.IntegralType.Login, 0);
                Instance<IMemberIntegralService>.Create.AddMemberIntegral(memberIntegralRecord, conversionMemberIntegralBase);
            }
        }

        public void AddMember(UserMemberInfo model)
        {
            throw new NotImplementedException();
        }

        public void BatchDeleteMember(long[] ids)
        {

            /*删除主表信息*/
            IQueryable<UserMemberInfo> _UserMemberInfo = from e in context.UserMemberInfo where ids.Contains(e.Id) select e;
            if (_UserMemberInfo != null)
            {
                context.UserMemberInfo.RemoveRange(_UserMemberInfo);
                context.SaveChanges();
            }

            /*删除详细表信息*/
            IQueryable<MemberDetail> _MemberDetail = from e in context.MemberDetail where ids.Contains(e.MemberId) select e;
            if (_MemberDetail != null)
            {
                context.MemberDetail.RemoveRange(_MemberDetail);
                context.SaveChanges();
            }


            long[] numArray = ids;
            for (int i = 0; i < numArray.Length; i++)
            {
                Cache.Remove(CacheKeyCollection.Member(numArray[i]));
            }
        }

        public void BatchLock(long[] ids)
        {
            IQueryable<UserMemberInfo> userMemberInfo =
                from item in context.UserMemberInfo
                where ids.Contains(item.Id)
                select item;
            foreach (UserMemberInfo userMemberInfo1 in userMemberInfo)
            {
                userMemberInfo1.Disabled = true;
            }
            context.SaveChanges();
            long[] numArray = ids;
            for (int i = 0; i < numArray.Length; i++)
            {
                Cache.Remove(CacheKeyCollection.Member(numArray[i]));
            }
        }

        public void BindMember(long userId, string serviceProvider, string openId, string headImage = null, string unionid = null, string unionopenid = null)
        {
            CheckOpenIdHasBeenUsed(serviceProvider, openId, userId);
            MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo()
            {
                UserId = userId,
                OpenId = openId,
                ServiceProvider = serviceProvider,
                UnionId = (unionid == null ? string.Empty : unionid),
                UnionOpenId = (string.IsNullOrWhiteSpace(unionopenid) ? string.Empty : unionopenid)
            };
            MemberOpenIdInfo memberOpenIdInfo1 = memberOpenIdInfo;
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo item) => item.Id == userId);
            if (!string.IsNullOrWhiteSpace(headImage) && string.IsNullOrWhiteSpace(userMemberInfo.Photo))
            {
                userMemberInfo.Photo = TransferHeadImage(headImage, userId);
            }
            context.MemberOpenIdInfo.Add(memberOpenIdInfo1);
            context.SaveChanges();
            Instance<IBonusService>.Create.DepositToRegister(userMemberInfo.Id);
            Cache.Remove(CacheKeyCollection.Member(userId));
        }

        public void BindMember(long userId, string serviceProvider, string openId, MemberOpenIdInfo.AppIdTypeEnum AppidType, string headImage = null, string unionid = null)
        {
            CheckOpenIdHasBeenUsed(serviceProvider, openId, userId);
            MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo()
            {
                UserId = userId,
                OpenId = openId,
                ServiceProvider = serviceProvider,
                AppIdType = AppidType,
                UnionId = (string.IsNullOrWhiteSpace(unionid) ? string.Empty : unionid)
            };
            MemberOpenIdInfo memberOpenIdInfo1 = memberOpenIdInfo;
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo item) => item.Id == userId);
            if (!string.IsNullOrWhiteSpace(headImage) && string.IsNullOrWhiteSpace(userMemberInfo.Photo))
            {
                userMemberInfo.Photo = TransferHeadImage(headImage, userId);
            }
            context.MemberOpenIdInfo.Add(memberOpenIdInfo1);
            context.SaveChanges();
            Instance<IBonusService>.Create.DepositToRegister(userMemberInfo.Id);
            if (serviceProvider.ToLower() == "ChemCloud.Plugin.OAuth.WeiXin".ToLower())
            {
                AddBindInergral(userMemberInfo);
            }
            Cache.Remove(CacheKeyCollection.Member(userId));
        }

        public void ChangePassWord(long id, string password)
        {
            if (password.Length < 6)
            {
                throw new HimallException("密码长度至少6位字符！");
            }
            UserMemberInfo passwrodWithTwiceEncode = context.UserMemberInfo.FindById<UserMemberInfo>(id);
            if (passwrodWithTwiceEncode.PasswordSalt.StartsWith("o"))
            {
                UserMemberInfo userMemberInfo = passwrodWithTwiceEncode;
                Guid guid = Guid.NewGuid();
                userMemberInfo.PasswordSalt = guid.ToString("N").Substring(12);
            }
            passwrodWithTwiceEncode.Password = GetPasswrodWithTwiceEncode(password, passwrodWithTwiceEncode.PasswordSalt);
            ManagerInfo passwordSalt = context.ManagerInfo.FirstOrDefault((ManagerInfo a) => (a.UserName == passwrodWithTwiceEncode.UserName) && a.ShopId != 0);
            if (passwordSalt != null)
            {
                passwordSalt.PasswordSalt = passwrodWithTwiceEncode.PasswordSalt;
                passwordSalt.Password = passwrodWithTwiceEncode.Password;
            }
            context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Member(passwrodWithTwiceEncode.Id));
            Cache.Remove(CacheKeyCollection.Seller(passwrodWithTwiceEncode.Id));
        }
        public bool ResetPwd(long id, string password)
        {
            if (password.Length < 6)
            {
                throw new HimallException("密码长度至少6位字符！");
            }
            password = password.Trim();
            Guid guid = Guid.NewGuid();
            string str = guid.ToString("N").Substring(12);
            password = GetPasswrodWithTwiceEncode(password, str);
            ManagerInfo manaInfo = context.ManagerInfo.FindById(id);
            manaInfo.Password = password;
            manaInfo.PasswordSalt = str;
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public void CheckContactInfoHasBeenUsed(string serviceProvider, string contact, MemberContactsInfo.UserTypes userType = 0)
        {
            if (context.MemberContactsInfo.FirstOrDefault((MemberContactsInfo item) => (item.ServiceProvider == serviceProvider) && (item.Contact == contact) && (int)item.UserType == (int)userType) != null)
            {
                throw new HimallException(string.Format("{0}已经被其它用户绑定", contact));
            }
        }

        private void CheckInputIsValidWhenQuickRegister(string username, string serviceProvider, string openId)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("用户名不能为空");
            }
            if (string.IsNullOrWhiteSpace(serviceProvider))
            {
                throw new ArgumentNullException("服务提供商不能为空");
            }
            if (string.IsNullOrWhiteSpace(openId))
            {
                throw new ArgumentNullException("OpenId不能为空");
            }
        }

        public bool CheckMemberExist(string username)
        {
            UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo item) => item.UserName == username);
            if (userInfo != null)
                return true;
            else
                return false;
        }
        public bool CheckEmailExist(string email)
        {
            UserMemberInfo userInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo item) => item.Email == email);
            if (userInfo != null)
                return true;
            else
                return false;
        }
        public bool CheckMobileExist(string mobile)
        {
            return context.UserMemberInfo.Any((UserMemberInfo item) => item.CellPhone == mobile);
        }

        private void CheckOpenIdHasBeenUsed(string serviceProvider, string openId, long userId = 0L)
        {
            if (context.MemberOpenIdInfo.FirstOrDefault((MemberOpenIdInfo item) => (item.ServiceProvider == serviceProvider) && (item.OpenId == openId)) != null)
            {
                throw new HimallException(string.Format("OpenId:{0}已经被使用", openId));
            }
        }

        /*删除采购商信息*/
        public void DeleteMember(long id)
        {
            /*删除主表信息*/
            IQueryable<UserMemberInfo> _UserMemberInfo = from e in context.UserMemberInfo where e.Id == id select e;
            if (_UserMemberInfo != null)
            {
                context.UserMemberInfo.RemoveRange(_UserMemberInfo);
                context.SaveChanges();
            }

            /*删除详细表信息*/
            IQueryable<MemberDetail> _MemberDetail = from e in context.MemberDetail where e.MemberId == id select e;
            if (_MemberDetail != null)
            {
                context.MemberDetail.RemoveRange(_MemberDetail);
                context.SaveChanges();
            }
            Cache.Remove(CacheKeyCollection.Member(id));
        }

        public void DeleteMemberOpenId(long userid, string openid)
        {
            IQueryable<MemberOpenIdInfo> memberOpenIdInfo =
                from e in context.MemberOpenIdInfo
                where e.UserId == userid
                select e into item
                where string.IsNullOrEmpty(openid) || !string.IsNullOrEmpty(openid) && (openid == item.OpenId)
                select item;
            context.MemberOpenIdInfo.RemoveRange(memberOpenIdInfo);
            context.SaveChanges();
        }

        public string GetLogo()
        {
            return (
                from p in context.SiteSettingsInfo
                where p.Key == "MemberLogo"
                select p).FirstOrDefault().MemberLogo;
        }

        public UserMemberInfo GetMember(long id)
        {
            return GetMember((UserMemberInfo d) => d.Id == id);

        }

        public List<UserMemberInfo> GetMemberList(long parentid)
        {
            return (from p in context.UserMemberInfo where p.ParentSellerId == parentid select p).ToList();

        }

        private UserMemberInfo GetMember(Func<UserMemberInfo, bool> predicate)
        {
            UserMemberInfo memberGrade = context.UserMemberInfo.Where(predicate).FirstOrDefault();
            if (memberGrade != null)
            {
                memberGrade.InitUserIntegralInfo();
                memberGrade.MemberGradeName = GetMemberGrade(memberGrade.HistoryIntegral);
            }
            return memberGrade;
        }

        public UserMemberInfo GetMemberByContactInfo(string contact)
        {
            UserMemberInfo userMemberInfo = null;
            MemberContactsInfo memberContactsInfo = context.MemberContactsInfo.FirstOrDefault((MemberContactsInfo item) => (item.Contact == contact) && item.UserType == 0);
            userMemberInfo = (memberContactsInfo == null ? (
                from a in context.UserMemberInfo
                where (a.UserName == contact) && context.MemberContactsInfo.Any((MemberContactsInfo item) => item.UserId == a.Id)
                select a).FirstOrDefault() : context.UserMemberInfo.FindById<UserMemberInfo>(memberContactsInfo.UserId));
            return userMemberInfo;
        }

        public UserMemberInfo GetMemberByName(string userName)
        {
            return GetMember((UserMemberInfo a) => a.UserName == userName);
        }

        public UserMemberInfo GetMemberByOpenId(string serviceProvider, string openId)
        {
            UserMemberInfo userMemberInfo = null;
            MemberOpenIdInfo memberOpenIdInfo = context.MemberOpenIdInfo.FirstOrDefault((MemberOpenIdInfo item) => (item.ServiceProvider == serviceProvider) && (item.OpenId == openId));
            if (memberOpenIdInfo != null)
            {
                userMemberInfo = context.UserMemberInfo.FindById<UserMemberInfo>(memberOpenIdInfo.UserId);
            }
            return userMemberInfo;
        }

        public UserMemberInfo GetMemberByUnionId(string serviceProvider, string UnionId)
        {
            UserMemberInfo userMemberInfo = null;
            MemberOpenIdInfo memberOpenIdInfo = context.MemberOpenIdInfo.FirstOrDefault((MemberOpenIdInfo item) => (item.ServiceProvider == serviceProvider) && (item.UnionId == UnionId));
            if (memberOpenIdInfo != null)
            {
                userMemberInfo = context.UserMemberInfo.FindById<UserMemberInfo>(memberOpenIdInfo.UserId);
            }
            return userMemberInfo;
        }

        private string GetMemberGrade(int historyIntegrals)
        {
            MemberGrade memberGrade = (
                from a in context.MemberGrade
                orderby a.Integral descending
                select a).FirstOrDefault((MemberGrade a) => a.Integral <= historyIntegrals);
            if (memberGrade == null)
            {
                return "Vip0";
            }
            return memberGrade.GradeName;
        }

        public PageModel<UserMemberInfo> GetMembers(MemberQuery query)
        {
            int num = 0;
            IQueryable<UserMemberInfo> disabled = context.UserMemberInfo.AsQueryable<UserMemberInfo>();

            if (query.Disabled == 1)
            {
                disabled = from p in disabled where p.Disabled.Equals(false) select p;/*未激活*/
            }
            else if (query.Disabled == 2)
            {
                disabled = from p in disabled where p.Disabled.Equals(true) select p;/*已激活*/
            }

            if (query.UserType != 0)
            {
                disabled =
                    from d in disabled
                    where d.UserType.Equals(query.UserType)
                    select d;
            }

            if (!string.IsNullOrWhiteSpace(query.keyWords))
            {
                disabled =
                    from d in disabled
                    where d.UserName.Equals(query.keyWords)
                    select d;
            }
            if (query.Status.HasValue)
            {
                disabled =
                    from d in disabled
                    where d.Disabled
                    select d;
            }
            disabled = disabled.GetPage(out num, query.PageNo, query.PageSize, (IQueryable<UserMemberInfo> d) =>
                from o in d
                orderby o.Id
                select o);
            return new PageModel<UserMemberInfo>()
            {
                Models = disabled,
                Total = num
            };
        }

        public IQueryable<UserMemberInfo> GetMembers(bool? status, string keyWords)
        {
            return context.UserMemberInfo.FindBy((UserMemberInfo item) => item.ParentSellerId == 0 && (!status.HasValue || item.Disabled == status.Value) && (keyWords == null || (keyWords == "") || item.UserName.Contains(keyWords)));
        }

        private string GetNewUserName()
        {
            DbSet<UserMemberInfo> userMemberInfo;
            string str = "";
            do
            {
                str = "wx";
                Random random = new Random();
                string[] strArrays = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                string[] strArrays1 = strArrays;
                int length = strArrays1.Length;
                str = string.Concat(str, strArrays1[random.Next(0, length)]);
                str = string.Concat(str, strArrays1[random.Next(0, length)]);
                str = string.Concat(str, strArrays1[random.Next(0, length)]);
                str = string.Concat(str, strArrays1[random.Next(0, length)]);
                str = string.Concat(str, strArrays1[random.Next(0, length)]);
                str = string.Concat(str, strArrays1[random.Next(0, length)]);
                userMemberInfo = context.UserMemberInfo;
            }
            while (userMemberInfo.Any((UserMemberInfo d) => d.UserName == str));
            return str;
        }

        private string GetPasswrodWithTwiceEncode(string password, string salt)
        {
            string str = SecureHelper.MD5(password);
            return SecureHelper.MD5(string.Concat(str, salt));
        }

        public UserMemberInfo GetUserByCache(long userId)
        {
            UserMemberInfo userMemberInfo = null;
            string str = CacheKeyCollection.Member(userId);
            if (Cache.Get(str) == null)
            {
                userMemberInfo = context.UserMemberInfo.FindById<UserMemberInfo>(userId);
                if (userMemberInfo != null)
                {
                    DateTime now = DateTime.Now;
                    Cache.Insert(str, userMemberInfo, now.AddMinutes(15));
                }
            }
            else
            {
                userMemberInfo = (UserMemberInfo)Cache.Get(str);
            }
            return userMemberInfo;
        }

        public UserCenterModel GetUserCenterModel(long id)
        {
            List<OrderInfo> list = (
                from a in context.OrderInfo
                where a.UserId == id
                select a).ToList();
            UserCenterModel userCenterModel = new UserCenterModel();
            int num = (
                from a in context.MemberIntegral
                where a.MemberId == id
                select a.HistoryIntegrals).FirstOrDefault();
            userCenterModel.GradeName = GetMemberGrade(num);
            userCenterModel.Intergral = (
                from a in context.MemberIntegral
                where a.MemberId == id
                select a.AvailableIntegrals).FirstOrDefault();
            userCenterModel.UserCoupon = context.CouponRecordInfo.Count((CouponRecordInfo a) => a.UserId == id && a.CounponStatus == 0 && (a.ChemCloud_Coupon.EndTime > DateTime.Now));
            UserCenterModel userCoupon = userCenterModel;
            userCoupon.UserCoupon = userCoupon.UserCoupon + context.ShopBonusReceiveInfo.Count((ShopBonusReceiveInfo p) => p.UserId == id && (int)p.State == 1 && (p.ChemCloud_ShopBonusGrant.ChemCloud_ShopBonus.BonusDateEnd > DateTime.Now));
            userCenterModel.RefundCount = context.OrderRefundInfo.Count((OrderRefundInfo a) => a.UserId == id && (int)a.SellerAuditStatus != 4 && (int)a.ManagerConfirmStatus != 7);
            userCenterModel.WaitPayOrders = (
                from a in list
                where a.OrderStatus == OrderInfo.OrderOperateStatus.WaitPay
                select a).Count();
            userCenterModel.WaitReceivingOrders = (
                from a in list
                where a.OrderStatus == OrderInfo.OrderOperateStatus.WaitReceiving
                select a).Count();
            userCenterModel.WaitEvaluationOrders = list.Where((OrderInfo a) =>
            {
                if (a.OrderStatus != OrderInfo.OrderOperateStatus.Finish)
                {
                    return false;
                }
                return a.OrderCommentInfo.Count == 0;
            }).Count();
            userCenterModel.FollowProductCount = context.FavoriteInfo.Count((FavoriteInfo a) => a.UserId == id);
            if (userCenterModel.FollowProductCount > 0)
            {
                userCenterModel.FollwProducts = (
                    from a in
                        (
                            from a in context.FavoriteInfo
                            where a.UserId == id
                            orderby a.Id descending
                            select a).ToArray()
                    select new FollowProduct()
                    {
                        ProductId = a.ProductId,
                        ProductName = a.ProductInfo.ProductName,
                        Price = a.ProductInfo.MinSalePrice,
                        ImagePath = a.ProductInfo.ImagePath
                    }).Take(4).ToList();
            }
            FavoriteShopInfo[] array = (
                from a in context.FavoriteShopInfo.Include("ChemCloud_Shops")
                where a.UserId == id
                orderby a.Id descending
                select a).ToArray();
            userCenterModel.FollowShopsCount = array.Length;
            if (array.Length > 0)
            {
                List<FollowShop> followShops = (
                    from a in array
                    select new FollowShop()
                    {
                        ShopName = a.ChemCloud_Shops.ShopName,
                        Logo = a.ChemCloud_Shops.Logo,
                        ShopID = a.ShopId
                    }).ToList();
                if (followShops.Count > 8)
                {
                    userCenterModel.FollowShops = followShops.Take(8).ToList<FollowShop>();
                }
                userCenterModel.FollowShops = followShops;
            }
            userCenterModel.Orders = Instance<IOrderService>.Create.GetTopOrders(3, id);
            userCenterModel.FollowShopCartsCount = context.ShoppingCartItemInfo_.Count((ShoppingCartItemInfo a) => a.UserId == id);
            if (userCenterModel.FollowShopCartsCount > 0)
            {
                //List<ProductInfo> productInfos = (
                //    from p in context.ShoppingCartItemInfo_
                //    join o in context.ProductInfo on p.ProductId equals o.Id
                //    join x in context.ShopInfo on o.ShopId equals x.Id
                //    where p.UserId == id && (int)o.SaleStatus != 4
                //    select o).Distinct<ProductInfo>().Take(4).ToList();

                //List<ProductInfo> productInfos = (
                //   from p in context.ShoppingCartItemInfo_
                //   join o in context.ProductInfo on p.ProductId equals o.Id
                //   join x in context.ShopInfo on o.ShopId equals x.Id
                //   where p.UserId == id && (int)o.SaleStatus != 4
                //   select o).Take(4).ToList();

                userCenterModel.FollowShopCarts = (
                    from p in context.ShoppingCartItemInfo_
                    join o in context.ProductInfo on p.ProductId equals o.Id
                    join x in context.ShopInfo on o.ShopId equals x.Id
                    where p.UserId == id && (int)o.SaleStatus != 4
                    select new FollowShopCart()
                    {
                        ImagePath = o.ImagePath,
                        ProductName = o.ProductName,
                        ProductId = o.Id,
                        Quantity = p.Quantity
                    }).ToList();
            }
            return userCenterModel;
        }

        public void LockMember(long id)
        {
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FindById<UserMemberInfo>(id);
            userMemberInfo.Disabled = true;
            context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Member(id));
        }

        public UserMemberInfo Login(string username, string password)
        {
            UserMemberInfo now = context.UserMemberInfo.FindBy((UserMemberInfo item) => item.UserName == username).FirstOrDefault();
            if (now != null)
            {
                if (GetPasswrodWithTwiceEncode(password, now.PasswordSalt).ToLower() == now.Password)
                {
                    now.LastLoginDate = DateTime.Now;
                    context.SaveChanges();
                    Task.Factory.StartNew(() => AddIntegel(now));
                    Cache.Remove(CacheKeyCollection.Member(now.Id));
                }
                else
                {
                    now = null;
                }
            }
            return now;
        }

        public UserMemberInfo QuickRegister(string username, string realName, string nickName, string serviceProvider, string openId, string headImage = null, MemberOpenIdInfo.AppIdTypeEnum appidtype = (MemberOpenIdInfo.AppIdTypeEnum)1, string unionid = null, string unionopenid = null)
        {
            username = GetNewUserName();
            CheckInputIsValidWhenQuickRegister(username, serviceProvider, openId);
            CheckOpenIdHasBeenUsed(serviceProvider, openId, 0);
            if (string.IsNullOrWhiteSpace(nickName))
            {
                nickName = username;
            }
            Guid guid = Guid.NewGuid();
            string str = string.Concat("o", guid.ToString("N").Substring(12));
            string passwrodWithTwiceEncode = GetPasswrodWithTwiceEncode("", str);
            UserMemberInfo userMemberInfo = new UserMemberInfo()
            {
                UserName = username,
                PasswordSalt = str,
                CreateDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Nick = nickName,
                RealName = realName
            };
            UserMemberInfo userMemberInfo1 = userMemberInfo;
            if (context.UserMemberInfo.Any((UserMemberInfo d) => d.UserName == userMemberInfo1.UserName))
            {
                throw new HimallException("用户名被占用");
            }
            using (TransactionScope transactionScope = new TransactionScope())
            {
                userMemberInfo1.Password = passwrodWithTwiceEncode;
                userMemberInfo1 = context.UserMemberInfo.Add(userMemberInfo1);
                context.SaveChanges();
                MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo()
                {
                    UserId = userMemberInfo1.Id,
                    OpenId = openId,
                    ServiceProvider = serviceProvider,
                    AppIdType = appidtype,
                    UnionId = (string.IsNullOrWhiteSpace(unionid) ? string.Empty : unionid),
                    UnionOpenId = (string.IsNullOrWhiteSpace(unionopenid) ? string.Empty : unionopenid)
                };
                context.MemberOpenIdInfo.Add(memberOpenIdInfo);
                context.SaveChanges();
                if (!string.IsNullOrWhiteSpace(headImage))
                {
                    userMemberInfo1.Photo = TransferHeadImage(headImage, userMemberInfo1.Id);
                }
                context.SaveChanges();
                transactionScope.Complete();
            }
            return userMemberInfo1;
        }
        public bool CheckUserNameIsExit(string username)
        {
            UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.UserName.Equals(username));
            if (memberInfo == null)
                return true;
            else
                return false;
        }
        public bool AddSellerChild(UserMemberInfo memberInfo)
        {
            memberInfo.PasswordSalt = Guid.NewGuid().ToString();
            memberInfo.CreateDate = DateTime.Now;
            string str = SecureHelper.MD5(memberInfo.Password);
            memberInfo.Password = SecureHelper.MD5(string.Concat(str, memberInfo.PasswordSalt));
            context.UserMemberInfo.Add(memberInfo);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 注册子帐号
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ParentSellerId"></param>
        /// <returns></returns>
        public bool RegisterChild(UserMemberInfo model)
        {
            context.UserMemberInfo.Add(model);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }


        public UserMemberInfo Register(string username, string password, string email, string registertype, long introducer = 0L)
        {
            UserMemberInfo nullable;
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("用户名不能为空");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("密码不能为空");
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException("邮箱不能为空");
            }

            if (CheckMemberExist(username))
            {
                throw new HimallException(string.Concat("此用户名 ", username, " 已经被其它会员注册"));
            }

            password = password.Trim();
            Guid guid = Guid.NewGuid();
            string str = guid.ToString("N").Substring(12);
            password = GetPasswrodWithTwiceEncode(password, str);

            using (TransactionScope transactionScope = new TransactionScope())
            {
                UserMemberInfo userMemberInfo = new UserMemberInfo()
                {
                    UserName = username,
                    PasswordSalt = str,
                    CreateDate = DateTime.Now,
                    LastLoginDate = DateTime.Now,
                    Nick = username,
                    RealName = username,
                    CellPhone = "",
                    UserType = int.Parse(registertype),
                    Email = email
                };
                nullable = userMemberInfo;
                if (introducer != 0)
                {
                    nullable.InviteUserId = new long?(introducer);
                }
                nullable.Password = password;
                nullable = context.UserMemberInfo.Add(nullable);

                context.SaveChanges();

                transactionScope.Complete();
            }
            return nullable;
        }


        public UserMemberInfo Register(string username, string password, string serviceProvider, string openId, string headImage = null, long introducer = 0L, string nickname = null, string unionid = null)
        {
            if (string.IsNullOrWhiteSpace(serviceProvider))
            {
                throw new ArgumentNullException("信任登录提供商不能为空");
            }
            if (string.IsNullOrWhiteSpace(openId))
            {
                throw new ArgumentNullException("openId不能为空");
            }
            CheckOpenIdHasBeenUsed(serviceProvider, openId, 0);
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("用户名不能为空");
            }
            if (CheckMemberExist(username))
            {
                throw new HimallException(string.Concat("用户名 ", username, " 已经被其它会员注册"));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("密码不能为空");
            }
            password = password.Trim();
            UserMemberInfo userMemberInfo = new UserMemberInfo()
            {
                UserName = username
            };
            Guid guid = Guid.NewGuid();
            userMemberInfo.PasswordSalt = guid.ToString("N").Substring(12);
            userMemberInfo.CreateDate = DateTime.Now;
            userMemberInfo.LastLoginDate = DateTime.Now;
            userMemberInfo.Nick = (string.IsNullOrWhiteSpace(nickname) ? username : nickname);
            UserMemberInfo nullable = userMemberInfo;
            if (introducer != 0)
            {
                nullable.InviteUserId = new long?(introducer);
            }
            if (nullable.UserType == 3)
            {
                nullable.InviteUserId = 1;
            }
            using (TransactionScope transactionScope = new TransactionScope())
            {
                nullable.Password = GetPasswrodWithTwiceEncode(password, nullable.PasswordSalt);
                nullable = context.UserMemberInfo.Add(nullable);
                context.SaveChanges();
                MemberOpenIdInfo memberOpenIdInfo = new MemberOpenIdInfo()
                {
                    UserId = nullable.Id,
                    OpenId = openId,
                    ServiceProvider = serviceProvider,
                    UnionId = (string.IsNullOrWhiteSpace(unionid) ? string.Empty : unionid)
                };
                context.MemberOpenIdInfo.Add(memberOpenIdInfo);
                context.SaveChanges();
                if (!string.IsNullOrWhiteSpace(headImage))
                {
                    nullable.Photo = TransferHeadImage(headImage, nullable.Id);
                }
                context.SaveChanges();
                transactionScope.Complete();
            }
            return nullable;
        }

        private string TransferHeadImage(string image, long memberId)
        {
            string empty = string.Empty;
            if (!string.IsNullOrWhiteSpace(image))
            {
                if (image.StartsWith("http://"))
                {
                    WebClient webClient = new WebClient();
                    string str = image.Substring(image.LastIndexOf('/'));
                    string empty1 = string.Empty;
                    empty1 = (str.LastIndexOf('.') > 0 ? str.Substring(str.LastIndexOf('.')) : string.Empty);
                    DateTime now = DateTime.Now;
                    string str1 = string.Concat("/temp/", now.ToString("yyMMddHHmmssff"), empty1);
                    try
                    {
                        webClient.DownloadFile(image, IOHelper.GetMapPath(str1));
                    }
                    catch
                    {
                        empty = null;
                    }
                    image = str1;
                }
                if (image.StartsWith("/temp/"))
                {
                    if (!image.EndsWith(".jpg"))
                    {
                        DateTime dateTime = DateTime.Now;
                        string str2 = string.Concat("/temp/", dateTime.ToString("yyMMddHHmmssffff"), ".jpg");
                        ImageHelper.TranserImageFormat(IOHelper.GetMapPath(image), IOHelper.GetMapPath(str2), ImageFormat.Jpeg);
                        image = str2;
                    }
                    string str3 = string.Format("/Storage/Member/{0}", memberId);
                    string mapPath = IOHelper.GetMapPath(str3);
                    if (!Directory.Exists(mapPath))
                    {
                        Directory.CreateDirectory(mapPath);
                    }
                    empty = string.Concat(str3, "/headImage.jpg");
                    File.Copy(IOHelper.GetMapPath(image), IOHelper.GetMapPath(empty), true);
                }
            }
            return empty;
        }

        public void UnLockMember(long id)
        {
            UserMemberInfo userMemberInfo = context.UserMemberInfo.FindById<UserMemberInfo>(id);
            userMemberInfo.Disabled = false;
            context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Member(id));
        }

        public void UpdateMember(UserMemberInfo model)
        {
            UserMemberInfo nick = context.UserMemberInfo.FindById<UserMemberInfo>(model.Id);
            nick.Nick = model.Nick;
            nick.RealName = model.RealName;
            nick.Email = model.Email;
            nick.QQ = model.QQ;
            nick.CellPhone = model.CellPhone;
            nick.Disabled = model.Disabled;
            context.SaveChanges();
            Cache.Remove(CacheKeyCollection.Member(model.Id));
        }
        public bool AddLimited(List<LimitedAmount> limiteds)
        {
            context.LimitedAmount.AddRange(limiteds);
            int i = context.SaveChanges();
            if (i > 0)
                return true;
            else
                return false;
        }
        public bool IsAdmin(long userId)
        {
            UserMemberInfo memberInfo = context.UserMemberInfo.FirstOrDefault((UserMemberInfo m) => m.Id.Equals(userId));
            if (memberInfo.ParentSellerId == 0)
                return true;
            else
                return false;
        }
    }
}