using System;

namespace Gisoo.Models
{
	public class Reserve
	{
		public int id { get; set; }
		public int userIdNoticeOwner { get; set; }
		public int userId { get; set; }
		public User user { get; set; }
		public long price { get; set; }
		public DateTime date { get; set; }
		public int? lineId { get; set; }
		public Line line { get; set; }

		public int? classroomId { get; set; }

		public ClassRoom classroom { get; set; }

		public int? productId { get; set; }
		public string LineWeekDateId { get; set; }
		public LineWeekDate lineWeekDate { get; set; }

		public Product product { get; set; } 
	}
}
