using ChemCloud.Core;
using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;

namespace ChemCloud.IServices
{
	public interface ITopicService : IService, IDisposable
	{
		void AddTopic(TopicInfo topicInfo);

		void DeleteTopic(long id);

		TopicInfo GetTopicInfo(long id);

		PageModel<TopicInfo> GetTopics(int pageNo, int pageSize, PlatformType platformType = 0);

		PageModel<TopicInfo> GetTopics(TopicQuery topicQuery);

		void UpdateTopic(TopicInfo topicInfo);
	}
}