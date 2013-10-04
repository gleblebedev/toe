using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using OpenTK;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework;
#else

#endif

namespace Toe.Utils.Mesh
{
	public class SeparateStreamsMesh : SceneItem, IMesh
	{
		#region Constants and Fields

		Dictionary<StreamKey, IMeshStream> availableStreams = new Dictionary<StreamKey, IMeshStream>();


		private readonly BoneCollection bones = new BoneCollection();


		private readonly List<SeparateStreamsSubmesh> submeshes = new List<SeparateStreamsSubmesh>();

		

		private bool areBoundsValid;

		private Vector3 boundingBoxMax;

		private Vector3 boundingBoxMin;

		private Vector3 boundingSphereCenter;

		private float boundingSphereR;

		#endregion

		#region Public Properties

		public string BaseName { get; set; }

		public IMeshStream GetStream(string key, int channel)
		{
			IMeshStream list;
			if (availableStreams.TryGetValue(new StreamKey(key,channel), out list))
			{
				return list;
			}
			return null;
		}

		
		public IMeshStream SetStream(string key, int channel, IMeshStream stream)
		{
			availableStreams[new StreamKey(key, channel)] = stream;
			return stream;
		}


		public BoneCollection Bones
		{
			get
			{
				return this.bones;
			}
		}

		public Vector3 BoundingBoxMax
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMax;
			}
		}

		public Vector3 BoundingBoxMin
		{
			get
			{
				this.CalculateBounds();
				return this.boundingBoxMin;
			}
		}

		public Vector3 BoundingSphereCenter
		{
			get
			{
				this.CalculateBounds();
				return this.boundingSphereCenter;
			}
		}

		public float BoundingSphereR
		{
			get
			{
				this.CalculateBounds();
				return this.boundingSphereR;
			}
		}


		public int Count
		{
			get
			{
				return this.submeshes.Sum(submesh => submesh.Count);
			}
		}

		/// <summary>
		/// Gets mesh stream reader if available.
		/// </summary>
		/// <typeparam name="T">Type of stream element.</typeparam>
		/// <param name="key">Stream key.</param>
		/// <param name="channel">Stream channel.</param>
		/// <returns>Stream reader if available, null if not.</returns>
		public IList<T> GetStreamReader<T>(string key, int channel)
		{
			var stream = this.GetStream(key, channel);
			if (stream == null) return null;
			return stream.GetReader<T>();
		}

		public bool HasStream(string key, int channel)
		{
			return this.GetStream(key, channel) != null;
		}



		public uint NameHash { get; set; }

		

		public object RenderData { get; set; }

		public float Scale { get; set; }

		public string Skeleton { get; set; }

		public string SkeletonModel { get; set; }

		IList<ISubMesh> IMesh.Submeshes
		{
			get
			{
				return this.submeshes.Cast<ISubMesh>().ToArray();
			}
		}

		

		public string useGeo { get; set; }

		public string useGroup { get; set; }

		#endregion

		#region Public Methods and Operators

		public SeparateStreamsSubmesh CreateSubmesh()
		{
			var streamSubmesh = new SeparateStreamsSubmesh(this);
			this.submeshes.Add(streamSubmesh);
			return streamSubmesh;
		}

		public int EnsureBone(string boneName)
		{
			return this.bones.EnsureBone(boneName);
		}

		


		public void InvalidateBounds()
		{
			this.areBoundsValid = false;
		}

	

		#endregion

		#region Methods

		protected void CalculateBounds()
		{
			if (this.areBoundsValid)
			{
				return;
			}
			this.areBoundsValid = true;
			this.boundingBoxMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
			this.boundingBoxMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
			var streamReader = this.GetStreamReader<Vector3>(Streams.Position, 0);
			if (streamReader != null)
			foreach (var vector3 in streamReader)
			{
				if (this.boundingBoxMax.X < vector3.X)
				{
					this.boundingBoxMax.X = vector3.X;
				}
				if (this.boundingBoxMax.Y < vector3.Y)
				{
					this.boundingBoxMax.Y = vector3.Y;
				}
				if (this.boundingBoxMax.Z < vector3.Z)
				{
					this.boundingBoxMax.Z = vector3.Z;
				}
				if (this.boundingBoxMin.X > vector3.X)
				{
					this.boundingBoxMin.X = vector3.X;
				}
				if (this.boundingBoxMin.Y > vector3.Y)
				{
					this.boundingBoxMin.Y = vector3.Y;
				}
				if (this.boundingBoxMin.Z > vector3.Z)
				{
					this.boundingBoxMin.Z = vector3.Z;
				}
			}
			this.boundingSphereCenter = (this.boundingBoxMax + this.boundingBoxMin) * 0.5f;
			this.boundingSphereR = (this.boundingBoxMax - this.boundingBoxMin).Length * 0.5f;
		}

		#endregion
	}
}