using ChemCloud.Core;
using ChemCloud.IServices;
using ChemCloud.Model;
using System;

namespace ChemCloud.Web
{
	public class SceneHelper
	{
		public SceneHelper()
		{
		}

		public SceneModel GetModel(string sceneid)
		{
			return (SceneModel)Cache.Get(CacheKeyCollection.SceneState(sceneid));
		}

		public SceneModel GetModel(int sceneid)
		{
			string str = CacheKeyCollection.SceneState(sceneid.ToString());
			return (SceneModel)Cache.Get(str);
		}

		public void RemoveModel<T>(string key)
		{
			Cache.Remove(CacheKeyCollection.SceneState(key));
		}

		public int SetModel(SceneModel model, int expireTime = 600)
		{
			int hashCode = model.GetHashCode();
			string str = CacheKeyCollection.SceneState(hashCode.ToString());
			Cache.Insert(str, model, expireTime);
			return hashCode;
		}
	}
}