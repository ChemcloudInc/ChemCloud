using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using ChemCloud;
using ChemColud.Shipping;
using System.Collections.Generic;

namespace ChemCloud.IServices
{
	public interface IShipmentService : IService, IDisposable
	{
        ShipmentEx CalcPrice(FedExQueryAdv ship);

        ShipmentEx CalcPriceAdv(FedExQueryAdv ship);

        ShipmentEx CalcSFCostAdv(FedExQueryAdv ship);

        ShipmentEx GetShipmnet(long orderId);

       List<CountryInfo> GetCountyList();

       List<CountryInfo> GetCityList();


       Address GetAddress(long userId,int regionId);

       ShipReply CreateShip(CreateShipQuery shipQuery);

       List<OrderShip> GetOrderShip(long orderId);

       OrderShip GetShipOrder(long id);


	}
}