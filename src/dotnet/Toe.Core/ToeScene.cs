using System;

namespace Toe.Core
{
	/// <summary>
	/// Scene container.
	/// </summary>
	public class ToeScene
	{

		private readonly ToeSceneConfiguration configuration;

		private readonly ToeMessageRegistry messageRegistry;

		public ToeScene(ToeSceneConfiguration configuration, ToeMessageRegistry messageRegistry)
		{
			this.configuration = configuration;
			this.messageRegistry = messageRegistry;
			AllocateScene();
		}

		#region Constants and Fields

		public ToeEntity[] entities;

		private int firstServerAvailable = 0;
		private int lastServerAvailable = 0;

		private int firstClientAvailable = 0;
		private int lastClientAvailable = 0;

		public int NumServerEntities
		{
			get
			{
				return this.configuration.NumServerEntities;
			}
			set
			{
				EnsureSceneIsNotAllocated();
				this.configuration.NumServerEntities = value;
			}
		}

		private void EnsureSceneIsNotAllocated()
		{
			if (entities != null)
				throw new ApplicationException("Entities are already allocated");
		}

		public int NumClientEntities
		{
			get
			{
				return this.configuration.NumClientEntities;
			}
			set
			{
				EnsureSceneIsNotAllocated();
				this.configuration.NumClientEntities = value;
			}
		}

		public ToeMessageRegistry MessageRegistry
		{
			get
			{
				return this.messageRegistry;
			}
		}

		#endregion

		public void DropScene()
		{
			if (entities == null)
				return;

			//TODO: drop scene

			entities = null;
		}

		public void AllocateScene()
		{
			DropScene();
			entities = new ToeEntity[1 + this.configuration.NumServerEntities + this.configuration.NumClientEntities];
			entities[0].Id = ToeEntityId.Empty;
			int i = 1;
			for (; i <= this.configuration.NumServerEntities; i++)
			{
				InitServerEntity(ref entities[i],i);
			}
			for (; i <= entities.Length; i++)
			{
				InitClientEntity(ref entities[i],i);
			}

		}

		private void InitClientEntity(ref ToeEntity toeEntity, int index)
		{
			toeEntity.Id = new ToeEntityId(index,0);
		}

		private void InitServerEntity(ref ToeEntity toeEntity, int index)
		{
			toeEntity.Id = new ToeEntityId(index, 0);
		}
	}
}