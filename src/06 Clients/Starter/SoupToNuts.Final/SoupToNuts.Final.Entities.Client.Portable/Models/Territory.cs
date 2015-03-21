using System;
using System.Collections.Generic;
using TrackableEntities.Client;

namespace SoupToNuts.Final.Entities.Client.Portable.Models
{
    public partial class Territory : EntityBase
    {
		public Territory()
		{
			this.Employees = new ChangeTrackingCollection<Employee>();
		}

		public string TerritoryId
		{ 
			get { return _TerritoryId; }
			set
			{
				if (Equals(value, _TerritoryId)) return;
				_TerritoryId = value;
				NotifyPropertyChanged(() => TerritoryId);
			}
		}
		private string _TerritoryId;

		public string TerritoryDescription
		{ 
			get { return _TerritoryDescription; }
			set
			{
				if (Equals(value, _TerritoryDescription)) return;
				_TerritoryDescription = value;
				NotifyPropertyChanged(() => TerritoryDescription);
			}
		}
		private string _TerritoryDescription;

		public ChangeTrackingCollection<Employee> Employees
		{
			get { return _Employees; }
			set
			{
				if (value != null) value.Parent = this;
				if (Equals(value, _Employees)) return;
				_Employees = value;
				NotifyPropertyChanged(() => Employees);
			}
		}
		private ChangeTrackingCollection<Employee> _Employees;

	}
}
