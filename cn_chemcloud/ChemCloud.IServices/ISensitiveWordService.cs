using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Collections.Generic;

namespace ChemCloud.IServices
{
	public interface ISensitiveWordService : IService, IDisposable
	{
		void AddSensitiveWord(SensitiveWordsInfo model);

		void BatchDeleteSensitiveWord(int[] ids);

		void DeleteSensitiveWord(int id);

		bool ExistSensitiveWord(string word);

		IEnumerable<string> GetCategories();

		SensitiveWordsInfo GetSensitiveWord(int id);

		PageModel<SensitiveWordsInfo> GetSensitiveWords(SensitiveWordQuery query);

		void UpdateSensitiveWord(SensitiveWordsInfo model);
	}
}