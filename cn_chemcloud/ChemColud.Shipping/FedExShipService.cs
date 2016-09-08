using ChemColud.Shipping.ShippingProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemColud.Shipping
{
    public class FedExShipService
    {
        private string _url;
        private string _accountNumber;
        private string _key;
        private string _meterNumber;
        private string _password;

        public FedExShipService()
        {

        }
        public FedExShipService(string url, string key, string password, string accountNumber, string meterNumber)
        {
            _url = url;
            _key = key;
            _password = password;
            _accountNumber = accountNumber;
            _meterNumber = meterNumber;
        }

        /// <summary>
        /// 创建在线运单
        /// </summary>
        /// <param name="shipEx"></param>
        /// <returns></returns>
        public CreateShipRep CreateShip(ShipmentEx shipEx)
        {
            CreateShipRep rep = new CreateShipRep();
            List<ShipReply> result = new List<ShipReply>();
            ShipReply reply = null;
            FedExShipProvider ship = new FedExShipProvider(_key, _password, _accountNumber, _meterNumber);
            ship.Shipment = shipEx;
            ship.Url = _url;
            //生成主单
            ship.CreateShipment();

            if (ship.ShipReplyEx != null)
            {
                
                ship.ShipReplyEx.Master=true;
                result.Add(ship.ShipReplyEx);
                ShipManager shipManage = new ShipManager();

                List<Package> pkgList = new List<Package>();
                foreach (Package item in shipEx.Packages)
                {
                    pkgList.Add(item);
                }
               
                pkgList.RemoveAt(0);
                //生成副单
                int i = 0;
                foreach (Package item in pkgList)
                {
                    FedExShipSubProvider shipSub = new FedExShipSubProvider(_key, _password, _accountNumber, _meterNumber,ship.ShipReplyEx.TrackFormId,ship.ShipReplyEx.TrackNumber);
                    shipSub.Shipment = shipEx;
                    shipSub.SequenceNumber = i + 2;
                    shipSub.CurPackage = item;
                    i++;
                    shipSub.Url = _url;

                    shipManage.AddProvider(shipSub);

                }

                List<ShipReply> tmpList = shipManage.CreateShips();
                foreach (ShipReply item in tmpList)
                {
                    item.MasterTrackFormId = ship.ShipReplyEx.TrackFormId;
                    item.MasterTrackNumber = ship.ShipReplyEx.MasterTrackNumber;
                }
                result.AddRange(tmpList);
            }


            // shipManage.

            //  reply=ship.CreateShipment(_url);


            rep.ReplyList = result;
            if(result==null){
                rep.Message = "邮编地址与地区不匹配，生成运单失败！";
            }

            //rep.Message = ship.ErrorMessage;//英文错误提示，翻译成中文
            return rep;
        }


    }
}
