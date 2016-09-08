using System;

namespace ChemCloud.Service.Market.Business
{
	internal interface IGenerateDetail
	{
		void Generate(long bounsId, decimal totalPrice);
	}
}