using ChemCloud.Core;
using ChemCloud.Entity;
using ChemCloud.IServices;
using ChemCloud.Model;
using ChemCloud.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ChemCloud.Service
{
	public class orderAddressService : ServiceBase, IorderAddressService, IService, IDisposable
	{
		public orderAddressService()
		{
		}

		public void AddorderAddress(orderAddressInfo shipinfo)
		{
			shipinfo.IsDefault = false;
			shipinfo.IsQuick = false;
			if ((
				from a in context.orderAddressInfo
				where a.UserId == shipinfo.UserId
				select a).Count() >= 20)
			{
				throw new HimallException("账单地址最多最能创建20个！");
			}
            context.orderAddressInfo.Add(shipinfo);
            context.SaveChanges();
            SetDefaultorderAddress(shipinfo.Id, shipinfo.UserId);
		}

		public void DeleteorderAddress(long id, long userId)
		{
			orderAddressInfo orderAddressInfo = (
				from a in context.orderAddressInfo
				where a.Id == id && a.UserId == userId
				select a).FirstOrDefault();
			if (orderAddressInfo == null)
			{
				throw new HimallException("该账单地址不存在或已被删除！");
			}
            context.orderAddressInfo.Remove(orderAddressInfo);
            context.SaveChanges();
		}

		public orderAddressInfo GetDefaultUserorderAddressByUserId(long userId)
		{
			orderAddressInfo regionFullName = context.orderAddressInfo.FirstOrDefault((orderAddressInfo item) => item.UserId == userId && item.IsDefault) ?? (
				from item in context.orderAddressInfo
				where item.UserId == userId
				orderby item.Id descending
				select item).FirstOrDefault();
			if (regionFullName != null)
			{
				IRegionService create = Instance<IRegionService>.Create;
				regionFullName.RegionFullName = create.GetRegionFullName(regionFullName.RegionId, " ");
				regionFullName.RegionIdPath = create.GetRegionIdPath(regionFullName.RegionId);
			}
			return regionFullName;
		}

		public orderAddressInfo GetUserorderAddress(long orderAddressId)
		{
			IRegionService create = Instance<IRegionService>.Create;
			DbSet<orderAddressInfo> orderAddressInfo = context.orderAddressInfo;
			object[] objArray = new object[] { orderAddressId };
			orderAddressInfo regionFullName = orderAddressInfo.Find(objArray);
			regionFullName.RegionFullName = create.GetRegionFullName(regionFullName.RegionId, " ");
			regionFullName.RegionIdPath = create.GetRegionIdPath(regionFullName.RegionId);
			return regionFullName;
		}

		public IQueryable<orderAddressInfo> GetUserorderAddressByUserId(long userId)
		{
			IRegionService create = Instance<IRegionService>.Create;
			IOrderedQueryable<orderAddressInfo> orderAddressInfo = 
				from a in context.orderAddressInfo
				where a.UserId == userId
				orderby a.Id descending
				select a;
			foreach (orderAddressInfo regionFullName in orderAddressInfo)
			{
                regionFullName.RegionFullName = create.GetRegionName(regionFullName.RegionId);
                regionFullName.RegionIdPath = create.GetRegionIdPath(regionFullName.RegionId);
			}
			return orderAddressInfo;
		}

		public void SetDefaultorderAddress(long id, long userId)
		{
			IQueryable<orderAddressInfo> orderAddressInfo = 
				from a in context.orderAddressInfo
				where a.UserId == userId
				select a;
			foreach (orderAddressInfo list in orderAddressInfo.ToList())
			{
				if (list.Id != id)
				{
					list.IsDefault = false;
				}
				else
				{
					list.IsDefault = true;
				}
			}
            context.SaveChanges();
		}

		public void SetQuickorderAddress(long id, long userId)
		{
			IQueryable<orderAddressInfo> orderAddressInfo = 
				from a in context.orderAddressInfo
				where a.UserId == userId
				select a;
			foreach (orderAddressInfo orderAddressInfo1 in orderAddressInfo)
			{
				if (orderAddressInfo1.Id != id)
				{
					orderAddressInfo1.IsQuick = false;
				}
				else
				{
					orderAddressInfo1.IsQuick = true;
				}
			}
            context.SaveChanges();
		}

		public void UpdateorderAddress(orderAddressInfo shipinfo)
		{
			orderAddressInfo phone = (
				from a in context.orderAddressInfo
				where a.Id == shipinfo.Id && a.UserId == shipinfo.UserId
				select a).FirstOrDefault();
			if (phone == null)
			{
				throw new HimallException("该账单地址不存在或已被删除！");
			}
			phone.Phone = shipinfo.Phone;
			phone.RegionId = shipinfo.RegionId;
			phone.ShipTo = shipinfo.ShipTo;
			phone.Address = shipinfo.Address;
            phone.PostCode = shipinfo.PostCode;

            context.SaveChanges();
		}
	}
}