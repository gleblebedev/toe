using System;

namespace Toe.Core
{
	/// <summary>
	/// Scene container.
	/// </summary>
	public class ToeScene
	{
		#region Constants and Fields

		public ToeEntity[] entities;

		private readonly ToeSceneConfiguration configuration;

		private readonly ToeMessageRegistry messageRegistry;

		private int firstClientAvailable;

		private int firstServerAvailable;

		private int lastClientAvailable;

		private int lastServerAvailable;

		#endregion

		#region Constructors and Destructors

		public ToeScene(ToeSceneConfiguration configuration, ToeMessageRegistry messageRegistry)
		{
			this.configuration = configuration;
			this.messageRegistry = messageRegistry;
			this.AllocateScene();
		}

		#endregion

		#region Public Properties

		public ToeMessageRegistry MessageRegistry
		{
			get
			{
				return this.messageRegistry;
			}
		}

		public int NumClientEntities
		{
			get
			{
				return this.configuration.NumClientEntities;
			}
			set
			{
				this.EnsureSceneIsNotAllocated();
				this.configuration.NumClientEntities = value;
			}
		}

		public int NumServerEntities
		{
			get
			{
				return this.configuration.NumServerEntities;
			}
			set
			{
				this.EnsureSceneIsNotAllocated();
				this.configuration.NumServerEntities = value;
			}
		}

		#endregion

		#region Public Methods and Operators

		public void AllocateScene()
		{
			this.DropScene();
			this.entities = new ToeEntity[1 + this.configuration.NumServerEntities + this.configuration.NumClientEntities];
			this.entities[0].Id = ToeEntityId.Empty;
			int i = 1;
			for (; i <= this.configuration.NumServerEntities; i++)
			{
				this.InitServerEntity(ref this.entities[i], i);
			}
			for (; i <= this.entities.Length; i++)
			{
				this.InitClientEntity(ref this.entities[i], i);
			}
		}

		public void DropScene()
		{
			if (this.entities == null)
			{
				return;
			}

			//TODO: drop scene

			this.entities = null;
		}

		#endregion

		#region Methods

		private void EnsureSceneIsNotAllocated()
		{
			if (this.entities != null)
			{
				throw new ApplicationException("Entities are already allocated");
			}
		}

		private void InitClientEntity(ref ToeEntity toeEntity, int index)
		{
			toeEntity.Id = new ToeEntityId(index, 0);
		}

		private void InitServerEntity(ref ToeEntity toeEntity, int index)
		{
			toeEntity.Id = new ToeEntityId(index, 0);
		}

		#endregion
	}
}